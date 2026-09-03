using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBullet : MonoBehaviour
{
    [HideInInspector]
    public int BulletDamage;

    public GameObject bulletAni;
    public GameObject BlastAni;

    public Vector3 initialPosition;
    public float distanceTraveled = 0f;

    bool isExplode;

    private void OnEnable()
    {
        initialPosition = transform.position;
        isExplode = false;
        //Animation
        bulletAni.SetActive(true);
        BlastAni.SetActive(false);
    }

    void Update()
    {
        if(!isExplode) {
            transform.Translate(Vector3.down * 3 * Time.deltaTime);

            distanceTraveled = Vector3.Distance(initialPosition, transform.position);

            if(distanceTraveled >= 2f * Camera.main.orthographicSize * Camera.main.aspect / 2.3f) {
                isExplode = true;
                Explode();
                SoundManager.Instance.playWeaponSFX(SoundManager.WeaponSFXType.poison);
            }
        } else {
            if(BlastAni.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1) {
                DestroyBullet();
            }
        }
    }

    public void Explode()
    {
        bulletAni.SetActive(false);
        BlastAni.SetActive(true);
        createPoisonArea();
    }

    public void createPoisonArea()
    {
        PoisonArea poisonArea = ObjectPoolBullet.Instance.GetPoolObject(PoolObjectTypeBullet.PoisonArea).GetComponent<PoisonArea>();
        poisonArea.poisonDamage = BulletDamage;
        poisonArea.transform.position = transform.position;

        //Scale if Passive
        if(PoisonPlane.instance.extraSizeOfPoisonArea) {
            poisonArea.transform.localScale *= 1.3f;
            poisonArea.hasScaled = true;
        }

        poisonArea.gameObject.SetActive(true);
    }


    public void DestroyBullet()
    {
        ObjectPoolBullet.Instance.CoolObject(this.gameObject, PoolObjectTypeBullet.Poison);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!isExplode) {
            if(collision.gameObject.GetComponent<Enemy>() != null) {
                isExplode = true;
                Explode();
            } else if(collision.gameObject.GetComponent<Dummy>() != null) {
                isExplode = true;
                Explode();
            }
        }
    }
}
