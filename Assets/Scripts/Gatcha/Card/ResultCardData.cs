using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultCardData : MonoBehaviour
{
    [Header("Borders")]
    public Image EquipBorder;

    [Header("Equip Result")]
    public Image EquipImage;

    public GameObject New;

    public TextMeshProUGUI StoneAmount;

    public void SetCardData(EquipFrame data)
    {
        EquipImage.sprite = data.equipData.Sprite_Equip;
        EquipBorder.color = data.Border.color;

        if(New != null)
            New.SetActive(EquipManager.Instance.All_Equips.Find(x => x == data).equipData.AmountOfEquip <= 1);
    }

    public void SetCardData(UpgradeStone data, int n)
    {
        EquipImage.transform.localScale = data.stoneGrade != UpgradeStone.TypeOfStone.Ancient ? new Vector3(1, 1, 1) : new Vector3(0.75f, 0.75f, 0.75f);
        StoneAmount.text = n.ToString();
    }

    public void SetCardData(UpgradeStone data)
    {
        EquipImage.transform.localScale = data.stoneGrade != UpgradeStone.TypeOfStone.Ancient ? new Vector3(1, 1, 1) : new Vector3(0.75f, 0.75f, 0.75f);

        if(StoneAmount != null)
            StoneAmount.text = "";
    }
}
