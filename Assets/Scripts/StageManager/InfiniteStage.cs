using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfiniteStage : MonoBehaviour, IDataPersistence
{
    public static InfiniteStage Instance;
    public Enemy[] enemyPrefab;
    public Enemy[] bossPrefab;

    [Header("UI")]
    [Space(5)]
    public TextMeshProUGUI stageText;
    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI RemainedMonsterText;

    [Header("#GameObject")]
    public GameObject WarningObject;

    [Header("#---Current Stage Info")]
    public List<int> CurrentEnemyIndex;
    int CurrentBossIndex;

    [Header("#----Stage Failed Tabs")]
    public GameObject ResultTab_Failed;
    public TextMeshProUGUI currentStageText_Failed;
    public Slider timeSlider_Failed;

    [Header("#----Result")]
    public GameObject ResultTab_Cleared;
    public Slider timeSlider_Cleared;

    //Prize
    public GameObject GoldReward_gameObject;
    public GameObject DiamondReward_gameObject;
    public GameObject NormalChestReward_gameObject;
    public GameObject SpecialChestReward_gameObject;
    public GameObject StoneChestReward_gameObject;
    public GameObject NormalStoneReward_gameObject;
    public GameObject RareStoneReward_gameObject;
    public GameObject EpicStoneReward_gameObject;
    public GameObject UniqueStoneReward_gameObject;
    public GameObject LegendaryStoneReward_gameObject;
    public GameObject AncientStoneReward_gameObject;

    [HideInInspector]
    public int playerHealth;

    public int CurrentStage;

    bool SpawningEnemy;
    bool InBattle;
    bool BossSpawned;
    int KilledEnemy;
    int SpawnedEnemy;

    public float EnemyScale;
    float SpawnSpeed;

    //Saved Killed Enemy and Health
    public int tempEnemy;
    public int tempHealth;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(InBattle) {
            if(KilledEnemy >= setEnemyNumber() && !BossSpawned) {
                if(CurrentStage % 10 == 0) {
                    BossSpawned = true;
                    WarningObject.gameObject.SetActive(true);
                    EnemyManager.Instance.CreateInfiniteBoss(bossPrefab[CurrentBossIndex], KilledEnemy * 0.005f);
                } else {
                    CurrentStage++;
                    StartCoroutine(nextStage());
                }
            }
        }
    }

    public void StartSpawningEnemy()
    {
        InBattle = true;
        SpawningEnemy = true;
        for(int i = 0; i < CurrentEnemyIndex.Count; i++) {
            StartCoroutine(spawnEnemy(enemyPrefab[CurrentEnemyIndex[i]]));
        }
    }


    IEnumerator spawnEnemy(Enemy e)
    {
        while(SpawningEnemy) {
            yield return new WaitForSeconds(SpawnSpeed * (1f - (0.1f * (KilledEnemy / Mathf.Max(1, (float)(setEnemyNumber() / 8))))));
            SpawnedEnemy++;
            EnemyManager.Instance.createInfiniteEnemy(e, EnemyScale);
            if(SpawnedEnemy >= setEnemyNumber()) {
                SpawningEnemy = false;
                StopAllCoroutines();
            }
        }
    }


    public static void EnemyDeadNormal(int exp, long gold)
    {
        GameManager.Enemy_Dead_Player_EXP_UP(exp);
        GameManager.Enemy_Dead_Money_UP(gold);
        Instance.KilledEnemy++;
        if(Instance.KilledEnemy <= Instance.setEnemyNumber())
            Instance.RemainedMonsterText.text = (Instance.setEnemyNumber() - Instance.KilledEnemy).ToString();
    }
    public static void EnemyDeadBoss(int exp, long gold)
    {
        Instance.WarningObject.gameObject.SetActive(false);
        GameManager.Enemy_Dead_Player_EXP_UP(exp);
        GameManager.Enemy_Dead_Money_UP(gold);
        Instance.CurrentStage++;
        Instance.StartCoroutine(Instance.nextStage());
    }

    public void enemyHit(Enemy data)
    {
        Instance.KilledEnemy++;

        if(data._typeofEnemy == Enemy.typeofEnemy.InfiniteStage)
            playerHealth--;
        else if(data._typeofEnemy == Enemy.typeofEnemy.InfiniteStageBoss)
            playerHealth -= 2;
        HealthText.text = playerHealth.ToString();
        if(playerHealth <= 0) {
            SpawningEnemy = false;
            InBattle = false;
            EnemyManager.Instance.removeAllEnemies();
            StopAllCoroutines();

            //Auto Restart
            StartCoroutine(restartStage());
        }
    }

    public void SetStageBeforeEntering()
    {
        //Sound
        SoundManager.Instance.playMusic(SoundManager.MusicType.MainTitle);
        GameManager.instance.loadGameSpeedandAuto();

        KilledEnemy = 0;
        SpawnedEnemy = 0;
        BossSpawned = false;
        playerHealth = 3;
        setEnemyIndex();
        setSpawnSpeed();
        setEnemyScale();
        StartSpawningEnemy();
        GridManager.instance.ToVillage();
        HealthText.text = playerHealth.ToString();
        RemainedMonsterText.text = (setEnemyNumber() - KilledEnemy).ToString();
        stageText.text = CurrentStage.ToString();
    }

    public IEnumerator nextStage()
    {
        SoundManager.Instance.playVictory();

        ResultTab_Cleared.SetActive(true);

        //SetPrize
        setPrize();

        //Achievement
        AchievementManager.instance.InfiniteStageDaily++;
        AchievementManager.instance.InfiniteStageWeekly++;

        //LeaderBoard Update
        LeaderBoardManager.Instance.AddInfiniteStageScore();

        stageText.text = CurrentStage.ToString();

        //Default the values
        EnemyManager.Instance.removeAllEnemies();
        KilledEnemy = 0;
        SpawnedEnemy = 0;
        BossSpawned = false;

        //SetIndex
        setEnemyIndex();
        setSpawnSpeed();
        setEnemyScale();
        GridManager.instance.ActiveGrid(CurrentStage / 120);
        RemainedMonsterText.text = (setEnemyNumber() - KilledEnemy).ToString();
        timeSlider_Cleared.maxValue = 1.5f;
        timeSlider_Cleared.value = 1.5f;

        while(timeSlider_Cleared.value > 0) {
            timeSlider_Cleared.value -= Time.deltaTime / Time.timeScale;
            yield return new WaitForSecondsRealtime(0.001f);
        }
        ResultTab_Cleared.SetActive(false);

        yield return new WaitForSecondsRealtime(1.5f);
        StartSpawningEnemy();
    }

    public void setPrize()
    {
        //Gold
        GoldReward_gameObject.SetActive(true);
        GoldReward_gameObject.GetComponentInChildren<TextMeshProUGUI>().text = GameManager.instance.MoneyStringForInfiniteStage(getPrizeMoney());
        GameManager.SetMoney(getPrizeMoney());

        //Diamond
        if((CurrentStage - 1) % 5 == 0) {
            DiamondReward_gameObject.SetActive(true);
            DiamondReward_gameObject.GetComponentInChildren<TextMeshProUGUI>().text = 25.ToString();
            GameManager.SetDiamond(50);
        } else {
            DiamondReward_gameObject.SetActive(false);
        }

        NormalStoneReward_gameObject.SetActive(false);
        RareStoneReward_gameObject.SetActive(false);
        EpicStoneReward_gameObject.SetActive(false);
        UniqueStoneReward_gameObject.SetActive(false);
        LegendaryStoneReward_gameObject.SetActive(false);
        AncientStoneReward_gameObject.SetActive(false);
        int ran = 0;
        //Stone
        if((CurrentStage - 1) % 4 == 0) {
            ran = UnityEngine.Random.Range(0, 100);
            if(ran <= 0.3f) {
                AncientStoneReward_gameObject.SetActive(true);
                AncientStoneReward_gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "1";
                UpgradeStoneManager.instance.addStone(UpgradeStone.TypeOfStone.Ancient, 1);
            } else if(ran <= 1.5f) {
                LegendaryStoneReward_gameObject.SetActive(true);
                LegendaryStoneReward_gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "2";
                UpgradeStoneManager.instance.addStone(UpgradeStone.TypeOfStone.Legendary, 2);
            } else if(ran <= 5) {
                UniqueStoneReward_gameObject.SetActive(true);
                UniqueStoneReward_gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "5";
                UpgradeStoneManager.instance.addStone(UpgradeStone.TypeOfStone.Unique, 5);
            } else if(ran <= 15) {
                EpicStoneReward_gameObject.SetActive(true);
                EpicStoneReward_gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "10";
                UpgradeStoneManager.instance.addStone(UpgradeStone.TypeOfStone.Epic, 10);
            } else if(ran <= 35) {
                RareStoneReward_gameObject.SetActive(true);
                RareStoneReward_gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "30";
                UpgradeStoneManager.instance.addStone(UpgradeStone.TypeOfStone.Rare, 30);
            } else if(ran <= 100) {
                NormalStoneReward_gameObject.SetActive(true);
                NormalStoneReward_gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "60";
                UpgradeStoneManager.instance.addStone(UpgradeStone.TypeOfStone.Normal, 60);
            }
        }
        //Ticket
        if((CurrentStage - 1) % 10 == 0) {
            if((CurrentStage - 1) % 20 == 0) {
                SpecialChestReward_gameObject.SetActive(true);
                NormalChestReward_gameObject.SetActive(false);
                SpecialChestReward_gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "x1";
                GachaManager.Instance.SpecialGachaTicket++;
            } else {
                SpecialChestReward_gameObject.SetActive(false);
                NormalChestReward_gameObject.SetActive(true);
                NormalChestReward_gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "x1";
                GachaManager.Instance.NormalGachaTicket++;
            }
        } else {
            SpecialChestReward_gameObject.SetActive(false);
            NormalChestReward_gameObject.SetActive(false);
        }

        //Stone
        if((CurrentStage - 1) % 5 == 0) {
            int ranNum = UnityEngine.Random.Range(1, 5);
            GachaManager.Instance.addTicket(GachaData.GachaType.UpgradeStone, ranNum);
            StoneChestReward_gameObject.SetActive(true);
            StoneChestReward_gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "x1";
        } else {
            StoneChestReward_gameObject.SetActive(false);
        }
    }

    public long getPrizeMoney()
    {
        long sum = (long)(CurrentStage * 500) + (long)Mathf.Pow((CurrentStage / 120), 8);
        if(sum > 500000000)
            sum = 500000000 + CurrentStage * 100000;
        return sum;
    }

    IEnumerator restartStage()
    {
        SoundManager.Instance.playDefeat();

        ResultTab_Failed.SetActive(true);
        currentStageText_Failed.text = "스테이지 " + CurrentStage;

        timeSlider_Failed.maxValue = 3;
        timeSlider_Failed.value = 3;

        while(timeSlider_Failed.value > 0) {
            timeSlider_Failed.value -= Time.deltaTime / Time.timeScale;
            yield return new WaitForSecondsRealtime(0.001f);
        }
        ResultTab_Failed.SetActive(false);
        SetStageBeforeEntering();
    }

    public int setEnemyNumber()
    {
        if(CurrentStage / 80 < StageManager.instance.planets.Count) {
            if(StageManager.instance.planets[CurrentStage / 80].PlanetCleared) {
                return (10 + CurrentStage)/2;
            } else {
                return 10 + CurrentStage;
            }
        } else {
            return 10 + CurrentStage;
        }
    }

    public void setEnemyScale()
    {
        if(CurrentStage / 120 < StageManager.instance.planets.Count-1) {
            EnemyScale = (CurrentStage % 120) * 0.025f;
        } else {
            EnemyScale = (CurrentStage - ((StageManager.instance.planets.Count-1) * 120)) * 0.005f;
        }
    }

    public void setEnemyIndex()
    {
        CurrentEnemyIndex[0] = (CurrentStage / 30);
        CurrentEnemyIndex[1] = (CurrentStage / 30) + 1;

        if(CurrentEnemyIndex[0] > enemyPrefab.Length - 2) {
            CurrentEnemyIndex[0] = enemyPrefab.Length - 2;
        }

        if(CurrentEnemyIndex[1] > enemyPrefab.Length - 1) {
            CurrentEnemyIndex[1] = enemyPrefab.Length - 1;
        }
    }

    public void setSpawnSpeed()
    {
        SpawnSpeed = 2.5f - (CurrentStage * 0.01f);
        if(SpawnSpeed <= 0.25f)
            SpawnSpeed = 0.25f;
    }

    public void ResumeInfiniteStage()
    {
        SoundManager.Instance.playMusic(SoundManager.MusicType.MainTitle);
        GameManager.instance.loadGameSpeedandAuto();

        ResultTab_Cleared.SetActive(false);
        ResultTab_Failed.SetActive(false);


        KilledEnemy = tempEnemy;
        SpawnedEnemy = KilledEnemy;
        playerHealth = tempHealth;
        BossSpawned = false;
        setEnemyIndex();
        setSpawnSpeed();
        setEnemyScale();
        StartSpawningEnemy();

        HealthText.text = playerHealth.ToString();
        RemainedMonsterText.text = (setEnemyNumber() - KilledEnemy).ToString();
        stageText.text = CurrentStage.ToString();
    }

    public void PauseInfiniteStage()
    {
        tempEnemy = KilledEnemy;
        tempHealth = playerHealth;
        EnemyManager.Instance.removeAllEnemies();
        StopAllCoroutines();
        InBattle = false;
    }

    public void LoadData(GameData data)
    {
        CurrentStage = data.StageIndex;
    }

    public void SaveData(GameData data)
    {
        data.StageIndex = CurrentStage;
    }
}
