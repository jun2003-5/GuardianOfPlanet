using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlanetBuff : MonoBehaviour
{
    [SerializeField] public string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public enum BuffType { GoldIncrease, EXPIncrease, DamageIncrease, CritChanceIncrease, StunIncrease, AttackSpeedIncrease, BulletSpeedIncrease, CritDamageIncrease };
    public BuffType buffType;

    [Header("#---UI")]
    public Button BuyButton;
    public TextMeshProUGUI BuffInfo;
    public TextMeshProUGUI priceText;
    public GameObject priceImage;
    public GameObject maxText;
    public GameObject Cover;
    public GameObject[] stars;

    public int Level;
    public float MaxValue;

    [HideInInspector]
    public float value;

    public void Upgrade()
    {
        GameManager.SetParts(-getBuffPriceParts());
        if(Level < 10) {
            Level++;
            ChangeShopSetting();
        }
    }

    public long getBuffPriceParts()
    {
        switch(Level) {
            case 0:
                return 1;
            case 1:
                return 3;
            case 2:
                return 5;
            case 3:
                return 10;
            case 4:
                return 20;
            case 5:
                return 50;
            case 6:
                return 75;
            case 7:
                return 100;
            case 8:
                return 300;
            case 9:
                return 500;
            default:
                return 0;
        }
    }

    public void ChangeShopSetting()
    {
        if(Level < 10) {
            value = MaxValue / 10.0f * Level;
            switch(buffType) {
                case BuffType.GoldIncrease:
                    BuffInfo.text = "획득 골드 " + value * 100 + "% 증가";
                    break;
                case BuffType.EXPIncrease:
                    BuffInfo.text = "획득 경험치 " + value * 100 + "% 증가";
                    break;
                case BuffType.DamageIncrease:
                    BuffInfo.text = "총 데미지 " + value * 100 + "% 증가";
                    break;
                case BuffType.CritChanceIncrease:
                    BuffInfo.text = "치명타 확률 " + value + "% 증가";
                    break;
                case BuffType.CritDamageIncrease:
                    BuffInfo.text = "치명타 데미지 " + value * 100 + "% 증가";
                    break;
                case BuffType.StunIncrease:
                    BuffInfo.text = "스턴 확률 " + value + "% 증가";
                    break;
                case BuffType.AttackSpeedIncrease:
                    BuffInfo.text = "공격 속도 " + value * 100 + "% 증가";
                    break;
                case BuffType.BulletSpeedIncrease:
                    BuffInfo.text = "총알 속도 " + value * 100 + "% 증가";
                    break;
            }
            priceText.gameObject.SetActive(true);
            priceImage.SetActive(true);
            maxText.SetActive(false);
            priceText.text = getBuffPriceParts().ToString();
            BuyButton.interactable = true;
        } else {
            value = MaxValue / 10.0f * Level;
            switch(buffType) {
                case BuffType.GoldIncrease:
                    BuffInfo.text = "획득 골드 " + value * 100 + "% 증가";
                    break;
                case BuffType.EXPIncrease:
                    BuffInfo.text = "획득 경험치 " + value * 100 + "% 증가";
                    break;
                case BuffType.DamageIncrease:
                    BuffInfo.text = "총 데미지 " + value * 100 + "% 증가";
                    break;
                case BuffType.CritChanceIncrease:
                    BuffInfo.text = "치명타 확률 " + value + "% 증가";
                    break;
                case BuffType.CritDamageIncrease:
                    BuffInfo.text = "치명타 데미지 " + value * 100 + "% 증가";
                    break;
                case BuffType.StunIncrease:
                    BuffInfo.text = "스턴 확률 " + value + "% 증가";
                    break;
                case BuffType.AttackSpeedIncrease:
                    BuffInfo.text = "공격 속도 " + value * 100 + "% 증가";
                    break;
                case BuffType.BulletSpeedIncrease:
                    BuffInfo.text = "총알 속도 " + value * 100 + "% 증가";
                    break;
            }
            priceText.gameObject.SetActive(false);
            priceImage.SetActive(false);
            maxText.SetActive(true);
            BuyButton.interactable = false;
        }


        //Star
        for(int i = 0; i < stars.Length; i++) {
            stars[i].SetActive(false);
            if(i < Level) {
                stars[i].SetActive(true);
            }
        }

    }
}
