using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DungeonData : MonoBehaviour
{
    public string DungeonName;
    public string DungeonDifficulty;
    public int RequiredDPS;

    public float[] spawnTime;

    public int[] spawnAmount;

    public Enemy[] DungeonEnimes;

    public int playerhealth;

    public UpgradeStone DungeonPriceStone;
    public int PriceStoneAmount;
    public long PriceGold;
    public int PriceEXP;

    [Header("#UI")]
    public TextMeshProUGUI dungeonName;
    public TextMeshProUGUI dungeonDifficulty;
    public TextMeshProUGUI dungeonRequiredDPS;
    public TextMeshProUGUI dungeonGoldPrice;
    public TextMeshProUGUI dungeonEXPPrice;
    public GameObject[] dungeonStones;
    public TextMeshProUGUI DungeonTicket;

    public Button DungeonEnterButton;
    public Button DungeonTicektBuyButton;

    private void Start()
    {
        dungeonName.text = DungeonName;
        dungeonDifficulty.text = "난이도: " + DungeonDifficulty;
        dungeonRequiredDPS.text = "권장 DPS: " + string.Format("{0:#,###}", RequiredDPS);
        dungeonGoldPrice.text = GameManager.MoneyStringForShop(PriceGold);
        dungeonEXPPrice.text = GameManager.MoneyStringForShop(PriceEXP);

        for(int i = 0; i < dungeonStones.Length; i++) {
            dungeonStones[i].SetActive(((int)DungeonPriceStone.stoneGrade) == i);
            if(dungeonStones[i].activeSelf) {
                dungeonStones[i].transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = PriceStoneAmount.ToString();
            }
        }
    }

    public int getTotalEnemy()
    {
        int sum = 0;
        for(int i = 0; i < spawnAmount.Length; i++) {
            sum += spawnAmount[i];
        }
        return sum;
    }
}
