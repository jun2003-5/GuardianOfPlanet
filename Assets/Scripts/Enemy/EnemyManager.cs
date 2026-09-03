using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IDataPersistence
{
    public static EnemyManager Instance;

    [Header("적")]
    [SerializeField] public List<Enemy> enemyData;
    public Enemy[] BossEnemies;
    [SerializeField] public List<Enemy> bossData;
    public int enemyKilled_Number;
    public List<int> bossKilled_number;
    public int totalBossKilled;

    public long totalKilledEnemy;

    public Transform TopEnemySpawnLoc;

    private void Start()
    {
        Instance = this;
        for(int i = 0; i < BossEnemies.Length; i++) {
            bossKilled_number.Add(i);
        }
    }
    public Enemy FindCloseEnemy()
    {
        Enemy closeEnemy = null;

        if(bossData.Count > 0 || enemyData.Count > 0) {
            //가장 가까운 거리의 적
            float distance = 100000;
            for(int i = 0; i < bossData.Count; i++) {
                if(distance >= bossData[i].distance && bossData[i].IsInRange) {
                    distance = bossData[i].distance;
                    closeEnemy = bossData[i];
                }
            }

            for(int i = 0; i < enemyData.Count; i++) {
                if(distance >= enemyData[i].distance && enemyData[i].IsInRange) {
                    if(closeEnemy == null) {
                        closeEnemy = enemyData[i];
                        distance = enemyData[i].distance;
                    } else {
                        if(closeEnemy._typeofEnemy == Enemy.typeofEnemy.boss || closeEnemy._typeofEnemy == Enemy.typeofEnemy.DungeonBoss || closeEnemy._typeofEnemy == Enemy.typeofEnemy.InfiniteStageBoss) {
                            if(enemyData[i].distance <= 3) {
                                closeEnemy = enemyData[i];
                                distance = enemyData[i].distance;
                            }
                        } else {
                            closeEnemy = enemyData[i];
                            distance = enemyData[i].distance;
                        }
                    }
                }
            }
        }
        return closeEnemy;
    }

    public Enemy FindCloseEnemy(BounceEnemy bounceEnemy)
    {
        Enemy closeEnemy = null;

        if(bossData.Count > 0 || enemyData.Count > 0) {
            //가장 가까운 거리의 적
            float distance = 100000;
            for(int i = 0; i < bossData.Count; i++) {
                if(distance >= bossData[i].distance && bossData[i].IsInRange && !bounceEnemy.checkBouncedEnemyisInList(bossData[i])) {
                    distance = bossData[i].distance;
                    closeEnemy = bossData[i];
                }
            }

            for(int i = 0; i < enemyData.Count; i++) {
                if(distance >= enemyData[i].distance && enemyData[i].IsInRange && !bounceEnemy.checkBouncedEnemyisInList(enemyData[i])) {
                    if(closeEnemy == null) {
                        closeEnemy = enemyData[i];
                        distance = enemyData[i].distance;
                    } else {
                        if(closeEnemy._typeofEnemy == Enemy.typeofEnemy.boss || closeEnemy._typeofEnemy == Enemy.typeofEnemy.DungeonBoss || closeEnemy._typeofEnemy == Enemy.typeofEnemy.InfiniteStageBoss) {
                            if(enemyData[i].distance <= 3) {
                                closeEnemy = enemyData[i];
                                distance = enemyData[i].distance;
                            }
                        } else {
                            closeEnemy = enemyData[i];
                            distance = enemyData[i].distance;
                        }
                    }
                }
            }
        }

        if(closeEnemy != null) {
            bounceEnemy.BouncedEnemies.Add(closeEnemy);
        }
        return closeEnemy;
    }

    public Enemy FindCloseEnemyWithoutRange()
    {
        Enemy closeEnemy = null;

        if(bossData.Count > 0 || enemyData.Count > 0) {
            //가장 가까운 거리의 적
            float distance = 100000;
            for(int i = 0; i < bossData.Count; i++) {
                if(distance >= bossData[i].distance) {
                    distance = bossData[i].distance;
                    closeEnemy = bossData[i];
                }
            }

            if(closeEnemy == null) {
                for(int i = 0; i < enemyData.Count; i++) {
                    if(distance >= enemyData[i].distance) {
                        closeEnemy = enemyData[i];
                        distance = enemyData[i].distance;
                    }
                }
            }
        }
        return closeEnemy;
    }

    public Enemy FindCrowdedEnemy()
    {
        Enemy e = null;
        int max = 0;

        int count = 0;
        for(int i = 0; i < enemyData.Count; i++) {
            count = 0;
            for(int j = 0; j < enemyData.Count; j++) {
                if(Vector2.Distance(enemyData[i].transform.position, enemyData[j].transform.position) <= 1) {
                    count++;
                }
            }

            if(count > max) {
                max = count;
                e = enemyData[i];
            } else if(count == max) {
                if(e != null) {
                    if(e.distance >= enemyData[i].distance) {
                        e = enemyData[i];
                    }
                }
            }

        }

        return e;

    }

    public void createEnemy(Enemy _e)
    {
        Enemy enemy = ObjectPoolEnemy.Instance.GetPoolObject(_e.typePool).GetComponent<Enemy>();
        int Side = Random.Range(1, 5);
        Vector2 ranLoc = RandomLocation(Side);
        enemy._typeofEnemy = Enemy.typeofEnemy.normal;
        enemy.transform.position = new Vector3(ranLoc.x, ranLoc.y, 0);
        enemy.gameObject.SetActive(true);
        enemy.CurrentHealth = enemy.EnemyHealth;
        enemyData.Add(enemy);
    }
    public void createEnemy(Enemy _e, float scale)
    {
        Enemy enemy = ObjectPoolEnemy.Instance.GetPoolObject(_e.typePool).GetComponent<Enemy>();
        int Side = Random.Range(1, 5);
        Vector2 ranLoc = RandomLocation(Side);
        enemy._typeofEnemy = Enemy.typeofEnemy.normal;
        enemy.transform.position = new Vector3(ranLoc.x, ranLoc.y, 0);
        enemy.gameObject.SetActive(true);
        enemy.EnemyHealth *= (1 + scale);
        enemy.enemyMovingSpeed *= (1 + scale / 50);
        enemy.enemy_Money = (long)(enemy.enemy_Money * (1 + scale / 5));
        enemy.Enemy_EXP = (int)(enemy.Enemy_EXP * (1 + scale / 5));
        enemy.StunResistence *= (1 + scale);
        enemy.CurrentHealth = enemy.EnemyHealth;
        enemyData.Add(enemy);
    }

    public void CreateBoss(Enemy _e, float scale)
    {
        Enemy enemy = ObjectPoolEnemy.Instance.GetPoolObject(_e.typePool).GetComponent<Enemy>();
        int Side = Random.Range(1, 3);
        Vector2 ranLoc = RandomLocation(Side * 2);
        enemy._typeofEnemy = Enemy.typeofEnemy.boss;
        enemy.transform.position = new Vector3(ranLoc.x, ranLoc.y, 0);
        enemy.gameObject.SetActive(true);
        enemy.EnemyHealth *= (1 + scale);
        enemy.enemy_Money = (long)(enemy.enemy_Money * (1 + scale / 5));
        enemy.Enemy_EXP = (int)(enemy.Enemy_EXP * (1 + scale / 5));
        enemy.CurrentHealth = enemy.EnemyHealth;
        bossData.Add(enemy);
    }

    public void createInfiniteEnemy(Enemy _e, float scale)
    {
        Enemy enemy = ObjectPoolEnemy.Instance.GetPoolObject(_e.typePool).GetComponent<Enemy>();
        int Side = Random.Range(1, 5);
        Vector2 ranLoc = RandomLocation(Side);
        enemy._typeofEnemy = Enemy.typeofEnemy.InfiniteStage;
        enemy.transform.position = new Vector3(ranLoc.x, ranLoc.y, 0);
        enemy.gameObject.SetActive(true);
        enemy.EnemyHealth *= (1 + scale);
        enemy.enemyMovingSpeed *= (1 + scale / 50);
        enemy.enemy_Money = (long)(enemy.enemy_Money * (1 + scale / 5));
        enemy.Enemy_EXP = (int)(enemy.Enemy_EXP * (1 + scale / 5));
        enemy.StunResistence *= (1 + scale);
        enemy.CurrentHealth = enemy.EnemyHealth;
        enemyData.Add(enemy);
    }

    public void CreateInfiniteBoss(Enemy _e, float scale)
    {
        Enemy enemy = ObjectPoolEnemy.Instance.GetPoolObject(_e.typePool).GetComponent<Enemy>();
        int Side = Random.Range(1, 3);
        Vector2 ranLoc = RandomLocation(Side * 2);
        enemy._typeofEnemy = Enemy.typeofEnemy.InfiniteStageBoss;
        enemy.transform.position = new Vector3(ranLoc.x, ranLoc.y, 0);
        enemy.gameObject.SetActive(true);
        enemy.EnemyHealth *= (1 + scale);
        enemy.enemy_Money = (long)(enemy.enemy_Money * (1 + scale / 5));
        enemy.Enemy_EXP = (int)(enemy.Enemy_EXP * (1 + scale / 5));
        enemy.CurrentHealth = enemy.EnemyHealth;
        bossData.Add(enemy);
    }

    public void CreateDungeonEnemy(Enemy _e, Enemy.typeofEnemy type)
    {
        Enemy enemy = ObjectPoolDungeon.Instance.GetPoolObject(_e.typeDungeonPool).GetComponent<Enemy>();
        int Side = Random.Range(1, 5);
        Vector2 ranLoc = RandomLocation(Side);
        enemy._typeofEnemy = type;
        enemy.transform.position = new Vector3(ranLoc.x, ranLoc.y, 0);
        enemy.gameObject.SetActive(true);
        enemyData.Add(enemy);
    }

    public void CreateLabMonster(Enemy data)
    {
        Enemy enemy = null;
        if(data._typeofEnemy == Enemy.typeofEnemy.Dungeon || data._typeofEnemy == Enemy.typeofEnemy.DungeonBoss) {
            enemy = ObjectPoolDungeon.Instance.GetPoolObject(data.typeDungeonPool).GetComponent<Enemy>();
        } else {
            enemy = ObjectPoolEnemy.Instance.GetPoolObject(data.typePool).GetComponent<Enemy>();
        }
        int Side = Random.Range(1, 5);
        Vector2 ranLoc = RandomLocation(Side);
        enemy._typeofEnemy = Enemy.typeofEnemy.LabMonster;
        enemy.transform.position = new Vector3(ranLoc.x, ranLoc.y, 0);
        enemy.gameObject.SetActive(true);
        enemy.CurrentHealth = enemy.EnemyHealth;
        enemyData.Add(enemy);
    }

    public Vector2 RandomLocation(int Side)
    {
        float pos_X;
        float pos_Y;
        //Left
        if(Side == 1) {
            pos_X = -(Camera.main.orthographicSize * Camera.main.aspect + 0.5f);
            pos_Y = Random.Range(-Camera.main.orthographicSize, Camera.main.orthographicSize);
        }
        //Top
        else if(Side == 2) {
            pos_X = Random.Range(-(Camera.main.orthographicSize * Camera.main.aspect), Camera.main.orthographicSize * Camera.main.aspect);
            pos_Y = Camera.main.ScreenToWorldPoint(TopEnemySpawnLoc.position).y + 1f;
        }
        //Right
        else if(Side == 3) {
            pos_X = Camera.main.orthographicSize * Camera.main.aspect + 0.5f;
            pos_Y = Random.Range(-Camera.main.orthographicSize, Camera.main.orthographicSize);
        }
        //Under
        else {
            pos_X = Random.Range(-(Camera.main.orthographicSize * Camera.main.aspect), Camera.main.orthographicSize * Camera.main.aspect);
            pos_Y = -(Camera.main.orthographicSize + 1f);
        }

        return new Vector2(pos_X, pos_Y);
    }

    public static void EnemyDead(Enemy enemy)
    {
        switch(enemy._typeofEnemy) {
            case Enemy.typeofEnemy.normal:
                MoneyPopup.Create(enemy.transform.position, GameManager.GetTotalGoldOfEnemy(enemy), 2.5f);

                StageManager.EnemyDeadNormal(GameManager.GetTotalExpOfEnemy(enemy), GameManager.GetTotalGoldOfEnemy(enemy));

                Instance.enemyKilled_Number++;

                //Achievement
                AchievementManager.instance.killedEnemyDaily++;
                AchievementManager.instance.killedEnemyWeekly++;

                CollectionManager.Instance.addEnemyKilled(enemy);
                break;
            case Enemy.typeofEnemy.boss:
                MoneyPopup.Create(enemy.transform.position, GameManager.GetTotalGoldOfEnemy(enemy), 2.5f);

                StageManager.EnemyDeadBoss(GameManager.GetTotalExpOfEnemy(enemy), GameManager.GetTotalGoldOfEnemy(enemy));

                Instance.AddBossKilled(enemy);

                //Achievement
                AchievementManager.instance.killedBossDaily++;
                AchievementManager.instance.killedBossWeekly++;

                CollectionManager.Instance.addEnemyKilled(enemy);
                Instance.totalBossKilled++;
                break;
            case Enemy.typeofEnemy.Dungeon:
                DungeonManager.EnemyDeadNormal();
                break;
            case Enemy.typeofEnemy.DungeonBoss:
                DungeonManager.EnemyDeadBoss();
                break;
            case Enemy.typeofEnemy.InfiniteStage:
                CollectionManager.Instance.addEnemyKilled(enemy);
                MoneyPopup.Create(enemy.transform.position, GameManager.GetTotalGoldOfEnemy(enemy), 2.5f);
                InfiniteStage.EnemyDeadNormal(GameManager.GetTotalExpOfEnemy(enemy), GameManager.GetTotalGoldOfEnemy(enemy));

                Instance.enemyKilled_Number++;

                //Achievement
                AchievementManager.instance.killedEnemyDaily++;
                AchievementManager.instance.killedEnemyWeekly++;
                break;
            case Enemy.typeofEnemy.InfiniteStageBoss:
                CollectionManager.Instance.addEnemyKilled(enemy);
                MoneyPopup.Create(enemy.transform.position, GameManager.GetTotalGoldOfEnemy(enemy), 2.5f);
                InfiniteStage.EnemyDeadBoss(GameManager.GetTotalExpOfEnemy(enemy), GameManager.GetTotalGoldOfEnemy(enemy));

                Instance.totalBossKilled++;

                //Achievement
                AchievementManager.instance.killedBossDaily++;
                AchievementManager.instance.killedBossWeekly++;

                break;
            case Enemy.typeofEnemy.LabMonster:
                break;
        }

        if(enemy._typeofEnemy != Enemy.typeofEnemy.LabMonster) {
            Instance.totalKilledEnemy++;
        }

        enemy.SetEnemyToDefault();
        enemy.EnemyDeath();
        Instance.enemyData.Remove(enemy);
        Instance.bossData.Remove(enemy);
    }

    public static void EnemyReached(Enemy enemy)
    {
        switch(enemy._typeofEnemy) {
            case Enemy.typeofEnemy.normal:
                MoneyPopup.Create(enemy.transform.position, (-enemy.enemy_Money / 5) + 1, 2.5f);
                StageManager.instance.Died();
                ObjectPoolEnemy.Instance.CoolObject(enemy.gameObject, enemy.typePool);
                break;
            case Enemy.typeofEnemy.InfiniteStage:
                InfiniteStage.Instance.enemyHit(enemy);
                ObjectPoolEnemy.Instance.CoolObject(enemy.gameObject, enemy.typePool);
                break;
            case Enemy.typeofEnemy.Dungeon:
                DungeonManager.EnemyHit();
                ObjectPoolDungeon.Instance.CoolObject(enemy.gameObject, enemy.typeDungeonPool);
                break;
            case Enemy.typeofEnemy.boss:
                MoneyPopup.Create(enemy.transform.position, (-enemy.enemy_Money / 5) + 1, 2.5f);
                StageManager.instance.Died();
                ObjectPoolEnemy.Instance.CoolObject(enemy.gameObject, enemy.typePool);
                break;
            case Enemy.typeofEnemy.LabMonster:
                break;
        }
        enemy.SetEnemyToDefault();
        Instance.enemyData.Remove(enemy);
    }

    public void removeAllEnemies()
    {
        for(int i = 0; i < enemyData.Count; i++) {
            enemyData[i].SetEnemyToDefault();
            ObjectPoolEnemy.Instance.CoolObject(enemyData[i].gameObject, enemyData[i].typePool);
        }
        enemyData.Clear();

        for(int i = 0; i < bossData.Count; i++) {
            bossData[i].SetEnemyToDefault();
            ObjectPoolEnemy.Instance.CoolObject(bossData[i].gameObject, bossData[i].typePool);
        }
        bossData.Clear();
    }

    public void removeAllDungeonEnemies()
    {
        for(int i = 0; i < enemyData.Count; i++) {
            enemyData[i].SetEnemyToDefault();
            ObjectPoolDungeon.Instance.CoolObject(enemyData[i].gameObject, enemyData[i].typeDungeonPool);
        }
        enemyData.Clear();

        for(int i = 0; i < bossData.Count; i++) {
            bossData[i].SetEnemyToDefault();
            ObjectPoolDungeon.Instance.CoolObject(bossData[i].gameObject, bossData[i].typeDungeonPool);
        }
        bossData.Clear();
    }

    public void AddBossKilled(Enemy enemy)
    {
        for(int x = 0; x < bossData.Count; x++) {
            if(bossData[x].id == enemy.id) {
                bossKilled_number[x]++;
            }
        }
    }

    public bool getEnemyOnField()
    {
        return false;
    }

    public void LoadData(GameData data)
    {
        enemyKilled_Number = data.enemyKilled_Number;

        for(int i = 0; i < bossKilled_number.Count; i++) {
            data.bossKilled_Number.TryGetValue(BossEnemies[i].id, out int amount);
            bossKilled_number[i] = amount;
        }

        totalBossKilled = data.totalBossKill;
        totalKilledEnemy = data.totalKilledEnemy;
    }

    public void SaveData(GameData data)
    {
        data.enemyKilled_Number = enemyKilled_Number;
        for(int i = 0; i < bossKilled_number.Count; i++) {
            if(data.bossKilled_Number.ContainsKey(BossEnemies[i].id))
                data.bossKilled_Number.Remove(BossEnemies[i].id);

            data.bossKilled_Number.Add(BossEnemies[i].id, bossKilled_number[i]);
        }

        data.totalBossKill = totalBossKilled;
        data.totalKilledEnemy = totalKilledEnemy;
    }
}
