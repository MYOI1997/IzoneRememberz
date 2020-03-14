using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetCardInformation : MonoBehaviour
{
    public int CardNumber;

    public Image TargetImage;
    public Image Property;
    public Image Rank;

    public Transform CardInformationPanel;
    public GetCardInformation CardManager;
    public Button BackButton;

    void Awake()
    {
        CardInformationPanel = GameObject.Find("Card Canvas").transform.Find("Card Information").GetComponent<Transform>();
        CardManager = GameObject.Find("Card Manager").GetComponent<GetCardInformation>();
        BackButton = GameObject.Find("Card Canvas").transform.Find("Card Information").transform.Find("Panel").transform.Find("Back").GetComponent<Button>();
    }

    void OnClick()
    {
        CardInformationPanel.gameObject.SetActive(true);
        CardManager.CardNumber = CardNumber;
        CardManager.Active = true;
        CardManager.TargetCard = gameObject;
    }

    void Start()
    {
        Button TargetButton = gameObject.GetComponent<Button>();
        TargetButton.onClick.AddListener(OnClick);
    }
}
