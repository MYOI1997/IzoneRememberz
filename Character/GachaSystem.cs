using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public enum SelectPickUpList { ColorIz, HeartIz, BloomIz, }

public class GachaSystem : MonoBehaviour
{

    [SerializeField]
    private List<GachaData> Deck = new List<GachaData>();

    FileInfo ColorIzInfo, HeartIzInfo, BloomIzInfo;
    LoadType Type;
    Button PickUpButton;

    private Image FirstAppearenceImage;
    private Image NotEnoughJewelImage;

    private Button CloseButton;
    private Text FirstGet;

    private Image ThemeImage;

    private Image GetImage;

    private Animator StarsController;
 
    public int Total = 0;

    SelectPickUpList SelectedPickUpList = SelectPickUpList.ColorIz;

    public GachaData Result;

    void Awake()
    {
        FirstAppearenceImage = GameObject.Find("Canvas").transform.Find("FirstAppearenceImage").GetComponent<Image>();
        StarsController = GameObject.Find("Canvas").transform.Find("Stars").GetComponent<Animator>();
        GetImage = GameObject.Find("Canvas").transform.Find("FirstAppearenceImage").transform.Find("GetImage").GetComponent<Image>();
        ThemeImage = GameObject.Find("Canvas").transform.Find("Image").transform.Find("ThemeImage").GetComponent<Image>();
        PickUpButton = GameObject.Find("Canvas").transform.Find("Image").transform.Find("Casting").GetComponent<Button>();
        NotEnoughJewelImage = GameObject.Find("Canvas").transform.Find("NotEnoughJewel").GetComponent<Image>();

        ColorIzInfo = new FileInfo(Application.dataPath + "/Resources/ColorIz.csv");
        HeartIzInfo = new FileInfo(Application.dataPath + "/Resources/HeartIz.csv");
        BloomIzInfo = new FileInfo(Application.dataPath + "/Resources/BloomIz.csv");

        if(ColorIzInfo.Exists)
        {
            CharacterDataLoad(Application.dataPath + "/Resources/ColorIz.csv", LoadType.Stream);
        }
        else
        {
            CharacterDataLoad("ColorIz", LoadType.Asset);
        }

        for (int i = 0; i < Deck.Count; i++)
        {
            Total += Deck[i].Weight;
        }
    }

    public void CharacterDataLoad(string FileName, LoadType Type)
    {
        PickUpButton.interactable = false;

        Deck.Clear();
        Total = 0;

        List<Dictionary<string, object>> StringData;

        if (Type == LoadType.Stream)
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
                case "S": TempCharacter.Rank = CardRank.S; break;
                case "A": TempCharacter.Rank = CardRank.A; break;
                case "B": TempCharacter.Rank = CardRank.B; break;
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

            Deck.Add(TempCharacter);
        }

        PickUpButton.interactable = true;
    }

    public void Return()
    {
        if(Deck.Count > 0)
        {
            if (ResourceManager.Instance.CanCasting())
            {
                Result = RandomGet();

                if (Result != null)
                {
                    CharacterInventory.Instance.CharacterList.Add(Result);

                    Debug.Log("1회 뽑기 완료");
                }
            }
            else
            {
                NotEnoughJewelImage.gameObject.SetActive(true);
                Debug.Log("자원이 부족합니다.");
            }
        }
    }

    public GachaData RandomGet()
    {
        int Weight = 0;
        int SelectNum = 0;

        SelectNum = Mathf.RoundToInt(Total * Random.Range(0.0f, 1.0f));

        Debug.Log("선택된 카드의 가중치" + SelectNum);

        for (int i = 0; i < Deck.Count; i++)
        {
            Weight += Deck[i].Weight;
            if(SelectNum <= Weight)
            {
                GachaData Temp = new GachaData(Deck[i]);

                string Key = Temp.Name + Random.Range(0, 20000);

                Temp.Key = Key;
                Temp.Lock = 1;

                FirstAppearenceImage.gameObject.SetActive(true);
                StarsController.gameObject.SetActive(true);

                Debug.Log(Temp.Image);

                GetImage.sprite = Resources.Load(Temp.Image, typeof(Sprite)) as Sprite;

                if(Temp.Appearence == 0)
                {
                    FirstGet = GameObject.Find("FirstAppearenceImage").transform.Find("FirstGet").GetComponent<Text>();             
                    FirstGet.color = new Color(255, 255, 0, 255);

                    Temp.Appearence = 1;
                    Deck[i].Appearence = 1;
                    CharacterDataSave();
                }

                switch (Temp.Rank)
                {
                    case CardRank.S     :   StarsController.Play("Five");   Debug.Log("S 랭크");  StartCoroutine(AnimationCheck("Five"));       break;
                    case CardRank.A     :   StarsController.Play("Four");   Debug.Log("A 랭크");  StartCoroutine(AnimationCheck("Four"));       break;
                    case CardRank.B     :   StarsController.Play("Three");  Debug.Log("B 랭크"); StartCoroutine(AnimationCheck("Three"));       break;
                    default: break;
                }
                return Temp;
            }
        }
        return null;
    }

    public void CharacterDataSave()
    {
        using (var Writer = new CsvFileWriter(Application.dataPath + "/Resources/" + SelectedPickUpList.ToString() + ".csv"))
        {
            List<string> Columns = new List<string>() { "Name", "Image", "Rank", "Property", "Power", "Appearence", "Weight", "Level", "MaxLevel", "NowExp", "MaxExp", "Enhance"};

            Writer.WriteRow(Columns);
            Columns.Clear();

            for (int i = 0; i < Deck.Count; i++)
            {
                Columns.Add(Deck[i].Name);
                Columns.Add(Deck[i].Image.ToString());
                Columns.Add(Deck[i].Rank.ToString());
                Columns.Add(Deck[i].Property.ToString());
                Columns.Add(Deck[i].Power.ToString());
                Columns.Add(Deck[i].Appearence.ToString());
                Columns.Add(Deck[i].Weight.ToString());
                Columns.Add(Deck[i].Level.ToString());
                Columns.Add(Deck[i].MaxLevel.ToString());
                Columns.Add(Deck[i].NowExp.ToString());
                Columns.Add(Deck[i].MaxExp.ToString());
                Columns.Add(Deck[i].Enhance.ToString());

                Writer.WriteRow(Columns);
                Columns.Clear();
            }

            Writer.Dispose();
        }
    }

    IEnumerator AnimationCheck(string AnimationName)
    {
        while (StarsController.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f)
        {
            yield return null;
        }

        CloseButton = FirstAppearenceImage.gameObject.GetComponent<Button>();
        CloseButton.onClick.AddListener(AppearenceOff);
    }

    public void AppearenceOff()
    {
        SpriteRenderer[] SpriteList = StarsController.gameObject.GetComponentsInChildren<SpriteRenderer>();

        for(int i = 0; i < SpriteList.Length; i++)
        {
            SpriteList[i].color = new Color(0, 0, 0, 255);
        }

        if(FirstGet != null)
        {
            FirstGet.color = new Color(255, 255, 0, 0);
        }

        CloseButton.onClick.RemoveAllListeners();
        FirstAppearenceImage.gameObject.SetActive(false);

        StarsController.gameObject.SetActive(false);

    }

    public void SetPickUpList(int Index)
    {
        SelectedPickUpList = (SelectPickUpList)Index;

        switch (SelectedPickUpList)
        {
            case SelectPickUpList.ColorIz   :
                ThemeImage.sprite = Resources.Load("Sprites/UI/PickUpSystem/ThemeImage/ColorIzTheme", typeof(Sprite)) as Sprite;

                if (ColorIzInfo.Exists)
                {
                    CharacterDataLoad(Application.dataPath + "/Resources/ColorIz.csv", LoadType.Stream);
                }
                else
                {
                    CharacterDataLoad("ColorIz", LoadType.Asset);
                }

                for (int i = 0; i < Deck.Count; i++)
                {
                    Total += Deck[i].Weight;
                }

                break;

            case SelectPickUpList.HeartIz   :
                ThemeImage.sprite = Resources.Load("Sprites/UI/PickUpSystem/ThemeImage/HeartIzTheme", typeof(Sprite)) as Sprite;

                if (HeartIzInfo.Exists)
                {
                    CharacterDataLoad(Application.dataPath + "/Resources/HeartIz.csv", LoadType.Stream);
                }
                else
                {
                    CharacterDataLoad("HeartIz", LoadType.Asset);
                }

                for (int i = 0; i < Deck.Count; i++)
                {
                    Total += Deck[i].Weight;
                }

                break;

            case SelectPickUpList.BloomIz   :         
                ThemeImage.sprite = Resources.Load("Sprites/UI/PickUpSystem/ThemeImage/BloomIzTheme", typeof(Sprite)) as Sprite;

                if (BloomIzInfo.Exists)
                {
                    CharacterDataLoad(Application.dataPath + "/Resources/BloomIz.csv", LoadType.Stream);
                }
                else
                {
                    CharacterDataLoad("BloomIz", LoadType.Asset);
                }

                for (int i = 0; i < Deck.Count; i++)
                {
                    Total += Deck[i].Weight;
                }

                break;

            default: break;
        }
    }
}
