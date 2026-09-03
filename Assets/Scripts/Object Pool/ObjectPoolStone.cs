using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectTypeStone
{
    Stone0,
    Stone1,
    Stone2,
    Stone3,
    Stone4,
    Stone5,
    Stone6,
    Stone7,
    Stone8,
    Stone9,
    Stone10,
    Stone11,
    Stone12,
    Stone13,
    Stone14,
}

[Serializable]
public class PoolInfo
{
    public PoolObjectTypeStone type;
    public int amount = 0;
    public GameObject prefab;
    public GameObject Parent;

    [HideInInspector]
    public List<GameObject> pool = new List<GameObject>();
}

public class ObjectPoolStone : Singleton<ObjectPoolStone>
{
    [SerializeField]
    List<PoolInfo> listofPool;

    private Vector3 defaultPos = new Vector3(-100,-100,-100);

    void Start()
    {
        for(int i = 0; i < listofPool.Count; i++) {
            FillPool(listofPool[i]);
        }
    }

    void FillPool(PoolInfo info)
    {
        for(int i = 0; i < info.amount; i++) {
            GameObject obInstance = null;
            obInstance = Instantiate(info.prefab, info.Parent.transform);
            obInstance.gameObject.SetActive(false);
            obInstance.transform.position = defaultPos;
            info.pool.Add(obInstance);
        }
    }

    public GameObject GetPoolObject(PoolObjectTypeStone type)
    {
        PoolInfo selected = GetPoolByType(type);
        List<GameObject> pool = selected.pool;

        GameObject obInstance = null;
        if(pool.Count > 0) {
            obInstance = pool[pool.Count - 1];
            pool.Remove(obInstance);
        } else 
            obInstance = Instantiate(selected.prefab, selected.Parent.transform);
        
        return obInstance;
    }

    public void CoolObject(GameObject ob, PoolObjectTypeStone type)
    {
        ob.SetActive(false);
        ob.transform.position = defaultPos;

        PoolInfo selected = GetPoolByType(type);
        List<GameObject> pool = selected.pool;

        if(!pool.Contains(ob))
            pool.Add(ob);
    }

    private PoolInfo GetPoolByType(PoolObjectTypeStone type)
    {
        for(int i = 0; i < listofPool.Count; i++) {
            if(type == listofPool[i].type) {
                return listofPool[i];
            }
        }

        return null;

    }
}