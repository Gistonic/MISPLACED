using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play : MonoBehaviour {

    public Generator g;
    
    public GameObject LevelPlane;
    public GameObject PlayPlane;
    

    public void LevelButton()
    {
        g.DestroyLevel();
        LevelPlane.SetActive(true);
        LevelPlane.GetComponent<Log>().LoadLevels();
        PlayPlane.SetActive(false);
    }
}
