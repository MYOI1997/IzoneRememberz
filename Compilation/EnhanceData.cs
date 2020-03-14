using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnhanceData : MonoBehaviour
{
    public string Key;
    public GetCardInformation Instance;

    Button TargetButton;

    void Start()
    {
        TargetButton = gameObject.GetComponent<Button>();

        TargetButton.onClick.AddListener(EnhanceEvent);
    }

    private void EnhanceEvent()
    {
        Instance.SelectedEnhanceCard = Key;
        Instance.EnhanceApplyButton.interactable = true;
    }
}
