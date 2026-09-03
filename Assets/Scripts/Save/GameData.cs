using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class GameData
{
    //Player Infor
    public string PlayerName;
    public int lvl;
    public long exp;
    public float AutoShootTime;
    public bool gameSpeedBought;
    public bool noAdsBought;

    //추가 스탯
    public float Attack_Damage;
    public float Attack_Damage_Percent;
    public float Attack_Speed;
    public float Attack_Bullet_Speed;
    public float StunPower;
    public float CriticalChance;
    public float CriticalDamage;
    public float ExtraEXP;
    public float ExtraGold;

    //최종 스탯
    public int requiredLvlindex;

    //GameInfor
    public long Money;
    public int Diamond;
    public long Ores;
    public long parts;
    public float totalPlayedTime;
    public long maxMoney;

    //무기
    public SerializableDictionary<string, int> UpgradeLevel_Weapon;

    //스탯
    public int StatsPoint;
    public int UsedPoint;
    public SerializableDictionary<string, int> Stats_Level;

    //총 처치 수
    public int enemyKilled_Number;
    public SerializableDictionary<string, int> bossKilled_Number;
    public int totalBossKill;
    public long totalKilledEnemy;

    //스테이지 처치수
    public SerializableDictionary<string, bool> Stage_Cleared;
    public SerializableDictionary<string, bool> Planet_Cleared;

    //장비
    public SerializableDictionary<string, int> Equip_Amount;

    public SerializableDictionary<string, int> Equip_Level;
    public SerializableDictionary<string, bool> Equip_IsEquiped;
    public SerializableDictionary<string, string> Equip_SetData;
    public int currentSetIndex;

    //장비 스탯
    public SerializableDictionary<string, string> Equip_UpgradeIndex;

    //-------강화석
    public SerializableDictionary<string, int> UpgradeStone_Amount;

    //-------재료
    public SerializableDictionary<string, int> Loot_Amount;

    //도감
    public SerializableDictionary<string, int> Collection_KilledAmount;
    public SerializableDictionary<string, int> Collection_Grade;
    public SerializableDictionary<string, int> Collection_EarnDiamond;

    //출석보상
    public int DailyGachaTime;
    public int Ticket;
    public int adsPerDay;

    //Daily
    public bool DailyRewarded;
    public int ResetTimeDaily;
    public float PlayedTimeDaily;
    public int killedEnemyDaily;
    public int killedBossDaily;
    public int GachaDaily;
    public int TowerAdventureDaily;
    public int IronworkTradeDaily;
    public int OremineDaily;
    public int UnminedOreDaily;
    public int InfiniteStageDaily;

    //Weekly
    public bool WeeklyRewarded;
    public int ResetTimeWeekly;
    public float PlayedTimeWeekly;
    public int killedEnemyWeekly;
    public int killedBossWeekly;
    public int GachaWeekly;
    public int OremineWeekly;
    public int UnminedOreWeekly;
    public int DailyQuestClearWeekly;
    public int InfiniteStageWeekly;

    //Quest
    public int GachaTimeQuest;
    public int TowerAdventureQuest;
    public int OreMinedQuest;
    public int UnminedOreQuest;

    public SerializableDictionary<string, int> currentQuest;
    public SerializableDictionary<string, bool> AchieveCondition;
    public SerializableDictionary<string, bool> CompletedTab;

    //Gacha
    public int NormalBoughtCount;
    public int SpecialBoughtCount;
    public int StoneBoughtCount;

    public int NormalGachaTicket;
    public int SpecialGachaTicket;
    public int StoneGachaTicket;

    //Tower
    public string towerTime;
    public float LeftTravelTime;
    public int lastTravelTower;
    public bool isTraveling;
    public bool playerRewarded;
    public int towerAdsPerDay;
    public int towerAdsTime;

    //Drill
    public SerializableDictionary<string, int> DrillLevel;
    public SerializableDictionary<string, int> DrillDamage;

    //Trait
    public SerializableDictionary<string, int> TraitLevel;

    //Miner
    public SerializableDictionary<string, int> MinerLevel;

    //Stone Shop
    public SerializableDictionary<string, int> railShopLevel;
    public SerializableDictionary<string, long> minedStoneAmount;
    public SerializableDictionary<string, long> possessedOre;
    public SerializableDictionary<string, long> unminedOre;
    public SerializableDictionary<string, int> railDrill_DamageLevel;
    public SerializableDictionary<string, int> railDrill_AttackSpeedLevel;
    public SerializableDictionary<string, int> railDrill_luckLevel;

    public int Clicklevel;

    //Buffs
    public SerializableDictionary<string, int> buffLevel;

    //Infinite Stage
    public int StageIndex;

    //DailyAttendance
    public int RewardIndex;

    //Dungeon
    public int DungeonTicket;
    public int LimitExtraDungeonTicket;
    public int ResetTimeDailyDungeon;

    //UserIcon
    public SerializableDictionary<string, bool> isUnlocked_Icon;
    public SerializableDictionary<string, bool> isSelected_Icon;

    //Shop Data
    public SerializableDictionary<string, bool> Shop_isBought;
    public SerializableDictionary<string, bool> Shop_isActivate;
    public SerializableDictionary<string, string> Shop_leftTime;

    //Notification
    public SerializableDictionary<string, bool> notification_Bool;
    public string newUserDailyGift_LastDate;
    public bool newUserGiftBool;

    //Auto Exchange Bool
    public bool autoExchangerBought;
    public int autoExchangeIndexBool;
    public SerializableDictionary<string, bool> autoSelectedRailBool;
    public bool firstTimeGame;

    //Default values
    public GameData()
    {
        PlayerName = "";

        //기본 총 지급
        Money = 0;
        maxMoney = 500;
        Diamond = 0;
        Ores = 0;
        parts = 0;
        lvl = 1;
        exp = 0;
        AutoShootTime = 0;

        gameSpeedBought = false;
        noAdsBought = false;

        //추가 스탯
        Attack_Damage = 10;
        Attack_Damage_Percent = 0;
        Attack_Speed = 0;
        Attack_Bullet_Speed = 0;
        StunPower = 0;
        CriticalChance = 0;
        CriticalDamage = 0;
        ExtraEXP = 0;
        ExtraGold = 0;

        requiredLvlindex = 0;
        totalPlayedTime = 0;

        //무기
        UpgradeLevel_Weapon = new SerializableDictionary<string, int>();

        //스탯
        StatsPoint = 0;
        UsedPoint = 0;
        Stats_Level = new SerializableDictionary<string, int>();

        //총 처치 수
        enemyKilled_Number = 0;
        bossKilled_Number = new SerializableDictionary<string, int>();
        totalBossKill = 0;
        totalKilledEnemy = 0;

        //스테이지 처치 수
        Stage_Cleared = new SerializableDictionary<string, bool>();
        Planet_Cleared = new SerializableDictionary<string, bool>();

        //장비
        Equip_Amount = new SerializableDictionary<string, int>();
        Equip_Level = new SerializableDictionary<string, int>();
        Equip_IsEquiped = new SerializableDictionary<string, bool>();
        Equip_SetData = new SerializableDictionary<string, string>();
        currentSetIndex = 0;

        //장비 스탯
        Equip_UpgradeIndex = new SerializableDictionary<string, string>();

        //-----강화석
        UpgradeStone_Amount = new SerializableDictionary<string, int>();

        //-----재료
        Loot_Amount = new SerializableDictionary<string, int>();

        //도감
        Collection_KilledAmount = new SerializableDictionary<string, int>();
        Collection_Grade = new SerializableDictionary<string, int>();
        Collection_EarnDiamond = new SerializableDictionary<string, int>();

        //출석보상
        DailyGachaTime = DateTime.Now.DayOfYear;
        Ticket = 1;
        adsPerDay = 0;

        //Daily
        DailyRewarded = false;
        ResetTimeDaily = DateTime.Now.DayOfYear;
        PlayedTimeDaily = 0;
        killedEnemyDaily = 0;
        killedBossDaily = 0;
        GachaDaily = 0;
        TowerAdventureDaily = 0;
        IronworkTradeDaily = 0;
        OremineDaily = 0;
        UnminedOreDaily = 0;
        InfiniteStageDaily = 0;

        //Weekly
        WeeklyRewarded = false;
        ResetTimeWeekly = DateTime.Now.DayOfYear;
        PlayedTimeWeekly = 0;
        killedEnemyWeekly = 0;
        killedBossWeekly = 0;
        GachaWeekly = 0;
        OremineWeekly = 0;
        UnminedOreWeekly = 0;
        DailyQuestClearWeekly = 0;
        InfiniteStageWeekly = 0;

        //Quest
        GachaTimeQuest = 0;
        TowerAdventureQuest = 0;
        OreMinedQuest = 0;
        UnminedOreQuest = 0;

        currentQuest = new SerializableDictionary<string, int>();
        AchieveCondition = new SerializableDictionary<string, bool>();
        CompletedTab = new SerializableDictionary<string, bool>();

        //Gacha
        NormalBoughtCount = 0;
        SpecialBoughtCount = 0;
        StoneBoughtCount = 0;

        NormalGachaTicket = 0;
        SpecialGachaTicket = 0;
        StoneGachaTicket = 0;

        //Tower
        towerTime = DateTime.Now.ToString();
        LeftTravelTime = 0;
        lastTravelTower = 0;
        isTraveling = false;
        playerRewarded = false;
        towerAdsPerDay = 0;
        towerAdsTime = 0;

        //Drill
        DrillLevel = new SerializableDictionary<string, int>();
        DrillDamage = new SerializableDictionary<string, int>();

        //Trait
        TraitLevel = new SerializableDictionary<string, int>();

        //Miner
        MinerLevel = new SerializableDictionary<string, int>();

        //Rail
        railShopLevel = new SerializableDictionary<string, int>();
        minedStoneAmount = new SerializableDictionary<string, long>();
        possessedOre = new SerializableDictionary<string, long>();
        unminedOre = new SerializableDictionary<string, long>();
        railDrill_DamageLevel = new SerializableDictionary<string, int>();
        railDrill_AttackSpeedLevel = new SerializableDictionary<string, int>();
        railDrill_luckLevel = new SerializableDictionary<string, int>();

        Clicklevel = 1;

        //Buff
        buffLevel = new SerializableDictionary<string, int>();

        //Infinite Stage
        StageIndex = 1;

        //Reward Attendance
        RewardIndex = 0;

        //DungeonTicket
        DungeonTicket = 3;
        LimitExtraDungeonTicket = 0;
        ResetTimeDailyDungeon = DateTime.Now.DayOfYear;

        //UserIcon
        isUnlocked_Icon = new SerializableDictionary<string, bool>();
        isSelected_Icon = new SerializableDictionary<string, bool>();

        //Shop Data
        Shop_isBought = new SerializableDictionary<string, bool>();
        Shop_isActivate = new SerializableDictionary<string, bool>();
        Shop_leftTime = new SerializableDictionary<string, string>();

        //Notification
        notification_Bool = new SerializableDictionary<string, bool>();
        newUserDailyGift_LastDate = "";
        newUserGiftBool = false;

        //Auto Exchange
        autoExchangerBought = false;
        autoExchangeIndexBool = 1;
        autoSelectedRailBool = new SerializableDictionary<string, bool>();
        firstTimeGame = true;
    }
}
