using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TraitsManager : MonoBehaviour, IDataPersistence
{
    public static TraitsManager instance;

    [Space(10)]
    [Header("TraitsData")]
    public List<TraitsData> TraitsData;

    public GameObject ExclamationMark;

    void Start()
    {
        instance = this;

        InvokeRepeating("checkCondition", 0, 1f);
    }

    public float GetStats(TraitsData.WeaponType WeaponType, TraitsData.TraitsType traitsType)
    {
        for(int i = 0; i < TraitsData.Count; i++) {
            if(TraitsData[i].Weapon_Type == WeaponType && TraitsData[i].Trait_Type == traitsType)
                return TraitsData[i].getTraitStat();
        }
        return 0;
    }

    void checkCondition()
    {
        for(int i = 0; i < TraitsData.Count; i++) {
            if(TraitsData[i].Weapon_Type == global::TraitsData.WeaponType.MainWeapon) {
                TraitsData[i].gameObject.SetActive(WeaponManager.instance.mainWeapon.UpgradeLevel > 0);
            } else if(TraitsData[i].Weapon_Type == global::TraitsData.WeaponType.DoubleGun) {
                TraitsData[i].gameObject.SetActive(WeaponManager.instance.doubleGun.UpgradeLevel > 0);
            } else if(TraitsData[i].Weapon_Type == global::TraitsData.WeaponType.TrackingMissile) {
                TraitsData[i].gameObject.SetActive(WeaponManager.instance.trackingGun.UpgradeLevel > 0);
            } else if(TraitsData[i].Weapon_Type == global::TraitsData.WeaponType.SniperGun) {
                TraitsData[i].gameObject.SetActive(WeaponManager.instance.sniperGun.UpgradeLevel > 0);
            } else if(TraitsData[i].Weapon_Type == global::TraitsData.WeaponType.Bomber) {
                TraitsData[i].gameObject.SetActive(WeaponManager.instance.BomberGun.UpgradeLevel > 0);
            } else if(TraitsData[i].Weapon_Type == global::TraitsData.WeaponType.BounceGun) {
                TraitsData[i].gameObject.SetActive(WeaponManager.instance.bounceGun.UpgradeLevel > 0);
            } else if(TraitsData[i].Weapon_Type == global::TraitsData.WeaponType.CircleLaser) {
                TraitsData[i].gameObject.SetActive(WeaponManager.instance.lazer.UpgradeLevel > 0);
            } else if(TraitsData[i].Weapon_Type == global::TraitsData.WeaponType.Poison) {
                TraitsData[i].gameObject.SetActive(WeaponManager.instance.poison.UpgradeLevel > 0);
            } else if(TraitsData[i].Weapon_Type == global::TraitsData.WeaponType.StrLaser) {
                TraitsData[i].gameObject.SetActive(WeaponManager.instance.strLaser.UpgradeLevel > 0);
            } else if(TraitsData[i].Weapon_Type == global::TraitsData.WeaponType.CodeA) {
                TraitsData[i].gameObject.SetActive(WeaponManager.instance.orbital.UpgradeLevel > 0);
            }
        }

        ExclamationMark.SetActive(false);
        for(int i = 0; i < TraitsData.Count; i++)
        {
            if(TraitsData[i].Trait_Level != 20)
            {
                if(GameManager.GetOre() >= TraitsData[i].totalPrice[TraitsData[i].Trait_Level])
                {
                    ExclamationMark.SetActive(true);
                    break;
                }
            }
        }
    }
    public void Upgrade(TraitsData data)
    {
        if(GameManager.GetOre() >= data.totalPrice[data.Trait_Level]) {
            GameManager.SetOre(-data.totalPrice[data.Trait_Level]);
            //Can Buy
            data.Trait_Level++;
            data.SetTraitData();

            SoundManager.Instance.Invoke("playCoinSFX", SoundManager.Instance.click.length);
        }
    }

    public void LoadData(GameData data)
    {
        for(int i = 0; i < TraitsData.Count; i++) {
            data.TraitLevel.TryGetValue(TraitsData[i].id, out int level);
            TraitsData[i].Trait_Level = level;

            TraitsData[i].SetTraitData();
        }
    }

    public void SaveData(GameData data)
    {
        for(int i = 0; i < TraitsData.Count; i++) {
            if(data.TraitLevel.ContainsKey(TraitsData[i].id))
                data.TraitLevel.Remove(TraitsData[i].id);

            data.TraitLevel.Add(TraitsData[i].id, TraitsData[i].Trait_Level);
        }
    }
}
