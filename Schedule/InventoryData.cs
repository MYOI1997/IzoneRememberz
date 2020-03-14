using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryData : MonoBehaviour
{
    public int InventoryIndex;
    public string Key;

    Button TargetButton;

    Schedule ScheduleManager;

    void Start()
    {
        ScheduleManager = GameObject.Find("Schedule Manager").GetComponent<Schedule>();

        TargetButton = gameObject.GetComponent<Button>();

        TargetButton.onClick.AddListener(InventoryEvent);
        TargetButton.onClick.AddListener(ScheduleManager.SwapCard);
    }

    public void InventoryEvent()
    {
        ScheduleManager.ChangeCardData = InventoryIndex;
        ScheduleManager.CardSelect = true;
    }
}
