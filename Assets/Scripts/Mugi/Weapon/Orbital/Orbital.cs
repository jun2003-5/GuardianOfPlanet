using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbital : MonoBehaviour
{
    [SerializeField] public string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public static Orbital instance;

    [HideInInspector]
    public int UpgradeLevel;

    [HideInInspector]
    public int OribitalDamage;

    [Header("#-----Passive Stats")]
    public int ExtraDamage;
    public bool extraRange;
    public float ExtraDamagePercent;
    public float ExtraStunPercent;
    public bool extraSpeed;

    public List<Enemy> enemyOnOrbital;
    public List<Dummy> dummyOnOrbital;

    public bool hasScaled;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        doubleCheckEnemyOnLazer();
    }

    public void UpgradeWeapon()
    {
        UpgradeLevel++;
        SetDamage();
    }

    public void SetDamage()
    {
        OribitalDamage = 100;

        if(UpgradeLevel >= 0)
            OribitalDamage += UpgradeLevel * 7;

        if(UpgradeLevel >= 50)
            OribitalDamage += (UpgradeLevel - 50) * 8;

        if(UpgradeLevel >= 100)
            OribitalDamage += (UpgradeLevel - 100) * 6;

        if(UpgradeLevel >= 200)
            OribitalDamage += (UpgradeLevel - 200) * 7;

        if(UpgradeLevel >= 300)
            OribitalDamage += (UpgradeLevel - 300) * 6;

        if(UpgradeLevel >= 500)
            OribitalDamage += (UpgradeLevel - 500) * 2;

    }

    public IEnumerator orbitalAttack(Enemy enemy)
    {
        while(enemyOnOrbital.Find(d => d.gameObject == enemy.gameObject) != null) {
            enemy.OnHit((int)(Player.instance.FinalAttack_Damage + ((OribitalDamage + ExtraDamage) * (1 + ExtraDamagePercent + TraitsManager.instance.GetStats(TraitsData.WeaponType.CodeA, TraitsData.TraitsType.DamagePercent)))), Random.Range(0,101) <= Player.instance.FinalStunPower + ExtraStunPercent, Random.Range(0, 101) <= Player.instance.FinalCriticalChance, Player.instance.FinalCriticalDamage);
            yield return new WaitForSeconds(0.15f);
        }
    }

    public IEnumerator orbitalAttack(Dummy dummy)
    {
        while(dummyOnOrbital.Find(d => d.gameObject == dummy.gameObject) != null) {
            dummy.addDamage((int)(Player.instance.FinalAttack_Damage + ((OribitalDamage + ExtraDamage) * (1 + ExtraDamagePercent + TraitsManager.instance.GetStats(TraitsData.WeaponType.CodeA, TraitsData.TraitsType.DamagePercent)))), Random.Range(0, 101) <= Player.instance.FinalCriticalChance, Player.instance.FinalCriticalDamage);
            yield return new WaitForSeconds(0.15f);
        }
    }

    public void addEnemyToList(Enemy data)
    {
        if(enemyOnOrbital.Find(d => d.gameObject == data.gameObject) == null) {
            enemyOnOrbital.Add(data);
            StartCoroutine(orbitalAttack(data));
        }
    }

    public void addDummyToList(Dummy dummy)
    {
        if(dummyOnOrbital.Find(d => d.gameObject == dummy.gameObject) == null) {
            dummyOnOrbital.Add(dummy);
            StartCoroutine(orbitalAttack(dummy));
        }
    }

    public void doubleCheckEnemyOnLazer()
    {
        List<Enemy> enemiesToRemove = new List<Enemy>();

        foreach(Enemy enemy in enemyOnOrbital) {
            if(!IsCollidingWithLaser(enemy.gameObject)) {
                enemiesToRemove.Add(enemy);
            }
        }

        foreach(Enemy enemyToRemove in enemiesToRemove) {
            enemyOnOrbital.Remove(enemyToRemove);
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
            enemyOnOrbital.Remove(collision.gameObject.GetComponent<Enemy>());
        } else if(collision.gameObject.GetComponent<Dummy>() != null) {
            dummyOnOrbital.Remove(collision.gameObject.GetComponent<Dummy>());
        }
    }
}
