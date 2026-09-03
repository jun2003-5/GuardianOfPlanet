using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;

    public GameObject[] grids;

    public GameObject MineGrid;
    public GameObject DungeonGrid;
    public GameObject LabGrid;

    public GameObject[] VillageGUIs;

    public GameObject[] MineGUIs;

    public GameObject[] AdventureGUIs;

    public GameObject[] DungeonGUIs;

    public GameObject[] LabGUIs;

    public GameObject Bullets;

    private void Awake()
    {
        instance = this;
    }

    public void ActiveGrid(int index)
    {
        if(index > grids.Length-1) index = grids.Length-1;
        for(int i = 0; i < grids.Length; i++) {
            grids[i].SetActive(i == index);
        }

        //Check
        StageManager.instance.checkAllPlanetState();
    }

    public void ToVillage()
    {
        //Check
        StageManager.instance.checkAllPlanetState();
        Railmanager.Instance.isInMine = false;
        //Camear
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 0, Camera.main.transform.position.z);

        Bullets.SetActive(true);
        MineGUI(false);
        AdventureGUI(false);
        DungeonGUI(false);
        LabGUI(false);
        VillageGUI(true);

        //Grid
        DungeonGrid.SetActive(false);
        LabGrid.SetActive(false);
        MineGrid.SetActive(false);
        ActiveGrid(InfiniteStage.Instance.CurrentStage / 80);
    }

    public void ToMine()
    {
        Railmanager.Instance.isInMine = true;
        AdventureGUI(false);
        VillageGUI(false);
        DungeonGUI(false);
        LabGUI(false);
        MineGUI(true);
        Bullets.SetActive(false);

        DungeonGrid.SetActive(false);
        LabGrid.SetActive(false);
        MineGrid.SetActive(true);
        ActiveGrid(-1);
        Time.timeScale = 1;
    }

    public void ToPlanet(int index)
    {
        VillageGUI(false);
        MineGUI(false);
        DungeonGUI(false);
        LabGUI(false);
        AdventureGUI(true);
        ActiveGrid(index);
    }

    public void ToDungeon()
    {
        VillageGUI(false);
        MineGUI(false);
        AdventureGUI(false);
        LabGUI(false);
        DungeonGUI(true);

        DungeonGrid.SetActive(true);
        LabGrid.SetActive(false);
        MineGrid.SetActive(false);
        ActiveGrid(-1);
    }

    public void ToLab()
    {
        AdventureGUI(false);
        VillageGUI(false);
        DungeonGUI(false);
        MineGUI(false);
        LabGUI(true);

        DungeonGrid.SetActive(false);
        LabGrid.SetActive(true);
        MineGrid.SetActive(false);
        ActiveGrid(-1);
        Time.timeScale = 1;
    }

    public void MineGUI(bool a)
    {
        for(int i = 0; i < MineGUIs.Length; i++) {
            MineGUIs[i].SetActive(a);
        }
    }
    public void VillageGUI(bool a)
    {
        for(int i = 0; i < VillageGUIs.Length; i++) {
            VillageGUIs[i].SetActive(a);
        }
    }
    public void AdventureGUI(bool a)
    {
        for(int i = 0; i < AdventureGUIs.Length; i++) {
            AdventureGUIs[i].SetActive(a);
        }
    }

    public void DungeonGUI(bool a)
    {
        for(int i = 0; i < DungeonGUIs.Length; i++) {
            DungeonGUIs[i].SetActive(a);
        }
    }

    public void LabGUI(bool a)
    {
        for(int i = 0; i < LabGUIs.Length; i++) {
            LabGUIs[i].SetActive(a);
        }
    }
}
