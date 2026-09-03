using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.Services.Authentication;

public class Stats : MonoBehaviour
{
    public TextMeshProUGUI PlayerName;
    public TextMeshProUGUI PlayerID;

    public TextMeshProUGUI Text_level;
    public Slider Slider_PlayerLevel;
    public TextMeshProUGUI Slider_PlayerlevelText;

    public TextMeshProUGUI Text_InfiniteStage;
    public TextMeshProUGUI Text_KilledEnemy;

    [Header("Stats")]
    public TextMeshProUGUI Damage_Text;
    public TextMeshProUGUI Attack_Speed_Text;
    public TextMeshProUGUI Attack_Bullet_Speed_Text;
    public TextMeshProUGUI TotalPlayedHour_text;
    public TextMeshProUGUI totalDamagePower_Text;
    public TextMeshProUGUI CriticalChance_Text;
    public TextMeshProUGUI CriticalDamage_Text;
    public TextMeshProUGUI StunPercent_Text;

    void Update()
    {
        if(Player.instance.PlayerName.Length > 2)
            PlayerName.text = Player.instance.PlayerName.Substring(0, Player.instance.PlayerName.IndexOf("#"));
        PlayerID.text = Player.instance.PlayerID;

        Slider_PlayerLevel.value = Player.instance.exp;
        Slider_PlayerLevel.maxValue = Player.instance.getEXPRequired();
        Slider_PlayerlevelText.text = string.Format("{0:#,##0}/{1:#,##0}", Player.instance.exp, Player.instance.getEXPRequired());
        Text_level.text = Player.instance.Lvl.ToString();

        Damage_Text.text = string.Format("{0:#,###0.##}", Player.instance.FinalAttack_Damage);
        Attack_Speed_Text.text = "+" + string.Format("{0:#,###0.##}", Player.instance.FinalAttack_SpeedPercent * 100) + "%";
        Attack_Bullet_Speed_Text.text = "+" + string.Format("{0:#,###0.##}", Player.instance.FinalAttack_Bullet_SpeedPercent) + "%";
        Text_InfiniteStage.text = "스테이지 " + string.Format("{0:#,###0.##}", InfiniteStage.Instance.CurrentStage);
        Text_KilledEnemy.text = string.Format("{0:#,###0.##}", EnemyManager.Instance.totalKilledEnemy) + "마리";
        totalDamagePower_Text.text = string.Format("{0:#,###0.##}", Player.instance.TotalDamagePower);
        if(Player.instance.FinalCriticalChance > 100)
            CriticalChance_Text.text = "100%";
        else if(Player.instance.FinalCriticalChance < 0)
            CriticalChance_Text.text = "0%";
        else
            CriticalChance_Text.text = string.Format("{0:#,###0.##}", Player.instance.FinalCriticalChance) + "%";

        CriticalDamage_Text.text = "+" + string.Format("{0:#,###0.##}", (Player.instance.FinalCriticalDamage * 100) + 100) + "%";
        StunPercent_Text.text = string.Format("{0:#,###0.##}", Player.instance.FinalStunPower) + "%";

        var ts = TimeSpan.FromSeconds(GameManager.instance.totalPlayedTime);
        TotalPlayedHour_text.text = string.Format("{0}시간 {1}분", (int)ts.TotalHours, ts.Minutes);
    }
}
