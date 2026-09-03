using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SniperGun : MonoBehaviour
{
    [SerializeField] public string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public static SniperGun instance;

    [HideInInspector]
    public int UpgradeLevel;

    [HideInInspector]
    public int Damage;

    [Header("#-----Laser")]
    public CreateLine m_createLine;

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
    public float ExtraCritDamge;
    public bool penetratingEffect;
    public bool DoubleAttack;

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
        Damage = 20;

        if(UpgradeLevel >= 0)
            Damage += UpgradeLevel * 4;

        if(UpgradeLevel >= 50)
            Damage += (UpgradeLevel - 50) * 5;

        if(UpgradeLevel >= 100)
            Damage += (UpgradeLevel - 100) * 6;

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

            if(TimerAs >= (getAttackSpeed() <= MaximumAttackSpeed ? MaximumAttackSpeed : getAttackSpeed() * 0.65f) && TimerAs < (getAttackSpeed() <= MaximumAttackSpeed ? MaximumAttackSpeed : getAttackSpeed())) {
                m_createLine.m_lineRenderer.enabled = true;
                m_createLine.ShootLaser();
            } else if(TimerAs >= (getAttackSpeed() <= MaximumAttackSpeed ? MaximumAttackSpeed : getAttackSpeed())) {
                m_createLine.m_lineRenderer.enabled = false;

                if(!DoubleAttack) {
                    CreateBullet();
                    TimerAs = 0;
                }
                else {
                    CreateBullet();
                    StartCoroutine(DelayedAttack());
                    TimerAs = 0;
                }

                SoundManager.Instance.playWeaponSFX(SoundManager.WeaponSFXType.Sniper);
            }
        }
    }

    private IEnumerator DelayedAttack()
    {
        yield return new WaitForSeconds(MaximumAttackSpeed / 2);
        r = Random.Range(0.0f, 101.0f);
        // Second attack with 75% chance
        if(r <= 75) {
            CreateBullet();
        }
    }


    public float getAttackSpeed()
    {
        return RelodingSpeed * (1 - TraitsManager.instance.GetStats(TraitsData.WeaponType.SniperGun, TraitsData.TraitsType.AttackSpeed)) * (1 - Player.instance.FinalAttack_SpeedPercent);
    }

    public void CreateBullet()
    {
        SniperBullet bullet_A = ObjectPoolBullet.Instance.GetPoolObject(PoolObjectTypeBullet.Sniper).GetComponent<SniperBullet>();
        bullet_A.setBulletStats((int)(Player.instance.FinalAttack_Damage + ((Damage + ExtraDamage) * (1 + ExtraDamagePercent + TraitsManager.instance.GetStats(TraitsData.WeaponType.SniperGun, TraitsData.TraitsType.DamagePercent)))), penetratingEffect, ExtraCritDamge);
        bullet_A.isCritical = true;
        bullet_A.isStunning = Random.Range(1, 101) <= Player.instance.FinalStunPower;
        bullet_A.transform.position = ShootingPoint.position;
        bullet_A.transform.rotation = transform.rotation;
        bullet_A.gameObject.SetActive(true);
    }

    public void DonShoot()
    {
        this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0);
        m_createLine.m_lineRenderer.enabled = false;
        TimerAs = 0;
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
            IsShooting = true;
        } else {
            Vector3 difference = new Vector3(0, 0, 0);
            difference = EnemyManager.Instance.FindCloseEnemy().pos - this.transform.position;
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ - 90);
            IsShooting = true;
        }
    }
}
