using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Out : MonoBehaviour {

    private Generator g;

    // Start
    private void Start()
    {
        g = gameObject.transform.parent.parent.GetComponent<Generator>();
    }

    // Detects Collision
    public void OnTriggerEnter(Collider other)
    {
        if (g != null) g.NextLevel();
    }
}
