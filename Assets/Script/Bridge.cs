using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour {

    public bool isVertical;
    private Generator g;

    // Start
    private void Start()
    {
        g = gameObject.transform.parent.parent.parent.GetComponent<Generator>();
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (isVertical)
        {
            gameObject.transform.parent.GetChild(2).gameObject.SetActive(false);
            g.MoveBridgeUp((int)gameObject.transform.parent.position.x - 1, (int)gameObject.transform.parent.position.z,(int)gameObject.transform.parent.position.x + 1, (int)gameObject.transform.parent.position.z);
        }
        else
        {
            gameObject.transform.parent.GetChild(1).gameObject.SetActive(false);
            g.MoveBridgeUp((int)gameObject.transform.parent.position.x, (int)gameObject.transform.parent.position.z + 1,(int)gameObject.transform.parent.position.x, (int)gameObject.transform.parent.position.z - 1);
        }
       
    }

    
    public void OnTriggerExit(Collider other)
    {
        if (isVertical)
        {
            gameObject.transform.parent.GetChild(2).gameObject.SetActive(true);
            g.MoveBridgeDown((int)gameObject.transform.parent.position.x - 1, (int)gameObject.transform.parent.position.z, (int)gameObject.transform.parent.position.x + 1, (int)gameObject.transform.parent.position.z);
        }
        else
        {
            gameObject.transform.parent.GetChild(1).gameObject.SetActive(true);
            g.MoveBridgeDown((int)gameObject.transform.parent.position.x, (int)gameObject.transform.parent.position.z + 1, (int)gameObject.transform.parent.position.x, (int)gameObject.transform.parent.position.z - 1);
        }
    }
}
