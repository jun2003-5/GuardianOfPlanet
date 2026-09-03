using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectTypePopUp
{
    Damage,
    Material,
    Money
}


[Serializable]
public class PoolInfoPopUp
{
    public PoolObjectTypePopUp type;
    public int amount = 0;
    public GameObject prefab;
    public GameObject Parent;

    [HideInInspector]
    public List<GameObject> pool = new List<GameObject>();
}

public class ObjectPoolPopUp : Singleton<ObjectPoolPopUp>
{
    [SerializeField]
    List<PoolInfoPopUp> listofPool;

    private Vector3 defaultPos = new Vector3(-100, -100, -100);

    void Start()
    {
        for(int i = 0; i < listofPool.Count; i++) {
            FillPool(listofPool[i]);
        }
    }

    void FillPool(PoolInfoPopUp info)
    {
        for(int i = 0; i < info.amount; i++) {
            GameObject obInstance = null;
            obInstance = Instantiate(info.prefab, info.Parent.transform);
            obInstance.gameObject.SetActive(false);
            obInstance.transform.position = defaultPos;
            info.pool.Add(obInstance);
        }
    }

    public GameObject GetPoolObject(PoolObjectTypePopUp type)
    {
        PoolInfoPopUp selected = GetPoolByType(type);
        List<GameObject> pool = selected.pool;

        GameObject obInstance = null;
        if(pool.Count > 0) {
            obInstance = pool[pool.Count - 1];
            pool.Remove(obInstance);
        } else
            obInstance = Instantiate(selected.prefab, selected.Parent.transform);

        return obInstance;
    }

    public void CoolObject(GameObject ob, PoolObjectTypePopUp type)
    {
        ob.SetActive(false);
        ob.transform.position = defaultPos;

        PoolInfoPopUp selected = GetPoolByType(type);
        List<GameObject> pool = selected.pool;

        if(!pool.Contains(ob))
            pool.Add(ob);
    }

    private PoolInfoPopUp GetPoolByType(PoolObjectTypePopUp type)
    {
        for(int i = 0; i < listofPool.Count; i++) {
            if(type == listofPool[i].type) {
                return listofPool[i];
            }
        }
        return null;
    }
}