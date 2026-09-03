using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AttendanceCheck : MonoBehaviour, IDataPersistence
{
    public static AttendanceCheck Instance;

    public List<AttendanceData> AttendanceDatas;
    public Sprite[] RewardSprites;
    public Vector2[] rewardSize;

    //Check Time
    private const string LastRewardTimeKey = "LastRewardTime";

    int RewardedIndex;

    public GameObject priceTab;
    public Image RewardTab_Image;
    public TextMeshProUGUI RewardTab_Text;

    public GameObject AttendanceCheckAlert;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        for(int i = 0; i < AttendanceDatas.Count; i++) {
            SetDataInfo(AttendanceDatas[i]);
        }
    }

    private void Update()
    {
        DateTime lastRewardTime = GetLastRewardTime();
        DateTime currentTime = DateTime.Now;

        // Check if it's the 1st day of the month and reset RewardedIndex
        if(currentTime.Day == 1 && lastRewardTime.Day != 1)
        {
            RewardedIndex = 0;
            SetLastRewardTime(currentTime);
            return;
        }

        // Check if a day has passed since the last reward
        AttendanceCheckAlert.SetActive(currentTime.Date > lastRewardTime.Date);
    }


    public void SetDataInfo(AttendanceData data)
    {
        if(data._RewardType == AttendanceData.AttedanceData.Parts) {
            data.SetDataInfo(RewardSprites[0], data.RewardAmount.ToString(), rewardSize[0]);
        } else if(data._RewardType == AttendanceData.AttedanceData.Diamond) {
            data.SetDataInfo(RewardSprites[1], data.RewardAmount.ToString(), rewardSize[1]);
        } else if(data._RewardType == AttendanceData.AttedanceData.NormalGacha) {
            data.SetDataInfo(RewardSprites[2], "x" + data.RewardAmount, rewardSize[2]);
        } else if(data._RewardType == AttendanceData.AttedanceData.SpecialGacha) {
            data.SetDataInfo(RewardSprites[3], "x" + data.RewardAmount, rewardSize[3]);
        } else if(data._RewardType == AttendanceData.AttedanceData.StoneGacha) {
            data.SetDataInfo(RewardSprites[4], "x" + data.RewardAmount, rewardSize[4]);
        } else if(data._RewardType == AttendanceData.AttedanceData.AutoFarm) {
            data.SetDataInfo(RewardSprites[5], data.RewardAmount + "m", rewardSize[5]);
        }
    }


    public void CheckForDailyReward()
    {
        DateTime lastRewardTime = GetLastRewardTime();
        DateTime currentTime = DateTime.Now;

        if(currentTime.Date > lastRewardTime.Date) {
            GiveReward();
        }
    }

    public void setRewardUI()
    {
        for(int i = 0; i < RewardedIndex; i++) {
            AttendanceDatas[i].setDimUI();
        }
    }

    private void GiveReward()
    {
        priceTab.gameObject.SetActive(true);
        RewardTab_Image.sprite = AttendanceDatas[RewardedIndex].Reward_Image.sprite;
        RewardTab_Image.rectTransform.sizeDelta = new Vector2(AttendanceDatas[RewardedIndex].Reward_Image.rectTransform.sizeDelta.x - 20, AttendanceDatas[RewardedIndex].Reward_Image.rectTransform.sizeDelta.y - 20);
        RewardTab_Text.text = AttendanceDatas[RewardedIndex].Reward_Text.text;

        setRewardUI();
    }

    public void playerReward()
    {
        priceTab.SetActive(false);
        GetReward(AttendanceDatas[RewardedIndex],1);
        RewardedIndex++;
        SetLastRewardTime(DateTime.Now);
        setRewardUI();
    }

    public void playerRewardAds()
    {
        priceTab.SetActive(false);
        GetReward(AttendanceDatas[RewardedIndex],2);
        RewardedIndex++;
        SetLastRewardTime(DateTime.Now);
        setRewardUI();
    }

    private DateTime GetLastRewardTime()
    {
        // Get the last reward time from PlayerPrefs
        int highBits = PlayerPrefs.GetInt(LastRewardTimeKey + "_High", 0);
        int lowBits = PlayerPrefs.GetInt(LastRewardTimeKey + "_Low", 0);

        long ticks = ((long)highBits << 32) | (uint)lowBits;
        return new DateTime(ticks);
    }

    private void SetLastRewardTime(DateTime time)
    {
        // Split the ticks into two int values for PlayerPrefs
        int highBits = (int)(time.Ticks >> 32);
        int lowBits = (int)time.Ticks;

        // Save the current time in PlayerPrefs
        PlayerPrefs.SetInt(LastRewardTimeKey + "_High", highBits);
        PlayerPrefs.SetInt(LastRewardTimeKey + "_Low", lowBits);
        PlayerPrefs.Save();
    }

    
    public void GetReward(AttendanceData data, int scale)
    {
        if(data._RewardType == AttendanceData.AttedanceData.Parts) {
            GameManager.SetParts(data.RewardAmount * scale);
        } else if(data._RewardType == AttendanceData.AttedanceData.Diamond) {
            GameManager.SetDiamond((int)data.RewardAmount * scale);
        } else if(data._RewardType == AttendanceData.AttedanceData.AutoFarm) {
            Player.instance.AutoShootTime += (int)data.RewardAmount * 600 * scale;
        } else if(data._RewardType == AttendanceData.AttedanceData.NormalGacha) {
            GachaManager.Instance.NormalGachaTicket += (int)data.RewardAmount * scale;
        } else if(data._RewardType == AttendanceData.AttedanceData.SpecialGacha) {
            GachaManager.Instance.SpecialGachaTicket += (int)data.RewardAmount * scale;
        } else if(data._RewardType == AttendanceData.AttedanceData.StoneGacha) {
            GachaManager.Instance.StoneGachaTicket += (int)data.RewardAmount * scale;
        }
    }  

    public void LoadData(GameData data)
    {
        RewardedIndex = data.RewardIndex;
        setRewardUI();
    }

    public void SaveData(GameData data)
    {
        data.RewardIndex = RewardedIndex;
    }
}
