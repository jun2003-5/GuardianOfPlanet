using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class BomberShop : MonoBehaviour
{
    public static BomberShop instance;

    [HideInInspector]
    public Bomber bomber;

    [Header("UI 바꾸기")]
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI WeaponStat;
    public TextMeshProUGUI priceText;

    [HideInInspector]
    public int purchaseSize;
    long TotalPrice;

    [Header("#-----잠금")]
    public GameObject LockCover;
    public TextMeshProUGUI LockText;

    [Header("#--------Passives")]
    public List<WeaponPassive> passiveObjects;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        setPassiveValues();

        //Check Lock
        LockCover.SetActive(!StageManager.instance.planets[2].PlanetCleared);
        LockText.text = StageManager.instance.planets[2].PlanetName + " 행성 클리어";
    }

    // Update is called once per frame
    void Update()
    {
        if(bomber.UpgradeLevel > 0)
            WeaponStat.text = "폭격기 데미지의 " + ((1 + bomber.ExploseDamagePercent + TraitsManager.instance.GetStats(TraitsData.WeaponType.Bomber, TraitsData.TraitsType.DamagePercent)) * 100).ToString("F0") + "%로 공격\n" + "현재 데미지: " + (long)Mathf.RoundToInt((int)(Player.instance.FinalAttack_Damage + ((bomber.Damage + bomber.ExploseDamage) * (1 + bomber.ExploseDamagePercent + TraitsManager.instance.GetStats(TraitsData.WeaponType.Bomber, TraitsData.TraitsType.DamagePercent)))));
        else
            WeaponStat.text = "폭탄으로 범위 공격을 하는 폭격기";
        LevelText.text = "Lv." + bomber.UpgradeLevel.ToString();
        TotalPrice = getTotalMoney(bomber.UpgradeLevel, purchaseSize);
        priceText.text = GameManager.MoneyString(TotalPrice);

        if(StageManager.instance.planets[2].PlanetCleared) {
            if(GameManager.GetMoney() >= TotalPrice)
                transform.GetChild(2).GetComponent<Image>().color = new Color(0, 1, 0.2204754f);
            else
                transform.GetChild(2).GetComponent<Image>().color = Color.white;
        }
    }

    public void LevelUP()
    {
        if(GameManager.GetMoney() >= TotalPrice) {
            GameManager.SetMoney(-TotalPrice);
            for(int i = 0; i < purchaseSize; i++) {
                if(bomber.UpgradeLevel > 0) {
                    bomber.UpgradeWeapon();
                } else {
                    WeaponManager.instance.BomberGun.gameObject.SetActive(true);
                    bomber.UpgradeWeapon();
                }
            }
            setPassiveValues();
            //Sound
            SoundManager.Instance.Invoke("playCoinSFX", SoundManager.Instance.click.length);
        }
    }

    public long getTotalMoney(int i, int purcahseSize)
    {
        long total = 0;

        for(int e = 0; e < purcahseSize; e++) {

            if(i >= 500) {
                total += (long)(50000000 * Mathf.Pow(1.0075f, i - 500));
            } else if(i >= 400) {
                total += (long)(15000000 * Mathf.Pow(1.015f, i - 400));
            } else if(i >= 300) {
                total += (long)(1000000 * Mathf.Pow(1.025f, i - 300));
            } else if(i >= 200) {
                total += (long)(100000 * Mathf.Pow(1.0175f, i - 200));
            } else if(i >= 100) {
                total += (long)(100 + i * 17 * Mathf.Pow(1.03f, i - 100));
            } else if(i >= 50) {
                total += 100 + i * 17;
            } else if(i >= 10) {
                total += 100 + i * 16;
            } else if(i >= 0) {
                total += 100 + i * 15;
            }
            i++;
        }
        return total;
    }

    public void setPassiveValues()
    {
        //겹치면
        bomber.ExploseDamagePercent = 0;

        //Passive 1
        if(bomber.UpgradeLevel >= 10) {
            bomber.ExploseDamage = 100;
            passiveObjects[0].PassiveCover.SetActive(false);
        }

        //Passive 2
        if(bomber.UpgradeLevel >= 50) {
            bomber.ExploseDamagePercent += 0.75f;
            passiveObjects[1].PassiveCover.SetActive(false);
        }

        //Passive 3
        if(bomber.UpgradeLevel >= 100) {
            bomber.ExploseDamagePercent += (bomber.UpgradeLevel - 99) * 0.012f;
            passiveObjects[2].PassiveCover.SetActive(false);
        }

        //Passive 4
        if(bomber.UpgradeLevel >= 200) {
            bomber.ExploseRangeIncrease = true;
            passiveObjects[3].PassiveCover.SetActive(false);
        }

        //Passive 5
        if(bomber.UpgradeLevel >= 300) {
            bomber.ExtraAttackSpeedScale = 0.35f;
            passiveObjects[4].PassiveCover.SetActive(false);
        }

        //Passive 6
        if(bomber.UpgradeLevel >= 500) {
            bomber.ExploseDamagePercent += 3f;
            passiveObjects[5].PassiveCover.SetActive(false);
        }

        //Passive 7
        if(bomber.UpgradeLevel >= 750) {
            bomber.ExploseDamagePercent += 5f;
            passiveObjects[6].PassiveCover.SetActive(false);
        }
    }
}
