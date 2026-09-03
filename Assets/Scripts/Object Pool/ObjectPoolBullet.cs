using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectTypeBullet
{
    Main,
    Double,    
    Tracking,
    Mining,
    Sniper,
    Bomber,
    Poison,
    PoisonArea,
    BounceBullet
}


[Serializable]
public class PoolInfoBullet
{
    public PoolObjectTypeBullet type;
    public int amount = 0;
    public GameObject prefab;
    public GameObject Parent;

    [HideInInspector]
    public List<GameObject> pool = new List<GameObject>();
}

public class ObjectPoolBullet : Singleton<ObjectPoolBullet>
{
    [SerializeField]
    List<PoolInfoBullet> listofPool;

    private Vector3 defaultPos = new Vector3(-100, -100, -100);

    void Start()
    {
        for(int i = 0; i < listofPool.Count; i++) {
            FillPool(listofPool[i]);
        }
    }

    void FillPool(PoolInfoBullet info)
    {
        for(int i = 0; i < info.amount; i++) {
            GameObject obInstance = null;
            obInstance = Instantiate(info.prefab, info.Parent.transform);
            obInstance.gameObject.SetActive(false);
            obInstance.transform.position = defaultPos;
            info.pool.Add(obInstance);
        }
    }

    public GameObject GetPoolObject(PoolObjectTypeBullet type)
    {
        PoolInfoBullet selected = GetPoolByType(type);
        List<GameObject> pool = selected.pool;

        GameObject obInstance = null;
        if(pool.Count > 0) {
            obInstance = pool[pool.Count - 1];
            pool.Remove(obInstance);
        } else
            obInstance = Instantiate(selected.prefab, selected.Parent.transform);

        return obInstance;
    }

    public void CoolObject(GameObject ob, PoolObjectTypeBullet type)
    {
        ob.SetActive(false);
        ob.transform.position = defaultPos;

        PoolInfoBullet selected = GetPoolByType(type);
        List<GameObject> pool = selected.pool;

        if(!pool.Contains(ob))
            pool.Add(ob);
    }

    private PoolInfoBullet GetPoolByType(PoolObjectTypeBullet type)
    {
        for(int i = 0; i < listofPool.Count; i++) {
            if(type == listofPool[i].type) {
                return listofPool[i];
            }
        }
        return null;

    }
}