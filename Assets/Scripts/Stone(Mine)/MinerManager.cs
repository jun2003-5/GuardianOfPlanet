using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerManager : MonoBehaviour, IDataPersistence
{
    public static MinerManager Instance;

    public MineShop[] mineShops;

    public float TotalMineBuff;
    public float TotalTabBuff;

    float timer;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;     
    }

    public void SetTotalBuff()
    {
        TotalMineBuff = 0;
        TotalTabBuff = 0;
        for(int i = 0; i < mineShops.Length; i++) {
            TotalMineBuff += mineShops[i].MinerBuff;
            TotalTabBuff += mineShops[i].TabBuff;
        }
    }

    public void CheckUnlockCondition()
    {
        for(int i = 1; i < mineShops.Length; i++) {
            if(mineShops[i - 1].ShopLevel >= 10) {
                mineShops[i].UnlockTab.SetActive(false);
            } else {
                mineShops[i].UnlockTab.SetActive(true);
            }
        }
    }

    public void CheckMineCondition()
    {
        for(int i = 0; i < mineShops.Length; i++) {
            mineShops[i].SetData();
            mineShops[i].ButtonCondition();
        }
        SetTotalBuff();
        CheckUnlockCondition();
    }

    public long GetTotalOreSec()
    {
        long Ore_Second = 0;
        for(int i = 0; i < mineShops.Length; i++) {
            Ore_Second += mineShops[i].TotalOrePerSecond;
        }
        return Ore_Second;
    }
    private void Update()
    {
        CheckUnlockCondition();
        SetTotalBuff();
        CheckMineCondition();

        timer += Time.deltaTime / Time.timeScale;
        if(timer >= 1) {
            timer = 0;
            GameManager.SetOre(GetTotalOreSec());
        }
    }

    public void LoadData(GameData data)
    {
        for(int i = 0; i < mineShops.Length; i++) {
            data.MinerLevel.TryGetValue(mineShops[i].id, out int level);
            mineShops[i].ShopLevel = level;
        }
    }

    public void SaveData(GameData data)
    {
        for(int i = 0; i < mineShops.Length; i++) {
            if(data.MinerLevel.ContainsKey(mineShops[i].id))
                data.MinerLevel.Remove(mineShops[i].id);

            data.MinerLevel.Add(mineShops[i].id, mineShops[i].ShopLevel);
        }
    }
}
