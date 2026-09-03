using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimeAttackShopData : ShopData
{
    [Header("Time Text")]
    public TextMeshProUGUI TimeText;

    public int StartTime;
    public string startTimeSpan;

    public bool isActivate;

    private void OnEnable()
    {
        gameObject.SetActive(isActivate);        
    }

    void Update()
    {
        if(isActivate) {
            DateTime startDateTime = DateTime.Parse(startTimeSpan);

            TimeSpan elapsedTime = DateTime.Now - startDateTime;
            int elapsedTimeInSeconds = (int)elapsedTime.TotalSeconds;

            int remainingTimeInSeconds = StartTime - elapsedTimeInSeconds;

            TimeSpan remainingTime = TimeSpan.FromSeconds(remainingTimeInSeconds);

            if(remainingTimeInSeconds > 0) {
                if(!isBought) {
                    TimeText.text = string.Format("{0:#0}D {1:00}H", remainingTime.Days, remainingTime.Hours);
                } else {
                    isActivate = false;
                    isBought = true;
                    setGUI();

                    if(ShopManager.Instance.package_Datas.Contains(this)) {
                        DataPersistenceManager.instance.SaveGame();
                        Destroy(gameObject);
                        ShopManager.Instance.package_Datas.Remove(this);
                    }
                }
            } else {
                isActivate = false;
                isBought = true;
                setGUI();

                if(ShopManager.Instance.package_Datas.Contains(this)) {
                    DataPersistenceManager.instance.SaveGame();
                    Destroy(gameObject);
                    ShopManager.Instance.package_Datas.Remove(this);
                }
            }
        }

    }

    public void startEvent()
    {
        gameObject.SetActive(true);
        startTimeSpan = DateTime.Now.ToString();
        isActivate = true;
    }
}
