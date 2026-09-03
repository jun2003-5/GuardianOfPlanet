using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WeaponManager : MonoBehaviour, IDataPersistence
{
    public static WeaponManager instance;

    public enum ShootType { NormalShoot, AutoShoot, DontShoot };
    [Header("#-----ShootType")]
    [Space(5)]
    public ShootType shootType;

    [Header("#-----Main Weapon")]
    [Space(5)]
    public MainWeapon mainWeapon_Pfb;
    public MainWeaponShop mainWeaponShop;
    [HideInInspector]
    public MainWeapon mainWeapon;

    [Header("#-----Double Gun")]
    [Space(5)]
    public DoubleGun doubleGun_Pfb;
    public DoubleGunShop doubleGunShop;
    [HideInInspector]
    public DoubleGun doubleGun;

    [Header("#-----Bomber")]
    [Space(5)]
    public Bomber Bomber_Pfb;
    public BomberShop BomberShop;
    [HideInInspector]
    public Bomber BomberGun;

    [Header("#-----Tracking Gun")]
    [Space(5)]
    public TrackingGun trackingGun_Pfb;
    public TrackingGunShop trackingGunShop;
    [HideInInspector]
    public TrackingGun trackingGun;

    [Header("#-----Poison Gun")]
    [Space(5)]
    public PoisonPlane poison_Pfb;
    public PoisonShop poisonShop;
    [HideInInspector]
    public PoisonPlane poison;

    [Header("#-----Orbital Gun")]
    [Space(5)]
    public Orbital orbital_Pfb;
    public OrbitalShop orbitalShop;
    [HideInInspector]
    public Orbital orbital;

    [Header("#-----360Lazer Gun")]
    [Space(5)]
    public Lazer lazer_Pfb;
    public LazerShop lazerShop;
    [HideInInspector]
    public Lazer lazer;

    [Header("#-----strLaser Gun")]
    [Space(5)]
    public strLazer strLaser_pfb;
    public strLazerShop strLaserShop;
    [HideInInspector]
    public strLazer strLaser;

    [Header("#-----Sniper Gun")]
    [Space(5)]
    public SniperGun sniperGun_Pfb;
    public SniperGunShop sniperGunShop;
    [HideInInspector]
    public SniperGun sniperGun;

    [Header("#-----Bounce Gun")]
    [Space(5)]
    public BounceGun bounceGun_pfb;
    public BounceGunShop bounceGun_Shop;
    [HideInInspector]
    public BounceGun bounceGun;

    [Header("#-----Gun Spawn Location")]
    [Space(5)]
    public Transform BottomPoint;
    public Transform TopPoint;

    public GameObject ExclamationMark;

    public void ActivateMainWeapon()
    {
        MainWeapon _w = Instantiate(mainWeapon_Pfb, new Vector3(0, (Camera.main.ScreenToWorldPoint(BottomPoint.position).y + Camera.main.ScreenToWorldPoint(TopPoint.position).y) / 2), Quaternion.identity, transform);
        mainWeapon = _w;
        mainWeaponShop.mainWeapon = _w;
        _w.gameObject.SetActive(false);
    }
    public void ActivateDoubleGun()
    {
        DoubleGun _w = Instantiate(doubleGun_Pfb, new Vector3(0, (Camera.main.ScreenToWorldPoint(BottomPoint.position).y + Camera.main.ScreenToWorldPoint(TopPoint.position).y) / 2), Quaternion.identity, transform);
        doubleGun = _w;
        doubleGunShop.weapon = _w;
        _w.gameObject.SetActive(false);
    }
    public void ActivateBomber()
    {
        Bomber _w = Instantiate(Bomber_Pfb, new Vector3(0, (Camera.main.ScreenToWorldPoint(BottomPoint.position).y + Camera.main.ScreenToWorldPoint(TopPoint.position).y) / 2), Quaternion.identity, transform);
        BomberGun = _w;
        BomberShop.bomber = _w;
        _w.gameObject.SetActive(false);
    }
    public void ActivateTrackingGun()
    {
        TrackingGun _w = Instantiate(trackingGun_Pfb, new Vector3(0, (Camera.main.ScreenToWorldPoint(BottomPoint.position).y + Camera.main.ScreenToWorldPoint(TopPoint.position).y) / 2), Quaternion.identity, transform);
        trackingGun = _w;
        trackingGunShop.weapon = _w;
        _w.gameObject.SetActive(false);
    }
    public void ActivatePoison()
    {
        PoisonPlane _w = Instantiate(poison_Pfb, new Vector3(0, (Camera.main.ScreenToWorldPoint(BottomPoint.position).y + Camera.main.ScreenToWorldPoint(TopPoint.position).y) / 2), Quaternion.identity, transform);
        poison = _w;
        poisonShop.weapon = _w;
        _w.gameObject.SetActive(false);
    }

    public void ActivateBounceGun()
    {
        BounceGun _w = Instantiate(bounceGun_pfb, new Vector3(0, (Camera.main.ScreenToWorldPoint(BottomPoint.position).y + Camera.main.ScreenToWorldPoint(TopPoint.position).y) / 2), Quaternion.identity, transform);
        bounceGun = _w;
        bounceGun_Shop.weapon = _w;
        _w.gameObject.SetActive(false);
    }

    public void ActivateOrbital()
    {
        Orbital _w = Instantiate(orbital_Pfb, new Vector3(0, (Camera.main.ScreenToWorldPoint(BottomPoint.position).y + Camera.main.ScreenToWorldPoint(TopPoint.position).y) / 2), Quaternion.identity, transform);
        orbital = _w;
        orbitalShop.weapon = _w;
        _w.gameObject.SetActive(false);
    }
    public void ActivateLazer()
    {
        Lazer _w = Instantiate(lazer_Pfb, new Vector3(0, (Camera.main.ScreenToWorldPoint(BottomPoint.position).y + Camera.main.ScreenToWorldPoint(TopPoint.position).y) / 2), Quaternion.identity, transform);
        lazer = _w;
        lazerShop.weapon = _w;
        _w.gameObject.SetActive(false);
    }

    public void ActivatestrLaser()
    {
        strLazer _w = Instantiate(strLaser_pfb, new Vector3(0, (Camera.main.ScreenToWorldPoint(BottomPoint.position).y + Camera.main.ScreenToWorldPoint(TopPoint.position).y) / 2), Quaternion.identity, transform);
        strLaser = _w;
        strLaserShop.weapon = _w;
        _w.gameObject.SetActive(false);
    }

    public void ActivateSniperGun()
    {
        SniperGun _w = Instantiate(sniperGun_Pfb, new Vector3(0, (Camera.main.ScreenToWorldPoint(BottomPoint.position).y + Camera.main.ScreenToWorldPoint(TopPoint.position).y) / 2), Quaternion.identity, transform);
        sniperGun = _w;
        sniperGunShop.weapon = _w;
        _w.gameObject.SetActive(false);
    }
    private void Awake()
    {
        instance = this;
        ActivateMainWeapon();
        ActivateDoubleGun();
        ActivateBomber();
        ActivateTrackingGun();
        ActivatePoison();
        ActivateOrbital();
        ActivateLazer();
        ActivatestrLaser();
        ActivateSniperGun();
        ActivateBounceGun();

        SetPurchaseSize(1);
    }

    private void Start()
    {
        InvokeRepeating("checkExclamationMark", 0, 1f);
    }

    private void Update()
    {
        //MainWeapon
        mainWeapon.SetDamage();
        mainWeaponShop.setPassiveValues();

        //DoubleGun
        doubleGun.SetDamage();
        doubleGunShop.setPassiveValues();

        //TrackingGun
        trackingGun.SetDamage();
        trackingGunShop.setPassiveValues();

        //Bomber
        BomberGun.SetDamage();
        BomberShop.setPassiveValues();

        //Poison
        poison.SetDamage();
        poisonShop.setPassiveValues();

        //Sniper
        sniperGun.SetDamage();
        sniperGunShop.setPassiveValues();

        //Lazer
        lazer.SetDamage();
        lazerShop.setPassiveValues();

        //Orbital
        orbital.SetDamage();
        orbitalShop.setPassiveValues();

        //Str Laser
        strLaser.SetDamage();
        strLaserShop.setPassiveValues();

        //BounceGun
        bounceGun.SetDamage();
        bounceGun_Shop.setPassiveValues();


        if(shootType == ShootType.NormalShoot) {
            mainWeapon.NormalShooting();
            doubleGun.NormalShooting();
            BomberGun.NormalShooting();
            trackingGun.NormalShooting();
            sniperGun.NormalShooting();
            lazer.NormalShooting();
            strLaser.NormalShooting();
            poison.NormalShooting();
            bounceGun.NormalShooting();
        } else if(shootType == ShootType.AutoShoot) {
            mainWeapon.AutoShooting();
            doubleGun.AutoShooting();
            BomberGun.AutoShooting();
            trackingGun.AutoShooting();
            sniperGun.AutoShooting();
            lazer.AutoShooting();
            strLaser.AutoShooting();
            poison.NormalShooting();
            bounceGun.AutoShooting();
        } else if(shootType == ShootType.DontShoot) {
            mainWeapon.DonShoot();
            doubleGun.DonShoot();
            BomberGun.DonShoot();
            trackingGun.DonShoot();
            sniperGun.DonShoot();
            lazer.DonShoot();
            strLaser.DonShoot();
            poison.DonShoot();
            bounceGun.DonShoot();
        }

        if(StageManager.instance.isInStage) {
            if(Time.timeScale > 0.5f) {
                if(shootType == ShootType.AutoShoot) {
                    if(Player.instance.AutoShootTime > 0)
                        Player.instance.AutoShootTime -= Time.deltaTime / Time.timeScale;
                    else {
                        shootType = ShootType.NormalShoot;
                        Player.instance.AutoShootTime = 0;
                    }
                }
            }
        }
    }

    public int getTotalWeaponLevel()
    {
        int sum = 0;
        sum = mainWeapon.UpgradeLevel + doubleGun.UpgradeLevel + trackingGun.UpgradeLevel + poison.UpgradeLevel + orbital.UpgradeLevel + lazer.UpgradeLevel + strLaser.UpgradeLevel + bounceGun.UpgradeLevel + BomberGun.UpgradeLevel + sniperGun.UpgradeLevel;
        return sum;
    }

    public void checkExclamationMark()
    {
        ExclamationMark.SetActive(false);
        if(GameManager.GetMoney() >= MainWeaponShop.Instance.getTotalMoney(mainWeapon.UpgradeLevel, 1))
        {
            ExclamationMark.SetActive(true);
            return;
        }

        if(GameManager.GetMoney() >= DoubleGunShop.Instance.getTotalMoney(doubleGun.UpgradeLevel, 1) && !doubleGunShop.LockCover.activeSelf)
        {
            ExclamationMark.SetActive(true);
            return;
        }
        if(GameManager.GetMoney() >= TrackingGunShop.instance.getTotalMoney(trackingGun.UpgradeLevel, 1) && !trackingGunShop.LockCover.activeSelf)
        {
            ExclamationMark.SetActive(true);
            return;
        }
        if(GameManager.GetMoney() >= PoisonShop.instance.getTotalMoney(poison.UpgradeLevel, 1) && !poisonShop.LockCover.activeSelf)
        {
            ExclamationMark.SetActive(true);
            return;
        }
        if(GameManager.GetMoney() >= OrbitalShop.Instance.getTotalMoney(orbital.UpgradeLevel, 1) && !orbitalShop.LockCover.activeSelf)
        {
            ExclamationMark.SetActive(true);
            return;
        }
        if(GameManager.GetMoney() >= LazerShop.Instance.getTotalMoney(lazer.UpgradeLevel, 1) && !lazerShop.LockCover.activeSelf)
        {
            ExclamationMark.SetActive(true);
            return;
        }
        if(GameManager.GetMoney() >= strLazerShop.instance.getTotalMoney(strLaser.UpgradeLevel, 1) && !strLaserShop.LockCover.activeSelf)
        {
            ExclamationMark.SetActive(true);
            return;
        }
        if(GameManager.GetMoney() >= BomberShop.instance.getTotalMoney(BomberGun.UpgradeLevel, 1) && !BomberShop.LockCover.activeSelf)
        {
            ExclamationMark.SetActive(true);
            return;
        }
        if(GameManager.GetMoney() >= SniperGunShop.instance.getTotalMoney(sniperGun.UpgradeLevel, 1) && !sniperGunShop.LockCover.activeSelf)
        {
            ExclamationMark.SetActive(true);
            return;
        }
        if(GameManager.GetMoney() >= BounceGunShop.Instance.getTotalMoney(bounceGun.UpgradeLevel, 1) && !bounceGun_Shop.LockCover.activeSelf)
        {
            ExclamationMark.SetActive(true);
            return;
        }
    }

    public void SetPurchaseSize(int size)
    {
        mainWeaponShop.purchaseSize = size;
        doubleGunShop.purchaseSize = size;
        BomberShop.purchaseSize = size;
        trackingGunShop.purchaseSize = size;
        poisonShop.purchaseSize = size;
        orbitalShop.purchaseSize = size;
        lazerShop.purchaseSize = size;
        strLaserShop.purchaseSize = size;
        sniperGunShop.purchaseSize = size;
        bounceGun_Shop.purchaseSize = size;
    }

    public void AutoAttack()
    {
        if(shootType != ShootType.AutoShoot) {
            if(Player.instance.AutoShootTime > 0) {
                shootType = ShootType.AutoShoot;
            } else {
                Debug.Log("구매하세요");
            }
        } else {
            shootType = ShootType.NormalShoot;
        }
        GameManager.instance.saveGameSpeedandAuto(Time.timeScale, shootType == ShootType.AutoShoot);
    }

    public void noLimitAutoAttack()
    {
        if(shootType == ShootType.NormalShoot)
        {
            shootType = ShootType.AutoShoot;
        } else
        {
            shootType = ShootType.NormalShoot;
        }
    }

    public void LoadData(GameData data)
    {
        //Main Weapon
        data.UpgradeLevel_Weapon.TryGetValue(mainWeapon.id, out int lvl);
        if(lvl > 0)
            mainWeapon.gameObject.SetActive(true);
        mainWeapon.UpgradeLevel = lvl;

        //Double Gun
        data.UpgradeLevel_Weapon.TryGetValue(doubleGun.id, out int lvl1);
        if(lvl1 > 0)
            doubleGun.gameObject.SetActive(true);
        doubleGun.UpgradeLevel = lvl1;

        //Bomber Gun
        data.UpgradeLevel_Weapon.TryGetValue(BomberGun.id, out int bomberlvl);
        if(bomberlvl > 0)
            BomberGun.gameObject.SetActive(true);
        BomberGun.UpgradeLevel = bomberlvl;

        //Tracking Gun
        data.UpgradeLevel_Weapon.TryGetValue(trackingGun.id, out int lvl2);
        if(lvl2 > 0)
            trackingGun.gameObject.SetActive(true);
        trackingGun.UpgradeLevel = lvl2;

        //Poison
        data.UpgradeLevel_Weapon.TryGetValue(poison.id, out int lvl3);
        if(lvl3 > 0)
            poison.gameObject.SetActive(true);
        poison.UpgradeLevel = lvl3;

        //Orbital
        data.UpgradeLevel_Weapon.TryGetValue(orbital.id, out int lvl4);
        if(lvl4 > 0)
            orbital.gameObject.SetActive(true);
        orbital.UpgradeLevel = lvl4;

        //Lazer
        data.UpgradeLevel_Weapon.TryGetValue(lazer.id, out int lvl5);
        if(lvl5 > 0)
            lazer.gameObject.SetActive(true);
        lazer.UpgradeLevel = lvl5;

        //strLaser
        data.UpgradeLevel_Weapon.TryGetValue(strLaser.id, out int lvl7);
        if(lvl7 > 0)
            strLaser.gameObject.SetActive(true);
        strLaser.UpgradeLevel = lvl7;

        //Sniper
        data.UpgradeLevel_Weapon.TryGetValue(sniperGun.id, out int lvl6);
        if(lvl6 > 0)
            sniperGun.gameObject.SetActive(true);
        sniperGun.UpgradeLevel = lvl6;

        //BounceGun
        data.UpgradeLevel_Weapon.TryGetValue(bounceGun.id, out int lvl8);
        if(lvl8 > 0)
            bounceGun.gameObject.SetActive(true);
        bounceGun.UpgradeLevel = lvl8;

        //MainWeapon
        mainWeapon.SetDamage();
        mainWeaponShop.setPassiveValues();

        //DoubleGun
        doubleGun.SetDamage();
        doubleGunShop.setPassiveValues();

        //TrackingGun
        trackingGun.SetDamage();
        trackingGunShop.setPassiveValues();

        //Bomber
        BomberGun.SetDamage();
        BomberShop.setPassiveValues();

        //Poison
        poison.SetDamage();
        poisonShop.setPassiveValues();

        //Sniper
        sniperGun.SetDamage();
        sniperGunShop.setPassiveValues();

        //Lazer
        lazer.SetDamage();
        lazerShop.setPassiveValues();

        //Orbital
        orbital.SetDamage();
        orbitalShop.setPassiveValues();

        //Str Laser
        strLaser.SetDamage();
        strLaserShop.setPassiveValues();

        //Bounce Gun
        bounceGun.SetDamage();
        bounceGun_Shop.setPassiveValues();
    }

    public void SaveData(GameData data)
    {
        //Main Weapon
        if(data.UpgradeLevel_Weapon.ContainsKey(mainWeapon.id))
            data.UpgradeLevel_Weapon.Remove(mainWeapon.id);
        data.UpgradeLevel_Weapon.Add(mainWeapon.id, mainWeapon.UpgradeLevel);

        //Double Gun
        if(data.UpgradeLevel_Weapon.ContainsKey(doubleGun.id))
            data.UpgradeLevel_Weapon.Remove(doubleGun.id);
        data.UpgradeLevel_Weapon.Add(doubleGun.id, doubleGun.UpgradeLevel);

        //Bomber Gun
        if(data.UpgradeLevel_Weapon.ContainsKey(BomberGun.id))
            data.UpgradeLevel_Weapon.Remove(BomberGun.id);
        data.UpgradeLevel_Weapon.Add(BomberGun.id, BomberGun.UpgradeLevel);

        //Tracking Gun
        if(data.UpgradeLevel_Weapon.ContainsKey(trackingGun.id))
            data.UpgradeLevel_Weapon.Remove(trackingGun.id);
        data.UpgradeLevel_Weapon.Add(trackingGun.id, trackingGun.UpgradeLevel);

        //Poison
        if(data.UpgradeLevel_Weapon.ContainsKey(poison.id))
            data.UpgradeLevel_Weapon.Remove(poison.id);
        data.UpgradeLevel_Weapon.Add(poison.id, poison.UpgradeLevel);

        //Orbital
        if(data.UpgradeLevel_Weapon.ContainsKey(orbital.id))
            data.UpgradeLevel_Weapon.Remove(orbital.id);
        data.UpgradeLevel_Weapon.Add(orbital.id, orbital.UpgradeLevel);

        //Laser
        if(data.UpgradeLevel_Weapon.ContainsKey(lazer.id))
            data.UpgradeLevel_Weapon.Remove(lazer.id);
        data.UpgradeLevel_Weapon.Add(lazer.id, lazer.UpgradeLevel);

        //Laser
        if(data.UpgradeLevel_Weapon.ContainsKey(strLaser.id))
            data.UpgradeLevel_Weapon.Remove(strLaser.id);
        data.UpgradeLevel_Weapon.Add(strLaser.id, strLaser.UpgradeLevel);

        //Sniper
        if(data.UpgradeLevel_Weapon.ContainsKey(sniperGun.id))
            data.UpgradeLevel_Weapon.Remove(sniperGun.id);
        data.UpgradeLevel_Weapon.Add(sniperGun.id, sniperGun.UpgradeLevel);

        //BounceGun
        if(data.UpgradeLevel_Weapon.ContainsKey(bounceGun.id))
            data.UpgradeLevel_Weapon.Remove(bounceGun.id);
        data.UpgradeLevel_Weapon.Add(bounceGun.id, bounceGun.UpgradeLevel);
    }
}
