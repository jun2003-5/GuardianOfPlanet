using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrbitalShop : MonoBehaviour
{
    public static OrbitalShop Instance;

    [HideInInspector]
    public Orbital weapon;

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

    [HideInInspector]
    public Orbital ExtraOrbit;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        setPassiveValues();

        //Check Lock
        LockCover.SetActive(!StageManager.instance.planets[7].PlanetCleared);
        LockText.text = StageManager.instance.planets[7].PlanetName + " 행성 클리어";
    }

    // Update is called once per frame
    void Update()
    {
        if(weapon.UpgradeLevel > 0)
            WeaponStat.text = "드론 데미지의 " + ((1 + weapon.ExtraDamagePercent + TraitsManager.instance.GetStats(TraitsData.WeaponType.CodeA, TraitsData.TraitsType.DamagePercent)) * 100).ToString("F0") + "%로 공격\n" + "현재 데미지: " + (long)Mathf.RoundToInt((int)(Player.instance.FinalAttack_Damage + ((weapon.OribitalDamage + weapon.ExtraDamage) * (1 + weapon.ExtraDamagePercent + TraitsManager.instance.GetStats(TraitsData.WeaponType.CodeA, TraitsData.TraitsType.DamagePercent)))));
        else
            WeaponStat.text = "우주선 주변을 돌며 몬스터를\n공격하는 설치물";

        LevelText.text = "Lv." + weapon.UpgradeLevel.ToString();
        TotalPrice = getTotalMoney(weapon.UpgradeLevel, purchaseSize);
        priceText.text = GameManager.MoneyString(TotalPrice);

        if(StageManager.instance.planets[7].PlanetCleared) {
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
                    WeaponManager.instance.orbital.gameObject.SetActive(true);
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
            weapon.ExtraDamage = 150;
            passiveObjects[0].PassiveCover.SetActive(false);
        }

        //Passive 2
        if(weapon.UpgradeLevel >= 50) {
            weapon.extraRange = true;
            passiveObjects[1].PassiveCover.SetActive(false);
        }

        //Passive 3
        if(weapon.UpgradeLevel >= 100) {
            weapon.ExtraDamagePercent += (weapon.UpgradeLevel - 99) * 0.0075f;
            passiveObjects[2].PassiveCover.SetActive(false);
        }

        //Passive 4
        if(weapon.UpgradeLevel >= 200) {
            weapon.ExtraStunPercent = 0.15f;
            passiveObjects[3].PassiveCover.SetActive(false);
        }

        //Passive 5
        if(weapon.UpgradeLevel >= 300) {
            weapon.extraSpeed = true;
            passiveObjects[4].PassiveCover.SetActive(false);
        }

        //Passive 6
        if(weapon.UpgradeLevel >= 500) {
            if(!weapon.hasScaled) {
                weapon.transform.localScale *= 2f;
                weapon.hasScaled = true;
            }

            passiveObjects[5].PassiveCover.SetActive(false);
        }

        //Passive 7
        if(weapon.UpgradeLevel >= 750) {
            weapon.ExtraDamagePercent += 4f;
            passiveObjects[6].PassiveCover.SetActive(false);
        }
    }
}
