using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;
using System;

public class AdmobManager : MonoBehaviour
{
    [Header("Loading Ads Screen")]
    public GameObject LoadingAdsScreen;
    public GameObject LoadingCircle;
    public GameObject LoadingAdsError;
    public GameObject NoAdsTab;


#if UNITY_ANDROID
    private string rewardedId = "ca-app-pub-1864928959429416/5359044322";
#elif UNITY_IPHONE
  private string rewardedId = "ca-app-pub-1864928959429416/8545542497";
#else
  private string rewardedId = "unused";
#endif

    RewardedAd rewardedAd;

    private void Start()
    {
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize(initStatus => {

            print("Ads Initialised !!");
            LoadRewardedAd();
        });
    }
    #region Rewarded

    public void LoadRewardedAd(Action callback = null)
    {
        if(rewardedAd != null) {
            rewardedAd.Destroy();
            rewardedAd = null;
        }
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        RewardedAd.Load(rewardedId, adRequest, (RewardedAd ad, LoadAdError error) => {
            if(error != null || ad == null) {
                print("Rewarded failed to load" + error);
                return;
            }

            print("Rewarded ad loaded !!");
            rewardedAd = ad;
            RewardedAdEvents(rewardedAd);

            // Call the callback function if provided
            callback?.Invoke();
        });
    }

    public void ShowRewardedAdDailyGachaTicket()
    {
        if(GameManager.instance.noAdsBought) {
            NoAdsTab.SetActive(true);
            DailyRewardManager.instance.adWatched();
        } else {
            LoadingAdsScreen.SetActive(true);
            LoadRewardedAd(() => {
                if(rewardedAd != null && rewardedAd.CanShowAd()) {
                    rewardedAd.Show((Reward reward) => {
                        LoadingAdsScreen.SetActive(false);
                        DailyRewardManager.instance.adWatched();
                    });
                } else {
                    LoadingAdsScreen.SetActive(false);
                    LoadingAdsError.SetActive(true);
                    print("Rewarded ad not ready");
                }
            });
            LoadRewardedAd();
        }
    }


    public void ShowRewardedAdDungeonTicket()
    {
        if(GameManager.instance.noAdsBought) {
            NoAdsTab.SetActive(true);
            DungeonManager.instance.RewardByAds();
        } else {
            LoadingAdsScreen.SetActive(true);
            LoadRewardedAd(() => {
                if(rewardedAd != null && rewardedAd.CanShowAd()) {
                    rewardedAd.Show((Reward reward) => {
                        LoadingAdsScreen.SetActive(false);
                        DungeonManager.instance.RewardByAds();
                    });
                } else {
                    LoadingAdsScreen.SetActive(false);
                    LoadingAdsError.SetActive(true);
                    print("Rewarded ad not ready");
                }
            });
            LoadRewardedAd();
        }
    }

    public void ShowRewardedAdRewardDouble()
    {
        if(GameManager.instance.noAdsBought) {
            NoAdsTab.SetActive(true);
            StageManager.instance.RewardDoubleAd();
        } else {
            LoadingAdsScreen.SetActive(true);
            LoadRewardedAd(() => {
                if(rewardedAd != null && rewardedAd.CanShowAd()) {
                    rewardedAd.Show((Reward reward) => {
                        LoadingAdsScreen.SetActive(false);
                        StageManager.instance.RewardDoubleAd();
                    });
                } else {
                    LoadingAdsScreen.SetActive(false);
                    LoadingAdsError.SetActive(true);
                    print("Rewarded ad not ready");
                }
            });
            LoadRewardedAd();
        }
    }

    public void showRewardedAdRewardTower()
    {
        if(GameManager.instance.noAdsBought) {
            NoAdsTab.SetActive(true);
            TowerManager.Instance.ReduceTimeByAds();
        } else {
            LoadingAdsScreen.SetActive(true);
            LoadRewardedAd(() => {
                if(rewardedAd != null && rewardedAd.CanShowAd()) {
                    rewardedAd.Show((Reward reward) => {
                        LoadingAdsScreen.SetActive(false);
                        TowerManager.Instance.ReduceTimeByAds();
                    });
                } else {
                    LoadingAdsScreen.SetActive(false);
                    LoadingAdsError.SetActive(true);
                    print("Rewarded ad not ready");
                }
            });
            LoadRewardedAd();
        }
    }

    public void showRewardedAdRewardTowerReward()
    {
        if(GameManager.instance.noAdsBought) {
            NoAdsTab.SetActive(true);
            TowerManager.Instance.DoubleTowerRewardAds();
        } else {
            LoadingAdsScreen.SetActive(true);
            LoadRewardedAd(() => {
                if(rewardedAd != null && rewardedAd.CanShowAd()) {
                    rewardedAd.Show((Reward reward) => {
                        LoadingAdsScreen.SetActive(false);
                        TowerManager.Instance.DoubleTowerRewardAds();
                    });
                } else {
                    LoadingAdsScreen.SetActive(false);
                    LoadingAdsError.SetActive(true);
                    print("Rewarded ad not ready");
                }
            });
            LoadRewardedAd();
        }
    }

    public void showRewardedAdAttendance()
    {
        if(GameManager.instance.noAdsBought) {
            NoAdsTab.SetActive(true);
            AttendanceCheck.Instance.playerRewardAds();
        } else {
            LoadingAdsScreen.SetActive(true);
            LoadRewardedAd(() => {
                if(rewardedAd != null && rewardedAd.CanShowAd()) {
                    rewardedAd.Show((Reward reward) => {
                        LoadingAdsScreen.SetActive(false);
                        AttendanceCheck.Instance.playerRewardAds();
                    });
                } else {
                    LoadingAdsScreen.SetActive(false);
                    LoadingAdsError.SetActive(true);
                    print("Rewarded ad not ready");
                }
            });
            LoadRewardedAd();
        }
    }

    public void showRewardedAdAchievement()
    {
        if(GameManager.instance.noAdsBought) {
            NoAdsTab.SetActive(true);
        } else {
            LoadingAdsScreen.SetActive(true);
            LoadRewardedAd(() => {
                if(rewardedAd != null && rewardedAd.CanShowAd()) {
                    rewardedAd.Show((Reward reward) => {
                        LoadingAdsScreen.SetActive(false);
                    });
                } else {
                    LoadingAdsScreen.SetActive(false);
                    LoadingAdsError.SetActive(true);
                    print("Rewarded ad not ready");
                }
            });
            LoadRewardedAd();
        }
    }
    public void RewardedAdEvents(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) => {
            Debug.Log("Rewarded ad paid {0} {1}." +
                adValue.Value +
                adValue.CurrencyCode);
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () => {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () => {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () => {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () => {
            Debug.Log("Rewarded ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) => {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    #endregion

    void Update()
    {
        if(LoadingAdsScreen.gameObject.activeSelf) {
            LoadingCircle.transform.Rotate(new Vector3(0, 0, 2f));
        }
    }
}