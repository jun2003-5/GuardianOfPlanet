using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingBullet : MonoBehaviour
{
    [HideInInspector]
    public bool isCritical;
    [HideInInspector]
    public bool isStunning;
    [Header("#-----총알 정보")]
    public float BulletSpeed;

    [Header("#-----총알 필요 요소")]
    //추적 미사일 전용
    private Rigidbody2D rb;
    [Header("추적 미사일 전용")]
    public float RotationControl;

    [Header("#----Stats")]
    public int bullet_Damage;
    public float missileSpeedScale;
    public float bullet_CritDamage;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //How it Moves
        if(EnemyManager.Instance.FindCloseEnemyWithoutRange() != null) {
            Vector2 direction = ((Vector2)EnemyManager.Instance.FindCloseEnemyWithoutRange().pos + (Vector2)EnemyManager.Instance.FindCloseEnemyWithoutRange().transform.forward * 2) - rb.position;

            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            rb.angularVelocity = -rotateAmount * RotationControl * 15;

            rb.velocity = 50 * transform.up * BulletSpeed * (1 + Player.instance.FinalAttack_Bullet_SpeedPercent) * (1 + TraitsManager.instance.GetStats(TraitsData.WeaponType.TrackingMissile, TraitsData.TraitsType.BulletSpeed)) * (1 + missileSpeedScale ) * Time.deltaTime;

        } else {
            transform.Translate(Vector3.up * 3f * Time.deltaTime);
        }

        if(transform.position.y > Camera.main.orthographicSize || transform.position.y < -(Camera.main.orthographicSize) || transform.position.x > Camera.main.orthographicSize * Camera.main.aspect || transform.position.x < Camera.main.orthographicSize * Camera.main.aspect * -1)
            DestroyBullet();
    }
    public void DestroyBullet()
    {
        ObjectPoolBullet.Instance.CoolObject(this.gameObject, PoolObjectTypeBullet.Tracking);
    }

    public void setBulletStats(int a, float b, float c)
    {
        bullet_Damage = a;
        missileSpeedScale = b;
        bullet_CritDamage = c;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Enemy>() != null) {
            //DamagePopup
            collision.gameObject.GetComponent<Enemy>().OnHit(bullet_Damage, isStunning, isCritical, bullet_CritDamage);
            DestroyBullet();
        } else if(collision.gameObject.GetComponent<Dummy>() != null) {
            collision.gameObject.GetComponent<Dummy>().addDamage(TrackingGun.instance.Damage + Player.instance.FinalAttack_Damage, isCritical, bullet_CritDamage);
            DestroyBullet();
        }
    }
}
