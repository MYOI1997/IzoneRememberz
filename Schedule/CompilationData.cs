using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompilationData : MonoBehaviour
{
    public int CompilationIndex;
    public string Key;

    Button TargetButton;
    
    Schedule ScheduleManager;

    void Start()
    {
        TargetButton = gameObject.GetComponent<Button>();
        ScheduleManager = GameObject.Find("Schedule Manager").GetComponent<Schedule>();

        TargetButton.onClick.AddListener(CompilationEvent);
    }

    public void CompilationEvent()
    {
        ScheduleManager.TargetIndex = CompilationIndex;
    }
}
