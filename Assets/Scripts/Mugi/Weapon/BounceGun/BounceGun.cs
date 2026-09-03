using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BounceGun : MonoBehaviour
{
    [SerializeField] public string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public static BounceGun instance;

    [HideInInspector]
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
    public float ExtraAttackSpeedScale;
    public bool ExtraBullet;
    public bool ExtraBounce;
    public bool NoDamageReduce;

    bool IsShooting;
    float TimerAs;
    public List<BounceEnemy> bounceEnemyList_Bullet;

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
        Damage = 10;

        if(UpgradeLevel >= 0)
            Damage += UpgradeLevel;

        if(UpgradeLevel >= 20)
            Damage += (UpgradeLevel - 20) * 1;

        if(UpgradeLevel >= 100)
            Damage += (UpgradeLevel - 100) * 2;

        if(UpgradeLevel >= 200)
            Damage += (UpgradeLevel - 200) * 3;

        if(UpgradeLevel >= 300)
            Damage += (UpgradeLevel - 300) * 2;

        if(UpgradeLevel >= 500)
            Damage += (UpgradeLevel - 500) * 1;

    }

    private void Update()
    {
        if(IsShooting) {
            TimerAs += Time.deltaTime;
            if(TimerAs >= getAttackSpeed()) {
                BounceEnemy be = new BounceEnemy();

                if(!ExtraBullet) {
                    CreateBullet(be);
                } else {
                    CreateBullet(be);
                    CreateBullet(be);
                }
                TimerAs = 0;
            }
        }
    }

    public float getAttackSpeed()
    {
        if(RelodingSpeed * (1 - TraitsManager.instance.GetStats(TraitsData.WeaponType.BounceGun, TraitsData.TraitsType.AttackSpeed)) * (1 - Player.instance.FinalAttack_SpeedPercent) * (1 - ExtraAttackSpeedScale) <= MaximumAttackSpeed) {
            return MaximumAttackSpeed;
        } else {
            return RelodingSpeed * (1 - TraitsManager.instance.GetStats(TraitsData.WeaponType.BounceGun, TraitsData.TraitsType.AttackSpeed)) * (1 - Player.instance.FinalAttack_SpeedPercent) * (1 - ExtraAttackSpeedScale);
        }
    }

    public void CreateBullet(BounceEnemy be)
    {
        BounceBullet bullet_A = ObjectPoolBullet.Instance.GetPoolObject(PoolObjectTypeBullet.BounceBullet).GetComponent<BounceBullet>();
        bullet_A.setBulletStats((int)(Player.instance.FinalAttack_Damage + ((Damage + ExtraDamage) * (1 + ExtraDamagePercent + TraitsManager.instance.GetStats(TraitsData.WeaponType.BounceGun, TraitsData.TraitsType.DamagePercent)))));
        bullet_A.isCritical = Random.Range(1, 101) <= Player.instance.FinalCriticalChance;
        bullet_A.isStunning = Random.Range(1, 101) <= Player.instance.FinalStunPower;
        bullet_A.transform.position = ShootingPoint.position;
        bullet_A.bounceEnemy = be;
        bullet_A.BounceTime = 0;

        Enemy e = EnemyManager.Instance.FindCloseEnemy(bullet_A.bounceEnemy);
        if(e != null) {
            //To Closest Enemy
            Vector3 difference = new Vector3(0, 0, 0);
            difference = e.pos - bullet_A.transform.position;
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            bullet_A.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ - 90);
            bullet_A.TargetEnemy = e;
            bounceEnemyList_Bullet.Add(be);
        } else {
            bullet_A.transform.rotation = transform.rotation;
        }

        bullet_A.gameObject.SetActive(true);
        bullet_A.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
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

[System.Serializable]
public class BounceEnemy
{
    public List<Enemy> BouncedEnemies;

    public BounceEnemy()
    {
        BouncedEnemies = new List<Enemy>();
    }

    public bool checkBouncedEnemyisInList(Enemy enemy)
    {
        if(BouncedEnemies != null) {
            if(BouncedEnemies.Find(x => x.gameObject == enemy.gameObject) != null) {
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
    }
}
