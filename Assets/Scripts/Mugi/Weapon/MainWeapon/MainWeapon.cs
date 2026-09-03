using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainWeapon : MonoBehaviour
{
    [SerializeField] public string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public static MainWeapon instance;

    public int UpgradeLevel;

    [HideInInspector]
    public int Damage;

    [Header("#-----Shooting Point")]
    [Space(10)]
    public Transform ShootingPoint;

    [Header("#-----Reload")]
    [Space(10)]
    public float MaximumAttackSpeed;
    public float RelodingSpeed;

    [Header("#-----Passive Stats")]
    public int ExtraDamage;
    public float ExtraDamagePercent;
    public float ExtraCritChance;
    public float ExtraCritDamage;
    public bool diamondPerHit;

    bool IsShooting;
    float TimerAs;

    private void Awake()
    {
        instance = this;
    }

    public void UpgradeWeapon()
    {
        UpgradeLevel++;
        SetDamage();
    }

    public void SetDamage()
    {
        Damage = 0;

        if(UpgradeLevel >= 0) 
            Damage += UpgradeLevel;
        
        if(UpgradeLevel >= 20) 
            Damage += (UpgradeLevel - 20) * 2;
        
        if(UpgradeLevel >= 100) 
            Damage += (UpgradeLevel - 100) * 3;
        
        if(UpgradeLevel >= 200) 
            Damage += (UpgradeLevel - 200) * 4;
        
        if(UpgradeLevel >= 300) 
            Damage += (UpgradeLevel - 300) * 3;
        
        if(UpgradeLevel >= 500) 
            Damage += (UpgradeLevel - 500);
        
    }

    private void Update()
    {
        if(IsShooting) {
            TimerAs += Time.deltaTime;
            if(TimerAs >= getAttackSpeed()) {
                CreateBullet();
                TimerAs = 0;
            }
        }
    }

    public float getAttackSpeed()
    {
        if(RelodingSpeed * (1 - TraitsManager.instance.GetStats(TraitsData.WeaponType.MainWeapon, TraitsData.TraitsType.AttackSpeed)) * (1 - Player.instance.FinalAttack_SpeedPercent) <= MaximumAttackSpeed) {
            return MaximumAttackSpeed;
        } else {
            return RelodingSpeed * (1 - TraitsManager.instance.GetStats(TraitsData.WeaponType.MainWeapon, TraitsData.TraitsType.AttackSpeed)) * (1 - Player.instance.FinalAttack_SpeedPercent);
        }
    }


    public void CreateBullet()
    {
        MainBullet bullet_A = ObjectPoolBullet.Instance.GetPoolObject(PoolObjectTypeBullet.Main).GetComponent<MainBullet>();
        bullet_A.setBulletStats((int)(Player.instance.FinalAttack_Damage + ((Damage + ExtraDamage) * (1 + ExtraDamagePercent + TraitsManager.instance.GetStats(TraitsData.WeaponType.MainWeapon, TraitsData.TraitsType.DamagePercent)))), Player.instance.FinalCriticalDamage + ExtraCritDamage);
        bullet_A.isCritical = Random.Range(1, 101) <= Player.instance.FinalCriticalChance + ExtraCritChance;
        bullet_A.isStunning = Random.Range(1, 101) <= Player.instance.FinalStunPower;
        bullet_A.transform.position = ShootingPoint.position;
        bullet_A.transform.rotation = transform.rotation;
        bullet_A.gameObject.SetActive(true);
    }
    public void DonShoot()
    {
        this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0);
        IsShooting = false;
    }
    public void NormalShooting()
    {
        Vector3 Mousetarget = new Vector3(0, 0, 0);
        Mousetarget = GameManager.instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 difference = Mousetarget - this.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ - 90);
        IsShooting = true;
    }

    public void AutoShooting()
    {
        Enemy _e = EnemyManager.Instance.FindCloseEnemy();
        if(_e == null) {
            Vector3 Mousetarget = new Vector3(0, 0, 0);
            Vector3 difference = Mousetarget - this.transform.position;
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ - 90); 
            IsShooting = false;
        } else {
            Vector3 difference = new Vector3(0, 0, 0);
            difference = EnemyManager.Instance.FindCloseEnemy().pos - this.transform.position;
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ - 90);
            IsShooting = true;
        }
    }
}
