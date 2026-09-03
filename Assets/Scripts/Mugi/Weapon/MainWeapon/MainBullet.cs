using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBullet : MonoBehaviour
{
    public Sprite DiamondSprite;

    [HideInInspector]
    public bool isCritical;
    [HideInInspector]
    public bool isStunning;
    public float BulletSpeed;

    public int Bullet_Damage;
    public float Bullet_CritDamage;

    void Update()
    {
        //How it Moves
        transform.Translate(Vector3.up * BulletSpeed * (1 + Player.instance.FinalAttack_Bullet_SpeedPercent) * (1 + TraitsManager.instance.GetStats(TraitsData.WeaponType.MainWeapon, TraitsData.TraitsType.BulletSpeed)) * Time.deltaTime);

        if(transform.position.y > Camera.main.orthographicSize + 1|| transform.position.y < -(Camera.main.orthographicSize + 1) || transform.position.x > Camera.main.orthographicSize * Camera.main.aspect + 1|| transform.position.x < (Camera.main.orthographicSize * Camera.main.aspect + 1) * -1)
            DestroyBullet();
    }
    public void DestroyBullet()
    {
        ObjectPoolBullet.Instance.CoolObject(this.gameObject, PoolObjectTypeBullet.Main);
    }

    public void setBulletStats(int damage, float critDamage)
    {
        Bullet_Damage = damage;
        Bullet_CritDamage = critDamage;
    }

    public void getDiamondRandomNumber(Transform loc)
    {
        float r = Random.Range(0.0f, 101.0f);
        if(r <= 0.05f) {
            MaterialPopUp.Create(loc.position, DiamondSprite, 1);
            GameManager.SetDiamond(1);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Enemy>() != null) {
            //Passive∑Œ ¥Ÿ¿Ãæ∆ »πµÊ
            if(MainWeapon.instance.diamondPerHit) {
                getDiamondRandomNumber(collision.gameObject.GetComponent<Enemy>().transform);
            }
            collision.gameObject.GetComponent<Enemy>().OnHit(Bullet_Damage, isStunning, isCritical, Bullet_CritDamage);
            DestroyBullet();
        } else if(collision.gameObject.GetComponent<Dummy>() != null) {
            collision.gameObject.GetComponent<Dummy>().addDamage(Bullet_Damage, isCritical, Bullet_CritDamage);
            DestroyBullet();
        }
    }
}
