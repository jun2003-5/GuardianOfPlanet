using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsData : MonoBehaviour
{
    public string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = "Stats_" + _typeOfStat.ToString();
    }

    public TextMeshProUGUI StatsLevelText;

    [HideInInspector]
    public int _statsLevel;

    public enum TypeofStat { Damage, Attack_Speed, Attack_Bullet_Speed, StunPercent, CritChance, CritDamage, ExtraEXP, ExtraMoney }

    public TypeofStat _typeOfStat;

    public float increasePerLevel;

    public void UpgradeStat()
    {
        _statsLevel++;

        setStatUI();
    }

    public void setStatUI()
    {
        StatsLevelText.text = "Lv. " + _statsLevel;
    }

    public float getStat()
    {
        return increasePerLevel * _statsLevel;
    }
}
