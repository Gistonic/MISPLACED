using UnityEngine;

[System.Serializable]
public class Level
{
    public static Level current; 
    public int[,] VisitedSave;    //[x,z]
    public int StepsSave;
    public int InXSave, InZSave;
    public int OutXSave, OutZSave;

    public Level(int[,] visited, int steps, int inXS, int inZS, int outXS, int outZS)
    {
        VisitedSave = visited;
        StepsSave = steps;
        InXSave = inXS;
        InZSave = inZS;
        OutXSave = outXS;
        OutZSave = outZS;
    }

    /*
    public void FillSave(int[,] visited, int steps)
    {
        VisitedSave = visited;
        StepsSave = steps;
    }
    */

}
