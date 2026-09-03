using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleGun : MonoBehaviour
{
    [SerializeField] public string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public static DoubleGun instance;

    [HideInInspector]
    public int UpgradeLevel;

    [HideInInspector]
    public int Damage;

    [Header("#-----Weapon Info")]
    [Space(10)]
    public Transform ShootingPoint1;
    public Transform ShootingPoint2;

    [Header("#-----Reload")]
    [Space(10)]
    public float MaximumAttackSpeed;
    public float RelodingSpeed;

    [Header("#-----Passive Stats")]
    public float ExtraDamagePercent;
    public float ExtraAttackSpeedScale;
    public float ExtraBulletSpeedScale;
    public bool doubleAttack;

    bool IsShooting;
    float TimerAs;
    float r;

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
        Damage = 5;

        if(UpgradeLevel >= 0)
            Damage += UpgradeLevel * 1;

        if(UpgradeLevel >= 50)
            Damage += (UpgradeLevel - 50) * 1;

        if(UpgradeLevel >= 100)
            Damage += (UpgradeLevel - 100) * 2;

        if(UpgradeLevel >= 200)
            Damage += (UpgradeLevel - 200) * 3;

        if(UpgradeLevel >= 300)
            Damage += (UpgradeLevel - 300) * 2;

        if(UpgradeLevel >= 500)
            Damage += (UpgradeLevel - 500);

    }
    private void Update()
    {
        if(IsShooting) {
            TimerAs += Time.deltaTime;
            if(TimerAs >= getAttackSpeed()) {
                CreateBullet();
                if(doubleAttack) {
                    StartCoroutine(DelayedAttack());
                    TimerAs = 0;
                } else
                    TimerAs = 0;
            }
        }
    }

    private IEnumerator DelayedAttack()
    {
        yield return new WaitForSeconds(MaximumAttackSpeed / 2);
        r = Random.Range(0.0f, 101.0f);
        if(r <= 1.0f) {
            CreateBullet();
        }
    }


    public float getAttackSpeed()
    {
        if(RelodingSpeed * (1 - TraitsManager.instance.GetStats(TraitsData.WeaponType.DoubleGun, TraitsData.TraitsType.AttackSpeed)) * (1 - Player.instance.FinalAttack_SpeedPercent) * (1 - ExtraAttackSpeedScale) <= MaximumAttackSpeed) {
            return MaximumAttackSpeed;
        } else {
            return RelodingSpeed * (1 - TraitsManager.instance.GetStats(TraitsData.WeaponType.DoubleGun, TraitsData.TraitsType.AttackSpeed)) * (1 - Player.instance.FinalAttack_SpeedPercent) * (1 - ExtraAttackSpeedScale);
        }
    }

    public void CreateBullet()
    {
        DoubleGunBullet bullet_A = ObjectPoolBullet.Instance.GetPoolObject(PoolObjectTypeBullet.Double).GetComponent<DoubleGunBullet>();
        bullet_A.setBulletStats((int)(Player.instance.FinalAttack_Damage + (Damage * (1 + ExtraDamagePercent + TraitsManager.instance.GetStats(TraitsData.WeaponType.DoubleGun, TraitsData.TraitsType.DamagePercent)))), ExtraBulletSpeedScale);
        bullet_A.isCritical = Random.Range(1, 101) <= Player.instance.FinalCriticalChance;
        bullet_A.isStunning = Random.Range(1, 101) <= Player.instance.FinalStunPower;
        bullet_A.transform.position = ShootingPoint1.position;
        bullet_A.transform.rotation = this.transform.GetChild(1).transform.rotation;
        bullet_A.gameObject.SetActive(true);

        DoubleGunBullet bullet_B = ObjectPoolBullet.Instance.GetPoolObject(PoolObjectTypeBullet.Double).GetComponent<DoubleGunBullet>();
        bullet_A.setBulletStats((int)(Player.instance.FinalAttack_Damage + (Damage * (1 + ExtraDamagePercent + TraitsManager.instance.GetStats(TraitsData.WeaponType.DoubleGun, TraitsData.TraitsType.DamagePercent)))), ExtraBulletSpeedScale);
        bullet_B.isCritical = Random.Range(1, 101) <= Player.instance.FinalCriticalChance;
        bullet_B.isStunning = Random.Range(1, 101) <= Player.instance.FinalStunPower;
        bullet_B.transform.position = ShootingPoint2.position;
        bullet_B.transform.rotation = this.transform.GetChild(0).transform.rotation;
        bullet_B.gameObject.SetActive(true);
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

        Vector3 difference2 = Mousetarget - this.transform.position;
        float rotationZ2 = Mathf.Atan2(difference2.y, difference2.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ2 - 90);

        Vector3 difference = Mousetarget - this.transform.GetChild(0).transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        this.transform.GetChild(0).transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ - 90);

        Vector3 difference1 = Mousetarget - this.transform.GetChild(1).transform.position;
        float rotationZ1 = Mathf.Atan2(difference1.y, difference1.x) * Mathf.Rad2Deg;
        this.transform.GetChild(1).transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ1 - 90);

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
            Vector3 difference2 = EnemyManager.Instance.FindCloseEnemy().pos - this.transform.position;
            float rotationZ2 = Mathf.Atan2(difference2.y, difference2.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ2 - 90);

            Vector3 difference = EnemyManager.Instance.FindCloseEnemy().pos - this.transform.GetChild(0).transform.position;
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            this.transform.GetChild(0).transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ - 90);

            Vector3 difference1 = EnemyManager.Instance.FindCloseEnemy().pos - this.transform.GetChild(1).transform.position;
            float rotationZ1 = Mathf.Atan2(difference1.y, difference1.x) * Mathf.Rad2Deg;
            this.transform.GetChild(1).transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ1 - 90);
            IsShooting = true;
        }
    }
}
