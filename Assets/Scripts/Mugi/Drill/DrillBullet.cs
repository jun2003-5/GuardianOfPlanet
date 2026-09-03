using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillBullet : MonoBehaviour
{
    public int bulletDamage;

    public float BulletSpeed;

    void Update()
    {
        //How it Moves
        transform.Translate(Vector3.up * (BulletSpeed)* Time.deltaTime);

        if(transform.position.x > Camera.main.orthographicSize * Camera.main.aspect + 1 || transform.position.x < (Camera.main.orthographicSize * Camera.main.aspect + 1) * -1) {
            DestroyBullet();
        }
    }
    public void DestroyBullet()
    {
        ObjectPoolBullet.Instance.CoolObject(this.gameObject, PoolObjectTypeBullet.Mining);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Stone>() != null) {
            //DamagePopup
            collision.gameObject.GetComponent<Stone>().OnHit(collision.gameObject.GetComponent<Stone>().transform.position, bulletDamage);
            DestroyBullet();
        }
    }
}
