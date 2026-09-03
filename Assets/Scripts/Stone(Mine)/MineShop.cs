using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MineShop : MonoBehaviour
{
    [SerializeField] public string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }
    [Header("SHOP INFO")]
    public string MinerName;
    [Header("Upgrade")]
    public long OrePerSecond;
    public int IncreasePerUpgrade;
    [HideInInspector]
    public long TotalOrePerSecond;

    [HideInInspector]
    public float MinerBuff;
    [HideInInspector]
    public float TabBuff;

    public long InitialMinerPrice;
    long MinerPrice;
    [HideInInspector]
    public int ShopLevel;


    [Header("UIs")]
    public TextMeshProUGUI LvlText;
    public TextMeshProUGUI MinerInfoText;
    public TextMeshProUGUI PriceText;
    public GameObject MinerShopEntire;
    public GameObject UnlockTab;
    public TextMeshProUGUI MinerNameText;

    public bool IsActivate;

    // Start is called before the first frame update
    void Start()
    {
        MinerNameText.text = MinerName;
        MinerPrice = (long)(InitialMinerPrice * Mathf.Pow(1.2f, ShopLevel));
    }

    public void ButtonCondition()
    {
        if(GameManager.GetOre() >= MinerPrice) {
            MinerShopEntire.GetComponent<Image>().color = Color.green;
        } else {
            MinerShopEntire.GetComponent<Image>().color = Color.white;
        }
    }

    // Update is called once per frame
    public void Upgrade()
    {
        if(GameManager.GetOre() >= MinerPrice) {
            GameManager.SetOre(-MinerPrice);
            ShopLevel++;
            OrePerSecond += IncreasePerUpgrade;
        }
        SetData();
    }

    public void SetData()
    {
        //´É·Â
        MinerBuff = 10 * (ShopLevel / 10);
        TabBuff = 5 * (ShopLevel / 10);
        OrePerSecond = IncreasePerUpgrade * ShopLevel;
        if(ShopLevel > 0)
            TotalOrePerSecond = (long)(OrePerSecond * (1 + (MinerManager.Instance.TotalMineBuff / 100)));
        else
            TotalOrePerSecond = 0;
        //UI
        LvlText.text = "Lv." + ShopLevel;
        MinerInfoText.text = "±¤ºÎ ´É·Â: " + GameManager.MoneyString(TotalOrePerSecond) + "/ÃÊ\n±¤ºÎ ¹öÇÁ: " + MinerBuff + "%\nÅÇ ¹öÇÁ: " + TabBuff + "%";
        PriceText.text = GameManager.MoneyString(MinerPrice);
        MinerPrice = (long)(InitialMinerPrice * Mathf.Pow(1.2f, ShopLevel));
    }
}
