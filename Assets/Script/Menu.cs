using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {
    
    public Generator g;

    public GameObject MenuPlane;
    public GameObject LevelPlane;
    public GameObject PlayPlane;

    // Use this for initialization
    void Start () {
        PlayerPrefs.SetInt("MaxLvl", -1);   //           <------------------------------------------------  Reset 
        PlayerPrefs.SetInt("CurrLvl", -1);
        g.CreateMenuMaze();
    }

    public void PlayButton()
    {
        MenuPlane.SetActive(false);
        PlayPlane.SetActive(true);
        g.NextLevel();
    }

     public void LevelButton()
    {
        MenuPlane.SetActive(false);
        LevelPlane.SetActive(true);
        LevelPlane.GetComponent<Log>().LoadLevels();
        g.DestroyLevel();
    }
	
}
