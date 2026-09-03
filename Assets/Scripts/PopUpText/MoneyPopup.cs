using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyPopup : MonoBehaviour
{
    public static MoneyPopup Create(Vector3 position, long moneyAmount, float fontSize)
    {
        GameObject moneyPopupTransform = ObjectPoolPopUp.Instance.GetPoolObject(PoolObjectTypePopUp.Money);
        moneyPopupTransform.transform.position = new Vector3(position.x, position.y + 0.2f, position.z);
        moneyPopupTransform.transform.localScale = Vector3.one;
        moneyPopupTransform.SetActive(true);
        MoneyPopup moneyPopup = moneyPopupTransform.GetComponent<MoneyPopup>();
        moneyPopup.SetUp(moneyAmount, fontSize);

        return moneyPopup;
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

    public void SetUp(long money, float fontSize)
    {
        textmesh.SetText(money + "G");
        transform.localScale = Vector3.one * ScreenSize.GetScreenToWorldWidth / 8f;
        textmesh.color = new Color(0.990566f, 0.8588184f, 0.01401742f, 1);
        textmesh.fontSize = fontSize;
        
        disappearTimer = DISAPPEAR_TIMER_MAX;

        sortingOrder++;
        textmesh.sortingOrder = sortingOrder;
        moveVector = new Vector3(0.7f, 1) * 0.6f;
    }

    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 5f * Time.deltaTime;


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
                ObjectPoolPopUp.Instance.CoolObject(this.gameObject, PoolObjectTypePopUp.Money);
            }
        }
    }
}
