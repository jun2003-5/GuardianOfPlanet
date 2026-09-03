using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using System;

public class DungeonManager : MonoBehaviour, IDataPersistence
{
    public static DungeonManager instance;

    [Header("Dungeons Data")]
    public List<DungeonData> DungeonDatas;

    [Header("player health")]
    int playerHealth;

    [Header("Dungeon UIs")]
    public TextMeshProUGUI DungeonHealth;
    public TextMeshProUGUI DungeonRemainEnemy;

    [Header("Gameobjects")]
    public TransformTab transform_tab;

    [Header("Dungeon Enemy Info")]
    public GameObject MapInfoTab;
    public Transform EnemiesParent;
    public TextMeshProUGUI TotalEnemyText;
    public TextMeshProUGUI StageInfoTitle;

    public Image EnemyInfo_Image;
    public TextMeshProUGUI Health_Text;
    public TextMeshProUGUI Movement_Text;
    public TextMeshProUGUI Stun_Text;

    [Header("#--Daily Ticket")]
    public int DungeonTicket;

    [Header("#--ResultTab")]
    public GameObject Result_VictoryTab;
    public GameObject Result_DefeatTab;
    public TextMeshProUGUI GoldPriceText;
    public TextMeshProUGUI EXPPriceText;
    public GameObject[] stonePriceObj;

    [Header("Error Tab")]
    public GameObject ErrorTab;

    [Header("#-----Pause Tab")]
    public GameObject PauseTab;
    public TextMeshProUGUI pauseTab_CurentStage;

    DungeonData currentDungeon;
    bool SpawningEnemy;
    int TotalKilledEnemy;
    int KilledEnemy;
    int SpawnedEnemy;
    int enemySpawnIndex;

    //Daily ExtraDungeonTicket Limit
    int LimitExtraDungeonTicket;

    private void Awake()
    {
        instance = this;
    }

    public void MapSelected(DungeonData data)
    {
        MapInfoTab.SetActive(true);

        for(int i = 0; i < EnemiesParent.childCount; i++)
            EnemiesParent.GetChild(i).gameObject.SetActive(false);
        StageInfoTitle.text = data.DungeonName;
        TotalEnemyText.text = "출현 몬스터: " + data.getTotalEnemy() + "마리";

        for(int i = 0; i < data.DungeonEnimes.Length; i++) {
            CreateMapInfo(data.DungeonEnimes[i], data.DungeonEnimes[i].RawImage, data.DungeonEnimes[i]._typeofEnemy == Enemy.typeofEnemy.DungeonBoss);
        }
    }

    public void CreateMapInfo(Enemy enemyData, Sprite EnemyImage, bool isBoss)
    {
        for(int i = 0; i < EnemiesParent.childCount; i++) {
            if(!EnemiesParent.GetChild(i).gameObject.activeSelf) {
                EnemiesParent.GetChild(i).GetComponent<LabMonsterData>().setMonsterData(enemyData, isBoss);
                EnemiesParent.GetChild(i).gameObject.SetActive(true);
                EnemiesParent.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = EnemyImage;
                break;
            }
        }
    }

    public void setMapInfoStats(Enemy data)
    {
        EnemyInfo_Image.sprite = data.RawImage;
        Health_Text.text = data.EnemyHealth.ToString("N0");
        Movement_Text.text = data.enemyMovingSpeed.ToString("F2");
        Stun_Text.text = data.StunResistence + "%";
    }

    public void setDungeonTicketText()
    {
        for(int i = 0; i < DungeonDatas.Count; i++) {
            DungeonDatas[i].DungeonTicket.text = DungeonTicket + "/3";
            DungeonDatas[i].DungeonEnterButton.interactable = DungeonTicket > 0;
            DungeonDatas[i].DungeonTicektBuyButton.interactable = LimitExtraDungeonTicket < 3;
        }
    }

    public void RewardByAds()
    {
        DungeonTicket++;
        LimitExtraDungeonTicket++;
        setDungeonTicketText();
    }

    public void RewardByDiamond()
    {
        if(GameManager.GetDiamond() >= 100) {
            GameManager.SetDiamond(-100);
            DungeonTicket++;
            LimitExtraDungeonTicket++;
        } else {
            ErrorTab.SetActive(true);
        }
        setDungeonTicketText();
    }

    public void Pausestage()
    {
        PauseTab.SetActive(true);
        pauseTab_CurentStage.text = currentDungeon.DungeonName;
    }

    //Dungeon Algorithm Related
    public async void EnterDungeon(DungeonData data)
    {
        if(DungeonTicket <= 0) {
            return;
        }

        //Sound
        SoundManager.Instance.playMusic(SoundManager.MusicType.DungeonBGM);

        //Turn Infinite Stage Off
        InfiniteStage.Instance.PauseInfiniteStage();

        currentDungeon = data;

        WeaponManager.instance.shootType = WeaponManager.ShootType.AutoShoot;
        TransformTab.instance.gameObject.SetActive(true);
        TransformTab.instance.startFading(data.DungeonName);

        //Default the values
        KilledEnemy = 0;
        SpawnedEnemy = 0;
        TotalKilledEnemy = 0;
        playerHealth = data.playerhealth;

        DungeonRemainEnemy.text = (currentDungeon.getTotalEnemy() - KilledEnemy).ToString();
        DungeonHealth.text = playerHealth.ToString();


        await Task.Delay(1500);

        GridManager.instance.ToDungeon();
        await Task.Delay(2000);
        StartSpawningEnemy();
    }

    public void StartSpawningEnemy()
    {
        SpawningEnemy = true;
        enemySpawnIndex = 0;
        StartCoroutine(spawnEnemy());
    }

    IEnumerator spawnEnemy()
    {
        while(SpawningEnemy) {
            if(SpawnedEnemy < currentDungeon.spawnAmount[enemySpawnIndex]) {
                yield return new WaitForSeconds(currentDungeon.spawnTime[enemySpawnIndex]);
                EnemyManager.Instance.CreateDungeonEnemy(currentDungeon.DungeonEnimes[enemySpawnIndex], Enemy.typeofEnemy.Dungeon);
                SpawnedEnemy++;
            } else {
                yield return new WaitForSecondsRealtime(1);
                if(KilledEnemy >= currentDungeon.spawnAmount[enemySpawnIndex]) {
                    yield return new WaitForSeconds(2);
                    SpawnedEnemy = 0;
                    KilledEnemy = 0;
                    if(enemySpawnIndex < currentDungeon.DungeonEnimes.Length - 2) {
                        enemySpawnIndex++;
                    } else {
                        enemySpawnIndex++;
                        EnemyManager.Instance.CreateDungeonEnemy(currentDungeon.DungeonEnimes[enemySpawnIndex], Enemy.typeofEnemy.DungeonBoss);
                        break;
                    }
                }
            }
        }
    }

    public static void EnemyHit()
    {
        instance.playerHealth--;
        instance.SpawnedEnemy--;
        instance.DungeonHealth.text = instance.playerHealth.ToString();
        if(instance.playerHealth <= 0) {
            instance.dungeonFailed();
            GameManager.instance.PauseGame();
        }
    }

    public static void EnemyDeadNormal()
    {
        instance.KilledEnemy++;
        instance.TotalKilledEnemy++;

        instance.DungeonRemainEnemy.text = (instance.currentDungeon.getTotalEnemy() - instance.TotalKilledEnemy).ToString();
    }
    public static void EnemyDeadBoss()
    {
        instance.dungeonClear();
    }

    public void GiveUP()
    {
        EnemyManager.Instance.removeAllDungeonEnemies();
        dungeonFailed();
    }
    public void dungeonFailed()
    {
        SoundManager.Instance.musicSource.Stop();
        SoundManager.Instance.playDefeat();

        StopAllCoroutines();
        EnemyManager.Instance.removeAllDungeonEnemies();
        GameManager.instance.PauseGame();
        Result_DefeatTab.SetActive(true);

        setDungeonTicketText();
    }
    public void dungeonClear()
    {
        SoundManager.Instance.musicSource.Stop();
        SoundManager.Instance.playVictory();

        StopAllCoroutines();
        EnemyManager.Instance.removeAllDungeonEnemies();
        GameManager.instance.PauseGame();


        Result_VictoryTab.SetActive(true);
        GoldPriceText.text = GameManager.MoneyString(currentDungeon.PriceGold);
        EXPPriceText.text = GameManager.MoneyString(currentDungeon.PriceEXP);

        for(int i = 0; i < stonePriceObj.Length; i++) {
            stonePriceObj[i].SetActive(((int)currentDungeon.DungeonPriceStone.stoneGrade) == i);
        }

        //Dungeon button
        DungeonTicket--;
        setDungeonTicketText();

        //Adding
        GameManager.SetMoney(currentDungeon.PriceGold);
        GameManager.Enemy_Dead_Player_EXP_UP(currentDungeon.PriceEXP);
        UpgradeStoneManager.instance.addStone(currentDungeon.DungeonPriceStone, currentDungeon.PriceStoneAmount);
    }

    public void LoadData(GameData data)
    {
        DungeonTicket = data.DungeonTicket;
        LimitExtraDungeonTicket = data.LimitExtraDungeonTicket;
    }

    public void SaveData(GameData data)
    {
        //Daily Time and Weekly Time
        if(data.ResetTimeDailyDungeon != 0) {
            if(DateTime.Now.DayOfYear - data.ResetTimeDailyDungeon != 0) {
                LimitExtraDungeonTicket = 0;
                DungeonTicket = 3;
                data.ResetTimeDailyDungeon = DateTime.Now.DayOfYear;
            }
        } else {
            LimitExtraDungeonTicket = 0;
            DungeonTicket = 3;
        }


        data.DungeonTicket = DungeonTicket;
        data.LimitExtraDungeonTicket = LimitExtraDungeonTicket;

        setDungeonTicketText();
    }
}
