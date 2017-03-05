using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad
{
    public static List<Level> l = new List<Level>();


    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedLevels.gd");
        bf.Serialize(file, l);
        file.Close();
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedLevels.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedLevels.gd", FileMode.Open);
            l = (List<Level>)bf.Deserialize(file);
            file.Close();
        }
    }

    public static void SetLevel(int lvl, int steps, int[,] Visited, int InX, int InZ, int OutX, int OutZ)
    {
        if (l.Count > lvl)
        {
            if (l[lvl].StepsSave > steps)
            {
                l[lvl] = new Level(Visited, steps,InX, InZ, OutX, OutZ);
                Save();
            }
        }
        else
        {
            l.Add(new Level(Visited, steps, InX, InZ, OutX, OutZ));
            Save();
        }
    }

    public static Level GetLevel(int lvl)
    {
        if (l.Count > lvl)
            return l[lvl];
        else return null;
    }
    
}