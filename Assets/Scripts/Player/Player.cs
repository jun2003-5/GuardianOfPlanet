using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Services.Authentication;

public class Player : MonoBehaviour, IDataPersistence
{
    [Header("Instance")]
    public static Player instance;

    public float AutoShootTime;
    [Header("PlayerInfor")]
    public string PlayerName;
    public string PlayerID;

    [Header("레벨")]
    [Space(5)]
    public int Lvl;

    [Header("경험치")]
    [Space(5)]
    public long exp;

    [Header("공격력 0.xxx")]
    [Space(5)]
    public float Attack_Damage;

    [Header("총 공격력 증가")]
    [Space(5)]
    public float Attack_Damage_Percent;

    [Header("공격속도 0 ~ 1 (%로 감소)")]
    [Range(0, 1)]
    [Space(5)]
    public float Attack_Speed;

    [Header("총알속도 0 ~ 1 (%로 감소)")]
    [Range(0, 10)]
    [Space(5)]
    public float Attack_Bullet_Speed;

    [Header("스턴확률 (%)")]
    [Space(5)]
    public float StunPower;

    [Header("전투력")]
    [Space(5)]
    public long TotalDamagePower;

    [Header("치명타 확률 (%)")]
    [Space(5)]
    public float CriticalChance;

    [Header("치명타 데미지 (0.xxx (1% = 0.01))")]
    [Space(5)]
    public float CriticalDamage;

    [Header("추가 경험치 (0.xxx (1% = 0.01))")]
    [Space(5)]
    public float ExtraEXP;

    [Header("추가 골드 (0.xxx (1% = 0.01))")]
    [Space(5)]
    public float ExtraGold;

    [Header("최종 스탯")]
    public long FinalAttack_Damage;
    public float FinalAttack_SpeedPercent;
    public float FinalAttack_Bullet_SpeedPercent;
    public float FinalStunPower;
    public float FinalCriticalChance;
    public float FinalCriticalDamage;
    public float FinalExtraEXP; 
    public float FinalExtraGold;

    [Header("Player Stats")]
    public int StatGiven_lvl;

    [Header("다음 레벨 경험치 Index")]
    //레벨업 관련
    public Slider expbar;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI currentEXPText;
    public Sprite[] levelMarkSprites;
    public Image levelMarkImage;


    public int requiredLvlindex;
    // Update is called once per frame
    long previousTotalDamagePower;

    [Header("총 획득 경험치")]
    public long expTotal;

    private void Awake()
    {
        instance = this;
    }
    void Update()
    {
        previousTotalDamagePower = TotalDamagePower;

        calculateValues();

        if(exp >= getEXPRequired()) {
            PlayerEXPUP(exp);
        }



        TotalDamagePower = (long)((FinalAttack_Damage * 5) + (FinalAttack_SpeedPercent * 20) + (FinalAttack_Bullet_SpeedPercent * 10) + (FinalStunPower * 15) + (FinalCriticalChance * 15) + (FinalCriticalDamage * 50) + (WeaponManager.instance.getTotalWeaponLevel() * 10));

        if(TotalDamagePower != previousTotalDamagePower) {
            if(AuthenticationService.Instance.IsSignedIn) {
                LeaderBoardManager.Instance.AddTotalDamageScore(TotalDamagePower);
                previousTotalDamagePower = TotalDamagePower;
            }
        }
        if(Lvl > 1500) {
            expbar.maxValue = 1;
            expbar.value = expbar.maxValue;
            currentEXPText.text = "MAX";
            LevelText.text = "1,500";
            levelMarkImage.sprite = levelMarkSprites[2];
        } else {
            expbar.maxValue = getEXPRequired();
            expbar.value = exp;
            currentEXPText.text = (long)expbar.value + "/" + (long)expbar.maxValue;
            LevelText.text = Lvl.ToString("N0");
            levelMarkImage.sprite = Lvl >= 500 ? levelMarkSprites[1] : levelMarkSprites[0];
        }
    }


    public long getEXPRequired()
    {
        long total = 10;

        if(Lvl >= 0) {
            total += Lvl * 5;
        }

        if(Lvl >= 100) {
            total += (Lvl - 100) * 500;
        }

        if(Lvl >= 200) {
            total += (Lvl - 200) * 2500;
        }

        if(Lvl >= 300) {
            total += (Lvl - 300) * 12500;
        }

        if(Lvl >= 400) {
            total += (Lvl - 400) * 50000;
        }

        if(Lvl >= 500) {
            total += (Lvl - 500) * 100000;
        }

        if(Lvl >= 600) {
            total += (Lvl - 600) * 300000;
        }

        if(Lvl >= 700) {
            total += (Lvl - 700) * 1000000;
        }

        if(Lvl >= 800) {
            total += (Lvl - 800) * 5000000;
        }

        if(Lvl >= 900) {
            total += (Lvl - 900) * 10000000;
        }

        if(Lvl >= 1000) {
            total += (Lvl - 1000) * 15000000;
        }

        if(Lvl >= 1100) {
            total += (Lvl - 1100) * 30000000;
        }

        if(Lvl >= 1200) {
            total += (Lvl - 1200) * 50000000;
        }

        if(Lvl >= 1300) {
            total += (Lvl - 1300) * 100000000;
        }

        if(Lvl >= 1400) {
            total += (Lvl - 1400) * 300000000;
        }


        return total;
    }

    public static float GetStatsByType(Upgrade_StatsPrefab.StatsType type)
    {
        switch(type) {
            case Upgrade_StatsPrefab.StatsType.Damage:
                return instance.FinalAttack_Damage;
            case Upgrade_StatsPrefab.StatsType.AttackSpeed:
                return instance.FinalAttack_SpeedPercent;
            case Upgrade_StatsPrefab.StatsType.BulletSpeed:
                return instance.FinalAttack_Bullet_SpeedPercent;
            case Upgrade_StatsPrefab.StatsType.CritChance:
                return instance.FinalCriticalChance;
            case Upgrade_StatsPrefab.StatsType.CritDamage:
                return instance.FinalCriticalDamage;
            case Upgrade_StatsPrefab.StatsType.StunPercent:
                return instance.FinalStunPower;
            case Upgrade_StatsPrefab.StatsType.ExtraEXP:
                return instance.FinalExtraEXP;
            case Upgrade_StatsPrefab.StatsType.ExtraMoney:
                return instance.FinalExtraGold;
        }
        return 0;
    }

    void calculateValues()
    {
        //Final Damage
        FinalAttack_Damage = (int)((EquipingManager.Instance.Attack_Damage_Equip + Attack_Damage) * (1 + EquipingManager.Instance.Attack_DamagePercent_Equip + Attack_Damage_Percent + PlanetBuffManager.instance.getValue(PlanetBuff.BuffType.DamageIncrease) + StatsManager.instance.getStatAmount(StatsData.TypeofStat.Damage)));
        FinalAttack_SpeedPercent = (Attack_Speed + EquipingManager.Instance.Attack_Speed_Equip + StatsManager.instance.getStatAmount(StatsData.TypeofStat.Attack_Speed)) * (1 + PlanetBuffManager.instance.getValue(PlanetBuff.BuffType.AttackSpeedIncrease));
        FinalAttack_Bullet_SpeedPercent = (Attack_Bullet_Speed + EquipingManager.Instance.Bullet_Speed_Equip + StatsManager.instance.getStatAmount(StatsData.TypeofStat.Attack_Bullet_Speed)) * (1 + PlanetBuffManager.instance.getValue(PlanetBuff.BuffType.BulletSpeedIncrease));
        FinalStunPower = StunPower + EquipingManager.Instance.StunPower_Equip + StatsManager.instance.getStatAmount(StatsData.TypeofStat.StunPercent) + PlanetBuffManager.instance.getValue(PlanetBuff.BuffType.StunIncrease);
        FinalCriticalChance = CriticalChance + EquipingManager.Instance.Critical_Chance_Equip + StatsManager.instance.getStatAmount(StatsData.TypeofStat.CritChance) + PlanetBuffManager.instance.getValue(PlanetBuff.BuffType.CritChanceIncrease);
        FinalCriticalDamage = CriticalDamage + EquipingManager.Instance.Critical_Damage_Equip + StatsManager.instance.getStatAmount(StatsData.TypeofStat.CritDamage) + PlanetBuffManager.instance.getValue(PlanetBuff.BuffType.CritDamageIncrease);
        FinalExtraEXP = ExtraEXP + EquipingManager.Instance.ExtraEXP_Equip + StatsManager.instance.getStatAmount(StatsData.TypeofStat.ExtraEXP) + PlanetBuffManager.instance.getValue(PlanetBuff.BuffType.EXPIncrease);
        FinalExtraGold = ExtraGold + EquipingManager.Instance.ExtraGold_Equip + StatsManager.instance.getStatAmount(StatsData.TypeofStat.ExtraMoney) + PlanetBuffManager.instance.getValue(PlanetBuff.BuffType.GoldIncrease);

        
        if(FinalAttack_Damage < 10) FinalAttack_Damage = 10;
        if(FinalStunPower < 0) FinalStunPower = 0;
        if(FinalStunPower > 70) FinalStunPower = 70;
        if(FinalCriticalChance < 0) FinalCriticalChance = 0;
        if(FinalCriticalChance > 100) FinalCriticalChance = 100;
        if(CriticalDamage < 0) FinalCriticalDamage = 0;
        if(FinalExtraEXP < 0) FinalExtraEXP = 0;
        if(FinalExtraGold < 0) FinalExtraGold = 0;
        if(FinalAttack_Bullet_SpeedPercent > 4) FinalAttack_Bullet_SpeedPercent = 4;      
    }

    public void extraCritchance(int amount)
    {
        CriticalDamage += amount * 0.01f;
    }

    public void PlayerEXPUP(long exp)
    {
        expTotal += exp;
        long remainingExp = exp;

        while(remainingExp > 0) {
            long expRequiredForNextLevel = getEXPRequired() - this.exp;

            if(remainingExp < expRequiredForNextLevel) {
                this.exp += remainingExp;
                remainingExp = 0;
            } else {
                this.exp = 0;
                remainingExp -= expRequiredForNextLevel;
                LevelUp();
            }
        }
    }



    public void LevelUp()
    {
        Lvl++;
        setAttackDamage();
        setStats();
    }

    public void setStats()
    {
        if(Lvl >= 30) {
            StatsManager.instance.statPoint++;
        }

        if(Lvl >= 500) {
            StatsManager.instance.statPoint += 2;
        }

        if(Lvl >= 750) {
            StatsManager.instance.statPoint += 3;
        }

        if(Lvl >= 900) {
            StatsManager.instance.statPoint += 4;
        }

        StatsManager.instance.setStatsPointUI();
    }

    public void setAttackDamage()
    {
        Attack_Damage = 10;

        if(Lvl >= 0) {
            Attack_Damage += Lvl;
        }

        if(Lvl >= 100) {
            Attack_Damage += (Lvl - 100) * 2;
        }

        if(Lvl >= 200) {
            Attack_Damage += (Lvl - 200) * 4;
        }

        if(Lvl >= 300) {
            Attack_Damage += (Lvl - 300) * 8;
        }

        if(Lvl >= 400) {
            Attack_Damage += (Lvl - 400) * 11;
        }

        if(Lvl >= 500) {
            Attack_Damage += (Lvl - 500) * 15;
        }

        if(Lvl >= 600) {
            Attack_Damage += (Lvl - 600) * 21;
        }

        if(Lvl >= 700) {
            Attack_Damage += (Lvl - 700) * 24;
        }

        if(Lvl >= 800) {
            Attack_Damage += (Lvl - 700) * 27;
        }

        if(Lvl >= 900) {
            Attack_Damage += (Lvl - 700) * 100;
        }
    }

    public void addAutoAttackTime_Sec(float time)
    {
        AutoShootTime += time;
    }

    public void LoadData(GameData data)
    {
        PlayerName = data.PlayerName;
        this.Lvl = data.lvl;
        this.exp = data.exp;
        this.AutoShootTime = data.AutoShootTime;

        //추가 스탯
        this.Attack_Damage = data.Attack_Damage;
        this.Attack_Damage_Percent = data.Attack_Damage_Percent;
        this.Attack_Speed = data.Attack_Speed;
        this.Attack_Bullet_Speed = data.Attack_Bullet_Speed;
        this.StunPower = data.StunPower;
        this.CriticalChance = data.CriticalChance;
        this.CriticalDamage = data.CriticalDamage;
        this.ExtraEXP = data.ExtraEXP;
        this.ExtraGold = data.ExtraGold;

        this.requiredLvlindex = data.requiredLvlindex;
        setAttackDamage();
    }

    public void SaveData(GameData data)
    {
        data.PlayerName = PlayerName;
        data.lvl = this.Lvl;
        data.exp = this.exp;
        data.AutoShootTime = this.AutoShootTime;


        //추가 스탯
        data.Attack_Damage = this.Attack_Damage;
        data.Attack_Damage_Percent = this.Attack_Damage_Percent;
        data.Attack_Speed = this.Attack_Speed;
        data.Attack_Bullet_Speed = this.Attack_Bullet_Speed;
        data.StunPower = this.StunPower;
        data.CriticalChance = this.CriticalChance;
        data.CriticalDamage = this.CriticalDamage;
        data.ExtraEXP = this.ExtraEXP;
        data.ExtraGold = this.ExtraGold;

        data.requiredLvlindex = this.requiredLvlindex;
    }
}
