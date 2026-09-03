using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class strLazer : MonoBehaviour
{
    [SerializeField] public string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public static strLazer instance;

    [HideInInspector]
    public int UpgradeLevel;

    [HideInInspector]
    public int strLazerDamage;

    [Header("Lazer Shoot")]
    public ShootLazer shootLazer;

    [Header("#-----Passive Stats")]
    public int ExtraDamage;
    public float ExtraDamagePercent;
    public float ExtraCritChance;
    public float ExtraCritDamage;
    public bool longerLaserTime;

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
        TimerAs += Time.deltaTime;
        if(TimerAs <= 12) {
            transform.GetComponent<Rigidbody2D>().isKinematic = true;
        } else if(TimerAs > 12 && TimerAs < (longerLaserTime ? 19 : 15)) {
            if(!SoundManager.Instance.strLaserSFXSource.isPlaying)
                SoundManager.Instance.playWeaponSFX(SoundManager.WeaponSFXType.strLaser);

            doubleCheckEnemyOnLazer();
            shootLazer.isShooting = true;
            transform.GetComponent<Rigidbody2D>().isKinematic = false;
        } else if(TimerAs >= (longerLaserTime ? 19 : 15)) {
            //Sound
            if(SoundManager.Instance.strLaserSFXSource.isPlaying)
                SoundManager.Instance.strLaserSFXSource.Stop();

            shootLazer.isShooting = false;
            this.transform.localRotation = Quaternion.Euler(0, 0, 0);
            transform.GetComponent<Rigidbody2D>().isKinematic = true;
            enemyOnLaser.Clear();
            dummyOnLaser.Clear();
            TimerAs = 0;
        }
    }

    public void UpgradeWeapon()
    {
        UpgradeLevel++;
        SetDamage();
    }

    public void SetDamage()
    {
        strLazerDamage = 10;

        if(UpgradeLevel >= 0)
            strLazerDamage += UpgradeLevel * 1;

        if(UpgradeLevel >= 10)
            strLazerDamage += (UpgradeLevel - 10) * 1;

        if(UpgradeLevel >= 50)
            strLazerDamage += (UpgradeLevel - 50) * 1;

        if(UpgradeLevel >= 100)
            strLazerDamage += (UpgradeLevel - 100) * 2;

        if(UpgradeLevel >= 200)
            strLazerDamage += (UpgradeLevel - 200) * 3;

        if(UpgradeLevel >= 300)
            strLazerDamage += (UpgradeLevel - 300) * 3;

        if(UpgradeLevel >= 500)
            strLazerDamage += (UpgradeLevel - 500) * 2;

    }

    public void DonShoot()
    {
        this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0);
        TimerAs = 0;
    }
    public void NormalShooting()
    {
        Vector3 Mousetarget = new Vector3(0, 0, 0);
        Mousetarget = GameManager.instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 difference = Mousetarget - this.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ - 90);
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
    }

    public IEnumerator strlaserAttack(Enemy enemy)
    {
        while(enemyOnLaser.Find(d => d.gameObject == enemy.gameObject) != null) {
            enemy.OnHit((int)(Player.instance.FinalAttack_Damage + ((strLazerDamage + ExtraDamage) * (1 + ExtraDamagePercent + TraitsManager.instance.GetStats(TraitsData.WeaponType.StrLaser, TraitsData.TraitsType.DamagePercent)))), false, Random.Range(0, 101) <= Player.instance.FinalCriticalChance, Player.instance.FinalCriticalDamage);
            yield return new WaitForSeconds(0.15f);
        }
    }

    public IEnumerator strlaserAttack(Dummy dummy)
    {
        while(dummyOnLaser.Find(d => d.gameObject == dummy.gameObject) != null) {
            dummy.addDamage((int)(Player.instance.FinalAttack_Damage + ((strLazerDamage + ExtraDamage) * (1 + ExtraDamagePercent + TraitsManager.instance.GetStats(TraitsData.WeaponType.StrLaser, TraitsData.TraitsType.DamagePercent)))), Random.Range(0, 101) <= Player.instance.FinalCriticalChance, Player.instance.FinalCriticalDamage);
            yield return new WaitForSeconds(0.15f);
        }
    }

    public void addEnemyToList(Enemy data)
    {
        if(enemyOnLaser.Find(d => d.gameObject == data.gameObject) == null) {
            enemyOnLaser.Add(data);
            StartCoroutine(strlaserAttack(data));
        }
    }

    public void addDummyToList(Dummy dummy)
    {
        if(dummyOnLaser.Find(d => d.gameObject == dummy.gameObject) == null) {
            dummyOnLaser.Add(dummy);
            StartCoroutine(strlaserAttack(dummy));
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
