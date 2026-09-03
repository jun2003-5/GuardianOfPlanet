using System.Collections;
using UnityEngine;

public class PoisonPlane : MonoBehaviour
{
    [SerializeField] public string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public static PoisonPlane instance;

    [Header("#-----Shooting Point")]
    [Space(10)]
    public Transform ShootingPoint;

    [HideInInspector]
    public int UpgradeLevel;

    [HideInInspector]
    public int PoisonDamage;

    [Header("#-----Passive Stats")]
    public int ExtraDamage;
    public float ExtraDamagePercent;
    public bool extraBullet;
    public bool extraHit;
    public bool extraSizeOfPoisonArea;
    public bool Poison_canCrit;

    public float TimerAs;

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
        PoisonDamage = 20;

        if(UpgradeLevel >= 0)
            PoisonDamage += UpgradeLevel ;

        if(UpgradeLevel >= 50)
            PoisonDamage += (UpgradeLevel - 50) * 2;

        if(UpgradeLevel >= 100)
            PoisonDamage += (UpgradeLevel - 100) * 3;

        if(UpgradeLevel >= 200)
            PoisonDamage += (UpgradeLevel - 200) * 4;

        if(UpgradeLevel >= 300)
            PoisonDamage += (UpgradeLevel - 300) * 3;

        if(UpgradeLevel >= 500)
            PoisonDamage += (UpgradeLevel - 500) * 2;

    }

    private void Update()
    {
        TimerAs += Time.deltaTime;
        if(TimerAs >= 10 * (1 - TraitsManager.instance.GetStats(TraitsData.WeaponType.Poison, TraitsData.TraitsType.AttackSpeed))) {
            if(extraBullet) {
                CreateBullet(0);
                CreateBullet(180);
                CreateBullet(90);
                CreateBullet(270);

            } else {
                CreateBullet(300);
                CreateBullet(180);
                CreateBullet(60);
            }
            TimerAs = 0;
        }
    }

    public void CreateBullet(int a)
    {
        PoisonBullet bullet_A = ObjectPoolBullet.Instance.GetPoolObject(PoolObjectTypeBullet.Poison).GetComponent<PoisonBullet>();
        bullet_A.BulletDamage = (int)(Player.instance.FinalAttack_Damage + ((PoisonDamage + ExtraDamage) * (1 + ExtraDamagePercent + TraitsManager.instance.GetStats(TraitsData.WeaponType.Poison, TraitsData.TraitsType.DamagePercent))));
        bullet_A.transform.position = ShootingPoint.position;
        Quaternion rotation = Quaternion.Euler(0, 0, a);
        bullet_A.transform.rotation = rotation * transform.rotation;
        bullet_A.gameObject.SetActive(true);
    }

    public void DonShoot()
    {
        this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0);
    }
    public void NormalShooting()
    {
        Vector3 Mousetarget = new Vector3(0, 0, 0);
        Mousetarget = GameManager.instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 difference = Mousetarget - this.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ - 90);
    }
}
