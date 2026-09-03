using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectTypeEnemy
{
    NONE,

    //Planet 1
    Planet1_Enemy1,
    Planet1_Enemy2,
    Planet1_Enemy3,
    Planet1_Enemy4,
    Planet1_Boss1,
    Planet1_Boss2,

    //Planet 2
    Planet2_Enemy1,
    Planet2_Enemy2,
    Planet2_Enemy3,
    Planet2_Enemy4,
    Planet2_Boss1,
    Planet2_Boss2,

    //Planet 3
    Planet3_Enemy1,
    Planet3_Enemy2,
    Planet3_Enemy3,
    Planet3_Enemy4,
    Planet3_Boss1,
    Planet3_Boss2,

    //Planet 4
    Planet4_Enemy1,
    Planet4_Enemy2,
    Planet4_Enemy3,
    Planet4_Enemy4,
    Planet4_Boss1,
    Planet4_Boss2,

    //Planet 5
    Planet5_Enemy1,
    Planet5_Enemy2,
    Planet5_Enemy3,
    Planet5_Enemy4,
    Planet5_Boss1,
    Planet5_Boss2,

    //Planet 6
    Planet6_Enemy1,
    Planet6_Enemy2,
    Planet6_Enemy3,
    Planet6_Enemy4,
    Planet6_Boss1,
    Planet6_Boss2,

    //Planet 7
    Planet7_Enemy1,
    Planet7_Enemy2,
    Planet7_Enemy3,
    Planet7_Enemy4,
    Planet7_Boss1,
    Planet7_Boss2,

    //Planet 8
    Planet8_Enemy1,
    Planet8_Enemy2,
    Planet8_Enemy3,
    Planet8_Enemy4,
    Planet8_Boss1,
    Planet8_Boss2,

    //Planet 9
    Planet9_Enemy1,
    Planet9_Enemy2,
    Planet9_Enemy3,
    Planet9_Enemy4,
    Planet9_Boss1,
    Planet9_Boss2,

    //Planet 10
    Planet10_Enemy1,
    Planet10_Enemy2,
    Planet10_Enemy3,
    Planet10_Enemy4,
    Planet10_Boss1,
    Planet10_Boss2,

    //Planet 11
    Planet11_Enemy1,
    Planet11_Enemy2,
    Planet11_Enemy3,
    Planet11_Enemy4,
    Planet11_Boss1,
    Planet11_Boss2,

    //Planet 12
    Planet12_Enemy1,
    Planet12_Enemy2,
    Planet12_Enemy3,
    Planet12_Enemy4,
    Planet12_Boss1,
    Planet12_Boss2,

    //Planet 13
    Planet13_Enemy1,
    Planet13_Enemy2,
    Planet13_Enemy3,
    Planet13_Enemy4,
    Planet13_Boss1,
    Planet13_Boss2,

    //Planet 14
    Planet14_Enemy1,
    Planet14_Enemy2,
    Planet14_Enemy3,
    Planet14_Enemy4,
    Planet14_Boss1,
    Planet14_Boss2,
}


[Serializable]
public class PoolInfoEnemy
{
    public PoolObjectTypeEnemy type;
    public int amount = 0;
    public GameObject prefab;
    public GameObject Parent;

    [HideInInspector]
    public List<GameObject> pool = new List<GameObject>();
}

public class ObjectPoolEnemy : Singleton<ObjectPoolEnemy>
{
    [SerializeField]
    List<PoolInfoEnemy> listofPool;

    private Vector3 defaultPos = new Vector3(-100, -100, -100);

    void Start()
    {
        for(int i = 0; i < listofPool.Count; i++) {
            FillPool(listofPool[i]);
        }
    }

    void FillPool(PoolInfoEnemy info)
    {
        for(int i = 0; i < info.amount; i++) {
            GameObject obInstance = null;
            obInstance = Instantiate(info.prefab, info.Parent.transform);
            obInstance.gameObject.SetActive(false);
            obInstance.transform.position = defaultPos;
            info.pool.Add(obInstance);
        }
    }

    public GameObject GetPoolObject(PoolObjectTypeEnemy type)
    {
        PoolInfoEnemy selected = GetPoolByType(type);
        List<GameObject> pool = selected.pool;

        GameObject obInstance = null;
        if(pool.Count > 0) {
            obInstance = pool[pool.Count - 1];
            pool.Remove(obInstance);
        } else
            obInstance = Instantiate(selected.prefab, selected.Parent.transform);

        return obInstance;
    }

    public void CoolObject(GameObject ob, PoolObjectTypeEnemy type)
    {
        ob.SetActive(false);
        ob.transform.position = defaultPos;

        PoolInfoEnemy selected = GetPoolByType(type);
        List<GameObject> pool = selected.pool;

        if(!pool.Contains(ob))
            pool.Add(ob);
    }

    private PoolInfoEnemy GetPoolByType(PoolObjectTypeEnemy type)
    {
        for(int i = 0; i < listofPool.Count; i++) {
            if(type == listofPool[i].type) {
                return listofPool[i];
            }
        }

        return null;

    }
}