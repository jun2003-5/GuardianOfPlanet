using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public class Labmanager : MonoBehaviour
{
    public static Labmanager instance;

    [Header("#-----Lab Setting Tab")]
    public GameObject LabTab;
    public TabGroup tabgroup;
    public TMP_Dropdown Setting_DRP_SpawnTime;
    public TMP_Dropdown Setting_DRP_SpawnAmount;

    public LabMonsterData monsterCard_pfb;
    public Transform monsterCard_Parent;

    public List<LabMonsterData> cards_data;

    [Header("#----Tabs")]
    public GameObject ErrorTab;
    public GameObject ToVillageTab;

    [Header("#---Controller")]
    public GameObject ControllerTab;
    public Button StartBtn;
    public Button StopBtn;
    public Button RestartBtn;
    public TMP_Dropdown controller_DRP_SpawnTime;
    public TMP_Dropdown controller_DRP_SpawnAmount;
    public LabMonsterData controller_Card;

    public EXPBar spawnProgressBar;

    float spawnCoolTime;
    int spawningEnemyAmount;
    Enemy currentSelectedEnemy;

    int spawnedAmount;
    bool SpawningEnemy;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for(int i = 0; i < CollectionManager.Instance.Enemies_Prefab.Count; i++) {
            createMonsterDataCard(CollectionManager.Instance.Enemies_Prefab[i], false);
            if((i + 1) % 4 == 0) {
                for(int j = (((i + 1) / 4) - 1) * 2; j < (((i + 1) / 4) - 1) * 2 + 2; j++) {
                //    createMonsterDataCard(CollectionManager.Instance.Boss_Prefab[j], true);
                }
            }
        }
    }

    private void Update()
    {
        StartBtn.interactable = (currentSelectedEnemy != null && spawnCoolTime != 0 && spawningEnemyAmount != 0) && !SpawningEnemy && EnemyManager.Instance.enemyData.Count <= 0;
        StopBtn.interactable = (currentSelectedEnemy != null && spawnCoolTime != 0 && spawningEnemyAmount != 0) && SpawningEnemy || EnemyManager.Instance.enemyData.Count > 0;
        RestartBtn.interactable = currentSelectedEnemy != null && spawnCoolTime != 0 && spawningEnemyAmount != 0 && SpawningEnemy || EnemyManager.Instance.enemyData.Count > 0;
        spawnProgressBar.gameObject.SetActive(SpawningEnemy);
    }

    public void enemySelected(Enemy data)
    {
        currentSelectedEnemy = data;
        controller_Card.setMonsterData(data, data._typeofEnemy == Enemy.typeofEnemy.boss || data._typeofEnemy == Enemy.typeofEnemy.DungeonBoss || data._typeofEnemy == Enemy.typeofEnemy.InfiniteStageBoss);
    }

    public void startSpawningEnemy()
    {
        if(currentSelectedEnemy != null && spawnCoolTime != 0 && spawningEnemyAmount != 0) {
            ControllerTab.SetActive(true);
            LabTab.SetActive(false);
            SpawningEnemy = true;
            spawnedAmount = 0;
            if(spawningEnemyAmount < 10000000)
                spawnProgressBar.setProgress(spawnedAmount, spawningEnemyAmount);
            else
                spawnProgressBar.setProgress(0, 0);
            StartCoroutine(spawnEnemy(currentSelectedEnemy));
        } else if(currentSelectedEnemy == null || spawnCoolTime == 0 || spawningEnemyAmount == 0) {
            ErrorTab.SetActive(true);
        }
    }

    IEnumerator spawnEnemy(Enemy e)
    {
        while(SpawningEnemy) {
            yield return new WaitForSeconds(spawnCoolTime);
            spawnedAmount++;
            //SpawnBar
            if(spawningEnemyAmount < 10000000)
                spawnProgressBar.setProgress(spawnedAmount, spawningEnemyAmount);
            else
                spawnProgressBar.setProgress(0,0);
            EnemyManager.Instance.CreateLabMonster(e);
            if(spawnedAmount >= spawningEnemyAmount) {
                SpawningEnemy = false;
                StopAllCoroutines();
            }
        }
    }

    public void controller_Start()
    {
        StopAllCoroutines();
        EnemyManager.Instance.removeAllEnemies();
        SpawningEnemy = false;
        startSpawningEnemy();
    }
    public void controller_Stop()
    {
        StopAllCoroutines();
        EnemyManager.Instance.removeAllEnemies();
        SpawningEnemy = false;
    }
    public void controller_Restart()
    {
        StopAllCoroutines();
        EnemyManager.Instance.removeAllEnemies();
        SpawningEnemy = false;
        startSpawningEnemy();
    }

    //Lab Setting Tab
    public void createMonsterDataCard(Enemy data, bool boss)
    {
        LabMonsterData _l = Instantiate(monsterCard_pfb, monsterCard_Parent);
        _l.setMonsterData(data, boss);
        _l.GetComponent<TabButton>().tabGroup = tabgroup;
        cards_data.Add(_l);
    }

    public void setAbleCard()
    {
        for(int i = 0; i < cards_data.Count; i++) {
            cards_data[i].gameObject.SetActive(CollectionManager.Instance.getEnemyKilledNumber(cards_data[i].enemy));
        }

        for(int i = 0; i < tabgroup.tabButtons.Count; i++) {
            if(tabgroup.tabButtons[i].gameObject.activeSelf) {
                tabgroup.SelectTabbyIndex(tabgroup.tabButtons[i]);
                enemySelected(tabgroup.tabButtons[i].gameObject.GetComponent<LabMonsterData>().enemy);
                break;
            }
        }
    }

    public void setSpawnTime(int index)
    {
        switch(index) {
            case 0: spawnCoolTime = 1; break;
            case 1: spawnCoolTime = 0.1f; break;
            case 2: spawnCoolTime = 0.25f; break;
            case 3: spawnCoolTime = 0.5f; break;
            case 4: spawnCoolTime = 3; break;
            case 5: spawnCoolTime = 5; break;
            case 6: spawnCoolTime = 10; break;
            case 7: spawnCoolTime = 15; break;
            case 8: spawnCoolTime = 30; break;
        }
        Setting_DRP_SpawnTime.value = index;
        controller_DRP_SpawnTime.value = index;
    }

    public void setSpawnAmount(int index)
    {
        switch(index) {
            case 0: spawningEnemyAmount = 1; break;
            case 1: spawningEnemyAmount = 5; break;
            case 2: spawningEnemyAmount = 10; break;
            case 3: spawningEnemyAmount = 30; break;
            case 4: spawningEnemyAmount = 50; break;
            case 5: spawningEnemyAmount = 100; break;
            case 6: spawningEnemyAmount = 300; break;
            case 7: spawningEnemyAmount = int.MaxValue-1; break;
        }
        Setting_DRP_SpawnAmount.value = index;
        controller_DRP_SpawnAmount.value = index;
    }

    public void openLabSetting()
    {
        LabTab.SetActive(true);
        setAbleCard();

        if(spawnCoolTime == 0) {
            spawnCoolTime = 1;
        }

        if(spawningEnemyAmount == 0) {
            spawningEnemyAmount = 1;
        }
    }

    public async void enterLab()
    {
        WeaponManager.instance.shootType = WeaponManager.ShootType.NormalShoot;
        TransformTab.instance.gameObject.SetActive(true);
        TransformTab.instance.startFading("실험실");
        await Task.Delay(1500);
        GridManager.instance.ToLab();
    }

    public void returnToVillage()
    {
        controller_Stop();
        StartCoroutine(ToVillage());
    }

    public IEnumerator ToVillage()
    {
        TransformTab.instance.gameObject.SetActive(true);
        TransformTab.instance.startFading("마을");
        yield return new WaitForSecondsRealtime(1.5f);
        GridManager.instance.ToVillage();
    }
}
