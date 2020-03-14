using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ResourceManager : MonoBehaviour
{
    private int BasicLP = 15;

    private int Level;

    [SerializeField]
    private int NowExp, MaxExp;

    private int LP, AP;
    private int MaxLP, MaxAP;

    private int Jewel;

    public static ResourceManager Instance;

    void Singleton()
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

    void Awake()
    {
        Singleton();

        Level = PlayerPrefs.GetInt("Level");

        if (PlayerPrefs.GetInt("Level") >= 1)
        {
            MaxLP = BasicLP + Level - 1;
            MaxAP = 5;
            MaxExp = Level * 100;
        }
        else
        {
            Level = 1;
            MaxLP = BasicLP;
            MaxAP = 5;
            MaxExp = Level * 100;
            Jewel = 3000;
        }

        LP = PlayerPrefs.GetInt("LP", 15);
        AP = PlayerPrefs.GetInt("AP", 5);
        Jewel = PlayerPrefs.GetInt("Jewel", 3000);
        NowExp = PlayerPrefs.GetInt("NowExp", 0);

        CheckResource();
    }

    void CheckResource()
    {
        if(LP > MaxLP)
        {
            LP = MaxLP;
        }

        if(AP > MaxAP)
        {
            AP = MaxAP;
        }
    }

    void Start()
    {
        StartCoroutine(AddLP(180)); 
        StartCoroutine(AddAP(900));
    }

    IEnumerator AddLP(float DelayTime)
    {
        yield return new WaitForSeconds(DelayTime);

        Debug.Log(LP);

        LP += 1;

        CheckResource();

        StartCoroutine(AddLP(DelayTime));
    }

    IEnumerator AddAP(float DelayTime)
    {
        yield return new WaitForSeconds(DelayTime);

        Debug.Log(AP);

        AP += 1;

        CheckResource();

        StartCoroutine(AddAP(DelayTime));
    }

    void OnApplicationQuit()
    {
        Debug.Log("종료, 현재 자원 저장중");

        PlayerPrefs.SetInt("LP", LP);
        PlayerPrefs.SetInt("AP", AP);
        PlayerPrefs.SetInt("Jewel", Jewel);
        PlayerPrefs.SetInt("NowExp", NowExp);
        PlayerPrefs.SetInt("Level", Level);

        Debug.Log("종료, 현재 자원 저장완료");
    }

    public bool CanCasting()
    {
        if(Jewel >= 300)
        {
            Jewel -= 300;

            return true;
        }

        return false;
    }

    public bool CanLevelUp()
    {
        if (NowExp > MaxExp)
        {
            NowExp = 0;
            Level += 1;

            AP = 5;
            LP = MaxLP+1;

            return true;
        }
        return false;
    }

    //아래 함수들 Get, Set 함수로 대체

    public int GetMaxLP()
    {
        return MaxLP;
    }

    public int GetMaxAP()
    {
        return MaxAP;
    }
    
    public void SetLP(int Index)
    {
        LP -= Index;
    }

    public void SetAP(int Index)
    {
        AP -= Index;
    }

    public int GetLP()
    {
        return LP;
    }

    public int GetAP()
    {
        return AP;
    }

    public int GetJewel()
    {
        return Jewel;
    }

    public void PlusJewel(int Index)
    {
        Jewel += Index;
    }

    public void SetExp(int Index)
    {
        NowExp += Index;
    }

    public int GetNowExp()
    {
        return NowExp;
    }

    public int GetMaxExp()
    {
        return MaxExp;
    }

    public int GetLevel()
    {
        return Level;
    }
}
