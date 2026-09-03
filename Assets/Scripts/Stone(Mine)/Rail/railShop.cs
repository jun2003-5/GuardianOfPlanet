using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class railShop : MonoBehaviour
{
    [SerializeField] public string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }
    [Header("Shop Info")]
    public Stone stone;
    public float InitialSpawnTime;
    public float SpawnTime;
    public long InitialStonePrice;
    [HideInInspector]
    public int ShopLevel;

    [Header("ETC Data")]
    public long StoneValue;
    public long finalStoneValue;
    public int AddStoneValuePerLevel;

    [Header("UIs")]
    public TextMeshProUGUI LvlText;
    public TextMeshProUGUI StoneInfoText;
    public TextMeshProUGUI PriceText;
    public TextMeshProUGUI SpawnTimeText;
    public Button buyButton;

    [Header("Rail Upgrade Button")]
    public RailInfoData railUpgradeButton;

    [Header("Lock")]
    public GameObject LockTab;

    long TotalMoney;

    public drillBasicInfo info;

    private void OnEnable()
    {
        SetShopInfo();
    }

    public void SetShopInfo()
    {
        stone.OnEnable();
        finalStoneValue = getStoneValue();
        SpawnTime = getSpawnTime();
        LvlText.text = "Lv. " + ShopLevel;
        StoneInfoText.text = "값어치: 광석 " + GameManager.MoneyString(finalStoneValue) + "개";
        SpawnTimeText.text = "생성 시간: " + SpawnTime.ToString("F1") + "초";
        TotalMoney = GetTotalPrice();
        PriceText.text = GameManager.MoneyString(TotalMoney);

        //Lock
        LockTab.SetActive(Railmanager.Instance.getLockTabCondition(this));
    }

    public void Upgrade()
    {
        if(GameManager.GetOre() >= TotalMoney) {
            GameManager.SetOre(-TotalMoney);
            if(ShopLevel <= 0) {
                ShopLevel++;
                UserIconManager.Instance.unlockStoneIcon(Railmanager.Instance.shops.IndexOf(this));
                Railmanager.Instance.activateRail(this);
            } else {
                ShopLevel++;
            }

            SoundManager.Instance.Invoke("playCoinSFX", SoundManager.Instance.click.length);
        }
        Railmanager.Instance.checkShopInfo();
    }

    private void Update()
    {
        if(!LockTab.activeSelf) {
            if(GameManager.GetOre() >= TotalMoney) {
                buyButton.GetComponent<Image>().color = Color.green;
            } else {
                buyButton.GetComponent<Image>().color = Color.white;
            }
        }
    }

    public float getSpawnTime()
    {
        float time = InitialSpawnTime;

        if(ShopLevel >= 10) {
            time *= 0.9f;
        } 

        if(ShopLevel >= 30) {
            time *= 0.92f;
        }

        if(ShopLevel >= 50) {
            time *= 0.94f;
        }
        if(ShopLevel >= 100) {
            time *= 0.96f;
        }
        if(ShopLevel >= 150) {
            time *= 0.965f;
        }
        if(ShopLevel >= 200) {
            time *= 0.97f;
        }
        if(ShopLevel >= 250) {
            time *= 0.97f;
        }
        if(ShopLevel >= 300) {
            time *= 0.971f;
        }
        if(ShopLevel >= 350) {
            time *= 0.972f;
        }
        if(ShopLevel >= 400) {
            time *= 0.973f;
        }
        if(ShopLevel >= 450) {
            time *= 0.975f;
        }
        if(ShopLevel >= 500) {
            time *= Mathf.Pow(0.976f, (ShopLevel - 490) / 50);
        }

        return time;
    }

    public long getStoneValue()
    {
        long n = StoneValue;

        if(ShopLevel >= 0)
            n += ShopLevel * AddStoneValuePerLevel;

        if(ShopLevel >= 100)
            n += (ShopLevel - 100) * (int)(AddStoneValuePerLevel * 1f);

        if(ShopLevel >= 200)
            n += (ShopLevel - 200) * (int)(AddStoneValuePerLevel * 2f);

        if(ShopLevel >= 300)
            n += (ShopLevel - 300) * (int)(AddStoneValuePerLevel * 3f);

        if(ShopLevel >= 500)
            n += (ShopLevel - 500) * (int)(AddStoneValuePerLevel * 5f);

        if(ShopLevel >= 750)
            n += (ShopLevel - 750) * (int)(AddStoneValuePerLevel * 7.5f);

        if(ShopLevel >= 1000)
            n += (ShopLevel - 1000) * (int)(AddStoneValuePerLevel * 10f);

        return n;
    }

    public long GetTotalPrice()
    {
        long total = InitialStonePrice;

        total = (long)(total * Mathf.Pow(1.05f, ShopLevel));


        return total;
    }
}

[System.Serializable]
public class drillBasicInfo
{
    public int baseDamage;
    public float baseAttackSpeed;
    public float baseLuck;

    public int increasePerLevel_Damage;
    public float increasePerLevel_AttackSpeed;
    public float increasePerLevel_Luck;
}
