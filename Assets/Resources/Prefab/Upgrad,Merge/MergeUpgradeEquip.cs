using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MergeUpgradeEquip : MonoBehaviour, IPointerClickHandler
{
    public EquipUpgradeManager M_M;
    //UIs
    public Image BorderImage;
    public Image EquipImage;
    public Text Level;

    public bool isSelected;

    public void OnPointerClick(PointerEventData eventData)
    {
        isSelected = !isSelected;
    }
}
