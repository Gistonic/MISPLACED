using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Generator : MonoBehaviour {
    // Level
    private int lvlNumber;

    // Prefabs
    public GameObject WallPrefab;
    public GameObject BridgeePrefab;
    public GameObject MazePrefab;
    public GameObject FloorPrefab;
    public GameObject PlayerPrefab;
    public GameObject InPrefab;
    public GameObject OutPrefab;
    public GameObject OutFloorPrefab;

    [HideInInspector]
    public GameObject Player;
    private GameObject In;
    private GameObject Out;

    // Joystick
    public Joystick j;

    // State
    private string[] currLvl;
    private GameObject[,] Blocs;
    private int[,] Visited;
    private int VisitedX;
    private int VisitedZ;
    private int Steps;
    private int InX, InZ;
    private int OutX, OutZ;


    // Materials
    public Material Floor;
    private Color floorColor = new Color (1,1,1,1);

    // Lerp
    private float lerpSpeed = 0.15f;
    private GameObject Lerp1;
    private Vector3 Lerp1Dest;
    private GameObject Lerp2;
    private Vector3 Lerp2Dest;
    private bool isLerping = false;


    // Use this for initialization
    void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        PlayerInput();
        if (isLerping) Lerp();
    }

    // Player Input
    private void PlayerInput()
    {
        if(j.Vertical() != 0) Player.GetComponent<Rigidbody>().AddForce(0, 0, 25 * j.Horizontal());
        if (j.Horizontal() != 0) Player.GetComponent<Rigidbody>().AddForce(-25 * j.Vertical(), 0, 0);

#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.DownArrow)) Player.GetComponent<Rigidbody>().AddForce(0, 0, -25);
        if (Input.GetKey(KeyCode.UpArrow  )) Player.GetComponent<Rigidbody>().AddForce(0, 0, 25);
        if (Input.GetKey(KeyCode.LeftArrow)) Player.GetComponent<Rigidbody>().AddForce(-25, 0, 0);
        if (Input.GetKey(KeyCode.RightArrow)) Player.GetComponent<Rigidbody>().AddForce(25, 0, 0);
#endif
    }

    private void Lerp()
    {
        Lerp1.transform.position = Vector3.MoveTowards(Lerp1.transform.position, Lerp1Dest, lerpSpeed);
        Lerp2.transform.position = Vector3.MoveTowards(Lerp2.transform.position, Lerp2Dest, lerpSpeed);
        if (Lerp1.transform.position == Lerp1Dest && Lerp2.transform.position == Lerp2Dest)
        {
            Lerp1 = null;
            Lerp2 = null;
            isLerping = false;
        }
    }

    // Generates level 
    private void GenerateLvl()
    {
        VisitedX = currLvl.Length;
        VisitedZ = currLvl[0].Length;
        Visited = new int[VisitedX, VisitedZ];
        Blocs = new GameObject[VisitedX, VisitedZ];
        Steps = 0;

        int x = 0, z = 0;
        foreach (string s in currLvl)
        {
            foreach (char c in s)
            {
                if (c == '#' || c== 'W')
                {
                    GameObject go = Instantiate(WallPrefab);
                    go.transform.position = new Vector3(x, 1, z);
                    go.transform.SetParent(MazePrefab.transform);
                    Blocs[x, z] = go;
                }
                else if(c == 'B')
                {
                    GameObject go = Instantiate(BridgeePrefab);
                    go.transform.position = new Vector3(x, 0, z);
                    go.transform.SetParent(MazePrefab.transform);
                    Blocs[x, z] = go;
                }
                else if (c == '.' || c=='I')
                {
                    GameObject go = Instantiate(FloorPrefab);
                    go.GetComponent<MeshRenderer>().material.SetColor("_Color", floorColor);
                    go.transform.position = new Vector3(x, 0, z);
                    go.transform.SetParent(MazePrefab.transform);
                    Blocs[x, z] = go;
                    if (c == 'I')
                    {
                        InX = x;
                        InZ = z;
                        Player = Instantiate(PlayerPrefab);
                        Player.transform.position = new Vector3(x, 0.75f, z);
                        Player.transform.SetParent(MazePrefab.transform);
                        Camera.main.transform.position = new Vector3(x + 5f, Camera.main.transform.position.y, z);
                        Camera.main.transform.SetParent(Player.transform);
                        In = Instantiate(InPrefab);
                        In.transform.position = new Vector3(x, 1.25f, z);
                        In.transform.SetParent(MazePrefab.transform);
                    }
                }
                else if(c == 'O')
                {
                    OutX = x;
                    OutZ = z;
                    GameObject go = Instantiate(OutFloorPrefab);
                    go.transform.position = new Vector3(x, 0, z);
                    go.transform.SetParent(MazePrefab.transform);
                    Blocs[x, z] = go;
                    Out = Instantiate(OutPrefab);
                    Out.transform.position = new Vector3(x, 1.25f, z);
                    Out.transform.SetParent(MazePrefab.transform);
                }
                ++z;
            }
            z = 0;
            ++x;
        }
        PaintAcordingToLevel();
    }

    // Load the level lvl
    private string[] LoadLvl(int lvl)
    {
        lvlNumber = lvl;
        string name = "lvl_" + lvl;
        TextAsset curr = Resources.Load(name) as TextAsset;
        string tmp = curr.text.Replace(Environment.NewLine, string.Empty);

        return tmp.Split('-');
    }
    
    // Save the path to the Visited Matrix
    public void SavePath(GameObject go)
    {
        //int aux = 
        ++Visited[(int)go.transform.position.x,(int)go.transform.position.z];
        ++Steps;
        /*
        floorColor = new Color(1, 0.9f, 0.9f, 1) - new Color(0, aux * 0.1f, aux * 0.1f, 0);
        if(go.tag == "Bridge")
            go.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", floorColor);
        else
            go.GetComponent<MeshRenderer>().material.SetColor("_Color", floorColor);
        */
    }

    // Check if a near bridge position is up or down
    public bool isBridgeUpAt(int x, int z)
    {
        if (Blocs[x,z].transform.position.y == 0) return false;
        else return true;
    }

    // Move the GO Bridge up
    public void MoveBridgeUp(int x1, int z1, int x2, int z2)
    {
        if (isLerping)
        {
            Lerp1.transform.position = Lerp1Dest;
            Lerp2.transform.position = Lerp2Dest;
            isLerping = false;
        }

        Lerp1 = Blocs[x1, z1];
        Lerp1Dest = Lerp1.transform.position + new Vector3(0, 1, 0);

        Lerp2 = Blocs[x2, z2];
        Lerp2Dest = Lerp2.transform.position + new Vector3(0, 1, 0);
        isLerping = true;
        //Blocs[x,z].transform.position += new Vector3(0, 1, 0);
    }

    // Move the GO Bridge down
    public void MoveBridgeDown(int x1, int z1, int x2, int z2)
    {
        if (isLerping)
        {
            Lerp1.transform.position = Lerp1Dest;
            Lerp2.transform.position = Lerp2Dest;
            isLerping = false;
        }

        Lerp1 = Blocs[x1, z1];
        Lerp1Dest = Lerp1.transform.position + new Vector3(0, -1, 0);

        Lerp2 = Blocs[x2, z2];
        Lerp2Dest = Lerp2.transform.position + new Vector3(0, -1, 0);
        isLerping = true;
        //Blocs[x, z].transform.position += new Vector3(0, -1, 0);
    }

    // Paint according ot level
    private void PaintAcordingToLevel()
    {
        int colorInt = lvlNumber % 6;
        Color currColor, nextColor;
        switch (colorInt)
        {
            case 0:
                currColor = new Color(1, 0, 0, 1);
                nextColor = new Color(1, 1, 0, 1);
                break;
            case 1:
                currColor = new Color(1, 1, 0, 1);
                nextColor = new Color(0, 1, 0, 1);
                break;
            case 2:
                currColor = new Color(0, 1, 0, 1);
                nextColor = new Color(0, 1, 1, 1);
                break;
            case 3:
                currColor = new Color(0, 1, 1, 1);
                nextColor = new Color(0, 0, 1, 1);
                break;
            case 4:
                currColor = new Color(0, 0, 1, 1);
                nextColor = new Color(1, 0, 1, 1);
                break;
            case 5:
                currColor = new Color(1, 0, 1, 1);
                nextColor = new Color(1, 0, 0, 1);
                break;
            default:
                currColor = new Color(1, 0, 0, 1);
                nextColor = new Color(1, 1, 0, 1);
                break;
        }

        if (Player != null) Player.GetComponent<MeshRenderer>().material.SetColor("_Color", currColor);
        if (Player != null) Player.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", currColor - new Color(0.2f, 0.2f, 0.2f, 0.0f));
        if (Player != null) Player.transform.GetChild(0).gameObject.GetComponent<Light>().color = currColor;
        if (In != null) In.GetComponent<Light>().color = currColor;
        if (Out != null) Out.GetComponent<Light>().color = nextColor;
    }
    
    public void DestroyLevel()
    {
        Camera.main.transform.SetParent(null);
        if (Blocs != null)
        {
            foreach (GameObject go in Blocs)
                Destroy(go);
        }

        if (Player != null) Destroy(Player);
        if (Out != null) Destroy(Out);
        if (In != null) Destroy(In);
        
        for(int i = 0; i < VisitedX; ++i)
        {
            for (int j = 0; j < VisitedZ; ++j) 
            {
                Visited[i, j] = 0;
            }
        }

        VisitedX = 0;
        VisitedZ = 0;
    }

    // Go to next level
    public void NextLevel()
    {
        if (PlayerPrefs.GetInt("CurrLvl") != -1) SaveState();
        DestroyLevel();

        PlayerPrefs.SetInt("CurrLvl", PlayerPrefs.GetInt("CurrLvl") +1);
        currLvl = LoadLvl(PlayerPrefs.GetInt("CurrLvl"));
        if (PlayerPrefs.GetInt("CurrLvl") > PlayerPrefs.GetInt("MaxLvl")) PlayerPrefs.SetInt("MaxLvl", PlayerPrefs.GetInt("CurrLvl"));
        GenerateLvl();
    }

    public void CreateMenuMaze()
    {
        currLvl = LoadLvl(-1);
        GenerateLvl();
        Camera.main.transform.position = new Vector3(Player.transform.position.x, Camera.main.transform.position.y, Player.transform.position.z );
    }

    private void SaveState()
    {
        SaveLoad.SetLevel(PlayerPrefs.GetInt("CurrLvl"), Steps, Visited, InX, InZ, OutX,OutZ);
    }
}
