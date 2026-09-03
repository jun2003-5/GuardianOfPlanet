using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabMonsterData : MonoBehaviour
{
    public Image monsterImage;
    public GameObject BossText;

    public Enemy enemy;

    [HideInInspector]
    public float Scale;

    [HideInInspector]
    public Planet currentPlanet;

    public void setMonsterData(Enemy e, bool boss)
    {
        enemy = e;
        monsterImage.sprite = e.RawImage;
        BossText.SetActive(boss);
    }

    public void monsterSelected()
    {
        Labmanager.instance.enemySelected(enemy);
    }

    public void monsterSelectedMap()
    {
        currentPlanet.mapInfoMonsterClicked(this);
    }

    public void MonsterSelectedDungeon()
    {
        DungeonManager.instance.setMapInfoStats(enemy);
    }
}
