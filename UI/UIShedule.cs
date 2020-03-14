using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShedule : UISetting
{
    void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Transform TargetCanvas = GameObject.Find("Schedule Window").GetComponent<Transform>();

        if(TargetCanvas != null)
        {
            gameObject.transform.SetParent(TargetCanvas);
        }
    }
}
