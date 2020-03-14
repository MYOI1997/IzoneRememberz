using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AcountSystem : MonoBehaviour
{
    private Text LevelText;
    private Text ExpText;

    [SerializeField]
    private Image ExpCircle;

    private int Level;
    private int NowExp;
    private int MaxExp;

    public Image CharacterImage;

    [SerializeField]
    private Image MaskImage;

    string ImageSrc;

    void Awake()
    {
        LevelText = GameObject.Find("LevelText").GetComponent<Text>();
        ExpText = GameObject.Find("ExpText").GetComponent<Text>();

        ExpCircle = GameObject.Find("Menu").transform.Find("Level").transform.Find("Circle").transform.Find("ExpCircle").GetComponent<Image>();
        CharacterImage = GameObject.Find("BackGround").transform.Find("Image").transform.Find("RawImage").transform.Find("Character").GetComponent<Image>();

        Level = ResourceManager.Instance.GetLevel();
        NowExp = ResourceManager.Instance.GetNowExp();
        MaxExp = ResourceManager.Instance.GetMaxExp();

        MaskImage = CharacterImage.transform.parent.GetComponent<Image>();

        ImageSrc = PlayerPrefs.GetString("ImageSrc", "Sprites/UI/Game Logo");

        SetImage(ImageSrc);

        Debug.Log(Application.dataPath);
    }

    void Update()
    {
        Level = ResourceManager.Instance.GetLevel();
        NowExp = ResourceManager.Instance.GetNowExp();
        MaxExp = ResourceManager.Instance.GetMaxExp();

        LevelText.text = "LV. " + Level;
        ExpText.text = "\n\n" + NowExp + " / " + MaxExp;
        ExpCircle.fillAmount = ((float)NowExp / (float)MaxExp);
    }

    public void SetImage(string Src)
    {
        CharacterImage.sprite = Resources.Load(Src, typeof(Sprite)) as Sprite;
        CharacterImage.gameObject.GetComponent<Image>().SetNativeSize();

        CharacterImage.rectTransform.anchorMin = new Vector2(0f, 0f);
        CharacterImage.rectTransform.anchorMax = new Vector2(1f, 1f);
        CharacterImage.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        CharacterImage.rectTransform.sizeDelta = new Vector2(0, 0);
    }
}
