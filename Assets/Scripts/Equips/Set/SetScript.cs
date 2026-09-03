using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SetScript : MonoBehaviour
{
    public TextMeshProUGUI SetTitleText;
    public TextMeshProUGUI[] StatsText;

    public List<SetEquip> setEquips;
    public Transform parent;

    public EquipsOption option;

    public List<Equips.SetType> setData;

    public void setInterface(string SetName, List<Equips> datas, List<int> equipState)
    {
        SetTitleText.text = SetName;

        for(int i = 0; i < setEquips.Count; i++) {
            setEquips[i].Game_Object.SetActive(false);
        }
        for(int i = 0; i < datas.Count; i++) {
            setEquips[i].Game_Object.SetActive(true);
            setEquips[i].equip = datas[i];
        //    setEquips[i].Border_Image.color = datas[i].Border.color;
       //     setEquips[i].EquipImage.sprite = datas[i].ItemImage.sprite;
            setEquips[i].Background_Ancient.SetActive(datas[i].Grade == Equips.MaterialClass.Ancient);
            setEquips[i].Background_Legendary.SetActive(datas[i].Grade == Equips.MaterialClass.Legendary);
            setEquips[i].Background_Unique.SetActive(datas[i].Grade == Equips.MaterialClass.Unique);
            setEquips[i].Background_Epic.SetActive(datas[i].Grade == Equips.MaterialClass.Epic);
            setEquips[i].Background_Rare.SetActive(datas[i].Grade == Equips.MaterialClass.Rare);
        }
        for(int i = 0; i < equipState.Count; i++) {
            setEquipState(setEquips[i], equipState[i]);
        }

        if(datas != null)
        EquipManager.Instance.setSetEffect(datas[0].setType, option);

        for(int i = 0; i < 4; i++) {
            StatsText[i].text = "";
        }

        string originalText = SetEquipStatsText();

        string[] stringArray = originalText.Split('@');

        for(int i = 0; i < stringArray.Length; i++) {
            StatsText[i].text = stringArray[i].Trim();
        }

        ReorderQuests();
    }

    public void setEquipState(SetEquip se, int i)
    {
        switch(i) {
            case 1:
                se.Border_Image.color = new Color(se.Border_Image.color.r, se.Border_Image.color.g, se.Border_Image.color.b, 0.5f);
                se.Background_Ancient.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                se.Background_Legendary.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                se.Background_Unique.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                se.Background_Epic.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                se.Background_Rare.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                se.EquipImage.color = Color.black;
                break;
            case 2:
                se.Border_Image.color = new Color(se.Border_Image.color.r, se.Border_Image.color.g, se.Border_Image.color.b, 0.5f);
                se.Background_Ancient.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                se.Background_Legendary.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                se.Background_Unique.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                se.Background_Epic.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                se.Background_Rare.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                se.EquipImage.color = new Color(1, 1, 1, 0.5f);
                break;
            case 3:
                se.Border_Image.color = new Color(se.Border_Image.color.r, se.Border_Image.color.g, se.Border_Image.color.b);
                se.Background_Ancient.GetComponent<Image>().color = Color.white;
                se.Background_Legendary.GetComponent<Image>().color = Color.white;
                se.Background_Unique.GetComponent<Image>().color = Color.white;
                se.Background_Epic.GetComponent<Image>().color = Color.white;
                se.Background_Rare.GetComponent<Image>().color = Color.white;
                se.EquipImage.color = Color.white;
                break;
        }
    }

    public string SetEquipStatsText()
    {
        string EquipStatsText = "";
        if(option.option.damage != 0)
            EquipStatsText += string.Format("데미지: {0}@", option.option.damage);
        if(option.option.damagePercent != 0)
            EquipStatsText += string.Format("데미지(%): {0}%@", option.option.damagePercent * 100);
        if(option.option.AttackSpeed != 0)
            EquipStatsText += string.Format("공격속도: {0}%@", option.option.AttackSpeed * 100);
        if(option.option.BulletSpeed != 0)
            EquipStatsText += string.Format("총알속도: {0}@", option.option.BulletSpeed) + "m/s";
        if(option.option.CritChance != 0)
            EquipStatsText += string.Format("치명타 확률: {0}%@", option.option.CritChance);
        if(option.option.CritDamage != 0)
            EquipStatsText += string.Format("치명타 데미지: {0}%@", option.option.CritDamage * 100);
        if(option.option.StunPercent != 0)
            EquipStatsText += string.Format("스턴 확률: {0}%@", option.option.StunPercent);
        if(option.option.ExtraEXP != 0)
            EquipStatsText += string.Format("추가 경험치(%): {0}%@", Mathf.Round(option.option.ExtraEXP * 100 * 10f) / 10f);
        if(option.option.ExtraMoney != 0)
            EquipStatsText += string.Format("추가 골드(%): {0}%@", Mathf.Round(option.option.ExtraMoney * 100 * 10f) / 10f);

        return EquipStatsText;
    }

    public void ReorderQuests()
    {
        setEquips.Sort((a, b) => {
            if(a.Game_Object.activeSelf && b.Game_Object.activeSelf)
                return b.equip.Grade.CompareTo(a.equip.Grade);
            else
                return 0;
        });
      
        for(int i = 0; i < setEquips.Count; i++) {
            if(setEquips[i].Game_Object.activeSelf) {
                setEquips[i].Game_Object.transform.SetSiblingIndex(i);
            }
        }     
    }

    public int getNumberOfGradeEquip(Equips.MaterialClass data)
    {
        int sum = 0;
        for(int i = 0; i < setEquips.Count; i++) {
            if(setEquips[i].Game_Object.activeSelf) {
                if(setEquips[i].equip.Grade == data)
                    sum++;
            }
        }
        return sum;
    }
}
