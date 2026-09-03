using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Net.Sockets;

public class TowerManager : MonoBehaviour, IDataPersistence
{
    public static TowerManager Instance;

    public GameObject LootPricePrefab;

    public List<TowerData> Towers;

    [Header("Tower GUI")]
    public TextMeshProUGUI TowernameText;
    public TextMeshProUGUI GoldPriceText;
    public TextMeshProUGUI gachaTicketText;
    public Image TicketImage;
    public GameObject[] StoneFrames;
    public TextMeshProUGUI TravelTimeText;

    [Header("#----Adventuring Tab")]
    [SerializeField] private CanvasGroup canvasGroup;
    private Tween fadeTween;
    public GameObject onTravelTab;
    public TextMeshProUGUI TravelLeftTimeText;
    public GameObject SpaceShipGUI;
    public GameObject SpaceShipReward;

    [Header("Exclamation Mark")]
    public GameObject ExclamationMark;

    [Header("Reward")]
    public GameObject RewardTab;
    public TextMeshProUGUI Reward_GoldPriceText;
    public TextMeshProUGUI Reward_gachaTicketText;
    public Image Reward_TicketImage;
    public GameObject[] Reward_StoneFrames;

    [Header("#-----Sprites")]
    public Sprite[] ticketSprites;

    [Header("#---Time")]
    public float TimeForAdventure;

    [Header("#----Map Scroll Rect")]
    public ScrollRect scrollRect;
    public RectTransform contentPanel;
    public TabGroup tabgroup;

    [Header("#----Tower Button")]
    public GameObject TowerTab;
    public GameObject AlertTab;
    public Button ReduceTimeButton;

    [Header("Error Tab")]
    public GameObject ErrorDiamondTab;


    TowerData selectedTower;
    bool isTraveling;

    int lastTravelTower;
    bool hasToReward;

    [Header("Ads Related")]
    public TextMeshProUGUI adsLeftPerDay_Text;
    public Button AdsButton;
    int adsPerDay;
    int myTime = DateTime.Now.DayOfYear;

    private void Awake()
    {
        Instance = this;
        InvokeRepeating("checkTime", 0, 1f);

    }

    void Update()
    {
        if(Time.timeScale > 0.5f) {
            if(isTraveling) {
                TimeForAdventure -= Time.deltaTime / Time.timeScale;

                var ts = TimeSpan.FromSeconds(TimeForAdventure);
                TravelLeftTimeText.text = string.Format("{0:00}:{1:00}:{2:00}", (int)ts.TotalHours, ts.Minutes, ts.Seconds);
                if(TimeForAdventure <= 0) {
                    if(onTravelTab.activeSelf) {
                        TravelLeftTimeText.text = "00:00:00";
                        isTraveling = false;
                        hasToReward = true;
                        ReduceTimeButton.interactable = false;
                        StartCoroutine(GetReward(selectedTower));
                    }
                }

                adsLeftPerDay_Text.text = "±¤°í " + (3 - adsPerDay) + "/3";
                AdsButton.interactable = adsPerDay != 3;
            }

            checkTowerExclamationMark();
        }
    }

    public void openTowerTab()
    {
        if(StageManager.instance.planets[0].PlanetCleared) {
            TowerTab.SetActive(true);
        } else {
            AlertTab.SetActive(true);
        }
    }

    public void selectPlanet(TowerData data)
    {
        selectedTower = data;

        TowernameText.text = data.TowerName;
        GoldPriceText.text = GameManager.MoneyStringForTower(data.goldReward);
        gachaTicketText.text = "x" + data.gachaTicketReward.ToString();
        TicketImage.sprite = ticketSprites[(int)data.ticketType];
        for(int i = 0; i < StoneFrames.Length; i++) {
            StoneFrames[i].SetActive(false);
            StoneFrames[i].SetActive((5 - i) == (int)data.stoneReward.stoneGrade);

            if(StoneFrames[i].activeSelf) {
                StoneFrames[i].GetComponentInChildren<TextMeshProUGUI>().text = data.stoneAmount.ToString();
            }
        }
        var ts = TimeSpan.FromSeconds(data.TimeForTravel);
        TravelTimeText.text = string.Format("{0:#0}½Ã°£ {1:00}ºÐ", (int)ts.TotalHours, ts.Minutes);
    }

    public void Travel()
    {
        TimeForAdventure = selectedTower.TimeForTravel;
        lastTravelTower = Towers.IndexOf(selectedTower);

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        ReduceTimeButton.interactable = true;
        StartCoroutine(TravelIenumerator());
    }

    //Travel
    IEnumerator TravelIenumerator()
    {
        float initialYPosition = SpaceShipGUI.transform.position.y;

        while(SpaceShipGUI.transform.position.y < initialYPosition + 200) {
            SpaceShipGUI.transform.Translate(Vector3.up * 200 * Time.deltaTime);
            yield return new WaitForSeconds(0.005f);
        }
        FadeIn(1f);
        isTraveling = true;
    }

    public void checkTowerExclamationMark()
    {
        ExclamationMark.SetActive(RewardTab.activeSelf);
    }

    IEnumerator GetReward(TowerData data)
    {
        //SpaceShip
        float initialYPosition = SpaceShipReward.transform.position.y;

        while(SpaceShipReward.transform.position.y < initialYPosition + 400) {
            SpaceShipReward.transform.Translate(Vector3.up * 400 * Time.deltaTime);
            yield return new WaitForSeconds(0.005f);
        }

        FadeOut(1f);


        Reward_GoldPriceText.text = GameManager.MoneyStringForTower(data.goldReward);
        Reward_gachaTicketText.text = "x" + data.gachaTicketReward.ToString();
        Reward_TicketImage.sprite = ticketSprites[(int)data.ticketType];
        for(int i = 0; i < Reward_StoneFrames.Length; i++) {
            Reward_StoneFrames[i].SetActive(false);
            Reward_StoneFrames[i].SetActive((5 - i) == (int)data.stoneReward.stoneGrade);

            if(Reward_StoneFrames[i].activeSelf) {
                Reward_StoneFrames[i].GetComponentInChildren<TextMeshProUGUI>().text = data.stoneAmount.ToString();
            }
        }

        //SetDefault
        TimeForAdventure = selectedTower.TimeForTravel;

        RewardTab.SetActive(true);
    }

    public void RewardPlayerOnlyUI(TowerData data)
    {
        Reward_GoldPriceText.text = GameManager.MoneyStringForTower(data.goldReward);
        Reward_gachaTicketText.text = "x" + data.gachaTicketReward.ToString();
        Reward_TicketImage.sprite = ticketSprites[(int)data.ticketType];
        for(int i = 0; i < Reward_StoneFrames.Length; i++) {
            Reward_StoneFrames[i].SetActive(false);
            Reward_StoneFrames[i].SetActive((5 - i) == (int)data.stoneReward.stoneGrade);

            if(Reward_StoneFrames[i].activeSelf) {
                Reward_StoneFrames[i].GetComponentInChildren<TextMeshProUGUI>().text = data.stoneAmount.ToString();
            }
        }

        //SetDefault
        TimeForAdventure = selectedTower.TimeForTravel;

        RewardTab.SetActive(true);
    }

    public void PlayerRewarded()
    {
        hasToReward = false;
        GameManager.SetMoney(selectedTower.goldReward);
        switch(selectedTower.ticketType) {
            case TowerData.TicketType.Normal:
                GachaManager.Instance.NormalGachaTicket += selectedTower.gachaTicketReward;
                break;
            case TowerData.TicketType.Speical:
                GachaManager.Instance.SpecialGachaTicket += selectedTower.gachaTicketReward;
                break;
            case TowerData.TicketType.Stone:
                GachaManager.Instance.StoneGachaTicket += selectedTower.gachaTicketReward;
                break;
        }
        UpgradeStoneManager.instance.addStone(selectedTower.stoneReward, selectedTower.stoneAmount);

        //Achievement
        AchievementManager.instance.TowerAdventureDaily++;
        AchievementManager.instance.TowerAdventureQuest++;
    }

    public void ReduceTimeByDiamond()
    {
        if(GameManager.GetDiamond() >= 150) {
            GameManager.SetDiamond(-150);

            TimeForAdventure -= 1800;
        } else {
            ErrorDiamondTab.SetActive(true);
        }
    }

    public void ReduceTimeByAds()
    {
        TimeForAdventure -= 1800;
        adsPerDay++;
    }

    public void checkTime()
    {
        if(myTime != 0) {
            if(DateTime.Now.DayOfYear - myTime != 0) {
                adsPerDay = 0;
                myTime = DateTime.Now.DayOfYear;
            }
        } else {
            adsPerDay = 0;
            myTime = DateTime.Now.DayOfYear;
        }
    }

    public void DoubleTowerRewardAds()
    {
        hasToReward = false;
        GameManager.SetMoney(selectedTower.goldReward*2);
        switch(selectedTower.ticketType) {
            case TowerData.TicketType.Normal:
                GachaManager.Instance.NormalGachaTicket += selectedTower.gachaTicketReward*2;
                break;
            case TowerData.TicketType.Speical:
                GachaManager.Instance.SpecialGachaTicket += selectedTower.gachaTicketReward*2;
                break;
            case TowerData.TicketType.Stone:
                GachaManager.Instance.StoneGachaTicket += selectedTower.gachaTicketReward*2;
                break;
        }
        UpgradeStoneManager.instance.addStone(selectedTower.stoneReward, selectedTower.stoneAmount*2);

        //Achievement
        AchievementManager.instance.TowerAdventureDaily++;
        AchievementManager.instance.TowerAdventureQuest++;
        RewardTab.SetActive(false);
    }

    public void lockActive()
    {
        selectPlanet(selectedTower);
        tabgroup.SelectTabbyIndex(selectedTower.gameObject.GetComponent<TabButton>());
        SnapTo(selectedTower.gameObject.GetComponent<RectTransform>());

        Towers[0].gameObject.SetActive(StageManager.instance.planets[0].PlanetCleared);
        Towers[1].gameObject.SetActive(StageManager.instance.planets[1].PlanetCleared);
        Towers[2].gameObject.SetActive(StageManager.instance.planets[2].PlanetCleared);
        Towers[3].gameObject.SetActive(StageManager.instance.planets[3].PlanetCleared);
        Towers[4].gameObject.SetActive(StageManager.instance.planets[4].PlanetCleared);
        Towers[5].gameObject.SetActive(StageManager.instance.planets[5].PlanetCleared);
        Towers[6].gameObject.SetActive(StageManager.instance.planets[6].PlanetCleared);
        Towers[7].gameObject.SetActive(StageManager.instance.planets[7].PlanetCleared);
        Towers[8].gameObject.SetActive(StageManager.instance.planets[8].PlanetCleared);
        Towers[9].gameObject.SetActive(StageManager.instance.planets[9].PlanetCleared);
        Towers[10].gameObject.SetActive(StageManager.instance.planets[10].PlanetCleared);
        Towers[11].gameObject.SetActive(StageManager.instance.planets[11].PlanetCleared);
        Towers[12].gameObject.SetActive(StageManager.instance.planets[12].PlanetCleared);
        Towers[13].gameObject.SetActive(StageManager.instance.planets[13].PlanetCleared);
    }

    public void SnapTo(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        Vector2 targetLocalPosition = scrollRect.transform.InverseTransformPoint(target.position);
        Vector2 contentPanelLocalPosition = scrollRect.transform.InverseTransformPoint(contentPanel.position);

        // Only modify the X position
        contentPanel.anchoredPosition = new Vector2(contentPanelLocalPosition.x - targetLocalPosition.x + 50f, contentPanel.anchoredPosition.y);
    }

    //Fade
    private void Fade(float endValue, float duration, TweenCallback onEnd)
    {
        if(fadeTween != null) {
            fadeTween.Kill(false);
        }

        fadeTween = canvasGroup.DOFade(endValue, duration).SetUpdate(true);
        fadeTween.onComplete += onEnd;
    }

    public void FadeIn(float duration)
    {
        Fade(1f, duration, () => {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        });
        SpaceShipReward.transform.localPosition = Vector3.zero;
    }

    public void FadeOut(float duration)
    {
        Fade(0f, duration, () => {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        });
        SpaceShipGUI.transform.localPosition = Vector3.zero;
    }

    public void LoadData(GameData data)
    {
        lastTravelTower = data.lastTravelTower;
        selectedTower = Towers[lastTravelTower];
        isTraveling = data.isTraveling;
        hasToReward = data.playerRewarded;
        myTime = data.towerAdsTime;
        adsPerDay = data.towerAdsPerDay;
        if(hasToReward) {
            RewardPlayerOnlyUI(Towers[lastTravelTower]);
        }

        if(isTraveling) {
            TimeSpan timePassed = DateTime.UtcNow - DateTime.Parse(data.towerTime);
            TimeForAdventure = data.LeftTravelTime - (float)timePassed.TotalSeconds;


            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void SaveData(GameData data)
    {
        data.towerTime = DateTime.UtcNow.ToString();
        data.LeftTravelTime = TimeForAdventure;
        data.lastTravelTower = lastTravelTower;
        data.isTraveling = isTraveling;
        data.playerRewarded = hasToReward;
        data.towerAdsPerDay = adsPerDay;
        data.towerAdsTime = myTime;
    }
}
