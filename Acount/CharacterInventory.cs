using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public enum LoadType
{
    Stream,
    Asset
}

public class CharacterInventory : MonoBehaviour
{
    public List<GachaData> CharacterList = new List<GachaData>();
    public List<GachaData> CompilationList = new List<GachaData>();

    FileInfo CharacterListInfo, CompilationListInfo;

    public static CharacterInventory Instance;

    List<Dictionary<string, object>> StringData;

    bool SyncComplete = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        CharacterListInfo = new FileInfo(Application.dataPath + "/Resources/Inventory.csv");
        CompilationListInfo = new FileInfo(Application.dataPath + "/Resources/CompilationList.csv");

        if(CharacterListInfo.Exists)
        {
            Load(Application.dataPath + "/Resources/Inventory.csv", CharacterList, LoadType.Stream);
        }
        else
        {
            Load("Inventory", CharacterList, LoadType.Asset);
        }

        if(CompilationListInfo.Exists)
        {
            Load(Application.dataPath + "/Resources/CompilationList.csv", CompilationList, LoadType.Stream);
        }
        else
        {
            Load("CompilationList", CompilationList, LoadType.Asset);
        }
    }

    public void Load(string FileName, List<GachaData> TargetList, LoadType Type)
    {
        if(Type == LoadType.Stream)
        {
            StringData = CSVReader.Read(FileName);
        }
        else
        {
            StringData = CSVReader.AssetRead(FileName);
        }

        for(int i = 0; i < StringData.Count; i++)
        {
            GachaData TempCharacter = new GachaData();

            TempCharacter.Name = StringData[i]["Name"].ToString();
            TempCharacter.Image = StringData[i]["Image"].ToString();

            switch (StringData[i]["Rank"])
            {
                case "S" : TempCharacter.Rank = CardRank.S; break;
                case "A" : TempCharacter.Rank = CardRank.A; break;
                case "B" : TempCharacter.Rank = CardRank.B; break;
                default: break;
            }

            TempCharacter.Property = StringData[i]["Property"].ToString();
            TempCharacter.Power = int.Parse(StringData[i]["Power"].ToString());
            TempCharacter.Appearence = int.Parse(StringData[i]["Appearence"].ToString());
            TempCharacter.Weight = int.Parse(StringData[i]["Weight"].ToString());
            TempCharacter.Level = int.Parse(StringData[i]["Level"].ToString());
            TempCharacter.MaxLevel = int.Parse(StringData[i]["MaxLevel"].ToString());
            TempCharacter.NowExp = int.Parse(StringData[i]["NowExp"].ToString());
            TempCharacter.MaxExp = int.Parse(StringData[i]["MaxExp"].ToString());
            TempCharacter.Enhance = int.Parse(StringData[i]["Enhance"].ToString());
            TempCharacter.Key = StringData[i]["Key"].ToString();
            TempCharacter.Lock = int.Parse(StringData[i]["Lock"].ToString());

            TargetList.Add(TempCharacter);
        }
    }

    public void Save(string FileName, List<GachaData> TargetList)
    {
        using (var Writer = new CsvFileWriter(FileName))
        {
            List<string> Columns = new List<string>() { "Name", "Image", "Rank", "Property", "Power", "Appearence", "Weight", "Level", "MaxLevel", "NowExp", "MaxExp", "Enhance", "Key", "Lock" };

            Writer.WriteRow(Columns);
            Columns.Clear();

            for(int i = 0; i < TargetList.Count; i++)
            {
                Columns.Add(TargetList[i].Name);
                Columns.Add(TargetList[i].Image.ToString());
                Columns.Add(TargetList[i].Rank.ToString());
                Columns.Add(TargetList[i].Property.ToString());
                Columns.Add(TargetList[i].Power.ToString());
                Columns.Add(TargetList[i].Appearence.ToString());
                Columns.Add(TargetList[i].Weight.ToString());
                Columns.Add(TargetList[i].Level.ToString());
                Columns.Add(TargetList[i].MaxLevel.ToString());
                Columns.Add(TargetList[i].NowExp.ToString());
                Columns.Add(TargetList[i].MaxExp.ToString());
                Columns.Add(TargetList[i].Enhance.ToString());
                Columns.Add(TargetList[i].Key.ToString());
                Columns.Add(TargetList[i].Lock.ToString());

                Writer.WriteRow(Columns);
                Columns.Clear();
            }

            Writer.Dispose();
        }

        Debug.Log("CSV 저장 완료");
    }

    public void PrintCharacterData()
    {
        for(int i = 0; i < CharacterList.Count; i++)
        {
            Debug.Log(CharacterList[i].Name);
            Debug.Log(CharacterList[i].Image);
        }
    }

    public void DataSync()
    {
        SyncComplete = false;

        for(int i = 0; i < CompilationList.Count; i++)
        {
            for(int j = 0; j < CharacterList.Count; j++)
            {
                if(CompilationList[i].Key == CharacterList[j].Key)
                {
                    Debug.Log("데이터 동기화 중");

                    CharacterList[j].Power = CompilationList[i].Power;
                    CharacterList[j].Level = CompilationList[i].Level;
                    CharacterList[j].MaxLevel = CompilationList[i].MaxLevel;
                    CharacterList[j].NowExp = CompilationList[i].NowExp;
                    CharacterList[j].MaxExp = CompilationList[i].MaxExp;
                    CharacterList[j].Enhance = CompilationList[i].Enhance;
                    CharacterList[j].Lock = CompilationList[i].Lock;
                }
            }
        }

        Debug.Log("데이터 동기화 완료");

        SyncComplete = true;
    }

    void OnApplicationQuit()
    {
        DataSync();

        if(SyncComplete)
        {
            Save(Application.dataPath + "/Resources/Inventory.csv", CharacterList);
            Save(Application.dataPath + "/Resources/CompilationList.csv", CompilationList);
        }
    }
}
