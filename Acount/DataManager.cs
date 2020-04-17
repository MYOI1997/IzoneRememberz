using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class DataManager : MonoBehaviour
{
    /* 싱글톤 */
    public static DataManager Instance = null;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    /* 저장과 불러오기 */

    public void DataLoad(string FileDirectory, LoadType Type, List<GachaData> TargetList)
    {
        List<Dictionary<string, object>> StringData;

        if (Type == LoadType.Stream)
        {
            StringData = CSVReader.Read(FileDirectory);
        }
        else
        {
            StringData = CSVReader.AssetRead(FileDirectory);
        }

        for (int i = 0; i < StringData.Count; i++)
        {
            GachaData TempCharacter = new GachaData();

            if(StringData[i]["Name"] != null)
            {
                TempCharacter.Name = StringData[i]["Name"].ToString();
            }

            if(StringData[i]["Image"] != null)
            {
                TempCharacter.Image = StringData[i]["Image"].ToString();

            }

            switch (StringData[i]["Rank"])
            {
                case "S": TempCharacter.Rank = CardRank.S; break;
                case "A": TempCharacter.Rank = CardRank.A; break;
                case "B": TempCharacter.Rank = CardRank.B; break;
                default: break;
            }

            if(StringData[i]["Property"] != null)
            {
                TempCharacter.Property = StringData[i]["Property"].ToString();
            }
            
            if(StringData[i]["Power"] != null)
            {
                TempCharacter.Power = int.Parse(StringData[i]["Power"].ToString());
            }

            if(StringData[i]["Appearence"] != null)
            {
                TempCharacter.Appearence = int.Parse(StringData[i]["Appearence"].ToString());
            }
            
            if(StringData[i]["Weight"] != null)
            {
                TempCharacter.Weight = int.Parse(StringData[i]["Weight"].ToString());

            }

            if(StringData[i]["Level"] != null)
            {
                TempCharacter.Level = int.Parse(StringData[i]["Level"].ToString());
            }

            if(StringData[i]["MaxLevel"] != null)
            {
                TempCharacter.MaxLevel = int.Parse(StringData[i]["MaxLevel"].ToString());
            }
            
            if(StringData[i]["NowExp"] != null)
            {
                TempCharacter.NowExp = int.Parse(StringData[i]["NowExp"].ToString());
            }

            if(StringData[i]["MaxExp"] != null)
            {
                TempCharacter.MaxExp = int.Parse(StringData[i]["MaxExp"].ToString());
            }
            
            if(StringData[i]["Enhance"] != null)
            {
                TempCharacter.Enhance = int.Parse(StringData[i]["Enhance"].ToString());
            }
            
            if(StringData[i]["Key"] != null)
            {
                TempCharacter.Key = StringData[i]["Key"].ToString();
            }

            if(StringData[i]["Lock"] != null)
            {
                TempCharacter.Lock = int.Parse(StringData[i]["Lock"].ToString());
            }

            TargetList.Add(TempCharacter);
        }
    }

    public void DataSave(string SaveDirectory, List<GachaData> TargetList, List<string> SaveDatas)
    {
        using (var Writer = new CsvFileWriter(SaveDirectory))
        {
            List<string> Columns = SaveDatas;

            Writer.WriteRow(Columns);
            Columns.Clear();

            Type DataType = typeof(GachaData);

            FieldInfo[] DataInfos = DataType.GetFields(BindingFlags.Instance |
                                             BindingFlags.NonPublic |
                                             BindingFlags.Public |
                                             BindingFlags.NonPublic);

            for(int i = 0; i < TargetList.Count; i++)
            {
                foreach(var f in DataInfos)
                {
                    object TempObject = f.GetValue(TargetList[i]);

                    Columns.Add(TempObject.ToString());              
                }

                Writer.WriteRow(Columns);
                Columns.Clear();
            }

            Writer.Dispose();
        }

        Debug.Log("CSV 저장 완료");
    }
}
