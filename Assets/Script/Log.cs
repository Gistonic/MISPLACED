using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour {
    public Generator g;
    private string[] currLvl;
    public int[,] Visited;

    public GameObject MenuPlane;
    public GameObject LevelPlane;
    public GameObject PlayPlane;
    public GameObject Trail;

    private GameObject PreviousTrail;
    private GameObject CurrTrail;
    private GameObject NextTrail;

    public GameObject Step;
    public GameObject InLight;
    public GameObject OutLight;
    private GameObject Map;

    public void Start()
    {
        Debug.Log(PlayerPrefs.GetInt("CurrLvl"));
        // Prev
        if (PlayerPrefs.GetInt("CurrLvl") > 0)
        {
            PreviousTrail = Instantiate(Trail);
            PreviousTrail.transform.SetParent(gameObject.transform);
            PreviousTrail.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
            PreviousTrail.transform.localScale = new Vector3(1, 1, 1);
            PreviousTrail.GetComponent<RectTransform>().localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            PreviousTrail.GetComponent<RectTransform>().localPosition -= new Vector3(0, 640, 0);
            Fill(PreviousTrail, PlayerPrefs.GetInt("CurrLvl") - 1);
        }
        else
        {
            PreviousTrail = null;
        }
            

        // Curr
        if (PlayerPrefs.GetInt("CurrLvl") == -1)
        {
            CurrTrail = Instantiate(Trail);
            CurrTrail.transform.SetParent(gameObject.transform);
            CurrTrail.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
            CurrTrail.transform.localScale = new Vector3(1, 1, 1);
            CurrTrail.GetComponent<RectTransform>().localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            CurrTrail.GetComponent<RectTransform>().localPosition -= new Vector3(0, 640, 0);
            Fill(CurrTrail, 0);
        }
        else
        {
            CurrTrail = Instantiate(Trail);
            CurrTrail.transform.SetParent(gameObject.transform);
            CurrTrail.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
            CurrTrail.transform.localScale = new Vector3(1, 1, 1);
            CurrTrail.GetComponent<RectTransform>().localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            CurrTrail.GetComponent<RectTransform>().localPosition -= new Vector3(0, 640, 0);
            Fill(CurrTrail, PlayerPrefs.GetInt("CurrLvl"));
        }

        // Next
        if(PlayerPrefs.GetInt("CurrLvl") == PlayerPrefs.GetInt("MaxLvl"))
        {
            NextTrail = null;
        }
            
        else
        {
            NextTrail = Instantiate(Trail);
            NextTrail.transform.SetParent(gameObject.transform);
            NextTrail.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
            NextTrail.transform.localScale = new Vector3(1, 1, 1);
            NextTrail.GetComponent<RectTransform>().localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            CurrTrail.GetComponent<RectTransform>().localPosition -= new Vector3(0, 640, 0);
            Fill(NextTrail, PlayerPrefs.GetInt("CurrLvl") + 1);
        }
    }

    public void LoadLevels()
    {
        SaveLoad.Load();
    }

    private void Fill(GameObject go, int lvl)
    {
        Map = go.transform.GetChild(0).gameObject;
        Level l = SaveLoad.GetLevel(lvl);
        if (l != null)
        {
            // Text
            go.transform.GetChild(1).gameObject.GetComponent<Text>().text = "lvl " + lvl;
            go.transform.GetChild(2).gameObject.GetComponent<Text>().text = l.StepsSave + " steps";

            // Lights
            GameObject aux = Instantiate(InLight);
            aux.transform.position = new Vector3(l.InXSave, 0, l.InZSave);
            aux.transform.SetParent(Map.transform);
            aux = Instantiate(OutLight);
            aux.transform.position = new Vector3(l.OutXSave, 0, l.OutZSave);
            aux.transform.SetParent(Map.transform);

            // Path
            Visited = l.VisitedSave;
            int VisitedX = currLvl.Length;
            int VisitedZ = currLvl[0].Length;
        }
        else
        {
            // Text
            go.transform.GetChild(1).gameObject.GetComponent<Text>().text = "lvl " + lvl;
            go.transform.GetChild(2).gameObject.GetComponent<Text>().text = "0 steps";

            // Lights
            currLvl = LoadLvl(lvl);
            int x = 0, z = 0;
            foreach (string s in currLvl)
            {
                foreach (char c in s)
                {
                    if(c == 'I')
                    {
                        GameObject aux = Instantiate(InLight);
                        aux.transform.position = new Vector3(l.InXSave, 0, l.InZSave);
                        aux.transform.SetParent(Map.transform);
                    }
                    else if( c == 'O')
                    {
                        GameObject aux = Instantiate(OutLight);
                        aux.transform.position = new Vector3(l.OutXSave, 0, l.OutZSave);
                        aux.transform.SetParent(Map.transform);
                    }
                    ++z;
                }
                z = 0;
                ++x;
            }
        }
    }

    private string[] LoadLvl(int lvl)
    {
        string name = "lvl_" + lvl;
        TextAsset curr = Resources.Load(name) as TextAsset;
        string tmp = curr.text.Replace(Environment.NewLine, string.Empty);

        return tmp.Split('-');
    }

    public void MenuButton()
    {
        LevelPlane.SetActive(false);
        MenuPlane.SetActive(true);
    }


 
}
