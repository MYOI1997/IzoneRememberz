using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

/* 최종 수정일 : 20.04.18 */
/* 수정 내용 : Reflection 기능을 사용하여 저장의 유지보수 향상 */

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

        if (CharacterListInfo.Exists)
        {
            Load(Application.dataPath + "/Resources/Inventory.csv", LoadType.Stream, CharacterList);
        }
        else
        {
            Load("Inventory", LoadType.Asset, CharacterList);
        }

        if (CompilationListInfo.Exists)
        {
            Load(Application.dataPath + "/Resources/CompilationList.csv", LoadType.Stream, CompilationList);
        }
        else
        {
            Load("CompilationList", LoadType.Asset, CompilationList);
        }
    }

    public void Load(string FileDirectory, LoadType Type, List<GachaData> TargetList)
    {
        DataManager.Instance.DataLoad(FileDirectory, Type, TargetList);
    }

    public void Save(string SaveDirectory, List<GachaData> TargetList)
    {
        List<string> Columns = new List<string>() { "Name", "Image", "Rank", "Property", "Power", "Appearence", "Weight", "Level", "MaxLevel", "NowExp", "MaxExp", "Enhance", "Key", "Lock" };

        DataManager.Instance.DataSave(SaveDirectory, TargetList, Columns);
    }

    public void PrintCharacterData()
    {
        for (int i = 0; i < CharacterList.Count; i++)
        {
            Debug.Log(CharacterList[i].Name);
            Debug.Log(CharacterList[i].Image);
        }
    }

    public void DataSync()
    {
        SyncComplete = false;

        for (int i = 0; i < CompilationList.Count; i++)
        {
            for (int j = 0; j < CharacterList.Count; j++)
            {
                if (CompilationList[i].Key == CharacterList[j].Key)
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

        if (SyncComplete)
        {
            Save(Application.dataPath + "/Resources/Inventory.csv", CharacterList);
            Save(Application.dataPath + "/Resources/CompilationList.csv", CompilationList);
        }
    }
}