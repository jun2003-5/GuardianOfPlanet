
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Planet : MonoBehaviour, IDataPersistence
{
    [SerializeField] public string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public string PlanetName;

    public Sprite PlanetSprite;

    public StageData[] StagesOfThisPlanet;

    [Header("#Planet Image")]
    public GameObject Planet_Map;

    [Header("#Level Map Components")]
    public Transform LevelMap_Pfb_Parent;
    public Image PlanetImage;
    public TextMeshProUGUI PlanetTitle;
    public TextMeshProUGUI TotalEnemy;

    [Header("#Stage Info")]
    public List<LabMonsterData> monsterCardData;

    public Image MonsterImage;
    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI MoveSpeedText;
    public TextMeshProUGUI GoldText;
    public TextMeshProUGUI ExpText;
    public TextMeshProUGUI StunText;
    public TextMeshProUGUI StageInfoTitle;
    public int GridIndex;

    [Header("Cleared")]
    public bool PlanetCleared;

    public void CreateLevelMap(string stageName, bool a)
    {
        for(int i = 0; i < LevelMap_Pfb_Parent.childCount; i++) {
            if(!LevelMap_Pfb_Parent.GetChild(i).gameObject.activeSelf) {
                LevelMap_Pfb_Parent.GetChild(i).gameObject.SetActive(true);
                LevelMap_Pfb_Parent.GetChild(i).gameObject.GetComponentInChildren<TextMeshProUGUI>().text = stageName;
                LevelMap_Pfb_Parent.GetChild(i).gameObject.GetComponentInChildren<TextMeshProUGUI>().color = a ? Color.black : new Color(0, 0, 0, 0.5f);
                LevelMap_Pfb_Parent.GetChild(i).gameObject.GetComponent<Button>().interactable = a;
                break;
            }
        }
    }

    public void SetMapInfo(Enemy enemy, float scale)
    {
        for(int i = 0; i < monsterCardData.Count; i++) {
            if(!monsterCardData[i].gameObject.activeSelf) {
                monsterCardData[i].gameObject.SetActive(true);
                monsterCardData[i].setMonsterData(enemy, enemy._typeofEnemy == Enemy.typeofEnemy.boss);
                monsterCardData[i].Scale = scale;
                monsterCardData[i].currentPlanet = this;
                break;
            }
        }
    }

    public void mapInfoMonsterClicked(LabMonsterData data)
    {
        MonsterImage.sprite = data.monsterImage.sprite;
        HealthText.text = string.Format("{0:#,###}", data.enemy.EnemyHealth * (1 + data.Scale));
        MoveSpeedText.text = string.Format("{0:#,##0.##}", data.enemy.enemyMovingSpeed * (1 + data.Scale / 50));
        GoldText.text = string.Format("{0:#,###}", (long)(data.enemy.enemy_Money * (1 + data.Scale / 5)));
        ExpText.text = string.Format("{0:#,###}", (long)(data.enemy.Enemy_EXP * (1 + data.Scale / 5)));
        StunText.text = data.enemy.StunResistence + "%";
    }

    public void SetPlanetInformation()
    {
        for(int i = 0; i < LevelMap_Pfb_Parent.childCount; i++) 
            LevelMap_Pfb_Parent.GetChild(i).gameObject.SetActive(false);


        for(int i = 0; i < StagesOfThisPlanet.Length; i++) {
            CreateLevelMap(StagesOfThisPlanet[i].StageName, i > 0 ? StagesOfThisPlanet[i-1].StageCleared : true);
        }

        MapSelected(0);
        //Planet Image
        PlanetImage.sprite = PlanetSprite;
        PlanetTitle.text = PlanetName;

        StageManager.instance.currentPlanet = this;
    }

    public void MapSelected(int index)
    {
        StageInfoTitle.text = PlanetName + " " + StagesOfThisPlanet[index].StageName;
        TotalEnemy.text = "출현 몬스터: " + StagesOfThisPlanet[index].EnemiesToKill + "마리";
        for(int i = 0; i < monsterCardData.Count; i++)
            monsterCardData[i].gameObject.SetActive(false);

        //Color of Map
        for(int i = 0; i < LevelMap_Pfb_Parent.childCount; i++) {
            if(i == index)
                LevelMap_Pfb_Parent.GetChild(i).GetComponent<Image>().color = new Color(0.08735882f, 0.7647059f, 0.003604494f);
            else
                LevelMap_Pfb_Parent.GetChild(i).GetComponent<Image>().color = Color.white;
        }

        for(int i = 0; i < StagesOfThisPlanet[index].enemiesPrefab.Length; i++) {
            SetMapInfo(StagesOfThisPlanet[index].enemiesPrefab[i], StagesOfThisPlanet[index].enemyScale[i]);
        }
        for(int i = 0; i < StagesOfThisPlanet[index].bossPrefab.Length; i++) {
            SetMapInfo(StagesOfThisPlanet[index].bossPrefab[i], StagesOfThisPlanet[index].BossScale[i]);
        }
    }

    public void CheckPlanetState(Planet p)
    {
        int j = 0;
        for(int i = 0; i < p.StagesOfThisPlanet.Length; i++) {
            if(!p.StagesOfThisPlanet[i].StageCleared)
                j++;
        }
        if(j == 0) {
            Planet_Map.GetComponent<Button>().interactable = true;
            Planet_Map.GetComponent<Image>().color = Color.white;
        } else {
            Planet_Map.GetComponent<Button>().interactable = false;
            Planet_Map.GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 1f);
        }
    }

    public void checkPlanetState()
    {
        //If All Cleared 
        int j = 0;
        for(int i = 0; i < StagesOfThisPlanet.Length; i++) {
            if(!StagesOfThisPlanet[i].StageCleared)
                j++;
        }
        if(j == 0) {
            UserIconManager.Instance.unlockPlanetIcon(StageManager.instance.planets.IndexOf(this));

            if(!PlanetCleared) {
                PlanetCleared = true;
                ShopManager.Instance.planetClearPackage.isActivate = true;
                ShopManager.Instance.planetClearPackage.startEvent();
                ShopManager.Instance.planetClearPackage.setGUI();

                StageManager.instance.PlanetPackageTab.SetActive(true);
            }
        }
    }

    public void LoadData(GameData data)
    {
        for(int i = 0; i < StagesOfThisPlanet.Length; i++) {
            data.Stage_Cleared.TryGetValue(id + StagesOfThisPlanet[i].StageName, out bool Killed);
            StagesOfThisPlanet[i].StageCleared = Killed;
        }

        //Planet Clear
        data.Planet_Cleared.TryGetValue(id, out bool clear);
        PlanetCleared = clear;
    }

    public void SaveData(GameData data)
    {
        for(int i = 0; i < StagesOfThisPlanet.Length; i++) {
            if(data.Stage_Cleared.ContainsKey(id + StagesOfThisPlanet[i].StageName))
                data.Stage_Cleared.Remove(id + StagesOfThisPlanet[i].StageName);

            data.Stage_Cleared.Add(id + StagesOfThisPlanet[i].StageName, StagesOfThisPlanet[i].StageCleared);
        }

        if(data.Planet_Cleared.ContainsKey(id))
            data.Planet_Cleared.Remove(id);

        data.Planet_Cleared.Add(id, PlanetCleared);
    }
}