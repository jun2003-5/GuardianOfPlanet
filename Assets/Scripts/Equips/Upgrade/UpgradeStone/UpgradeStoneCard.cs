using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UpgradeStoneCard : MonoBehaviour
{
    public UpgradeStone stoneData;

    [Header("UIs")]
    public TextMeshProUGUI ItemNumber;
    public GameObject Empty_Tab;

    [HideInInspector]
    public int SelectedAmount;

    public void setStoneAmountText()
    {
        if(stoneData.StoneAmount > 0) {
            ItemNumber.text = (stoneData.StoneAmount - SelectedAmount).ToString();
            if(Empty_Tab != null)
                Empty_Tab.SetActive(false);
        } else {
            if(Empty_Tab != null)
                Empty_Tab.SetActive(true);
        }
    }

    public void resetStoneAmount()
    {
        SelectedAmount = 0;
        setStoneAmountText();
    }

    public void setStoneAmount()
    {
        stoneData.StoneAmount -= SelectedAmount;
        resetStoneAmount();
    }
}
