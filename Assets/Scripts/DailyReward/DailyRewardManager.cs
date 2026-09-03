using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using DG.Tweening;
using Random = UnityEngine.Random;

public class DailyRewardManager : MonoBehaviour, IDataPersistence
{
    public static DailyRewardManager instance;

    public GameObject RewardTab;
    public Image Reward_Image;
    public TextMeshProUGUI Reward_Text;
    public Sprite[] rewardSprites;
    public Vector2[] rewardSpritesSize;

    public Transform rouletteTransform;
    public GameObject ExclamationMark;

    [Header("뽑기버튼")]
    public Button GatchaButton;
    public Button SpinButton;
    public TextMeshProUGUI SpinRemained_Text;
    public TextMeshProUGUI ButtonNameText;
    public GameObject AdsIcon_Obj;

    [Header("#---광고")]
    public GameObject Ads_Tab;

    [Header("티켓 보유량")]
    public int Ticket;

    [Header("출석 관련")]
    int myTime = DateTime.Now.DayOfYear;

    int adsPerDay;
    bool Drafting;
    public bool DraftingSet
    {
        get { return Drafting; }
        set { Drafting = value; }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InvokeRepeating("checkTimeSet", 0, 1f);
        InvokeRepeating("setDailyTabUI", 0, 0.05f);
    }

    public void Draft()
    {
        if(Ticket > 0) {
            if(Drafting)
                return;

            Drafting = true;
            Ticket--;
            DraftPrice();
            setDailyTabUI();
        } else {
            Ads_Tab.SetActive(true);
        }
    }
    void DraftPrice()
    {
        float randomAngle = Random.Range(0, 8) * 45;
        Vector3 targetRotation = new Vector3(0f, 0f, 1440 + randomAngle);
        rouletteTransform.DORotate(targetRotation, 5f, RotateMode.FastBeyond360)
            .SetEase(Ease.OutQuad) 
            .OnComplete(() => {
                GetReward();
                });

    }

     
    public void GetReward()
    {
        RewardTab.SetActive(true);
        switch(Mathf.RoundToInt(rouletteTransform.transform.rotation.eulerAngles.z % 360)) {
            case 0:
                GachaManager.Instance.addTicket(GachaData.GachaType.UpgradeStone, 1);
                Reward_Image.sprite = rewardSprites[0];
                Reward_Image.rectTransform.sizeDelta = rewardSpritesSize[0];
                Reward_Text.text = "x1";
                break;
            case 45:
                GachaManager.Instance.addTicket(GachaData.GachaType.Special, 1);
                Reward_Image.sprite = rewardSprites[1];
                Reward_Image.rectTransform.sizeDelta = rewardSpritesSize[1];
                Reward_Text.text = "x1";
                break;
            case 90:
                Player.instance.addAutoAttackTime_Sec(600);
                Reward_Image.sprite = rewardSprites[2];
                Reward_Image.rectTransform.sizeDelta = rewardSpritesSize[2];
                Reward_Text.text = "10m";
                break;
            case 135:
                GameManager.SetDiamond(100);
                Reward_Image.sprite = rewardSprites[3];
                Reward_Image.rectTransform.sizeDelta = rewardSpritesSize[3];
                Reward_Text.text = "100";
                break;
            case 180:
                GameManager.SetParts(3);
                Reward_Image.sprite = rewardSprites[4];
                Reward_Image.rectTransform.sizeDelta = rewardSpritesSize[4];
                Reward_Text.text = "3";
                break;
            case 225:
                GachaManager.Instance.addTicket(GachaData.GachaType.Normal, 1);
                Reward_Image.sprite = rewardSprites[5];
                Reward_Image.rectTransform.sizeDelta = rewardSpritesSize[5];
                Reward_Text.text = "x1";
                break;
            case 270:
                GameManager.SetDiamond(300);
                Reward_Image.sprite = rewardSprites[3];
                Reward_Image.rectTransform.sizeDelta = rewardSpritesSize[3];
                Reward_Text.text = "300";
                break;
            case 315:
                GachaManager.Instance.addTicket(GachaData.GachaType.Normal, 5);
                Reward_Image.sprite = rewardSprites[5];
                Reward_Image.rectTransform.sizeDelta = rewardSpritesSize[5];
                Reward_Text.text = "x5";
                break;
        }
    }
    
    void checkTimeSet()
    {
        //Ticket
        if(myTime != 0) {
            if(DateTime.Now.DayOfYear - myTime != 0) {
                Ticket++;
                adsPerDay = 0;
                myTime = DateTime.Now.DayOfYear;
            }
        } else {
            Ticket++;
            adsPerDay = 0;
            myTime = DateTime.Now.DayOfYear;
        }

        ExclamationMark.SetActive(Ticket > 0);
    }

    public void setDailyTabUI()
    {
        if(Drafting) {
            GatchaButton.interactable = !Drafting;
            SpinButton.interactable = !Drafting;
        } else {
            if(Ticket < 1) {
                SpinButton.interactable = false;

                AdsIcon_Obj.SetActive(true);
                ButtonNameText.text = "광고 보기";
                SpinRemained_Text.text = (1 - adsPerDay) + " / 1";
                GatchaButton.interactable = adsPerDay == 0;

            } else {
                GatchaButton.interactable = true;
                SpinButton.interactable = true;
                AdsIcon_Obj.SetActive(false);
                ButtonNameText.text = "일일 뽑기";
                SpinRemained_Text.text = Ticket + " / 1";
            }
        }
    }

    public void adWatched()
    {
        Ads_Tab.SetActive(false);
        adsPerDay++;
        Ticket++;
    }

    public void LoadData(GameData data)
    {
        Ticket = data.Ticket;
        myTime = data.DailyGachaTime;
        adsPerDay = data.adsPerDay;
    }

    public void SaveData(GameData data)
    {
        data.Ticket = Ticket;
        data.DailyGachaTime = myTime;
        data.adsPerDay = adsPerDay;
    }
}
