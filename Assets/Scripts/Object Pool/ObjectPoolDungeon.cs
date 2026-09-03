using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectTypeDungeon
{
    NONE,
    Dungeon1_1,
    Dungeon1_2,
    Dungeon1_3,
    Dungeon1_4,
    Dungeon1_Boss,
    Dungeon2_1,
    Dungeon2_2,
    Dungeon2_3,
    Dungeon2_4,
    Dungeon2_Boss,
    Dungeon3_1,
    Dungeon3_2,
    Dungeon3_3,
    Dungeon3_4,
    Dungeon3_Boss,
    Dungeon4_1,
    Dungeon4_2,
    Dungeon4_3,
    Dungeon4_4,
    Dungeon4_Boss,
    Dungeon5_1,
    Dungeon5_2,
    Dungeon5_3,
    Dungeon5_4,
    Dungeon5_Boss,
}


[Serializable]
public class PoolInfoDungeon
{
    public PoolObjectTypeDungeon type;
    public int amount = 0;
    public GameObject prefab;
    public GameObject Parent;

    [HideInInspector]
    public List<GameObject> pool = new List<GameObject>();
}

public class ObjectPoolDungeon : Singleton<ObjectPoolDungeon>
{
    [SerializeField]
    List<PoolInfoDungeon> listofPool;

    private Vector3 defaultPos = new Vector3(-100, -100, -100);

    void Start()
    {
        for(int i = 0; i < listofPool.Count; i++) {
            FillPool(listofPool[i]);
        }
    }

    void FillPool(PoolInfoDungeon info)
    {
        for(int i = 0; i < info.amount; i++) {
            GameObject obInstance = null;
            obInstance = Instantiate(info.prefab, info.Parent.transform);
            obInstance.gameObject.SetActive(false);
            obInstance.transform.position = defaultPos;
            info.pool.Add(obInstance);
        }
    }

    public GameObject GetPoolObject(PoolObjectTypeDungeon type)
    {
        PoolInfoDungeon selected = GetPoolByType(type);
        List<GameObject> pool = selected.pool;

        GameObject obInstance = null;
        if(pool.Count > 0) {
            obInstance = pool[pool.Count - 1];
            pool.Remove(obInstance);
        } else
            obInstance = Instantiate(selected.prefab, selected.Parent.transform);

        return obInstance;
    }

    public void CoolObject(GameObject ob, PoolObjectTypeDungeon type)
    {
        ob.SetActive(false);
        ob.transform.position = defaultPos;

        PoolInfoDungeon selected = GetPoolByType(type);
        List<GameObject> pool = selected.pool;

        if(!pool.Contains(ob))
            pool.Add(ob);
    }

    private PoolInfoDungeon GetPoolByType(PoolObjectTypeDungeon type)
    {
        for(int i = 0; i < listofPool.Count; i++) {
            if(type == listofPool[i].type) {
                return listofPool[i];
            }
        }

        return null;

    }
}