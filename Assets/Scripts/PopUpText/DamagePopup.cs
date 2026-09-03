using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public static DamagePopup Create(Vector3 position, long damageAmount, bool isCriticalHit)
    {
        GameObject damagepopupTransform = ObjectPoolPopUp.Instance.GetPoolObject(PoolObjectTypePopUp.Damage);
        damagepopupTransform.transform.position = position;
        damagepopupTransform.SetActive(true);
        DamagePopup damagepopup = damagepopupTransform.GetComponent<DamagePopup>();
        damagepopup.SetUp(damageAmount, isCriticalHit);

        return damagepopup;
    }
    public static DamagePopup Create(Vector3 position, long damageAmount , bool isCriticalHit, Color c)
    {
        GameObject damagepopupTransform = ObjectPoolPopUp.Instance.GetPoolObject(PoolObjectTypePopUp.Damage);
        damagepopupTransform.transform.position = position;
        damagepopupTransform.SetActive(true);
        damagepopupTransform.GetComponent<TextMeshPro>().color = c;
        DamagePopup damagepopup = damagepopupTransform.GetComponent<DamagePopup>();
        damagepopup.SetUp(damageAmount, isCriticalHit);

        return damagepopup;
    }

    private static int sortingOrder;

    private const float DISAPPEAR_TIMER_MAX = 1f;

    private TextMeshPro textmesh;
    private float disappearTimer;
    private Color textColor;
    private Vector3 moveVector;

    private void Awake()
    {
        textmesh = transform.GetComponent<TextMeshPro>();
    }

    public void SetUp(long damageAmount, bool isCritical)
    {
        textmesh.SetText(damageAmount.ToString());
        transform.localScale = Vector3.one * ScreenSize.GetScreenToWorldWidth / 6f;
        if(!isCritical) {
            //NormalHit
            textmesh.fontSize = 2.1f;
            textColor = Color.white;
        } else {
            //CritHit
            textmesh.fontSize = 3f;
            textColor = new Color(1, 0.1686275f, 0);
            
        }
        textmesh.color = textColor;
        disappearTimer = DISAPPEAR_TIMER_MAX;

        sortingOrder++;
        textmesh.sortingOrder = sortingOrder;
        moveVector = new Vector3(0.7f, 1) * 0.6f;
    }

    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 2.2f * Time.deltaTime;


        if(disappearTimer > DISAPPEAR_TIMER_MAX * .5f) {
            //FirstHalf of the popup lifetime
            float increaseScaleAmount = 0.2f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        } else {
            //secondHalf of the popup lifetime
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }
        disappearTimer -= Time.deltaTime;
        if(disappearTimer < 0) {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textmesh.color = textColor;

            if(textColor.a < 0) {
                ObjectPoolPopUp.Instance.CoolObject(this.gameObject, PoolObjectTypePopUp.Damage);
            }
        }
    }
}
