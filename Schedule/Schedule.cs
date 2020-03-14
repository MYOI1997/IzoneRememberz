using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Schedule : MonoBehaviour
{
    [SerializeField]
    private List<StageData> StageList = new List<StageData>();

    /* Stage 패널, Schedule 패널 */
    public GameObject StageUI;
    public GameObject ScheduleUI;
    public GameObject CompilationPanel;

    public GameObject CompilationListUI;
    public GameObject CompilationListParentUI;

    public GameObject CompilationPartyUI;
    public GameObject SmallCompilationPartyUI;

    public GameObject ScheduleWindowUI;

    /* 스테이지 정보 */
    List<Dictionary<string, object>> StringData;

    private int SelectedStageNumber;

    /* 편성 리스트, 캐릭터 리스트 */
    private CharacterInventory Inventory;

    public List<GachaData> CharacterList = new List<GachaData>();
    public List<GameObject> TempCharacterList = new List<GameObject>();

    /* 카드를 교체하기 위해 선택된 카드 숫자 */
    public int TargetIndex;
    public int ChangeCardData;
    private int IndexData;

    public bool CardSelect = false;
    bool OverLapFlag = false;
    bool DeleteFlag = false;

    /* 스테이지 정보 구성에 필요한 오브젝트 */
    [SerializeField]
    Image TitleImage;
    [SerializeField]
    Image PropertyIcon;
    [SerializeField]
    Text PropertyPointText;
    [SerializeField]
    Text StageLevelText;
    [SerializeField]
    Text CompilationText;
    [SerializeField]
    Image PropertyLevelImage;
    [SerializeField]
    Slider PointSlider;

    [SerializeField]
    Image[] TitleImageList;
    [SerializeField]
    Button[] BigCompilationImageList;
    [SerializeField]
    Button[] SmallCompilationImageList;

    /* 카드 슬롯 프리팹 */
    [SerializeField]
    private GameObject SmallCompilationCard;
    [SerializeField]
    private GameObject BigCompilationCard;
    [SerializeField]
    private GameObject HugeCompilationCard;
    [SerializeField]
    private Sprite PanelPrefab;

    /* 스케줄을 실행하기 위해서 필요한 변수 */
    [SerializeField]
    private bool SkipBool = false;
    private Toggle SkipToggle;

    private int IZPower;
    [SerializeField]
    Outline[] SelectedIZPower;
    [SerializeField]
    Image[] SelectScheduleImage;
    [SerializeField]
    Image[] SelectedScheduleImage;

    private bool ScheduleStart = false;
    [SerializeField]
    private bool[] CardSelectedBool = new bool[3];
    [SerializeField]
    private bool[] SheduleProcessBool = new bool[3];

    private int NowPoint = 0;

    /* 편성 카드 중 스케줄에 필요한 5장의 카드 리스트 */
    [SerializeField]
    List<int> SelectedCard = new List<int>();

    /* 카드 점수를 계산하기 위한 리스트 */
    List<string> SelectedProprty = new List<string>();
    List<string> SelectedName = new List<string>();
    List<string> SelectedImage = new List<string>();

    /* 배치된 카드 리스트 */
    List<string> SelectProprty = new List<string>();
    List<string> SelectName = new List<string>();
    List<string> SelectImage = new List<string>();

    /* 점수 계산을 위한 bool 리스트 */
    bool[] PointCalcBool = { false, false, false };

    UI.UISet ProcessList = UI.UISet.First;

    private Text NowPointText;

    void Awake()
    {
        TitleImage = GameObject.Find("Character").transform.Find("Panel").transform.Find("ScheduleImage").GetComponent<Image>();
        PropertyIcon = GameObject.Find("Character").transform.Find("Panel").transform.Find("Property Icon").GetComponent<Image>();
        PropertyPointText = GameObject.Find("Character").transform.Find("Panel").transform.Find("Slider").transform.Find("PropertyPoint Text").GetComponent<Text>();
        StageLevelText = GameObject.Find("Character").transform.Find("Panel").transform.Find("Schedule Level").transform.Find("Text").GetComponent<Text>();
        CompilationText = GameObject.Find("Character").transform.Find("Panel").transform.Find("TeamList").transform.Find("Point Text Area").transform.Find("Text").GetComponent<Text>();
        PropertyLevelImage = GameObject.Find("Character").transform.Find("Panel").transform.Find("Schedule Level").GetComponent<Image>();
        PointSlider = GameObject.Find("Character").transform.Find("Panel").transform.Find("Slider").GetComponent<Slider>();
        SkipToggle = GameObject.Find("Character").transform.Find("Panel").transform.Find("Skip").GetComponent<Toggle>();

        SkipToggle.onValueChanged.AddListener(SkipToggleListener);

        TitleImageList = GameObject.Find("Stage").transform.Find("Panel").transform.Find("Scroll View").transform.Find("Viewport").transform.Find("Content").GetComponentsInChildren<Image>();
        BigCompilationImageList = GameObject.Find("Compilation").transform.Find("Panel").transform.Find("Compilation View").GetComponentsInChildren<Button>();
        SmallCompilationImageList = GameObject.Find("Character").transform.Find("Panel").transform.Find("TeamList").transform.Find("Small Compilation Panel").GetComponentsInChildren<Button>();

        Inventory = GameObject.Find("CharacterInventory").GetComponent<CharacterInventory>();

        SelectedIZPower = GameObject.Find("Character").transform.Find("Panel").transform.Find("Boost").transform.Find("Panel").GetComponentsInChildren<Outline>();
        NowPointText = GameObject.Find("Schedule Window").transform.Find("Card Panel").transform.Find("Now Point").transform.Find("Text").GetComponent<Text>();

        IndexData = 0;
        IZPower = 1;

        SetIZPower(IZPower);
    }

    private void SkipToggleListener(bool Value)
    {
        SkipBool = Value;
    }

    public void ScheduleLoad(string FileName)
    {
        List<Dictionary<string, object>> StringData = CSVReader.AssetRead(FileName);

        for(int i = 0; i < StringData.Count; i++)
        {
            StageData TempData = new StageData();

            TempData.Index = int.Parse(StringData[i]["Index"].ToString());
            TempData.Level = int.Parse(StringData[i]["Level"].ToString());
            TempData.Property = StringData[i]["Property"].ToString();
            TempData.Point = int.Parse(StringData[i]["Point"].ToString());
            TempData.Exp = int.Parse(StringData[i]["Exp"].ToString());
            TempData.Jewel = int.Parse(StringData[i]["Jewel"].ToString());

            StageList.Add(TempData);
        }
    }

    public void SetSchedule(int Index)
    {
        SelectedStageNumber = Index;

        ScheduleLoad("StageData");

        StageUI.gameObject.SetActive(false);
        ScheduleUI.gameObject.SetActive(true);

        if (TitleImageList[Index] != null)
        {
            TitleImage.overrideSprite = TitleImageList[Index].sprite as Sprite;
        }

        if (StageList[SelectedStageNumber].Property != null)
        {
            switch (StageList[SelectedStageNumber].Property)
            {
                case "Red":
                    PropertyIcon.sprite = Resources.Load("Sprites/UI/StageSelect/Red Icon", typeof(Sprite)) as Sprite;
                    PropertyLevelImage.color = new Color(255 / 255f, 124 / 255f, 121 / 255f);
                    break;
                case "Orange":
                    PropertyIcon.sprite = Resources.Load("Sprites/UI/StageSelect/Orange Icon", typeof(Sprite)) as Sprite;
                    PropertyLevelImage.color = new Color(255 / 255f, 151 / 255f, 0);
                    break;
                case "Green":
                    PropertyIcon.sprite = Resources.Load("Sprites/UI/StageSelect/Green Icon", typeof(Sprite)) as Sprite;
                    PropertyLevelImage.color = new Color(67 / 255f, 251 / 255f, 24 / 255f);
                    break;
            }
        }

        PointSlider.maxValue = StageList[SelectedStageNumber].Point;
        PointSlider.value = CalPlayerPoint();

        SetScheduleText();

        NewChangeComilationCard(SmallCompilationPartyUI, SmallCompilationImageList);
    }

    public void OffSchedule()
    {
        SetScheduleText();

        ScheduleUI.gameObject.SetActive(false);
        StageUI.gameObject.SetActive(true);
    }

    public void OpenCompilationTap()
    {
        SetScheduleText();

        ScheduleUI.gameObject.SetActive(false);
        StageUI.gameObject.SetActive(false);
        CompilationPanel.gameObject.SetActive(true);

        NewChangeComilationCard(CompilationPartyUI, BigCompilationImageList);
    }

    public void OpenCompilationListTap()
    {
        SetScheduleText();

        CompilationPartyUI.gameObject.SetActive(false);
        CompilationListUI.gameObject.SetActive(true);

        SetInventoryCardData();
    }

    public void CloseCompilationTap()
    {
        SetScheduleText();

        /* 중복 카드가 없고, 카드가 선택된 경우 편성창에 카드를 추가하거나 교체한다. */
        if (!OverLapFlag && CardSelect)
        {
            if (DeleteFlag)
            {
                Debug.Log("카드 삭제");

                ClearCardImage(TargetIndex);
                ClearCardImage(Inventory.CompilationList.Count - 1);

                Inventory.CompilationList.RemoveAt(TargetIndex);
            }
            else if (CardSelect)
            {
                Debug.Log("카드 추가");

                Inventory.CompilationList.Add(Inventory.CharacterList[ChangeCardData]);
            }
            else
            {
                Debug.Log("카드 교환");

                Inventory.CompilationList.RemoveAt(TargetIndex);

                Inventory.CompilationList.Add(Inventory.CharacterList[ChangeCardData]);
            }

            TargetIndex = 0;
            ChangeCardData = 0;
            CardSelect = false;
        }

        if (CompilationPartyUI.activeInHierarchy == false)
        {
            NewChangeComilationCard(CompilationPartyUI, BigCompilationImageList);

            CompilationPartyUI.gameObject.SetActive(true);
            CompilationListUI.gameObject.SetActive(false);
        }
        else
        {
            NewChangeComilationCard(SmallCompilationPartyUI, SmallCompilationImageList);

            ScheduleUI.gameObject.SetActive(true);
            StageUI.gameObject.SetActive(false);
            CompilationPanel.gameObject.SetActive(false);
        }

        SetSchedule(SelectedStageNumber);
    }

    public void NewChangeComilationCard(GameObject Parent, Button[] TargetList)
    {
        /* 이전에 사용한 TempCharacterList 및 IndexData 초기화 */
        for (int i = 0; i < TempCharacterList.Count; i++)
        {
            Destroy(TempCharacterList[i]);
        }

        TempCharacterList.Clear();
        IndexData = 0;

        /* 인벤토리를 순회하며 카드에 맞는 이미지로 변경한다 */
        for (int i = 0; i < Inventory.CompilationList.Count; i++)
        {
            Image[] ImageList = TargetList[i].GetComponentsInChildren<Image>();

            for (int j = 0; j < ImageList.Length; j++)
            {
                if (ImageList[j].name == "Property Image")
                {
                    switch (Inventory.CompilationList[i].Property)
                    {
                        case "Red": ImageList[j].sprite = Resources.Load("Sprites/UI/StageSelect/Red Icon", typeof(Sprite)) as Sprite; break;
                        case "Orange": ImageList[j].sprite = Resources.Load("Sprites/UI/StageSelect/Orange Icon", typeof(Sprite)) as Sprite; break;
                        case "Green": ImageList[j].sprite = Resources.Load("Sprites/UI/StageSelect/Green Icon", typeof(Sprite)) as Sprite; break;
                        default: break;
                    }
                }

                if (ImageList[j].name == "Rank")
                {
                    switch (Inventory.CompilationList[i].Rank)
                    {
                        case CardRank.S: ImageList[j].sprite = Resources.Load("Sprites/Compilation/S Rank", typeof(Sprite)) as Sprite; break;
                        case CardRank.A: ImageList[j].sprite = Resources.Load("Sprites/Compilation/A Rank", typeof(Sprite)) as Sprite; break;
                        case CardRank.B: ImageList[j].sprite = Resources.Load("Sprites/Compilation/B Rank", typeof(Sprite)) as Sprite; break;
                        default: ImageList[j].sprite = PanelPrefab; break;
                    }
                }

                if (ImageList[j].name == "Card Image")
                {
                    ImageList[j].sprite = Resources.Load(Inventory.CompilationList[i].Image, typeof(Sprite)) as Sprite;
                }

            }
        }

        /* 부모 오브젝트가 Compilation View일 경우 편성창을 여는 버튼 기능을 추가한다 */
        for (int i = 0; i < TargetList.Length; i++)
        {
            if (Parent.name == "Compilation View")
            {
                Button TempButton = TargetList[i].GetComponent<Button>();
                TempButton.onClick.AddListener(OpenCompilationListTap);

                CompilationData TempData = TargetList[i].GetComponent<CompilationData>();
                TempData.CompilationIndex = IndexData++;
            }
        }
    }

    public void ClearCardImage(int TargetIndex)
    {
        Image[] SamllTargetImageList = SmallCompilationImageList[TargetIndex].GetComponentsInChildren<Image>();
        Image[] BigTargetImageList = BigCompilationImageList[TargetIndex].GetComponentsInChildren<Image>();

        SamllTargetImageList[0].sprite = PanelPrefab as Sprite;
        SamllTargetImageList[1].sprite = PanelPrefab as Sprite;
        SamllTargetImageList[2].sprite = PanelPrefab as Sprite;
        SamllTargetImageList[3].sprite = PanelPrefab as Sprite;

        BigTargetImageList[0].sprite = PanelPrefab as Sprite;
        BigTargetImageList[1].sprite = PanelPrefab as Sprite;
        BigTargetImageList[2].sprite = PanelPrefab as Sprite;
        BigTargetImageList[3].sprite = PanelPrefab as Sprite;
    }

    public void SetInventoryCardData()
    {
        /* 이전에 사용한 TempCharacterList 및 IndexData 초기화 */
        for (int i = 0; i < TempCharacterList.Count; i++)
        {
            Destroy(TempCharacterList[i]);
        }

        IndexData = 0;
        TempCharacterList.Clear();

        /* 인벤토리를 순회하며 각 카드에 맞는 이미지로 변경한다. */
        for (int i = 0; i < Inventory.CharacterList.Count; i++)
        {
            GameObject TempCard = Instantiate(HugeCompilationCard) as GameObject;
            TempCard.transform.SetParent(CompilationListParentUI.transform);

            Image[] ImageList = TempCard.GetComponentsInChildren<Image>();

            for (int j = 0; j < ImageList.Length; j++)
            {
                if (ImageList[j].name == "Property Image")
                {
                    switch (Inventory.CharacterList[i].Property)
                    {
                        case "Red": ImageList[j].sprite = Resources.Load("Sprites/UI/StageSelect/Red Icon", typeof(Sprite)) as Sprite; break;
                        case "Orange": ImageList[j].sprite = Resources.Load("Sprites/UI/StageSelect/Orange Icon", typeof(Sprite)) as Sprite; break;
                        case "Green": ImageList[j].sprite = Resources.Load("Sprites/UI/StageSelect/Green Icon", typeof(Sprite)) as Sprite; break;
                        default: break;
                    }
                }

                if (ImageList[j].name == "Rank")
                {
                    switch (Inventory.CharacterList[i].Rank)
                    {
                        case CardRank.S: ImageList[j].sprite = Resources.Load("Sprites/Compilation/S Rank", typeof(Sprite)) as Sprite; break;
                        case CardRank.A: ImageList[j].sprite = Resources.Load("Sprites/Compilation/A Rank", typeof(Sprite)) as Sprite; break;
                        case CardRank.B: ImageList[j].sprite = Resources.Load("Sprites/Compilation/B Rank", typeof(Sprite)) as Sprite; break;
                        default: ImageList[j].sprite = null; break;
                    }
                }

                if (ImageList[j].name == "Card Image")
                {
                    ImageList[j].sprite = Resources.Load(Inventory.CharacterList[i].Image, typeof(Sprite)) as Sprite;
                }
            }

            InventoryData TempData = TempCard.GetComponent<InventoryData>();
            TempData.InventoryIndex = IndexData++;
            TempData.Key = Inventory.CharacterList[i].Key;

            TempCharacterList.Add(TempCard);
        }

        /* 현재 편성된 카드들에 OutLine을 추가해준다 */
        for (int i = 0; i < TempCharacterList.Count; i++)
        {
            for (int j = 0; j < Inventory.CompilationList.Count; j++)
            {
                InventoryData TempData = TempCharacterList[i].GetComponent<InventoryData>();

                if (Inventory.CompilationList[j].Key == TempData.Key)
                {
                    Image TempImage = TempData.transform.GetChild(0).GetComponent<Image>();
                    Outline TempLine = TempImage.gameObject.AddComponent<Outline>();

                    TempLine.effectColor = new Color(255, 0, 0, 255);
                    TempLine.effectDistance = new Vector2(3, -3);

                    break;
                }
            }
        }
    }

    public void SwapCard()
    {
        /* 이전에 사용한 Bool값 초기화 */
        OverLapFlag = false;
        DeleteFlag = false;

        /* 편상창에 중복된 카드가 있는지 확인 */
        for (int i = 0; i < Inventory.CompilationList.Count; i++)
        {
            if (Inventory.CharacterList[ChangeCardData].Image == Inventory.CompilationList[i].Image)
            {
                if (Inventory.CharacterList[ChangeCardData].Key != Inventory.CompilationList[i].Key)
                {
                    Debug.Log("중복");

                    OverLapFlag = true;
                }
            }
        }

        for (int i = 0; i < Inventory.CompilationList.Count; i++)
        {
            if (Inventory.CharacterList[ChangeCardData].Image == Inventory.CompilationList[i].Image)
            {
                if (Inventory.CharacterList[ChangeCardData].Key == Inventory.CompilationList[i].Key)
                {
                    Debug.Log("중복");

                    OverLapFlag = true;
                }
            }
        }

        if ((Inventory.CharacterList.Count > ChangeCardData) && (Inventory.CompilationList.Count > TargetIndex))
        {
            if (Inventory.CharacterList[ChangeCardData].Key == Inventory.CompilationList[TargetIndex].Key)
            {
                Debug.Log("삭제");

                DeleteFlag = true;
                OverLapFlag = false;
            }
        }

        /* 중복값이 없는 경우 편성창을 닫는다. */
        if (!OverLapFlag)
        {
            CloseCompilationTap();
        }
    }

    private void SetScheduleText()
    {
        PropertyPointText.text = CalPlayerPoint().ToString() + " / " + StageList[SelectedStageNumber].Point;
        StageLevelText.text = "LV." + StageList[SelectedStageNumber].Level;
        CompilationText.text = CalPlayerPoint().ToString();
    }

    private int CalPlayerPoint()
    {
        int PlayerPoint = 0;

        for (int i = 0; i < Inventory.CompilationList.Count; i++)
        {
            if(Inventory.CompilationList[i].Property == StageList[SelectedStageNumber].Property)
            {
                PlayerPoint += Inventory.CompilationList[i].Power;
            }
            else
            {
                PlayerPoint += (Inventory.CompilationList[i].Power / 2);
            }
        }

        return PlayerPoint;
    }

    public void SetIZPower(int Index)
    {
        SelectedIZPower[IZPower - 1].effectColor = new Color(0, 0, 0, 0);

        IZPower = Index;

        SelectedIZPower[IZPower - 1].effectColor = Color.red;
    }

    public void StartShedule()
    {
        /* 스케줄 알고리즘 */
        /* 1. 우선 AP가 선택된 IZPower 이상 있는지 확인 */
        /* 2. 캐릭터 포인트가 많다면 성공, 아니면 실패 출력 */

        if(SkipBool)
        {
            if(IZPower <= ResourceManager.Instance.GetAP())
            {
                ResourceManager.Instance.SetAP(IZPower);

                if(CalPlayerPoint() >= StageList[SelectedStageNumber].Point)
                {
                    Debug.Log("점수 달성 성공 !");

                    StageList[SelectedStageNumber].Level++;
                    StageList[SelectedStageNumber].Point += 500;

                    /* 쥬얼, 플레이어 경험치 보상, 레벨업 확인 */
                    ResourceManager.Instance.PlusJewel(StageList[SelectedStageNumber].Jewel);
                    ResourceManager.Instance.SetExp(StageList[SelectedStageNumber].Exp);
                    ResourceManager.Instance.CanLevelUp();

                    /* 카드 경험치 보상, 레벨업 확인 */
                    for (int i = 0; i < CharacterInventory.Instance.CompilationList.Count; i++)
                    {
                        Debug.Log("경험치 분배중");
                        CharacterInventory.Instance.CompilationList[i].NowExp += StageList[SelectedStageNumber].Exp;
                        CharacterInventory.Instance.CompilationList[i].LevelUpCheck();
                    }

                    ResultProcess(true);
                }
                else
                {
                    ResultProcess(false);
                }

                StageDataSave();

                SetSchedule(SelectedStageNumber);
            }
        }
        else
        {
            if (IZPower <= ResourceManager.Instance.GetAP())
            {
                ResourceManager.Instance.SetAP(IZPower);

                ScheduleWindowUI.gameObject.SetActive(true);

                while (SelectedCard.Count < 3)
                {
                    SelectedCard.Add(Random.Range(0, Inventory.CompilationList.Count));

                    SelectedCard.Distinct();
                }

                NowPointText.text = NowPoint.ToString() + " / " + StageList[SelectedStageNumber].Point.ToString();

                StartCoroutine(SheduleProcessUI(ProcessList));
                ProcessList++;

                SelectScheduleImage = GameObject.Find("Schedule Window").transform.Find("Card Panel").transform.Find("Select Panel").GetComponentsInChildren<Image>();
                SelectedScheduleImage = GameObject.Find("Schedule Window").transform.Find("Card Panel").transform.Find("Selected Panel").GetComponentsInChildren<Image>();

                for (int i = 0; i < 5; i++)
                {
                    int RandomIndex = Random.Range(SelectedCard[0], SelectedCard[2]);

                    SelectScheduleImage[i + 1].sprite = Resources.Load(Inventory.CompilationList[RandomIndex].Image, typeof(Sprite)) as Sprite;

                    SelectProprty.Add(Inventory.CompilationList[RandomIndex].Property);
                    SelectName.Add(Inventory.CompilationList[RandomIndex].Name);
                    SelectImage.Add(Inventory.CompilationList[RandomIndex].Image);
                }

                ScheduleStart = true;
            }
        }
    }

    void Update()
    {
        if (ScheduleStart)
        {
            /* 3차례 스케줄을 진행한 경우 */
            if (SheduleProcessBool[2] == true)
            {
                ScheduleWindowUI.gameObject.SetActive(false);

                /* 스케줄에 사용한 변수 초기화 */
                CardSelectedBool = new bool[3];
                SheduleProcessBool = new bool[3];
                ScheduleStart = false;
                SelectedCard.Clear();
                ProcessList = 0;
                SelectProprty.Clear();
                SelectName.Clear();
                SelectImage.Clear();
                SelectedProprty.Clear();
                SelectedName.Clear();
                SelectedImage.Clear();

                if (NowPoint >= StageList[SelectedStageNumber].Point)
                {
                    Debug.Log("점수 달성 성공 !");

                    StageList[SelectedStageNumber].Level++;
                    StageList[SelectedStageNumber].Point += 500;

                    /* 쥬얼, 플레이어 경험치 보상, 레벨업 확인 */
                    ResourceManager.Instance.PlusJewel(StageList[SelectedStageNumber].Jewel);
                    ResourceManager.Instance.SetExp(StageList[SelectedStageNumber].Exp);
                    ResourceManager.Instance.CanLevelUp();

                    /* 카드 경험치 보상, 레벨업 확인 */
                    for (int i = 0; i < Inventory.CompilationList.Count; i++)
                    {
                        Debug.Log("경험치 분배중");
                        Inventory.CompilationList[i].NowExp += StageList[SelectedStageNumber].Exp;
                        Inventory.CompilationList[i].LevelUpCheck();
                    }

                    Debug.Log("경험치 분배 완료");

                    ResultProcess(true);
                }
                else
                {
                    Debug.Log("점수 달성 실패 !");

                    ResultProcess(false);
                }

                StageDataSave();

                SetSchedule(SelectedStageNumber);
            }

            /* 카드가 세장 선택된 경우 */
            if (CardSelectedBool[2] == true)
            {
                /* 획득 점수 판단 */
                PointCalc();

                Debug.Log(NowPoint + " / " + StageList[SelectedStageNumber].Point);
                NowPointText.text = NowPoint.ToString() + " / " + StageList[SelectedStageNumber].Point.ToString();

                /* 상호작용이 불가능해진 버튼들을 초기화 */
                for(int i = 1; i < SelectScheduleImage.Length; i++)
                {
                    Button TargetButton = SelectScheduleImage[i].gameObject.GetComponent<Button>();
                    TargetButton.interactable = true;
                }

                SelectProprty.Clear();
                SelectName.Clear();
                SelectImage.Clear();

                SelectedProprty.Clear();
                SelectedName.Clear();
                SelectedImage.Clear();

                PointCalcBool[0] = false;
                PointCalcBool[1] = false;
                PointCalcBool[2] = false;

                for (int i = 0; i < SheduleProcessBool.Length; i++)
                {
                    if (SheduleProcessBool[i] == false)
                    {
                        SheduleProcessBool[i] = true;
                        CardSelectedBool = new bool[3];

                        if (ProcessList <= UI.UISet.Last + 1)
                        {
                            if(ProcessList != UI.UISet.Last + 1)
                            {
                                StartCoroutine(SheduleProcessUI(ProcessList));
                                ProcessList++;
                            }
                        }

                        for (int j = 0; j < SelectedScheduleImage.Length; j++)
                        {
                            SelectedScheduleImage[j].sprite = PanelPrefab as Sprite;
                        }

                        SelectedCard.Clear();

                        /* 카드 중복 제거 하면서 생성될 카드 리스트 생성 */
                        while (SelectedCard.Count < 5)
                        {
                            SelectedCard.Add(Random.Range(0, Inventory.CompilationList.Count));

                            SelectedCard.Distinct();
                        }


                        for (int j = 0; j < 5; j++)
                        {
                            int RandomIndex = Random.Range(SelectedCard.Min(), SelectedCard.Max());

                            SelectScheduleImage[j + 1].sprite = Resources.Load(Inventory.CompilationList[RandomIndex].Image, typeof(Sprite)) as Sprite;

                            SelectProprty.Add(Inventory.CompilationList[RandomIndex].Property);
                            SelectName.Add(Inventory.CompilationList[RandomIndex].Name);
                            SelectImage.Add(Inventory.CompilationList[RandomIndex].Image);
                        }

                        break;
                    }
                }
            }
        }
    }

    /* Index는 0 ~ 4 */
    public void SetScheduleButton(int Index)
    {
        for (int i = 0; i < CardSelectedBool.Length; i++)
        {
            if (CardSelectedBool[i] == false)
            {
                /* 선택한 버튼의 상호작용을 불가능하게 한다 */
                Button TargetButton = SelectScheduleImage[Index + 1].gameObject.GetComponent<Button>();
                TargetButton.interactable = false;

                CardSelectedBool[i] = true;
                SelectedScheduleImage[i + 1].sprite = SelectScheduleImage[Index + 1].sprite as Sprite;
                SelectedProprty.Add(SelectProprty[Index]);
                SelectedName.Add(SelectName[Index]);
                SelectedImage.Add(SelectImage[Index]);
                break;
            }
        }
    }

    private IEnumerator SheduleProcessUI(UI.UISet TargetProcess)
    {
        UI.Instance.Load<UIShedule>(TargetProcess);

        yield return new WaitForSeconds(1.0f);

        UI.Instance.DestroyUI(TargetProcess);
    }

    /* 점수 계산 알고리즘 생각해보기 */
    private void PointCalc()
    {
        if (SelectedName[0] == SelectedName[1] && SelectedName[0] == SelectedName[2])
        {
            PointCalcBool[0] = true;
        }

        if (SelectedProprty[0] == SelectedProprty[1] && SelectedProprty[0] == SelectedProprty[2])
        {
            PointCalcBool[1] = true;
        }

        if (SelectedImage[0] == SelectedImage[1] && SelectedImage[0] == SelectedImage[2])
        {
            PointCalcBool[2] = true;
        }

        if (PointCalcBool[0] == true && PointCalcBool[1] == true && PointCalcBool[2] == true)
        {
            NowPoint += (CalPlayerPoint() * IZPower);

            return;
        }

        if ((PointCalcBool[0] == true && PointCalcBool[1] == true) || (PointCalcBool[0] == true && PointCalcBool[2] == true) || (PointCalcBool[1] == true && PointCalcBool[2]))
        {
            NowPoint += ((CalPlayerPoint() / 2) * IZPower);

            return;
        }

        NowPoint += ((CalPlayerPoint() / 5) * IZPower);
    }

    private void StageDataSave()
    {
        using (var Writer = new CsvFileWriter(Application.dataPath + "/StageData.csv"))
        {
            List<string> Columns = new List<string>() { "Index", "Level", "Property", "Point", "Exp", "Jewel"};

            Writer.WriteRow(Columns);
            Columns.Clear();

            for (int i = 0; i < StageList.Count; i++)
            {
                Columns.Add(StageList[i].Index.ToString());
                Columns.Add(StageList[i].Level.ToString());
                Columns.Add(StageList[i].Property);
                Columns.Add(StageList[i].Point.ToString());
                Columns.Add(StageList[i].Exp.ToString());
                Columns.Add(StageList[i].Jewel.ToString());

                Writer.WriteRow(Columns);
                Columns.Clear();
            }

            Writer.Dispose();
        }
    }

    private void ResultProcess(bool IsSuccess)
    {
        GameObject ResultUI = Instantiate(Resources.Load("Prefabs/UI/Result", typeof(GameObject))) as GameObject;

        UIResult Result = ResultUI.GetComponent<UIResult>();

        if (SkipBool)
        {
            Result.PointText.text = CalPlayerPoint().ToString();

            Result.GaugeSlider.maxValue = StageList[SelectedStageNumber].Point;
            Result.GaugeSlider.value = CalPlayerPoint();
        }
        else
        {
            Result.PointText.text = NowPoint.ToString();

            Result.GaugeSlider.maxValue = StageList[SelectedStageNumber].Point;
            Result.GaugeSlider.value = NowPoint;
        }

        if (IsSuccess)
        {
            Result.SuccessWhether.text = "Success!";

            Result.GetJewelText.text = "+ " + StageList[SelectedStageNumber].Jewel.ToString();
            Result.GetExpText.text = "+ " + StageList[SelectedStageNumber].Exp.ToString();
        }
        else
        {
            Result.SuccessWhether.text = "Fail!";

            Result.GetJewelText.text = "+ 0";
            Result.GetExpText.text = "+ 0";
        }

        Result.LevelCircle.fillAmount = ((float)ResourceManager.Instance.GetNowExp() / (float)ResourceManager.Instance.GetMaxExp());

        Result.LevelText.text = "LV. " + ResourceManager.Instance.GetLevel().ToString();
        Result.ExpText.text = "\n\n" + ResourceManager.Instance.GetNowExp().ToString() + " / " + ResourceManager.Instance.GetMaxExp().ToString();

        NowPoint = 0;
    }
}
