using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardRank { S, A, B, }

[System.Serializable]
public class GachaData
{
    public string Name;
    public string Image;
    public CardRank Rank;
    public string Property;
    public int Power;
    public int Appearence;

    public int Weight;

    public int Level;
    public int MaxLevel;

    public int NowExp;
    public int MaxExp;

    public int Enhance;

    public string Key;
    public int Lock;

    public GachaData()
    {

    }

    public GachaData(GachaData Data)
    {
        this.Name = Data.Name;
        this.Image = Data.Image;
        this.Rank = Data.Rank;
        this.Property = Data.Property;
        this.Power = Data.Power;
        this.Appearence = Data.Appearence;
        this.Weight = Data.Weight;
        this.Level = Data.Level;
        this.MaxLevel = Data.MaxLevel;
        this.NowExp = Data.NowExp;
        this.MaxExp = Data.MaxExp;
        this.Enhance = Data.Enhance;
        this.Key = Data.Key;
        this.Lock = Data.Lock;
    }

    public void LevelUpCheck()
    {
        if(NowExp >= MaxExp)
        {
            if(Level < MaxLevel)
            {
                Level++;
                NowExp -= MaxExp;
                MaxExp += 100;

                switch (Rank)
                {
                    case CardRank.S: Power = (Power + 30); break;
                    case CardRank.A: Power = (Power + 20); break;
                    case CardRank.B: Power = (Power + 10); break;
                }
            }
        }
    }
}
