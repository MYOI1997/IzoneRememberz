using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compilation : MonoBehaviour
{
    private SetCardInformation CardInformation;

    public GameObject Content;

    [SerializeField]
    private GameObject TargetSlot;

    List<GameObject> DataList = new List<GameObject>();

    void Start()
    {
        DataSetting();
    }

    public void DataSetting()
    {
        for(int i = 0; i < DataList.Count; i++)
        {
            Destroy(DataList[i].gameObject);
        }

        for (int i = 0; i < CharacterInventory.Instance.CharacterList.Count; i++)
        {
            GameObject TempCard = Instantiate(TargetSlot) as GameObject;
            TempCard.transform.SetParent(Content.transform);

            CardInformation = TempCard.GetComponent<SetCardInformation>();

            CardInformation.CardNumber = i;

            switch (CharacterInventory.Instance.CharacterList[i].Property)
            {
                case "Red": CardInformation.Property.sprite = Resources.Load("Sprites/UI/StageSelect/Red Icon", typeof(Sprite)) as Sprite; break;
                case "Orange": CardInformation.Property.sprite = Resources.Load("Sprites/UI/StageSelect/Orange Icon", typeof(Sprite)) as Sprite; break;
                case "Green": CardInformation.Property.sprite = Resources.Load("Sprites/UI/StageSelect/Green Icon", typeof(Sprite)) as Sprite; break;
                default: break;
            }

            switch (CharacterInventory.Instance.CharacterList[i].Rank)
            {
                case CardRank.S: CardInformation.Rank.sprite = Resources.Load("Sprites/Compilation/S Rank", typeof(Sprite)) as Sprite; break;
                case CardRank.A: CardInformation.Rank.sprite = Resources.Load("Sprites/Compilation/A Rank", typeof(Sprite)) as Sprite; break;
                case CardRank.B: CardInformation.Rank.sprite = Resources.Load("Sprites/Compilation/B Rank", typeof(Sprite)) as Sprite; break;
                default: break;
            }

            CardInformation.TargetImage.sprite = Resources.Load(CharacterInventory.Instance.CharacterList[i].Image, typeof(Sprite)) as Sprite;

            DataList.Add(TempCard);
        }
    }
}
