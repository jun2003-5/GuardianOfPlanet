using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBullet : MonoBehaviour
{
    [HideInInspector]
    public bool isCritical;
    [HideInInspector]
    public bool isStunning;
    public float BulletSpeed;

    public int Bullet_Damage;
    public bool penetrateEffect;
    public float critDamage_Bullet;

    void Update()
    {
        //How it Moves
        transform.Translate(Vector3.up * BulletSpeed * (1 + Player.instance.FinalAttack_Bullet_SpeedPercent) * (1 + TraitsManager.instance.GetStats(TraitsData.WeaponType.SniperGun, TraitsData.TraitsType.BulletSpeed)) * Time.deltaTime);

        if(transform.position.y > Camera.main.orthographicSize || transform.position.y < -(Camera.main.orthographicSize) || transform.position.x > Camera.main.orthographicSize * Camera.main.aspect || transform.position.x < Camera.main.orthographicSize * Camera.main.aspect * -1)
            DestroyBullet();
    }
    public void DestroyBullet()
    {
        ObjectPoolBullet.Instance.CoolObject(this.gameObject, PoolObjectTypeBullet.Sniper);
    }

    public void setBulletStats(int damage, bool penetrate, float critDamage)
    {
        Bullet_Damage = damage;
        penetrateEffect = penetrate;
        critDamage_Bullet = critDamage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Enemy>() != null) {
            collision.gameObject.GetComponent<Enemy>().OnHit(Bullet_Damage, isStunning, isCritical, Player.instance.FinalCriticalDamage + critDamage_Bullet);
            if(!penetrateEffect)
                DestroyBullet();
        } else if(collision.gameObject.GetComponent<Dummy>() != null) {
            collision.gameObject.GetComponent<Dummy>().addDamage(Bullet_Damage, isCritical, Player.instance.FinalCriticalDamage + critDamage_Bullet);
            if(!penetrateEffect)
                DestroyBullet();
        }
    }
}
