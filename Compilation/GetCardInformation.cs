using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void EventHandler(bool ActiveBool);

public class GetCardInformation : MonoBehaviour
{
    /* 카드 정보 */
    public Image CardImage;
    public Text LevelText;
    public Slider ExpSlider;
    public Image PropertyImage;
    public Text EnhanceText;
    public Text CardName;
    public int CardNumber;
    public Image ProtectImage;

    public Button BackButton;
    public Button FireButton;
    public Button ImageChangButton;
    public Button LockButton;
    public Button EnhanceButton;

    public Button EnhanceApplyButton;
    public Button EnhanceBackButton;

    public Transform CardInformationPanel;
    public Transform EnhancePanel;
    public Transform EnhanceListPanel;

    public GameObject TargetCard;
    public GameObject TargetEnhanceCard;

    GameObject[] CardList;
    List<GameObject> EnhanceList = new List<GameObject>();

    Compilation CompilationInstance;

    public string SelectedEnhanceCard = null;

    void Awake()
    {
        CardInformationPanel = GameObject.Find("Card Canvas").transform.Find("Card Information").GetComponent<Transform>();
        EnhancePanel = GameObject.Find("Card Canvas").transform.Find("Card Information").transform.Find("Enhance Panel").GetComponent<Transform>();
        EnhanceListPanel = GameObject.Find("Card Canvas").transform.Find("Card Information").transform.Find("Enhance Panel").transform.Find("Scroll View").transform.Find("Viewport").transform.Find("Content").GetComponent<Transform>();

        CardName = GameObject.Find("Card Canvas").transform.Find("Card Information").transform.Find("Panel").transform.Find("Information Panel").transform.Find("CardName").GetComponent<Text>();

        BackButton = GameObject.Find("Card Canvas").transform.Find("Card Information").transform.Find("Panel").transform.Find("Back").GetComponent<Button>();
        FireButton = GameObject.Find("Card Canvas").transform.Find("Card Information").transform.Find("Panel").transform.Find("Fire").GetComponent<Button>();
        LockButton = GameObject.Find("Card Canvas").transform.Find("Card Information").transform.Find("Panel").transform.Find("Lock").GetComponent<Button>();
        ImageChangButton = GameObject.Find("Card Canvas").transform.Find("Card Information").transform.Find("Panel").transform.Find("Information Panel").transform.Find("Profile").GetComponent<Button>();
        EnhanceButton = GameObject.Find("Card Canvas").transform.Find("Card Information").transform.Find("Panel").transform.Find("Information Panel").transform.Find("Enhance").GetComponent<Button>();

        EnhanceApplyButton = GameObject.Find("Card Canvas").transform.Find("Card Information").transform.Find("Enhance Panel").transform.Find("Apply").GetComponent<Button>();
        EnhanceBackButton = GameObject.Find("Card Canvas").transform.Find("Card Information").transform.Find("Enhance Panel").transform.Find("Back").GetComponent<Button>();

        CompilationInstance = GameObject.Find("Canvas").transform.Find("Panel").transform.Find("Inventory").GetComponent<Compilation>();

        EnhanceApplyButton.interactable = false;
    }

    public event EventHandler CardHandler;

    bool IsActive = false;

    public bool Active
    {
        get { return IsActive; }

        set
        {
            IsActive = value;

            if(Active) { CardHandler(IsActive); }
        }
    }

    void ChangeInformation(bool IsActive)
    {
        Debug.Log("핸들러 실행됨");

        string[] FindName = CharacterInventory.Instance.CharacterList[CardNumber].Image.Split('/');

        CardName.text = "[" + FindName[1] + "] " + FindName[FindName.Length-1];

        CardImage.sprite = Resources.Load(CharacterInventory.Instance.CharacterList[CardNumber].Image, typeof(Sprite)) as Sprite;
        LevelText.text = "LV. " + CharacterInventory.Instance.CharacterList[CardNumber].Level.ToString();

        ExpSlider.maxValue = CharacterInventory.Instance.CharacterList[CardNumber].MaxExp;
        ExpSlider.value = CharacterInventory.Instance.CharacterList[CardNumber].NowExp;

        switch (CharacterInventory.Instance.CharacterList[CardNumber].Property)
        {
            case "Red": PropertyImage.sprite = Resources.Load("Sprites/UI/StageSelect/Red Icon", typeof(Sprite)) as Sprite; break;
            case "Orange": PropertyImage.sprite = Resources.Load("Sprites/UI/StageSelect/Orange Icon", typeof(Sprite)) as Sprite; break;
            case "Green": PropertyImage.sprite = Resources.Load("Sprites/UI/StageSelect/Green Icon", typeof(Sprite)) as Sprite; break;
            default: break;
        }

        EnhanceText.text = "재능 개화 :               " + CharacterInventory.Instance.CharacterList[CardNumber].Enhance.ToString();
    }

    private void CardInformationPanelOn()
    {
        CardInformationPanel.gameObject.SetActive(false);
    }

    void Start()
    {
        CardHandler += new EventHandler(ChangeInformation);

        BackButton.onClick.AddListener(CardInformationPanelOn);
        ImageChangButton.onClick.AddListener(ImageChange);
        LockButton.onClick.AddListener(Lock);
        EnhanceButton.onClick.AddListener(EnhancePanelOn);
        EnhanceBackButton.onClick.AddListener(EnhancePanelOff);
        EnhanceApplyButton.onClick.AddListener(ApplyEnhance);

        if(CharacterInventory.Instance.CharacterList[CardNumber].Lock == 1)
        {
            ProtectImage.gameObject.SetActive(true);
            FireButton.gameObject.SetActive(false);
        }
        else
        {
            ProtectImage.gameObject.SetActive(false);
            FireButton.gameObject.SetActive(true);
        }
    }

    public void Fire()
    {
        if(CharacterInventory.Instance.CharacterList[CardNumber].Lock == 1)
        {
            Debug.Log("잠금 상태를 풀어주세요!");

            return;
        }

        if(PlayerPrefs.GetString("ImageSrc") == CharacterInventory.Instance.CharacterList[CardNumber].Image)
        {
            PlayerPrefs.SetString("ImageSrc", "Sprites/UI/Game Logo");
        }

        for(int i = 0; i < CharacterInventory.Instance.CompilationList.Count; i++)
        {
            if(CharacterInventory.Instance.CharacterList[CardNumber].Key == CharacterInventory.Instance.CompilationList[i].Key)
            {
                CharacterInventory.Instance.CompilationList.Remove(CharacterInventory.Instance.CompilationList[i]);
            }
        }

        CharacterInventory.Instance.CharacterList.Remove(CharacterInventory.Instance.CharacterList[CardNumber]);

        Destroy(TargetCard);

        CompilationInstance.DataSetting();

        CardInformationPanelOn();
    }

    private void Lock()
    {
        if(CharacterInventory.Instance.CharacterList[CardNumber].Lock == 1)
        {
            CharacterInventory.Instance.CharacterList[CardNumber].Lock = -1;
            Debug.Log("보호 해체");
            ProtectImage.gameObject.SetActive(false);
            FireButton.gameObject.SetActive(true);
        }
        else
        {
            CharacterInventory.Instance.CharacterList[CardNumber].Lock = 1;
            Debug.Log("보호");
            ProtectImage.gameObject.SetActive(true);
            FireButton.gameObject.SetActive(false);
        }
    }

    private void ImageChange()
    {
        string Src = CharacterInventory.Instance.CharacterList[CardNumber].Image;

        PlayerPrefs.SetString("ImageSrc", Src);
    }

    private void EnhancePanelOn()
    {
        if(CharacterInventory.Instance.CharacterList[CardNumber].Enhance <= 4)
        {
            for (int i = 0; i < EnhanceList.Count; i++)
            {
                Destroy(EnhanceList[i]);
            }

            EnhanceList.Clear();

            EnhanceApplyButton.interactable = false;
            SelectedEnhanceCard = null;

            for (int i = 0; i < CharacterInventory.Instance.CharacterList.Count; i++)
            {
                if (CharacterInventory.Instance.CharacterList[CardNumber].Name == CharacterInventory.Instance.CharacterList[i].Name)
                {
                    if (CharacterInventory.Instance.CharacterList[CardNumber].Key != CharacterInventory.Instance.CharacterList[i].Key)
                    {
                        GameObject TempCard = Instantiate(TargetEnhanceCard) as GameObject;
                        TempCard.transform.SetParent(EnhanceListPanel.transform);

                        Image[] TempCardImageList = TempCard.GetComponentsInChildren<Image>();

                        for (int j = 0; j < TempCardImageList.Length; j++)
                        {
                            if (TempCardImageList[j].name == "Property Image")
                            {
                                switch (CharacterInventory.Instance.CharacterList[i].Property)
                                {
                                    case "Red": TempCardImageList[j].sprite = Resources.Load("Sprites/UI/StageSelect/Red Icon", typeof(Sprite)) as Sprite; break;
                                    case "Orange": TempCardImageList[j].sprite = Resources.Load("Sprites/UI/StageSelect/Orange Icon", typeof(Sprite)) as Sprite; break;
                                    case "Green": TempCardImageList[j].sprite = Resources.Load("Sprites/UI/StageSelect/Green Icon", typeof(Sprite)) as Sprite; break;
                                    default: break;
                                }
                            }

                            if (TempCardImageList[j].name == "Rank")
                            {
                                switch (CharacterInventory.Instance.CharacterList[i].Rank)
                                {
                                    case CardRank.S: TempCardImageList[j].sprite = Resources.Load("Sprites/Compilation/S Rank", typeof(Sprite)) as Sprite; break;
                                    case CardRank.A: TempCardImageList[j].sprite = Resources.Load("Sprites/Compilation/A Rank", typeof(Sprite)) as Sprite; break;
                                    case CardRank.B: TempCardImageList[j].sprite = Resources.Load("Sprites/Compilation/B Rank", typeof(Sprite)) as Sprite; break;
                                    default: break;
                                }
                            }

                            if (TempCardImageList[j].name == "Card Image")
                            {
                                TempCardImageList[j].sprite = Resources.Load(CharacterInventory.Instance.CharacterList[i].Image, typeof(Sprite)) as Sprite;
                            }

                            EnhanceData TempData = TempCard.AddComponent<EnhanceData>();
                            TempData.Key = CharacterInventory.Instance.CharacterList[i].Key;
                            TempData.Instance = this;

                            EnhanceList.Add(TempCard);
                        }
                    }
                }
            }

            EnhancePanel.gameObject.SetActive(true);
        }
    }

    private void ApplyEnhance()
    {
        if(CharacterInventory.Instance.CharacterList[CardNumber].Enhance < 4)
        {
            CharacterInventory.Instance.CharacterList[CardNumber].MaxLevel += 10;
            CharacterInventory.Instance.CharacterList[CardNumber].Enhance += 1;

            for (int i = 0; i < CharacterInventory.Instance.CharacterList.Count; i++)
            {
                if (CharacterInventory.Instance.CharacterList[i].Key == SelectedEnhanceCard)
                {
                    CharacterInventory.Instance.CharacterList.Remove(CharacterInventory.Instance.CharacterList[i]);
                }
            }

            for (int i = 0; i < CharacterInventory.Instance.CompilationList.Count; i++)
            {
                if (CharacterInventory.Instance.CompilationList[i].Key == SelectedEnhanceCard)
                {
                    CharacterInventory.Instance.CompilationList.Remove(CharacterInventory.Instance.CompilationList[i]);
                }
            }

            CompilationInstance.DataSetting();

            EnhancePanelOff();

            ChangeInformation(true);
        }      
    }

    private void EnhancePanelOff()
    {
        EnhancePanel.gameObject.SetActive(false);
    }
}
