using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    [SerializeField] public string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public static Lazer instance;

    [HideInInspector]
    public int UpgradeLevel;

    [HideInInspector]
    public int LazerDamage;

    [Header("Lazer Shoot")]
    public ShootLazer shootLazer;

    [Header("#-----Passive Stats")]
    public int ExtraDamage;
    public float ExtraDamagePercent;
    public bool ExtraHit;
    public float ExtraCritDamage;
    public bool SpinTwoTimes;

    float TimerAs;
    bool canShoot;

    public List<Enemy> enemyOnLaser;
    public List<Dummy> dummyOnLaser;

    private void Awake()
    {
        instance = this;
        transform.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    private void Update()
    {
        if(canShoot) {
            TimerAs += Time.deltaTime;
            if(TimerAs <= 10 * (1 - TraitsManager.instance.GetStats(TraitsData.WeaponType.CircleLaser, TraitsData.TraitsType.AttackSpeed))) {
                transform.GetComponent<Rigidbody2D>().isKinematic = true;
            } else if(TimerAs > 10 * (1 - TraitsManager.instance.GetStats(TraitsData.WeaponType.CircleLaser, TraitsData.TraitsType.AttackSpeed)) && TimerAs < (SpinTwoTimes ? 10 * (1 - TraitsManager.instance.GetStats(TraitsData.WeaponType.CircleLaser, TraitsData.TraitsType.AttackSpeed)) + 10 : 10 * (1 - TraitsManager.instance.GetStats(TraitsData.WeaponType.CircleLaser, TraitsData.TraitsType.AttackSpeed)) + 5)) {
                //Sound
                if(!SoundManager.Instance.laserSFXSource.isPlaying)
                    SoundManager.Instance.playWeaponSFX(SoundManager.WeaponSFXType.laser);

                doubleCheckEnemyOnLazer();
                transform.GetComponent<Rigidbody2D>().isKinematic = false;
                shootLazer.isShooting = true;
                this.transform.rotation = Quaternion.Euler(0, 0, 72 * (TimerAs - 10 * (1 - TraitsManager.instance.GetStats(TraitsData.WeaponType.CircleLaser, TraitsData.TraitsType.AttackSpeed))));
            } else if(TimerAs >= (SpinTwoTimes ? 10 * (1 - TraitsManager.instance.GetStats(TraitsData.WeaponType.CircleLaser, TraitsData.TraitsType.AttackSpeed)) + 10 : 10 * (1 - TraitsManager.instance.GetStats(TraitsData.WeaponType.CircleLaser, TraitsData.TraitsType.AttackSpeed)) + 5)) {
                if(SoundManager.Instance.laserSFXSource.isPlaying)
                    SoundManager.Instance.laserSFXSource.Stop();

                transform.GetComponent<Rigidbody2D>().isKinematic = true;
                shootLazer.isShooting = false;
                this.transform.localRotation = Quaternion.Euler(0, 0, 0);
                TimerAs = 0;
                enemyOnLaser.Clear();
            }
        }
    }


    public void UpgradeWeapon()
    {
        UpgradeLevel++;
        SetDamage();
    }

    public void SetDamage()
    {
        LazerDamage = 100;

        if(UpgradeLevel >= 0)
            LazerDamage += UpgradeLevel * 6;

        if(UpgradeLevel >= 20)
            LazerDamage += (UpgradeLevel - 20) * 7;

        if(UpgradeLevel >= 100)
            LazerDamage += (UpgradeLevel - 100) * 8;

        if(UpgradeLevel >= 200)
            LazerDamage += (UpgradeLevel - 200) * 9;

        if(UpgradeLevel >= 300)
            LazerDamage += (UpgradeLevel - 300) * 7;

        if(UpgradeLevel >= 500)
            LazerDamage += (UpgradeLevel - 500) * 2;

    }


    public void DonShoot()
    {
        shootLazer.isShooting = false;
        this.transform.localRotation = Quaternion.Euler(0, 0, 0);
        TimerAs = 0;
        canShoot = false;
    }
    public void NormalShooting()
    {
        Vector3 Mousetarget = new Vector3(0, 0, 0);
        Mousetarget = GameManager.instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 difference = Mousetarget - this.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ - 90);
        canShoot = true;
    }

    public void AutoShooting()
    {
        Enemy _e = EnemyManager.Instance.FindCloseEnemy();
        if(_e == null) {
            Vector3 Mousetarget = new Vector3(0, 0, 0);
            Vector3 difference = Mousetarget - this.transform.position;
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ - 90);
        } else {
            Vector3 difference = new Vector3(0, 0, 0);
            difference = EnemyManager.Instance.FindCloseEnemy().pos - this.transform.position;
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ - 90);
        }
        canShoot = true;
    }

    public IEnumerator laserAttack(Enemy enemy)
    {
        while(enemyOnLaser.Find(d => d.gameObject == enemy.gameObject) != null) {
            enemy.OnHit((int)(Player.instance.FinalAttack_Damage + ((LazerDamage + ExtraDamage) * (1 + ExtraDamagePercent + TraitsManager.instance.GetStats(TraitsData.WeaponType.CircleLaser, TraitsData.TraitsType.DamagePercent)))), false, Random.Range(0, 101) <= Player.instance.FinalCriticalChance, Player.instance.FinalCriticalDamage);
            yield return new WaitForSeconds(ExtraHit ? 0.05f : 0.1f);
        }
    }

    public IEnumerator laserAttack(Dummy dummy)
    {
        while(dummyOnLaser.Find(d => d.gameObject == dummy.gameObject) != null) {
            dummy.addDamage((int)(Player.instance.FinalAttack_Damage + ((LazerDamage + ExtraDamage) * (1 + ExtraDamagePercent + TraitsManager.instance.GetStats(TraitsData.WeaponType.SniperGun, TraitsData.TraitsType.DamagePercent)))), Random.Range(0, 101) <= Player.instance.FinalCriticalChance, Player.instance.FinalCriticalDamage);
            yield return new WaitForSeconds(ExtraHit ? 0.05f : 0.1f);
        }
    }

    public void addEnemyToList(Enemy data)
    {
        if(enemyOnLaser.Find(d => d.gameObject == data.gameObject) == null) {
            enemyOnLaser.Add(data);
            StartCoroutine(laserAttack(data));
            
        }
    }

    public void doubleCheckEnemyOnLazer()
    {
        List<Enemy> enemiesToRemove = new List<Enemy>();

        foreach(Enemy enemy in enemyOnLaser) {
            if(!IsCollidingWithLaser(enemy.gameObject)) {
                enemiesToRemove.Add(enemy);
            }
        }

        foreach(Enemy enemyToRemove in enemiesToRemove) {
            enemyOnLaser.Remove(enemyToRemove);
        }
    }

    private bool IsCollidingWithLaser(GameObject gameObject)
    {
        return Physics2D.OverlapPoint(gameObject.transform.position, LayerMask.GetMask("Player")) != null;
    }

    public void addDummyToList(Dummy dummy)
    {
        if(dummyOnLaser.Find(d => d.gameObject == dummy.gameObject) == null) {
            dummyOnLaser.Add(dummy);
            StartCoroutine(laserAttack(dummy));
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Enemy>() != null) {
            addEnemyToList(collision.gameObject.GetComponent<Enemy>());
        } else if(collision.gameObject.GetComponent<Dummy>() != null) {
            addDummyToList(collision.gameObject.GetComponent<Dummy>());
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Enemy>() != null) {
            enemyOnLaser.Remove(collision.gameObject.GetComponent<Enemy>());
        } else if(collision.gameObject.GetComponent<Dummy>() != null) {
            dummyOnLaser.Remove(collision.gameObject.GetComponent<Dummy>());
        }
    }
}
