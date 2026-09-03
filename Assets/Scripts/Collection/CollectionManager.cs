using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectionManager : MonoBehaviour, IDataPersistence
{
    public static CollectionManager Instance;

    [Header("Prefab")]
    [Space(5)]
    public CollectionData Collection_Prefab;
    public List<Enemy> Enemies_Prefab;

    [Header("Transform - Parent")]
    [Space(5)]
    public Transform Parent_Collections;

    [Header("Datas")]
    [Space(5)]
    public List<CollectionData> CollectionDatas;
    public TextMeshProUGUI collectionCollected_text;

    [Header("보상 창")]
    public GameObject DiamondReward_Tab;
    public TextMeshProUGUI DiamondReward_Text;

    [Header("경고 표시")]
    public GameObject ExclamationMark;

    public bool checkingReward;

    bool quitLoop;

    private void Awake()
    {
        Instance = this;

        InvokeRepeating("checkCollectionGUI", 0, 0.5f);
    }

    private void Start()
    {
        foreach(Enemy e in Enemies_Prefab) {
            CreateCollection(e, Parent_Collections);
        }
    }
    public void CreateCollection(Enemy enemy, Transform Parent)
    {
        CollectionData _C = Instantiate(Collection_Prefab);
        CollectionDatas.Add(_C);
        _C.transform.SetParent(Parent, false);
        _C.Collection_MonsterImage.sprite = enemy.RawImage;
        _C.Collection_Stage.text = FindStageName(enemy);
        _C.id = enemy.id;

        if(enemy._typeofEnemy == Enemy.typeofEnemy.normal) {
            _C._typeofEnemy = CollectionData.typeofEnemy.normal;
            _C.Boss_Obj.SetActive(false);
        } else {
            _C._typeofEnemy = CollectionData.typeofEnemy.boss;
            _C.Boss_Obj.SetActive(true);
        }
    }

    public string FindStageName(Enemy enemy)
    {
        if(enemy._typeofEnemy == Enemy.typeofEnemy.normal) {
            for(int i = 0; i < StageManager.instance.planets.Count; i++) {
                for(int x = 0; x < StageManager.instance.planets[i].StagesOfThisPlanet.Length; x++) {
                    for(int j = 0; j < StageManager.instance.planets[i].StagesOfThisPlanet[x].enemiesPrefab.Length; j++) {
                        if(StageManager.instance.planets[i].StagesOfThisPlanet[x].enemiesPrefab[j] == enemy)
                            return StageManager.instance.planets[i].PlanetName + " " + StageManager.instance.planets[i].StagesOfThisPlanet[x].StageName;

                    }
                }
            }
        } else if(enemy._typeofEnemy == Enemy.typeofEnemy.boss) {
            for(int i = 0; i < StageManager.instance.planets.Count; i++) {
                for(int x = 0; x < StageManager.instance.planets[i].StagesOfThisPlanet.Length; x++) {
                    for(int j = 0; j < StageManager.instance.planets[i].StagesOfThisPlanet[x].bossPrefab.Length; j++) {
                        if(StageManager.instance.planets[i].StagesOfThisPlanet[x].bossPrefab[j] == enemy)
                            return StageManager.instance.planets[i].PlanetName + " " + StageManager.instance.planets[i].StagesOfThisPlanet[x].StageName;

                    }
                }
            }
        }
        return "";
    }

    public void addEnemyKilled(Enemy enemy)
    {
        if(enemy._typeofEnemy != Enemy.typeofEnemy.Dungeon) {
            int index = CollectionDatas.IndexOf(CollectionDatas.Find(x => x.id == enemy.id));
            CollectionDatas[index].Killed_Num++;
        }
        checkCollectionGUI();
    }

    public void checkCollectionGUI()
    {
        //Check KilledNum
        for(int i = 0; i < CollectionDatas.Count; i++) {
            CollectionDatas[i].checkEarnDiamond();
        }

        ExclamationMark.SetActive(false);
        for(int i = 0; i < CollectionDatas.Count; i++) {
            for(int j = 0; j < CollectionDatas[i].EarnDiamond.Length; j++) {
                if(CollectionDatas[i].EarnDiamond[j] == 1) {
                    ExclamationMark.SetActive(true);
                    break;
                }
            }
        }

        quitLoop = false;
        if(!DiamondReward_Tab.activeSelf) {
            for(int i = 0; i < CollectionDatas.Count; i++) {
                for(int j = 0; j < CollectionDatas[i].EarnDiamond.Length; j++) {
                    if(CollectionDatas[i].EarnDiamond[j] == 1) {
                        if(j == 0) {
                            EarnDiamond(20);
                        } else if(j == 1) {
                            EarnDiamond(40);
                        } else if(j == 2) {
                            EarnDiamond(75);
                        } else if(j == 3) {
                            EarnDiamond(100);
                        } else if(j == 4) {
                            EarnDiamond(150);
                        } else if(j == 5) {
                            EarnDiamond(300);
                        }
                        CollectionDatas[i].EarnDiamond[j] = 2;
                        quitLoop = true;
                        break;
                    }
                }

                if(quitLoop)
                    break;
            }
        }


        //Check User Icon
        for(int i = 0; i < CollectionDatas.Count; i++) {
            if(CollectionDatas[i]._typeofEnemy == CollectionData.typeofEnemy.boss) {
                if(CollectionDatas[i].grade != CollectionData.Grade.Unknown) {
                    UserIconManager.Instance.unlockBossIcon(CollectionDatas[i].id);
                }
            }
        }

        int count = 0;
        for(int i = 0; i < CollectionDatas.Count; i++) {
            if(CollectionDatas[i].grade == CollectionData.Grade.Master) {
                count++;
            }
        }
        collectionCollected_text.text = count + "/" + CollectionDatas.Count;
    }


    public void EarnDiamond(int diaAmount)
    {
        GameManager.SetDiamond(diaAmount);
        DiamondReward_Text.text = diaAmount + "개";
        DiamondReward_Tab.SetActive(true);
    }

    public int getHigherGradeCollectionFoundAmount(CollectionData.Grade data)
    {
        int sum = 0;
        for(int i = 0; i < CollectionDatas.Count; i++) {
            if(CollectionDatas[i].grade >= data) {
                sum++;
            }
        }
        return sum;
    }

    public int getGradeCollectionFoundAmount(CollectionData.Grade data)
    {
        int sum = 0;
        for(int i = 0; i < CollectionDatas.Count; i++) {
            if(CollectionDatas[i].grade == data) {
                sum++;
            }
        }
        return sum;
    }

    public int getNormalCollectionFoundAmount()
    {
        int sum = 0;
        for(int i = 0; i < CollectionDatas.Count; i++) {
            if(CollectionDatas[i].grade > 0 && CollectionDatas[i]._typeofEnemy == CollectionData.typeofEnemy.normal) {
                sum++;
            }
        }
        return sum;
    }

    public int getBossCollectionFoundAmount()
    {
        int sum = 0;
        for(int i = 0; i < CollectionDatas.Count; i++) {
            if(CollectionDatas[i].grade > 0 && CollectionDatas[i]._typeofEnemy == CollectionData.typeofEnemy.boss) {
                sum++;
            }
        }
        return sum;
    }

    public bool getEnemyKilledNumber(Enemy e)
    {
        for(int i = 0; i < CollectionDatas.Count; i++) {
            if(CollectionDatas[i].id == e.id) {
                return CollectionDatas[i].grade >= CollectionData.Grade.Silver;
            }
        }
        return false;
    }

    public void LoadData(GameData data)
    {
        for(int i = 0; i < CollectionDatas.Count; i++) {
            data.Collection_KilledAmount.TryGetValue(CollectionDatas[i].id, out int value);
            CollectionDatas[i].Killed_Num = value;

            data.Collection_Grade.TryGetValue(CollectionDatas[i].id, out int value2);
            CollectionDatas[i].grade = (CollectionData.Grade)value2;

            for(int j = 0; j < CollectionDatas[i].EarnDiamond.Length; j++) {
                data.Collection_EarnDiamond.TryGetValue(CollectionDatas[i].id + "EarnDiamond" + j, out int value3);
                CollectionDatas[i].EarnDiamond[j] = value3;
            }
        }
    }

    public void SaveData(GameData data)
    {
        for(int i = 0; i < CollectionDatas.Count; i++) {
            if(data.Collection_KilledAmount.ContainsKey(CollectionDatas[i].id))
                data.Collection_KilledAmount.Remove(CollectionDatas[i].id);

            data.Collection_KilledAmount.Add(CollectionDatas[i].id, CollectionDatas[i].Killed_Num);

            //등급
            if(data.Collection_Grade.ContainsKey(CollectionDatas[i].id))
                data.Collection_Grade.Remove(CollectionDatas[i].id);

            data.Collection_Grade.Add(CollectionDatas[i].id, ((int)CollectionDatas[i].grade));

            for(int j = 0; j < CollectionDatas[i].EarnDiamond.Length; j++) {
                if(data.Collection_EarnDiamond.ContainsKey(CollectionDatas[i].id + "EarnDiamond" + j))
                    data.Collection_EarnDiamond.Remove(CollectionDatas[i].id + "EarnDiamond" + j);

                data.Collection_EarnDiamond.Add(CollectionDatas[i].id + "EarnDiamond" + j, CollectionDatas[i].EarnDiamond[j]);
            }
        }
    }
}
