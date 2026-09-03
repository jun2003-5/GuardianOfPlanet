using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MainWeaponShop : MonoBehaviour
{
    public static MainWeaponShop Instance;
    [HideInInspector]
    public MainWeapon mainWeapon;

    [Header("UI 바꾸기")]
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI WeaponStat;
    public TextMeshProUGUI priceText;

    [HideInInspector]
    public int purchaseSize;
    long TotalPrice;

    [Header("#--------Passives")]
    public List<WeaponPassive> passiveObjects;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        setPassiveValues();
    }

    // Update is called once per frame
    void Update()
    {
        if(mainWeapon.UpgradeLevel > 0)
            WeaponStat.text = "우주선 데미지의 " + ((1 + mainWeapon.ExtraDamagePercent + TraitsManager.instance.GetStats(TraitsData.WeaponType.MainWeapon, TraitsData.TraitsType.DamagePercent)) * 100).ToString("F0") + "%로 공격\n" + "현재 데미지: " + (long)Mathf.RoundToInt((int)(Player.instance.FinalAttack_Damage + ((mainWeapon.Damage + mainWeapon.ExtraDamage) * (1 + mainWeapon.ExtraDamagePercent + TraitsManager.instance.GetStats(TraitsData.WeaponType.MainWeapon, TraitsData.TraitsType.DamagePercent)))));
        else
            WeaponStat.text = "몬스터를 공격하는 기본 총";
        LevelText.text = "Lv." + mainWeapon.UpgradeLevel.ToString();
        TotalPrice = getTotalMoney(mainWeapon.UpgradeLevel, purchaseSize);
        priceText.text = GameManager.MoneyString(TotalPrice);
        if(GameManager.GetMoney() >= TotalPrice)
            transform.GetChild(2).GetComponent<Image>().color = new Color(0, 1, 0.2204754f);
        else
            transform.GetChild(2).GetComponent<Image>().color = Color.white;
    }

    public void LevelUP()
    {
        if(GameManager.GetMoney() >= TotalPrice) {
            GameManager.SetMoney(-TotalPrice);
            for(int i = 0; i < purchaseSize; i++) {
                if(mainWeapon.UpgradeLevel > 0) {
                    mainWeapon.UpgradeWeapon();
                } else {
                    WeaponManager.instance.mainWeapon.gameObject.SetActive(true);
                    mainWeapon.UpgradeWeapon();
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
                total += (long)(15 + i * 10 * Mathf.Pow(1.03f, i - 100));
            } else if(i >= 50) {
                total += 15 + i * 10;
            } else if(i >= 10) {
                total += 15 + i * 9;
            } else if(i >= 0) {
                total += 15 + i * 8;
            }

            i++;
        }

        return total;
    }

    public void setPassiveValues()
    {
        //겹치면
        mainWeapon.ExtraDamagePercent = 0;

        //Passive 1
        if(mainWeapon.UpgradeLevel >= 10) {
            mainWeapon.ExtraDamage = 50;
            passiveObjects[0].PassiveCover.SetActive(false);
        }

        //Passive 2
        if(mainWeapon.UpgradeLevel >= 50) {
            mainWeapon.ExtraDamagePercent += 1f;
            passiveObjects[1].PassiveCover.SetActive(false);
        }

        //Passive 3
        if(mainWeapon.UpgradeLevel >= 100) {
            mainWeapon.ExtraDamagePercent += (mainWeapon.UpgradeLevel - 99) * 0.015f;
            passiveObjects[2].PassiveCover.SetActive(false);
        }

        //Passive 4
        if(mainWeapon.UpgradeLevel >= 200) {
            mainWeapon.ExtraCritChance = 15f;
            passiveObjects[3].PassiveCover.SetActive(false);
        }

        //Passive 5
        if(mainWeapon.UpgradeLevel >= 300) {
            mainWeapon.ExtraCritDamage = 1f;
            passiveObjects[4].PassiveCover.SetActive(false);
        }

        //Passive 6
        if(mainWeapon.UpgradeLevel >= 500) {
            mainWeapon.diamondPerHit = true;
            passiveObjects[5].PassiveCover.SetActive(false);
        }

        //Passive 7
        if(mainWeapon.UpgradeLevel >= 750) {
            mainWeapon.ExtraDamagePercent += 10f;
            passiveObjects[6].PassiveCover.SetActive(false);
        }
    }
}
