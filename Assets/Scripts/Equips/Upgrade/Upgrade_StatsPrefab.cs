using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public class Upgrade_StatsPrefab : MonoBehaviour
{
    public enum StatsType
    {
        Damage, DamagePercent, AttackSpeed, BulletSpeed, CritChance, CritDamage, StunPercent, ExtraEXP, ExtraMoney
    }

    public StatsType _type;

    public float BaseStat;
    public List<float> StatsNumberRange;

    [Header("UIs")]
    public TextMeshProUGUI StatsNumber;

    int RandomStat;

    public void OnEnable()
    {
        if(StatsNumberRange.Count > 0) {
            if(_type == StatsType.Damage)
                StatsNumber.text = string.Format("{0:0.##}", (int)(BaseStat * StatsNumberRange[0])) + "~" + string.Format("{0:0.##}", (int)(BaseStat * StatsNumberRange[StatsNumberRange.Count - 1]));
            else if(_type == StatsType.CritChance || _type == StatsType.StunPercent)
                StatsNumber.text = string.Format("{0:0.##}%", BaseStat * StatsNumberRange[0]) + "~" + string.Format("{0:0.##}%", BaseStat * StatsNumberRange[StatsNumberRange.Count - 1]);
            else
                StatsNumber.text = string.Format("{0:0.##}%", BaseStat * StatsNumberRange[0] * 100) + "~" + string.Format("{0:0.##}%", BaseStat * StatsNumberRange[StatsNumberRange.Count - 1] * 100);
        }
    }

    public void ChangeTextSuccess()
    {
        if(_type == StatsType.Damage)
            StatsNumber.text = string.Format("{0:0.##} <color=green><size={1}>+{2:0.##}</size></color>", BaseStat + ((int)(BaseStat * StatsNumberRange[RandomStat])), StatsNumber.fontSize - 8, (int)(BaseStat * StatsNumberRange[RandomStat]));
        else if(_type == StatsType.CritChance || _type == StatsType.StunPercent)
            StatsNumber.text = string.Format("{0:0.##}% <color=green><size={1}>+{2:0.##}%</size></color>", BaseStat + (BaseStat * StatsNumberRange[RandomStat]), StatsNumber.fontSize - 8, BaseStat * StatsNumberRange[RandomStat]);
        else {
            StatsNumber.text = string.Format("{0:0.##}% <color=green><size={1}>+{2:0.##}%</size></color>", (BaseStat + (BaseStat * StatsNumberRange[RandomStat])) * 100, StatsNumber.fontSize - 8, BaseStat * StatsNumberRange[RandomStat] * 100);
        }
    }

    public void noChangeText()
    {
        if(_type == StatsType.Damage)
            StatsNumber.text = string.Format("{0:0.##}", BaseStat);
        else if(_type == StatsType.CritChance || _type == StatsType.StunPercent)
            StatsNumber.text = string.Format("{0:0.##}%", BaseStat);
        else
            StatsNumber.text = string.Format("{0:0.##}%", BaseStat * 100);
    }

    public void ChangeTextFailed(int index)
    {
        if(_type == StatsType.Damage) {
            // Calculate intermediate values for better readability
            int damageReduction = (int)((int)(BaseStat / (1 + StatsNumberRange[index])) * StatsNumberRange[index]);
            int updatedStat = (int)(BaseStat - damageReduction);
            float fontSize = StatsNumber.fontSize - 8;

            // Format the string
            StatsNumber.text = string.Format("{0:0.##} <color=red><size={1}>-{2:0.##}</size></color>", (int)updatedStat, fontSize, Mathf.RoundToInt(damageReduction));

        } else if(_type == StatsType.CritChance || _type == StatsType.StunPercent) {
            float damageReduction = BaseStat / (1 + StatsNumberRange[index]) * StatsNumberRange[index];
            float updatedStat = BaseStat - damageReduction;
            float fontSize = StatsNumber.fontSize - 8;

            StatsNumber.text = string.Format("{0:0.##}% <color=red><size={1}>-{2:0.##}%</size></color>", updatedStat, fontSize, damageReduction);
        } else {
            float damageReduction = BaseStat / (1 + StatsNumberRange[index]) * StatsNumberRange[index];
            float updatedStat = BaseStat - damageReduction;
            float fontSize = StatsNumber.fontSize - 8;

            StatsNumber.text = string.Format("{0:0.##}% <color=red><size={1}>-{2:0.##}%</size></color>", updatedStat * 100, fontSize, damageReduction * 100);
        }
    }

    public int StartSlotMachine()
    {
        RandomStat = Random.Range(0, 5);
        return RandomStat;
    }
}
