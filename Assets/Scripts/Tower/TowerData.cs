using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TowerData : MonoBehaviour
{
    [Header("#----Name")]
    public string TowerName;

    [Header("#----Time")]
    public float TimeForTravel;

    [Header("#---Reward")]
    [Space(5)]
    public long goldReward;
    public UpgradeStone stoneReward;
    public int stoneAmount;
    public enum TicketType { Normal, Speical, Stone};
    public TicketType ticketType;
    public int gachaTicketReward;

    public void Selected()
    {
        TowerManager.Instance.selectPlanet(this);
    }
}
