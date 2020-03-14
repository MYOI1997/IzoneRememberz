using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public enum UISet
    {
        First,
        Second,
        Last,
        Result,
    }

    private static UI UIInstance = null;

    public static UI Instance
    {
        get
        {
            if(UIInstance == null)
            {
                GameObject Temp = new GameObject("UI Manager", typeof(UI));
                UIInstance = Temp.GetComponent<UI>();
                DontDestroyOnLoad(Temp);
            }
            return UIInstance;
        }
    }

    private Dictionary<UISet, UISetting> UIDictionary = new Dictionary<UISet, UISetting>();

    public T Get<T>(UISet TargetUI) where T : UISetting
    {
        if(UIDictionary.ContainsKey(TargetUI))
        {
            return UIDictionary[TargetUI].GetComponent<T>();
        }

        return null;
    }

    public void DestroyUI(UISet TargetUI)
    {
        if(UIDictionary.ContainsKey(TargetUI))
        {
            Destroy(UIDictionary[TargetUI].gameObject);
            UIDictionary.Remove(TargetUI);
        }
    }

    public T Load<T>(UISet TargetUI) where T : UISetting
    {
        T Target = Get<T>(TargetUI);

        if(Target != null ) { return Target; }

        string Path = "Prefabs/UI/" + TargetUI.ToString();

        Debug.Log(Path);

        T Temp = Resources.Load(Path, typeof(T)) as T;

        Target = GameObject.Instantiate<T>(Temp, Vector3.zero, Quaternion.identity);

        if(Target != null)
        {
            Target.Init();
            UIDictionary.Add(TargetUI, Target);
        }

        return Temp;
    }
}
