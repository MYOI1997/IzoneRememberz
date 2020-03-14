using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCheck : MonoBehaviour
{
    private int LP, AP;

    string[] NowSplitTime, SaveSplitTime;

    private int AddResource = 0;

    public static ResourceCheck Instance;

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

    void Start()
    {
        NowSplitTime = System.DateTime.Now.ToString("dd-HH-mm").Split('-');

        for(int i = 0; i < NowSplitTime.Length; i++)
        {
            Debug.Log(NowSplitTime[i]);
        }

        LP = PlayerPrefs.GetInt("LP", 0);
        AP = PlayerPrefs.GetInt("AP", 0);

        if(LP <= 0) { LP = 0; }
        if(AP <= 0) { AP = 0; }

        if(PlayerPrefs.GetString("SaveTime").Length > 7)
        {
            SaveSplitTime = PlayerPrefs.GetString("SaveTime").Split('-');

            Debug.Log((int.Parse(NowSplitTime[0]) - int.Parse(SaveSplitTime[0])));
            Debug.Log((int.Parse(NowSplitTime[1]) - int.Parse(SaveSplitTime[1])));
            Debug.Log((int.Parse(NowSplitTime[2]) - int.Parse(SaveSplitTime[2])));

            AddResource += (int.Parse(NowSplitTime[0]) - int.Parse(SaveSplitTime[0])) * 1440;
            AddResource += (int.Parse(NowSplitTime[1]) - int.Parse(SaveSplitTime[1])) * 60;
            AddResource += int.Parse(NowSplitTime[2]) - int.Parse(SaveSplitTime[2]);

            Debug.Log("Add Resources : " + AddResource);

            int TempResource = (AddResource / 3);

            Debug.Log("LP : " + TempResource);

            if(TempResource >= 1)
            {
                LP += TempResource;
            }

            TempResource = (AddResource / 10);

            Debug.Log("AP : " + TempResource);

            if(TempResource >= 1)
            {
                AP += TempResource;
            }

            PlayerPrefs.SetInt("LP", LP);
            PlayerPrefs.SetInt("AP", AP);
        }
        else
        {
            Debug.Log("추가할 자원이 없습니다.");
        }
    }

    void OnApplicationQuit()
    {
        Debug.Log("종료, 현재 시간 저장중");

        PlayerPrefs.SetString("SaveTime", System.DateTime.Now.ToString("dd-HH-mm"));
    }
}
