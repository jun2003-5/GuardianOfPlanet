using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class BounceGunShop : MonoBehaviour
{
    public static BounceGunShop Instance;

    [HideInInspector]
    public  BounceGun weapon;

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
        Instance = this;
    }

    private void OnEnable()
    {
        setPassiveValues();

        //Check Lock
        LockCover.SetActive(!StageManager.instance.planets[4].PlanetCleared);
        LockText.text = StageManager.instance.planets[4].PlanetName + " 행성 클리어";
    }

    // Update is called once per frame
    void Update()
    {
        if(weapon.UpgradeLevel > 0)
            WeaponStat.text = "추적 우주선 데미지의 " + ((1 + weapon.ExtraDamagePercent + TraitsManager.instance.GetStats(TraitsData.WeaponType.BounceGun, TraitsData.TraitsType.DamagePercent)) * 100).ToString("F0") + "%로 공격\n" + "현재 데미지: " + (long)Mathf.RoundToInt((int)(Player.instance.FinalAttack_Damage + ((weapon.Damage + weapon.ExtraDamage) * (1 + weapon.ExtraDamagePercent + TraitsManager.instance.GetStats(TraitsData.WeaponType.BounceGun, TraitsData.TraitsType.DamagePercent)))));
        else
            WeaponStat.text = "적을 추적하고 튕기는 총알을\n발사하는 우주선";
        LevelText.text = "Lv." + weapon.UpgradeLevel.ToString();
        TotalPrice = getTotalMoney(weapon.UpgradeLevel, purchaseSize);
        priceText.text = GameManager.MoneyString(TotalPrice);

        if(StageManager.instance.planets[4].PlanetCleared) {
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
                if(weapon.UpgradeLevel > 0) {
                    weapon.UpgradeWeapon();
                } else {
                    WeaponManager.instance.bounceGun.gameObject.SetActive(true);
                    weapon.UpgradeWeapon();
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
        weapon.ExtraDamagePercent = 0;

        //Passive 1
        if(weapon.UpgradeLevel >= 10) {
            weapon.ExtraDamage = 30;
            passiveObjects[0].PassiveCover.SetActive(false);
        }

        //Passive 2
        if(weapon.UpgradeLevel >= 50) {
            weapon.ExtraBullet = true;
            passiveObjects[1].PassiveCover.SetActive(false);
        }

        //Passive 3
        if(weapon.UpgradeLevel >= 100) {
            weapon.ExtraDamagePercent += (weapon.UpgradeLevel - 99) * 0.01f;
            passiveObjects[2].PassiveCover.SetActive(false);
        }

        //Passive 4
        if(weapon.UpgradeLevel >= 200) {
            weapon.ExtraAttackSpeedScale = 0.25f;
            passiveObjects[3].PassiveCover.SetActive(false);
        }

        //Passive 5
        if(weapon.UpgradeLevel >= 300) {
            weapon.ExtraBounce = true;
            passiveObjects[4].PassiveCover.SetActive(false);
        }

        //Passive 6
        if(weapon.UpgradeLevel >= 500) {
            weapon.NoDamageReduce = true;
            passiveObjects[5].PassiveCover.SetActive(false);
        }

        //Passive 7
        if(weapon.UpgradeLevel >= 750) {
            weapon.ExtraDamagePercent += 3.75f;
            passiveObjects[6].PassiveCover.SetActive(false);
        }
    }
}
