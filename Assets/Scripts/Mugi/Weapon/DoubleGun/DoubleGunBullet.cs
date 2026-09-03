using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleGunBullet : MonoBehaviour
{
    [HideInInspector]
    public bool isCritical;
    [HideInInspector]
    public bool isStunning;
    public float BulletSpeed;

    public int bullet_Damage;
    public float bullet_BulletSpeedScale;

    void Update()
    {
        //How it Moves
        transform.Translate(Vector3.up * BulletSpeed * (1 + Player.instance.FinalAttack_Bullet_SpeedPercent) * (1 + TraitsManager.instance.GetStats(TraitsData.WeaponType.DoubleGun, TraitsData.TraitsType.BulletSpeed)) * (1 + bullet_BulletSpeedScale) * Time.deltaTime);

        if(transform.position.y > Camera.main.orthographicSize + 1 || transform.position.y < -(Camera.main.orthographicSize + 1) || transform.position.x > Camera.main.orthographicSize * Camera.main.aspect + 1 || transform.position.x < (Camera.main.orthographicSize * Camera.main.aspect + 1) * -1)
            DestroyBullet();
    }
    public void DestroyBullet()
    {
        ObjectPoolBullet.Instance.CoolObject(this.gameObject, PoolObjectTypeBullet.Double);
    }

    public void setBulletStats(int damage, float bulletSpeedScale)
    {
        bullet_Damage = damage;
        bullet_BulletSpeedScale = bulletSpeedScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Enemy>() != null) {
            //DamagePopup
            collision.gameObject.GetComponent<Enemy>().OnHit(bullet_Damage, isStunning, isCritical, Player.instance.FinalCriticalDamage);
            DestroyBullet();
        } else if(collision.gameObject.GetComponent<Dummy>() != null) {
            collision.gameObject.GetComponent<Dummy>().addDamage(bullet_Damage, isCritical, Player.instance.FinalCriticalDamage);
            DestroyBullet();
        }
    }
}
