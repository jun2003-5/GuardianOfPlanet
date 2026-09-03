using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using TMPro;
using System.Threading.Tasks;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    [Header("#----Map Scroll Rect")]
    public ScrollRect scrollRect;
    public RectTransform contentPanel;
    [Header("#----Parellel Map Scroll Rect")]
    public ScrollRect scrollRect_Parellel;
    public RectTransform contentPanel_Parellel;

    [Header("#-----Planets")]
    public List<Planet> planets;
    [HideInInspector]
    public Planet currentPlanet;
    StageData currentStage;

    [Header("#-----UIs")]
    [Space(20)]
    public TextMeshProUGUI RemainedMonster_Text;
    public Image PlanetImage;

    [Header("#-----Result UIS")]
    [Space(20)]
    public GameObject StageFailed;
    public GameObject StageCleared;
    public GameObject GoldText;
    public GameObject DiamondText;
    public GameObject[] stoneObjs;
    public Button nextStageButton;

    public GameObject WatchAdButton;
    public GameObject PopUpAdButton;

    [Header("#-----Messages")]
    [Space(20)]
    public GameObject WarningObject;
    public GameObject PlanetPackageTab;

    [Header("#----Pause Game")]
    public GameObject PauseTab;
    public TextMeshProUGUI pauseTab_CurentStage;

    bool SpawningEnemy;
    bool InBattle;
    bool BossSpawned;
    int KilledEnemy;
    int SpawnedEnemy;

    public bool GameSpeedBought;

    public bool isInStage;

    //Reward Double Temps
    long rd_Money;
    int rd_Stone;
    int rd_Diamond;

    public void Subscribe(Planet planet)
    {
        if(planets == null)
            planets = new List<Planet>();

        planets.Add(planet);
    }

    private void Awake()
    {
        instance = this;

        for(int i = 14; i < planets.Count; i++) {
            for(int j = 0; j < planets[j].StagesOfThisPlanet.Length; j++) {
                planets[i].StagesOfThisPlanet[j].goldPrice *= 10;
                planets[i].StagesOfThisPlanet[j].DiamondPrice *= 2;
                planets[i].StagesOfThisPlanet[j].stonePriceAmount *= 2;
            }
        }
    }

    void Start()
    {
        planets.Sort(SortByScore);
        if(planets[0].name.Contains("Home")) {
            Planet temp = planets[0];
            planets[0] = planets[1];
            planets[1] = temp;
        }
        currentPlanet = planets[0];
        currentStage = currentPlanet.StagesOfThisPlanet[0];
        SetmapInfo(0);
    }

    void Update()
    {
        //Under stats
        if(InBattle) {
            if(KilledEnemy >= currentStage.EnemiesToKill && !BossSpawned) {
                if(currentStage.bossPrefab.Length != 0) {
                    BossSpawned = true;
                    WarningObject.gameObject.SetActive(true);
                    for(int i = 0; i < currentStage.bossPrefab.Length; i++)
                        EnemyManager.Instance.CreateBoss(currentStage.bossPrefab[i], currentStage.BossScale[i]);
                } else {
                    Cleared();
                }
            }
        }
    }

    public void checkAllPlanetState()
    {
        for(int i = 1; i < planets.Count/2; i++) {
            planets[i].CheckPlanetState(planets[i - 1]);
        }
        for(int i = planets.Count/2 + 1; i < planets.Count; i++) {
            planets[i].CheckPlanetState(planets[i - 1]);
        }

        for(int i = 0; i < planets.Count; i++) {
            planets[i].checkPlanetState();
        }

    }

    public void setMapPosition()
    {
        PlayerPrefs.SetInt("lastAnchoredPosition", (int)contentPanel.anchoredPosition.y);
        PlayerPrefs.SetInt("lastAnchoredPosition2", (int)contentPanel_Parellel.anchoredPosition.y);
    }

    public void getMapPosition()
    {
        contentPanel.anchoredPosition = new Vector2(contentPanel.anchoredPosition.x, PlayerPrefs.GetInt("lastAnchoredPosition"));
        contentPanel_Parellel.anchoredPosition = new Vector2(contentPanel_Parellel.anchoredPosition.x, PlayerPrefs.GetInt("lastAnchoredPosition2"));
    }

    public void StartSpawningEnemy()
    {
        InBattle = true;
        SpawningEnemy = true;
        for(int i = 0; i < currentStage.enemiesPrefab.Length; i++) {
            StartCoroutine(spawnEnemy(currentStage.enemiesPrefab[i], currentStage.SpawnSpeed[i], currentStage.enemyScale[i]));
        }
    }

    IEnumerator spawnEnemy(Enemy e, float SpawningTime, float Scale)
    {
        while(SpawningEnemy) {

            yield return new WaitForSeconds(SpawningTime);
            SpawnedEnemy++;
            EnemyManager.Instance.createEnemy(e, Scale);
            if(SpawnedEnemy >= currentStage.EnemiesToKill && currentStage.bossPrefab.Length == 0) {
                SpawningEnemy = false;
                StopAllCoroutines();
            }
        }
    }

    public static void EnemyDeadNormal(int exp, long gold)
    {
        GameManager.Enemy_Dead_Player_EXP_UP(exp);
        GameManager.Enemy_Dead_Money_UP(gold);
        instance.KilledEnemy++;
        if(instance.KilledEnemy <= instance.currentStage.EnemiesToKill)
            instance.RemainedMonster_Text.text = (instance.currentStage.EnemiesToKill - instance.KilledEnemy).ToString();
        else
            instance.RemainedMonster_Text.text = 0.ToString();
    }
    public static void EnemyDeadBoss(int exp, long gold)
    {
        GameManager.Enemy_Dead_Player_EXP_UP(exp);
        GameManager.Enemy_Dead_Money_UP(gold);
        instance.Cleared();
    }

    public void SetmapInfo(int i)
    {
        currentPlanet.MapSelected(i);
        currentStage = currentPlanet.StagesOfThisPlanet[i];
        PlanetImage.sprite = currentPlanet.PlanetSprite;
    }

    public void Move()
    {
        SetStageBeforeEntering();
    }

    public async void SetStageBeforeEntering()
    {
        //Sound
        SoundManager.Instance.playMusic(SoundManager.MusicType.AdventureBattleBGM);

        TransformTab.instance.gameObject.SetActive(true);
        TransformTab.instance.startFading(currentPlanet.PlanetName);

        //Load GameSpeed and Auto
        GameManager.instance.loadGameSpeedandAuto();

        //Stop Infinite Stage
        InfiniteStage.Instance.PauseInfiniteStage();

        //Default the values
        KilledEnemy = 0;
        SpawnedEnemy = 0;
        BossSpawned = false;
        WarningObject.SetActive(false);

        await Task.Delay(1500);

        GridManager.instance.ToPlanet(currentPlanet.GridIndex);
        RemainedMonster_Text.text = (currentStage.EnemiesToKill - KilledEnemy).ToString();
        await Task.Delay(2000);
        StartSpawningEnemy();

        isInStage = true;
    }

    public void moveToNextStage()
    {
        for(int i = 0; i < currentPlanet.StagesOfThisPlanet.Length; i++) {
            if(currentPlanet.StagesOfThisPlanet[i].StageName == currentStage.StageName) {
                currentStage = currentPlanet.StagesOfThisPlanet[i + 1];
                Move();
                break;
            }
        }
    }

    public void MoveToVillage()
    {
        MoveToVillage2();
    }
    public async void MoveToVillage2()
    {
        //Sound
        SoundManager.Instance.playMainTitleMusic();

        TransformTab.instance.gameObject.SetActive(true);
        TransformTab.instance.startFading("지구");

        //게임 속도
        Time.timeScale = 1;

        await Task.Delay(2000);

        for(int i = 0; i < planets.Count; i++) {
            planets[i].checkPlanetState();
        }

        EnemyManager.Instance.removeAllEnemies();
        GridManager.instance.ToVillage();
        InfiniteStage.Instance.ResumeInfiniteStage();
        isInStage = false;
    }

    public void Pausestage()
    {
        PauseTab.SetActive(true);
        pauseTab_CurentStage.text = currentPlanet.PlanetName + " " + currentStage.StageName;
    }

    public void Cleared()
    {
        InBattle = false;
        StageFailed.SetActive(false);
        StageCleared.SetActive(true);
        ResultTabActive(true);

        SoundManager.Instance.musicSource.Stop();
        SoundManager.Instance.playVictory();
    }

    public void Died()
    {
        InBattle = false;
        StageFailed.SetActive(true);
        StageCleared.SetActive(false);
        ResultTabActive(false);
        SoundManager.Instance.musicSource.Stop();
        SoundManager.Instance.playDefeat();
    }
    public void ResultTabActive(bool a)
    {
        GameManager.instance.saveGameSpeedandAuto(Time.timeScale, WeaponManager.instance.shootType == WeaponManager.ShootType.AutoShoot);
        WeaponManager.instance.shootType = WeaponManager.ShootType.NormalShoot;
        //Next Stage Button
        nextStageButton.interactable = currentPlanet.StagesOfThisPlanet[currentPlanet.StagesOfThisPlanet.Length - 1].StageName != currentStage.StageName;

        //UIS
        EnemyManager.Instance.removeAllEnemies();
        StopAllCoroutines();

        StageCleared.SetActive(a);
        StageFailed.SetActive(!a);

        WatchAdButton.SetActive(true);

        DiamondText.SetActive(false);
        GoldText.SetActive(true);
        for(int i = 0; i < stoneObjs.Length; i++) {
            stoneObjs[i].SetActive(false);
        }

        //Result
        long PriceGold = (long)(currentStage.goldPrice * Mathf.Max(0.3f, 1f - (0.2f * currentStage.ClearedTime)));
        int PriceStone = (int)(currentStage.stonePriceAmount * Mathf.Max(0.3f, 1f - (0.5f * currentStage.ClearedTime)));
        GoldText.GetComponentInChildren<TextMeshProUGUI>().text = GameManager.instance.MoneyStringForInfiniteStage(PriceGold);
        GameManager.SetMoney(PriceGold);
        rd_Money = PriceGold;

        stoneObjs[(int)currentStage.stonePrice].SetActive(true);
        stoneObjs[(int)currentStage.stonePrice].GetComponentInChildren<TextMeshProUGUI>().text = PriceStone.ToString();
        UpgradeStoneManager.instance.addStone(currentStage.stonePrice, PriceStone);
        rd_Stone = PriceStone;

        if(a && !currentStage.StageCleared) {
            DiamondText.gameObject.SetActive(true);
            DiamondText.GetComponentInChildren<TextMeshProUGUI>().text = currentStage.DiamondPrice.ToString();
            GameManager.SetDiamond(currentStage.DiamondPrice);
            rd_Diamond = currentStage.DiamondPrice;
            currentStage.StageCleared = true;
        } else {
            rd_Diamond = 0;
        }

        //ClearedTime ++
        if(a) {
            currentStage.ClearedTime++;
        }


        EnemyManager.Instance.removeAllEnemies();
    }

    public void RewardDoubleAd()
    {
        GoldText.GetComponentInChildren<TextMeshProUGUI>().text = GameManager.instance.MoneyStringForInfiniteStage(rd_Money * 2);
        stoneObjs[(int)currentStage.stonePrice].GetComponentInChildren<TextMeshProUGUI>().text = (rd_Stone * 2).ToString();
        DiamondText.GetComponentInChildren<TextMeshProUGUI>().text = (rd_Diamond * 2).ToString();

        GameManager.SetMoney(rd_Money);
        GameManager.SetDiamond(rd_Diamond);
        UpgradeStoneManager.instance.addStone(currentStage.stonePrice, rd_Stone);
        WatchAdButton.SetActive(false);
        PopUpAdButton.SetActive(false);
    }

    public string FindNextPlanet()
    {
        for(int i = 0; i < currentPlanet.StagesOfThisPlanet.Length; i++) {
            if(currentPlanet.StagesOfThisPlanet[i].StageName == currentStage.StageName) {
                if(i == currentPlanet.StagesOfThisPlanet.Length - 1) {
                    for(int j = 0; j < planets.Count - 1; j++) {
                        if(planets[j].PlanetName == currentPlanet.PlanetName) {
                            return planets[j + 1].PlanetName + " " + planets[j + 1].StagesOfThisPlanet[0].StageName;
                        }
                    }
                } else {
                    return currentPlanet.PlanetName + " " + currentPlanet.StagesOfThisPlanet[i + 1].StageName;
                }
            }
        }
        return "";
    }

    public static int SortByScore(Planet stage1, Planet stage2)
    {
        int stage1Num = int.Parse(Regex.Replace(stage1.name, "[^0-9]", ""));
        int stage2Num = int.Parse(Regex.Replace(stage2.name, "[^0-9]", ""));
        return stage1Num.CompareTo(stage2Num);
    }
}
