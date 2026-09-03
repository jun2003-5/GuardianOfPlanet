using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBullet : MonoBehaviour
{
    public BounceEnemy bounceEnemy;

    public int BounceTime;

    [HideInInspector]
    public bool isCritical;
    [HideInInspector]
    public bool isStunning;
    public float BulletSpeed;

    public int bullet_Damage;

    public Enemy TargetEnemy;

    void Update()
    {
        //How it Moves
        transform.Translate(Vector3.up * BulletSpeed * (1 + Player.instance.FinalAttack_Bullet_SpeedPercent) * (1 + TraitsManager.instance.GetStats(TraitsData.WeaponType.BounceGun, TraitsData.TraitsType.BulletSpeed)) * Time.deltaTime);

        if(transform.position.y > Camera.main.orthographicSize || transform.position.y < -(Camera.main.orthographicSize) || transform.position.x > Camera.main.orthographicSize * Camera.main.aspect || transform.position.x < Camera.main.orthographicSize * Camera.main.aspect * -1) {
            BounceGun.instance.bounceEnemyList_Bullet.Remove(bounceEnemy);
            DestroyBullet();
        }

        if(TargetEnemy != null) {
            if(TargetEnemy._state == Enemy.EnemyState.dead) {
                BounceTime++;
                if(BounceTime < (BounceGun.instance.ExtraBounce ? 3 : 2)) {
                    if(BounceTime == 1) {
                        CreateBullet();
                        CreateBullet();
                    } else {
                        CreateBullet();
                        CreateBullet();
                        CreateBullet();
                    }
                    DestroyBullet();
                } else {
                    BounceGun.instance.bounceEnemyList_Bullet.Remove(bounceEnemy);
                    DestroyBullet();
                }
            }
        }
    }
    public void DestroyBullet()
    {
        ObjectPoolBullet.Instance.CoolObject(this.gameObject, PoolObjectTypeBullet.BounceBullet);
    }

    public void setBulletStats(int damage)
    {
        bullet_Damage = damage;
    }

    public void CreateBullet()
    {
        BounceBullet bullet_A = ObjectPoolBullet.Instance.GetPoolObject(PoolObjectTypeBullet.BounceBullet).GetComponent<BounceBullet>();

        if(BounceGun.instance.NoDamageReduce) {
            bullet_A.setBulletStats(bullet_Damage);
        } else {
            bullet_A.setBulletStats((int)(bullet_Damage * Mathf.Pow(0.9f, BounceTime)));
        }
        bullet_A.isCritical = Random.Range(1, 101) <= Player.instance.FinalCriticalChance;
        bullet_A.isStunning = Random.Range(1, 101) <= Player.instance.FinalStunPower;
        bullet_A.transform.position = transform.position;
        bullet_A.bounceEnemy = bounceEnemy;
        bullet_A.BounceTime = BounceTime;

        Enemy e = EnemyManager.Instance.FindCloseEnemy(bullet_A.bounceEnemy);
        if(e != null) {
            //To Closest Enemy
            Vector3 difference = new Vector3(0, 0, 0);
            difference = e.pos - bullet_A.transform.position;
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            bullet_A.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ - 90);
            bullet_A.TargetEnemy = e;
        } else {
            bullet_A.transform.rotation = transform.rotation;
        }

        bullet_A.gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Enemy>() != null && collision.gameObject.GetComponent<Enemy>() == TargetEnemy) {
            collision.gameObject.GetComponent<Enemy>().OnHit(bullet_Damage, isStunning, isCritical, Player.instance.FinalCriticalDamage);
            bounceEnemy.BouncedEnemies.Add(collision.gameObject.GetComponent<Enemy>());

            BounceTime++;
            if(BounceTime < (BounceGun.instance.ExtraBounce ? 3 : 2)) {
                if(BounceTime == 1) {
                    CreateBullet();
                    CreateBullet();
                } else {
                    CreateBullet();
                    CreateBullet();
                    CreateBullet();
                }
                DestroyBullet();
            } else {
                BounceGun.instance.bounceEnemyList_Bullet.Remove(bounceEnemy);
                DestroyBullet();
            }

        } else if(collision.gameObject.GetComponent<Dummy>() != null) {
            collision.gameObject.GetComponent<Dummy>().addDamage(bullet_Damage, isCritical, Player.instance.FinalCriticalDamage);



        }
    }
}
