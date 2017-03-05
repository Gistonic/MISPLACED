using UnityEngine;

public class Floor : MonoBehaviour {

    private Generator g;

    // Start
    private void Start()
    {
        if (gameObject.transform.parent.gameObject.tag == "Bridge")
            g = gameObject.transform.parent.parent.parent.GetComponent<Generator>();
        else
            g = gameObject.transform.parent.parent.GetComponent<Generator>();
    }

    // Detects Collision
    public void OnTriggerEnter(Collider other)
    {
        if (gameObject.transform.parent.gameObject.tag == "Bridge")
            g.SavePath(gameObject.transform.parent.gameObject);
        else
            if (g != null) g.SavePath(gameObject);
    }
}
