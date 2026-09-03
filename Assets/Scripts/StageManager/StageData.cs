using UnityEngine;

[System.Serializable]
public class StageData
{
    public string StageName;

    [Header("Prefabs")]
    public Enemy[] enemiesPrefab;
    public Enemy[] bossPrefab;
    [Header("SpawnSpeed")]
    public float[] SpawnSpeed;
    [Header("#Requried Kills")]
    public int EnemiesToKill;
    [Header("#Scaling")]
    public float[] enemyScale;
    public float[] BossScale;

    [Header("#Bools")]
    public bool StageCleared;

    [Header("#----Price")]
    public long goldPrice;
    public int DiamondPrice;
    public UpgradeStone.TypeOfStone stonePrice;
    public int stonePriceAmount;

    [Header("Cleared Time")]
    public int ClearedTime;
}
