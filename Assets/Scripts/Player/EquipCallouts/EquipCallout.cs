using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class EquipCallout : MonoBehaviour, IPointerClickHandler
{
    [Header("Group")]
    public EquipCalloutGroup group;

    [Header("#---Equip")]
    [Space(5)]
    public EquipingData equipData;

    [Header("#---CallOut")]
    [Space(5)]
    public GameObject CalloutObject;

    [Header("#---GUIs")]
    [Space(5)]
    public List<Image> star_List;
    public Image EquipImage;
    public Image EquipBorder;
    public GameObject[] Borders;
    public TextMeshProUGUI EquipName;
    public TextMeshProUGUI EquipStats;

    public void setGUIs()
    {
        EquipImage.sprite = equipData.equipment.equipData.Sprite_Equip;
        //Border
        Borders[0].SetActive(equipData.equipment.equipData.Grade == Equips.MaterialClass.Rare);
        Borders[1].SetActive(equipData.equipment.equipData.Grade == Equips.MaterialClass.Epic);
        Borders[2].SetActive(equipData.equipment.equipData.Grade == Equips.MaterialClass.Unique);
        Borders[3].SetActive(equipData.equipment.equipData.Grade == Equips.MaterialClass.Legendary);
        Borders[4].SetActive(equipData.equipment.equipData.Grade == Equips.MaterialClass.Ancient);

        EquipName.text = equipData.equipment.equipData.EquipName;
        //EquipBorder.color = equipData.equipment.Border.color;

        ChangeStarSetting(equipData.equipment.equipData.Level);
    }

    public void ChangeStarSetting(int n)
    {
        for(int i = 0; i < 10 + n; i++) {
            if(i < 10) {
                star_List[i].color = new Color(0.35f, 0.35f, 0.35f, 0.6f);
            } else if(i >= 10 && i < 20) {
                star_List[i - 10].color = new Color(1, 1, 1, 1f);
            } else {
                star_List[i - 20].color = new Color(1, 0, 0.085289f);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(equipData.equipment.equipData.EquipName != "base") {
            setGUIs();
            group.activateCallout(this);
        }
    }
}
