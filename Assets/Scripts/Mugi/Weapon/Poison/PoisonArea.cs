using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonArea : MonoBehaviour
{
    public int poisonDamage;

    [HideInInspector]
    public bool hasScaled;

    public List<Enemy> enemyOnPoison;
    public List<Dummy> dummyOnPoison;

    public IEnumerator poisonAttack(Enemy enemy)
    {
        while(enemyOnPoison.Find(d => d.gameObject == enemy.gameObject) != null) {
            enemy.OnHit(poisonDamage, false, PoisonPlane.instance.Poison_canCrit ? Random.Range(0, 101) <= Player.instance.FinalCriticalChance : false, Player.instance.FinalCriticalDamage);
            yield return new WaitForSeconds(PoisonPlane.instance.extraHit ? 0.35f : 0.7f);
        }
    }

    public IEnumerator poisonAttack(Dummy dummy)
    {
        while(dummyOnPoison.Find(d => d.gameObject == dummy.gameObject) != null) {
            dummy.addDamage(poisonDamage, PoisonPlane.instance.Poison_canCrit ? Random.Range(0, 101) <= Player.instance.FinalCriticalChance : false, Player.instance.FinalCriticalDamage);
            yield return new WaitForSeconds(PoisonPlane.instance.extraHit ? 0.35f : 0.7f);
        }
    }

    public void addEnemyToList(Enemy data)
    {
        if(enemyOnPoison.Find(d => d.gameObject == data.gameObject) == null) {
            enemyOnPoison.Add(data);
            StartCoroutine(poisonAttack(data));
        }
    }

    public void addDummyToList(Dummy dummy)
    {
        if(dummyOnPoison.Find(d => d.gameObject == dummy.gameObject) == null) {
            dummyOnPoison.Add(dummy);
            StartCoroutine(poisonAttack(dummy));
        }
    }


    public void OnEnable()
    {
        StartCoroutine(LifeTime());
    }

    public IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(6);

        if(hasScaled) {
            transform.localScale /= 1.3f;
            hasScaled = false;
        }

        ObjectPoolBullet.Instance.CoolObject(this.gameObject, PoolObjectTypeBullet.PoisonArea);
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
            enemyOnPoison.Remove(collision.gameObject.GetComponent<Enemy>());
        } else if(collision.gameObject.GetComponent<Dummy>() != null) {
            dummyOnPoison.Remove(collision.gameObject.GetComponent<Dummy>());
        }
    }
}
