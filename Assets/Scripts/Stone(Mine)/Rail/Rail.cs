using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Rail : MonoBehaviour
{
    public static Rail Instance;

    [Header("#---Stone")]
    public Transform StoneSpawnLoc;
    public Stone stonePrefab;
    public List<Stone> spawnedStones;

    [Header("#---Tilemap")]
    public Tilemap tilemap;
    public railShop shop;
    //MineAmount
    public long minedStoneAmount;

    [Header("#---º¸À¯ÇÑ ±¤¼®")]
    public long possessedOre;

    [Header("#---¹ÌÈ®ÀÎ ±¤¼®")]
    public long unMinedOre;

    float TimerAs;

    [Header("#-----Drill")]
    public Drill drill;

    public int damageLevel;
    public int attackSpeedLevel;
    public int luckLevel;

    [Header("#-----Points")]
    public Transform[] points;

    int random;

    private void Awake()
    {
        Instance = this;
    }


    private void Update()
    {
        shop.SetShopInfo();
        drill.setDrillStat(shop.info, damageLevel, attackSpeedLevel, luckLevel);

        TimerAs += Time.deltaTime;

        TimerAs += Time.deltaTime;
        if(TimerAs >= shop.SpawnTime) {
            random = Random.Range(0, 100);
            if(spawnedStones.Count >= 12) {
                if(random <= 50) {
                    spawnStone(true);
                }
            } else {
                spawnStone(random <= 35);
            }
            TimerAs = 0;

        }
    }

    public void UpgradeDrillDamage()
    {
        if(possessedOre >= getTotalPrice(1)) {
            possessedOre -= getTotalPrice(1);
            damageLevel++;
        }
    }

    public void UpgradeDrillSpeed()
    {
        if(possessedOre >= getTotalPrice(2)) {
            possessedOre -= getTotalPrice(2);
            attackSpeedLevel++;
        }
    }
    public void UpgradeDrillLuck()
    {
        if(possessedOre >= getTotalPrice(3)) {
            possessedOre -= getTotalPrice(3);
            luckLevel++;
        }
    }

    public long getTotalPrice(int n)
    {
        long result = 1;

        switch(n) {
            case 1:
                result += damageLevel;

                if(damageLevel >= 10) 
                    result += 2 * damageLevel;
                
                if(damageLevel >= 30)
                    result += 3 * damageLevel;

                if(damageLevel >= 50)
                    result += 4 * damageLevel;

                if(damageLevel >= 100)
                    result += 6 * damageLevel;
                break;
            case 2:
                result += attackSpeedLevel;

                if(attackSpeedLevel >= 10)
                    result += 5 * attackSpeedLevel;

                if(attackSpeedLevel >= 30)
                    result += 15 * attackSpeedLevel;

                if(attackSpeedLevel >= 50)
                    result += 50 * attackSpeedLevel;

                if(attackSpeedLevel >= 100)
                    result += 100 * attackSpeedLevel;
                break;
            case 3:
                result += luckLevel;

                if(luckLevel >= 10)
                    result += 2 * luckLevel;

                if(luckLevel >= 30)
                    result += 4 * luckLevel;

                if(luckLevel >= 50)
                    result += 4 * luckLevel;

                if(luckLevel >= 100)
                    result += 5 * luckLevel;
                break;
        }
        return (long)(result * (1 - MineManager.instance.tradeSalePercent));
    }

    public void spawnStone(bool isOver)
    {
        Stone _s = ObjectPoolStone.Instance.GetPoolObject(stonePrefab.type).GetComponent<Stone>();

        _s.StoneHealth = (int)(_s.initialHealth * (1 + (drill.luck * 5f)));
        _s.transform.position = StoneSpawnLoc.position;
        _s.StoneValue = (long)(shop.getStoneValue() * (1 + drill.luck));
        _s.tilemap = this.tilemap;
        _s.rail = this;
        _s.isOverLimit = isOver;
        _s.gameObject.SetActive(true);

        drill.IsShooting = true;

        if(!isOver) {
            spawnedStones.Add(_s);
            _s.GetComponent<CapsuleCollider2D>().enabled = true;
        } else {
            _s.pointsIndex = 0;
            _s.GetComponent<CapsuleCollider2D>().enabled = false;
        }
    }

    public void unMinedStoneAdd()
    {
        //Achievement
        AchievementManager.instance.UnminedOreDaily++;
        AchievementManager.instance.UnminedOreQuest++;
        AchievementManager.instance.UnminedOreWeekly++;

        unMinedOre++;
    }

    public void stoneDied()
    {
        //Achievement
        AchievementManager.instance.OremineDaily++;
        AchievementManager.instance.OremineWeekly++;
        AchievementManager.instance.OreMinedQuest++;

        minedStoneAmount++;
        possessedOre += (int)(1 + drill.luck);
    }

    public int getTradingAmountForUnminedOre(int n)
    {
        switch (n){
            case 1:
                return (int)(100 / shop.getSpawnTime());
            case 2:
                return (int)(shop.getStoneValue() * 50);
            case 3:
                return (int)(700 / shop.getSpawnTime());
        }
        return 0;
    }

    public int getStoneHealth()
    {
        return (int)(stonePrefab.StoneHealth * (1 + (drill.luck * 5f)));
    }
}
