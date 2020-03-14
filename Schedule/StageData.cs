using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageData
{
    public int Index;
    public int Level;
    public string Property;
    public int Point;
    public int Exp;
    public int Jewel;

    public StageData()
    {

    }

    public StageData(StageData Data)
    {
        this.Index = Data.Index;
        this.Level = Data.Level;
        this.Property = Data.Property;
        this.Point = Data.Point;
        this.Exp = Data.Exp;
        this.Jewel = Data.Jewel;
    }
}
