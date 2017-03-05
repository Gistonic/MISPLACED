using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image bgImg;
    private Image JoystickImg;
    private Vector3 inputVector;

    private void Start()
    {
        bgImg = GetComponent<Image>();
        JoystickImg = transform.GetChild(0).GetComponent<Image>(); // bg es el pare de la img del joystik real
    }



    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            // Debug.Log("claro q si guapi");
            pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x * 2 , 0, pos.y * 2);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            // Move Joystick IMG
            JoystickImg.rectTransform.anchoredPosition =
                   new Vector3(inputVector.x * (bgImg.rectTransform.sizeDelta.x / 5), inputVector.z * (bgImg.rectTransform.sizeDelta.y / 5));

        }

    }


    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);

    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector3.zero;
        JoystickImg.rectTransform.anchoredPosition = Vector3.zero;

    }

    public float Horizontal()
    {
        if (inputVector.x != 0)
        {
            //Debug.Log(inputVector.x);
            return inputVector.x;
        }
        else
            return 0;
    }

    public float Vertical()
    {
        if (inputVector.z != 0)
        {
            //Debug.Log(inputVector.z);
            return inputVector.z;
        }
        else
            return 0;
    }


}
