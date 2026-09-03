using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotificationManager : MonoBehaviour, IDataPersistence
{
    public static NotificationManager instance;

    public List<notification> notificationDatas;

    public GameObject NewUserDailyGift_Tab;
    public GameObject[] NewUserDailyGift_Obj;
    public GameObject NewUserDailyGIft_Button;

    public GameObject RewardTab;
    public GameObject DiamondRewardFrame;
    public GameObject PriceFrame;
    public GameObject PosseidonRing_Obj;
    public Image RewardImage;
    public TextMeshProUGUI RewardText;

    public Sprite[] rewardSprites;

    int newUserDailyGift_Index;

    bool giveNewUserGift;

    private void Awake()
    {
        instance = this;
    }

    public void giveNewUserDailyGift()
    {
        NewUserDailyGift_Tab.SetActive(true);
        DiamondRewardFrame.SetActive(newUserDailyGift_Index != 6);
        PriceFrame.SetActive(newUserDailyGift_Index != 6);
        PosseidonRing_Obj.SetActive(newUserDailyGift_Index == 6);

        if(newUserDailyGift_Index == 0) {
            GameManager.SetDiamond(2000);
            Player.instance.addAutoAttackTime_Sec(600);
            RewardImage.sprite = rewardSprites[0];
            RewardImage.rectTransform.sizeDelta = new Vector2(108, 108);
            RewardText.text = "10m";
        } else if(newUserDailyGift_Index == 1) {
            GameManager.SetDiamond(2000);
            GameManager.SetMoney(100000000);
            RewardImage.sprite = rewardSprites[1];
            RewardImage.rectTransform.sizeDelta = new Vector2(108, 108);
            RewardText.text = "100M";
        } else if(newUserDailyGift_Index == 2) {
            GameManager.SetDiamond(2000);
            GameManager.SetParts(10);
            RewardImage.sprite = rewardSprites[2];
            RewardImage.rectTransform.sizeDelta = new Vector2(57, 75);
            RewardText.text = "10";
        } else if(newUserDailyGift_Index == 3) {
            GameManager.SetDiamond(2000);
            GachaManager.Instance.addTicket(GachaData.GachaType.Special, 10);
            RewardImage.sprite = rewardSprites[3];
            RewardImage.rectTransform.sizeDelta = new Vector2(108, 108);
            RewardText.text = "x10";
        } else if(newUserDailyGift_Index == 4) {
            GameManager.SetDiamond(2000);
            GachaManager.Instance.addTicket(GachaData.GachaType.UpgradeStone, 3);
            RewardImage.sprite = rewardSprites[4];
            RewardImage.rectTransform.sizeDelta = new Vector2(108, 108);
            RewardText.text = "x20";
        } else if(newUserDailyGift_Index == 5) {
            GameManager.SetDiamond(2000);
            GachaManager.Instance.gachaOneMust(10, Equips.MaterialClass.Epic, Equips.MaterialClass.Unique, Equips.MaterialClass.Unique, false, 3);
            RewardImage.sprite = rewardSprites[5];
            RewardImage.rectTransform.sizeDelta = new Vector2(108, 108);
            RewardText.text = "x10";
        } else if(newUserDailyGift_Index == 6) {
            EquipManager.Instance.addEquipByID("07ee1bbb-2059-4700-a2fd-e119f422f0f7");
        }

        newUserDailyGift_Index++;
        RewardTab.SetActive(true);
        setNewUserDailyGift_UI();
    }

    public void setNewUserDailyGift_UI()
    {
        for(int i = 0; i < newUserDailyGift_Index; i++) {
            NewUserDailyGift_Obj[i].transform.GetChild(4).GetComponent<CanvasGroup>().alpha = 0.4f;
            NewUserDailyGift_Obj[i].transform.GetChild(5).gameObject.SetActive(true);
        }
    }

    public void checkObjSetwtf()
    {
        if(giveNewUserGift) {
            giveNewUserDailyGift();
            giveNewUserGift = false;
        }

        for(int i = 0; i < notificationDatas.Count; i++) {
            if(!notificationDatas[i].neverShowAgain) {
                notificationDatas[i].gameObject.SetActive(true);
            }
        }
    }

    public void LoadData(GameData data)
    {
        newUserDailyGift_Index = PlayerPrefs.GetInt("newUserDaily_Indexv2");

        if(newUserDailyGift_Index < 7) {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

            if(data.newUserDailyGift_LastDate != currentDate) {
                giveNewUserGift = true;
                data.newUserGiftBool = true;

                data.newUserDailyGift_LastDate = currentDate;
            }
            NewUserDailyGIft_Button.SetActive(true);
        } else {
            NewUserDailyGIft_Button.SetActive(false);
        }



        for(int i = 0; i < notificationDatas.Count; i++) {
            data.notification_Bool.TryGetValue(notificationDatas[i].id, out bool value);
            notificationDatas[i].neverShowAgain = value;
        }
    }

    public void SaveData(GameData data)
    {
        if(StartScreenScript.Instance.isSignedIn) {
          
            if(newUserDailyGift_Index < 7) {
                string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

                if(data.newUserDailyGift_LastDate != currentDate) {
                    giveNewUserGift = true;

                    data.newUserDailyGift_LastDate = currentDate;
                }
                NewUserDailyGIft_Button.SetActive(true);
            } else {
                NewUserDailyGIft_Button.SetActive(false);
            }
            PlayerPrefs.SetInt("newUserDaily_Indexv2", newUserDailyGift_Index);

            for(int i = 0; i < notificationDatas.Count; i++) {
                if(data.notification_Bool.ContainsKey(notificationDatas[i].id)) {
                    data.notification_Bool.Remove(notificationDatas[i].id);
                }

                data.notification_Bool.Add(notificationDatas[i].id, notificationDatas[i].neverShowAgain);
            }

        }
    }
}
