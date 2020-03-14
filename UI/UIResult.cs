using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIResult : MonoBehaviour
{
    public Text PointText;
    public Slider GaugeSlider;
    public Image LevelCircle;
    public Text LevelText;
    public Text ExpText;
    public Text GetJewelText;
    public Text GetExpText;
    public Text SuccessWhether;

    public Button CloseButton;

    void Awake()
    {
        PointText = GameObject.Find("Result(Clone)").transform.Find("Result").transform.Find("Score").transform.Find("ScoreText").GetComponent<Text>();
        GaugeSlider = GameObject.Find("Result(Clone)").transform.Find("Result").transform.Find("Score Gauge").transform.Find("Slider").GetComponent<Slider>();
        LevelCircle = GameObject.Find("Result(Clone)").transform.Find("Result").transform.Find("Level").Find("ExpCircle").GetComponent<Image>();
        LevelText = GameObject.Find("Result(Clone)").transform.Find("Result").transform.Find("Level").Find("LevelText").GetComponent<Text>();
        ExpText = GameObject.Find("Result(Clone)").transform.Find("Result").transform.Find("Level").Find("ExpText").GetComponent<Text>();
        GetJewelText = GameObject.Find("Result(Clone)").transform.Find("Result").transform.Find("Jewel").transform.Find("Text").GetComponent<Text>();
        GetExpText = GameObject.Find("Result(Clone)").transform.Find("Result").transform.Find("EXP").transform.Find("Text").GetComponent<Text>();
        SuccessWhether = GameObject.Find("Result(Clone)").transform.Find("SuccessWhether").GetComponent<Text>();

        CloseButton = GameObject.Find("Result(Clone)").GetComponent<Button>();
    }

    void Start()
    {
        Transform TargetCanvas = GameObject.Find("Schedule Window").GetComponent<Transform>();

        if (TargetCanvas != null)
        {
            gameObject.transform.SetParent(TargetCanvas);
        }

        StartCoroutine(CloseButtonAdd());
    }

    private IEnumerator CloseButtonAdd()
    {
        yield return new WaitForSeconds(3.0f);

        CloseButton.onClick.AddListener(ButtonClick);
    }

    private void ButtonClick()
    {
        Destroy(gameObject);
    }
}
