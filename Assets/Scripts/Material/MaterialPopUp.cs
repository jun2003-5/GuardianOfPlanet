using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MaterialPopUp : MonoBehaviour
{
    public static MaterialPopUp Create(Vector3 position, Sprite materialAmount, int Number)
    {
        GameObject materialpopupTransform = ObjectPoolPopUp.Instance.GetPoolObject(PoolObjectTypePopUp.Material);
        materialpopupTransform.transform.position = position;
        materialpopupTransform.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        materialpopupTransform.SetActive(true);
        MaterialPopUp materialpopup = materialpopupTransform.GetComponent<MaterialPopUp>();
        materialpopup.SetUp(materialAmount, Number);
        return materialpopup;
    }

    private const float DISAPPEAR_TIMER_MAX = 1f;

    private SpriteRenderer MaterialImage;
    public TextMeshPro text;
    private float disappearTimer;
    private Color imageColor;

    private void Awake()
    {
        MaterialImage = transform.GetComponent<SpriteRenderer>();
        MaterialImage.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f) * ScreenSize.GetScreenToWorldWidth / 5.5f;
    }

    public void SetUp(Sprite materialAmount, int amount)
    {
        MaterialImage.sprite = materialAmount;
        text.text = "+" + GameManager.MoneyString(amount);
        disappearTimer = DISAPPEAR_TIMER_MAX;
        MaterialImage.color = new Color(1, 1, 1, 1);
        MaterialImage.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f) * ScreenSize.GetScreenToWorldWidth / 5.5f;
    }

    private void Update()
    {
        transform.position += Vector3.up/2 * Time.deltaTime;

        if(disappearTimer > DISAPPEAR_TIMER_MAX * .5f) {
            //FirstHalf of the popup lifetime
            float increaseScaleAmount = 0.5f;
            transform.localScale += new Vector3(0.8f, 0.8f, 0.8f) * increaseScaleAmount * Time.deltaTime;
        } else {
            //secondHalf of the popup lifetime
            float decreaseScaleAmount = 0.5f;
            transform.localScale -= new Vector3(0.8f, 0.8f, 0.8f) * decreaseScaleAmount * Time.deltaTime;
        }
        disappearTimer -= Time.deltaTime;
        if(disappearTimer < 0) {
            float disappearSpeed = 3f;
            imageColor.a -= disappearSpeed * Time.deltaTime;
            MaterialImage.color = imageColor;

            if(imageColor.a < 0) {
                ObjectPoolPopUp.Instance.CoolObject(this.gameObject, PoolObjectTypePopUp.Material);
            }
        }
    }
}
