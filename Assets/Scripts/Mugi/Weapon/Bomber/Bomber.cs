using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bomber : MonoBehaviour
{
    [SerializeField] public string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public static Bomber instance;

    [HideInInspector]
    public int UpgradeLevel;

    [HideInInspector]
    public int Damage;

    [Header("#-----Shooting Point")]
    [Space(10)]
    public Transform ShootingPoint;
    public Quaternion rotate;

    [Header("#-----Reload")]
    [Space(10)]
    public float MaximumAttackSpeed;
    public float RelodingSpeed;

    [Header("#-----Passive Stats")]
    public int ExploseDamage;
    public bool ExploseRangeIncrease;
    public float ExtraAttackSpeedScale;
    public float ExploseDamagePercent;

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
        Damage = 100;

        if(UpgradeLevel >= 0)
            Damage += UpgradeLevel * 4;

        if(UpgradeLevel >= 20)
            Damage += (UpgradeLevel - 20) * 6;

        if(UpgradeLevel >= 100)
            Damage += (UpgradeLevel - 100) * 7;

        if(UpgradeLevel >= 200)
            Damage += (UpgradeLevel - 200) * 6;

        if(UpgradeLevel >= 300)
            Damage += (UpgradeLevel - 300) * 5;

        if(UpgradeLevel >= 500)
            Damage += (UpgradeLevel - 500) * 3;

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
        if(RelodingSpeed * (1 - TraitsManager.instance.GetStats(TraitsData.WeaponType.Bomber, TraitsData.TraitsType.AttackSpeed)) * (1 - Player.instance.FinalAttack_SpeedPercent) * (1 - ExtraAttackSpeedScale) <= MaximumAttackSpeed) {
            return MaximumAttackSpeed;
        } else {
            return RelodingSpeed * (1 - TraitsManager.instance.GetStats(TraitsData.WeaponType.Bomber, TraitsData.TraitsType.AttackSpeed)) * (1 - Player.instance.FinalAttack_SpeedPercent) * (1 - ExtraAttackSpeedScale);
        }
    }

    public void CreateBullet()
    {
        BomberBullet bullet_A = ObjectPoolBullet.Instance.GetPoolObject(PoolObjectTypeBullet.Bomber).GetComponent<BomberBullet>();
        bullet_A.setBulletStats((int)(Player.instance.FinalAttack_Damage + ((Damage + ExploseDamage) * (1 + ExploseDamagePercent + TraitsManager.instance.GetStats(TraitsData.WeaponType.Bomber, TraitsData.TraitsType.DamagePercent)))));
        bullet_A.isCritical = Random.Range(1, 101) <= Player.instance.FinalCriticalChance;
        bullet_A.isStunning = Random.Range(1, 101) <= Player.instance.FinalStunPower;
        bullet_A.transform.position = ShootingPoint.position;
        bullet_A.transform.rotation = rotate;
        bullet_A.gameObject.SetActive(true);
    }

    public void NormalShooting()
    {
        Vector3 Mousetarget = new Vector3(0, 0, 0);
        Mousetarget = GameManager.instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 difference = Mousetarget - this.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ - 90);
        IsShooting = true;

        if(EnemyManager.Instance.FindCrowdedEnemy() != null) {
            difference = EnemyManager.Instance.FindCrowdedEnemy().pos - transform.position;
            rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            rotate = Quaternion.Euler(0, 0, rotationZ - 90);
        } else {
            rotate = Quaternion.Euler(0.0f, 0.0f, rotationZ - 90);
        }
    }

    public void DonShoot()
    {
        this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0);
        IsShooting = false;
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

            //Actual Shooting Rotation
            if(EnemyManager.Instance.FindCrowdedEnemy() != null) {
                difference = EnemyManager.Instance.FindCrowdedEnemy().pos - transform.position;
            } else {
                difference = Vector3.zero - this.transform.position;
            }
            rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            rotate = Quaternion.Euler(0, 0, rotationZ - 90);          
        }
    }
}
