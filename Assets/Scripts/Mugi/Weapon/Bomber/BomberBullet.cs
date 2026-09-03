using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberBullet : MonoBehaviour
{
    [HideInInspector]
    public bool isCritical;
    [HideInInspector]
    public bool isStunning;

    [HideInInspector]
    public bool hasScaled;

    public float BulletSpeed;

    [HideInInspector]
    public int Bullet_Damage;

    public SpriteRenderer sprite;
    public Sprite DefaultSprite;
    public Animator ExplosionEffect;
    public CircleCollider2D circleCollider;

    bool isFirst = false;

    bool changeRadius;

    private void OnEnable()
    {
        changeRadius = false;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        ExplosionEffect.SetBool("isMoving", true);
        isFirst = false;
        circleCollider.radius = 1.1f;
    }

    void Update()
    {
        //How it Moves
        if(!isFirst)
            transform.Translate(Vector3.up * BulletSpeed * (1 + Player.instance.FinalAttack_Bullet_SpeedPercent) * (1 + TraitsManager.instance.GetStats(TraitsData.WeaponType.Bomber, TraitsData.TraitsType.BulletSpeed)) * Time.deltaTime);

        if(transform.position.y > Camera.main.orthographicSize || transform.position.y < -(Camera.main.orthographicSize) || transform.position.x > Camera.main.orthographicSize * Camera.main.aspect || transform.position.x < Camera.main.orthographicSize * Camera.main.aspect * -1)
            DestroyBullet();
    }

    private void LateUpdate()
    {
        if(changeRadius) {
            circleCollider.radius = 10f;
        }
    }

    public void DestroyBullet()
    {
        ObjectPoolBullet.Instance.CoolObject(this.gameObject, PoolObjectTypeBullet.Bomber);
    }

    public void setBulletStats(int a)
    {
        Bullet_Damage = a;
    }


    public IEnumerator PlayExplosionEffect()
    {
        
        //Sound
        SoundManager.Instance.playWeaponSFX(SoundManager.WeaponSFXType.BomberExplosion);

        //Scale if Lv.500
        if(Bomber.instance.ExploseRangeIncrease) {
            transform.localScale *= 1.5f;
            hasScaled = true;
        }

        changeRadius = true;
        ExplosionEffect.SetBool("isMoving", false);
        yield return new WaitForSeconds(1);

        //Scale if Lv.500
        if(hasScaled) {
            transform.localScale /= 1.5f;
            hasScaled = false;
        }

        DestroyBullet();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!isFirst) {
            isFirst = true;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            if(collision.gameObject.GetComponent<Enemy>() != null) {
                collision.gameObject.GetComponent<Enemy>().OnHit(Bullet_Damage, isStunning, isCritical, Player.instance.FinalCriticalDamage);
                StartCoroutine(PlayExplosionEffect());
            } else if(collision.gameObject.GetComponent<Dummy>() != null) {
                collision.gameObject.GetComponent<Dummy>().addDamage(Bullet_Damage, isCritical, Player.instance.FinalCriticalDamage);
                StartCoroutine(PlayExplosionEffect());
            }
        } else {
            if(collision.gameObject.GetComponent<Enemy>() != null) {             
                collision.gameObject.GetComponent<Enemy>().OnHit((int)(Bullet_Damage * 0.95f), isStunning, isCritical, Player.instance.FinalCriticalDamage);
            } else if(collision.gameObject.GetComponent<Dummy>() != null) {
                collision.gameObject.GetComponent<Dummy>().addDamage((int)(Bullet_Damage * 0.95f), isCritical, Player.instance.FinalCriticalDamage);
            }
        }
    }
}
