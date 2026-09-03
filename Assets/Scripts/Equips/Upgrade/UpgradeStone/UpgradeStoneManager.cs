using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeStoneManager : MonoBehaviour, IDataPersistence
{
    public static UpgradeStoneManager instance;

    public List<UpgradeStone> stonesData;

    private void Awake()
    {
        instance = this;
    }

    public UpgradeStone getStone(UpgradeStone.TypeOfStone stone)
    {
        for(int i = 0; i < stonesData.Count; i++) {
            if(stone == stonesData[i].stoneGrade) {
                return stonesData[i];
            }
        }
        return null;
    }

    public void addStone(UpgradeStone us, int n) 
    {
        for(int i = 0; i < stonesData.Count; i++) {
            if(us.stoneGrade == stonesData[i].stoneGrade) {
                stonesData[i].StoneAmount += n;
                break;
            }
        }
    }

    public void addStone(UpgradeStone.TypeOfStone type, int n)
    {
        for(int i = 0; i < stonesData.Count; i++) {
            if(type == stonesData[i].stoneGrade) {
                stonesData[i].StoneAmount += n;
                break;
            }
        }
    }

    public UpgradeStone RandomUpgradeStone(int amount)
    {
        int random = Random.Range(0, 101);
        if(random < 3) {
            addStone(stonesData[4], amount);
            return stonesData[4];
        } else if(random >= 3 && random < 10) {
            addStone(stonesData[3], amount);
            return stonesData[3];
        } else if(random >= 10 && random < 25) {
            addStone(stonesData[2], amount);
            return stonesData[2];
        } else if(random >= 25 && random < 55) {
            addStone(stonesData[1], amount);
            return stonesData[1];
        } else if(random >= 55 && random <= 100) {
            addStone(stonesData[0], amount);
            return stonesData[0];
        }
        return null;
    }

    public void LoadData(GameData data)
    {
        for(int i = 0; i < stonesData.Count; i++) {
            data.UpgradeStone_Amount.TryGetValue(stonesData[i].id, out int value);
            stonesData[i].StoneAmount = value;
        }
    }

    public void SaveData(GameData data)
    {
        for(int i = 0; i < stonesData.Count; i++) {
            if(data.UpgradeStone_Amount.ContainsKey(stonesData[i].id))
                data.UpgradeStone_Amount.Remove(stonesData[i].id);

            data.UpgradeStone_Amount.Add(stonesData[i].id, stonesData[i].StoneAmount);
        }
    }
}
