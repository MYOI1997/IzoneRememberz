using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceSlider : MonoBehaviour
{
    [SerializeField]
    private Slider LPBar, APBar;

    [SerializeField]
    private Text LPText, APText, JewelText;

    void Start()
    {
        LPBar.maxValue = ResourceManager.Instance.GetMaxLP();
        APBar.maxValue = ResourceManager.Instance.GetMaxAP();
    }

    void Update()
    {
        LPBar.value = ResourceManager.Instance.GetLP();
        APBar.value = ResourceManager.Instance.GetAP();

        JewelText.text = ResourceManager.Instance.GetJewel().ToString();

        LPText.text = ResourceManager.Instance.GetLP().ToString() + " / " + ResourceManager.Instance.GetMaxLP().ToString();
        APText.text = ResourceManager.Instance.GetAP().ToString() + " / " + ResourceManager.Instance.GetMaxAP().ToString();
    }
}
