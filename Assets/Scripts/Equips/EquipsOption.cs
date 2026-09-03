using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipsOption
{
    public options option;
}

[System.Serializable]
public class options
{
    public int damage;
    [Header("#---0.01 = 1%")]
    public float damagePercent;
    [Header("#---0.01 = 1%")]
    public float AttackSpeed;
    [Header("#---0.01 = 1%")]
    public float BulletSpeed;
    [Header("#---1%")]
    public float CritChance;
    [Header("#---0.01 = 1%")]
    public float CritDamage;
    [Header("#---1%")]
    public float StunPercent;
    [Header("#---0.01 = 1%")]
    public float ExtraEXP;
    [Header("#---0.01 = 1%")]
    public float ExtraMoney;
}
