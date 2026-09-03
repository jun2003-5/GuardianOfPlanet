using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementManager : MonoBehaviour, IDataPersistence
{
    public static AchievementManager instance;

    public List<AchievementData> AchievementDatas;

    [Header("ETC")]
    public Sprite[] sprites;
    public Vector2[] sizeOfSprites;

    [Header("#--------DailyQuest")]
    [Space(5)]
    public Transform Daily_Datas_Parent;
    public List<AchievementData> Daily_Datas;
    public Slider DailyQuest_Slider;
    public Button DailyQuest_presentBox;
    public GameObject DailyQuest_CheckMark;
    public TextMeshProUGUI DailyQuest_SliderText;
    public TextMeshProUGUI DailyQuest_TimerText;
    public TextMeshProUGUI DailyQuest_CompletedAmountText;
    public bool dailyRewarded;
    public float PlayedTimeDaily;
    public int killedEnemyDaily;
    public int killedBossDaily;
    public int GachaDaily;
    public int TowerAdventureDaily;
    public int IronworkTradeDaily;
    public int OremineDaily;
    public int UnminedOreDaily;
    public int InfiniteStageDaily;

    [Header("#--------WeeklyQuest")]
    [Space(5)]
    public Transform Weekly_Datas_Parent;
    public List<AchievementData> Weekly_Datas;
    public Slider WeeklyQuest_Slider;
    public Button WeeklyQuest_presentBox;
    public GameObject WeeklyQuest_CheckMark;
    public TextMeshProUGUI WeeklyQuest_SliderText;
    public TextMeshProUGUI WeeklyQuest_TimerText;
    public TextMeshProUGUI WeeklyQuest_CompletedAmountText;
    public bool weeklyRewarded;
    public float PlayedTimeWeekly;
    public int killedEnemyWeekly;
    public int killedBossWeekly;
    public int GachaWeekly;
    public int OremineWeekly;
    public int UnminedOreWeekly;
    public int DailyQuestClearWeekly;
    public int InfiniteStageWeekly;

    [Header("#--------Quest")]
    [Space(5)]
    public Transform Normal_Datas_Parent;
    public List<AchievementData> Normal_Datas;
    public int GachaTimeQuest;
    public int TowerAdventureQuest;
    public int OreMinedQuest;
    public int UnminedOreQuest;

    [Header("#----Reward Tab")]
    public GameObject RewardTab;
    public Image RewardIcon;
    public TextMeshProUGUI RewardAmountText;

    public GameObject DailyBoxRewardTab;
    public GameObject WeeklyBoxRewardTab;

    [Header("#--------Ore Tab")]
    [Space(5)]
    public GameObject OrePrizeTab;
    public Button OrePrizeButton;
    public RailInfoData prizeData_pfb;
    public Transform prizeData_parent;
    public TabGroup tabgroup;
    [HideInInspector]
    public List<RailInfoData> createdPrizeData;
    public TextMeshProUGUI orePrizeAmount_Text;

    int prizeAmount;
    private Rail currentOrePrizeRail;


    [Header("ExclamationMark")]
    public GameObject ExclmationMark;
    public GameObject alertDaily;
    public GameObject alertWeekly;
    public GameObject alertQuest;

    [Header("#-----Reset Time")]
    public int ResetTimeDaily;
    public int ResetTimeWeekly;

    AchievementData currentAchieve;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for(int i = 0; i < AchievementDatas.Count; i++) {

            ChangeRewardUI(AchievementDatas[i]);
        }

        //Daily Slider
        DailyQuest_Slider.maxValue = Daily_Datas.Count;
        WeeklyQuest_Slider.maxValue = Weekly_Datas.Count;
        InvokeRepeating("SetQuestText", 0f, 0.1f);
        InvokeRepeating("setExclamationMark", 0f, 0.1f);
        InvokeRepeating("SetQuestCondition", 0f, 0.1f);
        InvokeRepeating("setQuestsTimer", 0f, 1f);
    }

    void Update()
    {
        if(Time.timeScale > 0.3f) {
            //퀘스트 도우미
            PlayedTimeDaily += Time.deltaTime / Time.timeScale;
            PlayedTimeWeekly += Time.deltaTime / Time.timeScale;
        }
    }

    public void setQuestsTimer()
    {
        var timeLeft = System.DateTime.Now.AddDays(1).Date - System.DateTime.Now;
        string timeString = string.Format("{0:D2}H {1:D2}M",
            timeLeft.Hours, timeLeft.Minutes);

        DailyQuest_TimerText.text = timeString;

        int daysUntilSunday = ((int)DayOfWeek.Sunday - (int)DateTime.Now.DayOfWeek + 7) % 7;

        TimeSpan timeUntilSunday = TimeSpan.FromDays(daysUntilSunday).Subtract(DateTime.Now.TimeOfDay);

        // Format the time left into days, hours, and minutes
        string timeS = string.Format("{0}D {1:D2}H {2:D2}M",
            timeUntilSunday.Days, timeUntilSunday.Hours, timeUntilSunday.Minutes);

        // Assign the formatted time string to your UI text component
        WeeklyQuest_TimerText.text = timeS;

    }

    public void SetQuestCondition()
    {
        for(int i = 0; i < AchievementDatas.Count; i++) {
            AchievementDatas[i].changeQuestGUI();
        }

        ReorderQuests(Daily_Datas_Parent, Daily_Datas);
        ReorderQuests(Weekly_Datas_Parent, Weekly_Datas);
        ReorderQuests(Normal_Datas_Parent, Normal_Datas);
    }

    public void setExclamationMark()
    {
        bool showExclamationMark = false;
        bool showDailyMark = false;
        bool showWeeklyMark = false;
        bool showQuestMark = false;

        for(int i = 0; i < AchievementDatas.Count; i++) {
            if(AchievementDatas[i].isCompleted && !AchievementDatas[i].CompletedTab.activeSelf) {
                if(AchievementDatas[i]._type == AchievementData.TypeofQuest.Daily) {
                    showDailyMark = true;
                    showExclamationMark = true;
                } else if(AchievementDatas[i]._type == AchievementData.TypeofQuest.Weekly) {
                    showWeeklyMark = true;
                    showExclamationMark = true;
                } else if(AchievementDatas[i]._type == AchievementData.TypeofQuest.Quest) {
                    showQuestMark = true;
                    showExclamationMark = true;
                }
            }
        }

        alertDaily.SetActive(showDailyMark);
        alertWeekly.SetActive(showWeeklyMark);
        alertQuest.SetActive(showQuestMark);
        ExclmationMark.SetActive(showExclamationMark);
    }


    public void SetQuestText()
    {
        for(int i = 0; i < AchievementDatas.Count; i++) {

            AchievementData data = AchievementDatas[i];

            string ResultText = data.QuestName;
            string newText = data.QuestName;

            if(data._type == AchievementData.TypeofQuest.Daily) { //일간 퀘스트
                switch(data.QuestNumber) {
                    case 1: //1시간 플레이
                        data.slider.maxValue = 60;
                        data.slider.value = ((int)(PlayedTimeDaily / 60));
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 2: //몬스터 300마리 처치
                        data.slider.maxValue = 300;
                        data.slider.value = killedEnemyDaily;
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 3: //보스 5마리 처치
                        data.slider.maxValue = 5;
                        data.slider.value = killedBossDaily;
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 4: //장비 뽑기 5회
                        data.slider.maxValue = 5;
                        data.slider.value = GachaDaily;
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 5: //타워 탐사하기
                        data.slider.maxValue = 1;
                        data.slider.value = TowerAdventureDaily;
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 6: //광산 제철소에서 거래하기 3회
                        data.slider.maxValue = 3;
                        data.slider.value = IronworkTradeDaily;
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 7: //광석 500개 캐기
                        data.slider.maxValue = 500;
                        data.slider.value = OremineDaily;
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 8: //미확인 광석 500개 획득하기
                        data.slider.maxValue = 500;
                        data.slider.value = UnminedOreDaily;
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 9: //사냥터 스테이지 10회 클리어
                        data.slider.maxValue = 10;
                        data.slider.value = InfiniteStageDaily;
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                }
            } else if(data._type == AchievementData.TypeofQuest.Weekly) {
                switch(data.QuestNumber) {
                    case 1: //20시간 플레이하기
                        data.slider.maxValue = 600;
                        data.slider.value = ((int)PlayedTimeWeekly) / 60;
                        data.SliderText.text = ((int)data.slider.value) + "/" + data.slider.maxValue;
                        break;
                    case 2: //몬스터 3000마리 처치하기 (사냥터 제외)
                        data.slider.maxValue = 3000;
                        data.slider.value = killedEnemyWeekly;
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 3: //장비 뽑기 50회
                        data.slider.maxValue = 50;
                        data.slider.value = GachaWeekly;
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 4: //광석 10000개 캐기
                        data.slider.maxValue = 10000;
                        data.slider.value = OremineWeekly;
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 5: //미확인 광석 10000개 획득하기
                        data.slider.maxValue = 10000;
                        data.slider.value = UnminedOreWeekly;
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 6: //일일 퀘스트 35회 깨기
                        data.slider.maxValue = 35;
                        data.slider.value = DailyQuestClearWeekly;
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 7: //보스 100회 처치하기 (사냥터 제외)
                        data.slider.maxValue = 100;
                        data.slider.value = killedBossWeekly;
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 8: //사냥터 스테이지 100회 클리어
                        data.slider.maxValue = 50;
                        data.slider.value = InfiniteStageWeekly;
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                }
            } else if(data._type == AchievementData.TypeofQuest.Quest) {
                switch(data.QuestNumber) {
                    case 1: //몬스터 총 @마리 처치
                        ResultText = newText.Replace("@", data.QuestConditionAmount[data.CurrentQuest].ToString());
                        data.slider.maxValue = data.QuestConditionAmount[data.CurrentQuest];
                        data.slider.value = EnemyManager.Instance.enemyKilled_Number;
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 2: //@ 행성 클리어하기
                        ResultText = newText.Replace("@", StageManager.instance.planets[data.CurrentQuest].PlanetName);
                        data.slider.maxValue = data.QuestConditionAmount[data.CurrentQuest];
                        data.slider.value = Convert.ToInt32(StageManager.instance.planets[data.CurrentQuest].PlanetCleared);
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 3: //@시간 플레이하기
                        ResultText = newText.Replace("@", data.QuestConditionAmount[data.CurrentQuest].ToString());
                        data.slider.maxValue = data.QuestConditionAmount[data.CurrentQuest] * 60;
                        data.slider.value = GameManager.instance.totalPlayedTime / 60;
                        data.SliderText.text = data.slider.value.ToString("F0") + "/" + data.slider.maxValue;
                        break;
                    case 4: //장비 뽑기 @회
                        ResultText = newText.Replace("@", data.QuestConditionAmount[data.CurrentQuest].ToString());
                        data.slider.maxValue = data.QuestConditionAmount[data.CurrentQuest];
                        data.slider.value = GachaTimeQuest;
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 5: //타워 탐사하기 @회
                        ResultText = newText.Replace("@", data.QuestConditionAmount[data.CurrentQuest].ToString());
                        data.slider.maxValue = data.QuestConditionAmount[data.CurrentQuest];
                        data.slider.value = TowerAdventureQuest;
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 6: //광석 @개 캐기
                        ResultText = newText.Replace("@", string.Format("{0:#,###}", data.QuestConditionAmount[data.CurrentQuest]));
                        data.slider.maxValue = data.QuestConditionAmount[data.CurrentQuest];
                        data.slider.value = OreMinedQuest;
                        data.SliderText.text = string.Format("{0:#,###}", data.slider.value) + "/" + string.Format("{0:#,###}", data.slider.maxValue);
                        break;
                    case 7: //보스 @마리 처차하기
                        ResultText = newText.Replace("@", data.QuestConditionAmount[data.CurrentQuest].ToString());
                        data.slider.maxValue = data.QuestConditionAmount[data.CurrentQuest];
                        data.slider.value = EnemyManager.Instance.totalBossKilled;
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 8: //미확인 광석 @개 획득하기
                        ResultText = newText.Replace("@", data.QuestConditionAmount[data.CurrentQuest].ToString());
                        data.slider.maxValue = data.QuestConditionAmount[data.CurrentQuest];
                        data.slider.value = UnminedOreQuest;
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 9: //사냥터 스테이지 @ 클리어
                        ResultText = newText.Replace("@", data.QuestConditionAmount[data.CurrentQuest].ToString());
                        data.slider.maxValue = data.QuestConditionAmount[data.CurrentQuest];
                        data.slider.value = InfiniteStage.Instance.CurrentStage - 1;
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 10: //데미지 스탯 @레벨 달성
                        ResultText = newText.Replace("@", data.QuestConditionAmount[data.CurrentQuest].ToString());
                        data.slider.maxValue = data.QuestConditionAmount[data.CurrentQuest];
                        data.slider.value = StatsManager.instance.getStatLevel(StatsData.TypeofStat.Damage);
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 11: //공격속도 스탯 @레벨 달성
                        ResultText = newText.Replace("@", data.QuestConditionAmount[data.CurrentQuest].ToString());
                        data.slider.maxValue = data.QuestConditionAmount[data.CurrentQuest];
                        data.slider.value = StatsManager.instance.getStatLevel(StatsData.TypeofStat.Attack_Speed);
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 12: //총알속도 스탯 @레벨 달성
                        ResultText = newText.Replace("@", data.QuestConditionAmount[data.CurrentQuest].ToString());
                        data.slider.maxValue = data.QuestConditionAmount[data.CurrentQuest];
                        data.slider.value = StatsManager.instance.getStatLevel(StatsData.TypeofStat.Attack_Bullet_Speed);
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 13: //치명타 확률 스탯 @레벨 달성
                        ResultText = newText.Replace("@", data.QuestConditionAmount[data.CurrentQuest].ToString());
                        data.slider.maxValue = data.QuestConditionAmount[data.CurrentQuest];
                        data.slider.value = StatsManager.instance.getStatLevel(StatsData.TypeofStat.CritChance);
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 14: //치명타 데미지 스탯 @레벨 달성
                        ResultText = newText.Replace("@", data.QuestConditionAmount[data.CurrentQuest].ToString());
                        data.slider.maxValue = data.QuestConditionAmount[data.CurrentQuest];
                        data.slider.value = StatsManager.instance.getStatLevel(StatsData.TypeofStat.CritDamage);
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 15: //스턴 확률 스탯 @레벨 달성
                        ResultText = newText.Replace("@", data.QuestConditionAmount[data.CurrentQuest].ToString());
                        data.slider.maxValue = data.QuestConditionAmount[data.CurrentQuest];
                        data.slider.value = StatsManager.instance.getStatLevel(StatsData.TypeofStat.StunPercent);
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 16: //추가 경험치 스탯 @레벨 달성
                        ResultText = newText.Replace("@", data.QuestConditionAmount[data.CurrentQuest].ToString());
                        data.slider.maxValue = data.QuestConditionAmount[data.CurrentQuest];
                        data.slider.value = StatsManager.instance.getStatLevel(StatsData.TypeofStat.ExtraEXP);
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 17: //추가 골드 스탯 @레벨 달성
                        ResultText = newText.Replace("@", data.QuestConditionAmount[data.CurrentQuest].ToString());
                        data.slider.maxValue = data.QuestConditionAmount[data.CurrentQuest];
                        data.slider.value = StatsManager.instance.getStatLevel(StatsData.TypeofStat.ExtraMoney);
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 18: //일반 몬스터 도감 채우기
                        data.slider.maxValue = data.QuestConditionAmount[data.CurrentQuest];
                        data.slider.value = CollectionManager.Instance.getNormalCollectionFoundAmount();
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 19: //보스 몬스터 도감 채우기
                        data.slider.maxValue = data.QuestConditionAmount[data.CurrentQuest];
                        data.slider.value = CollectionManager.Instance.getBossCollectionFoundAmount();
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 20: //반지 @개 획득하기
                        ResultText = newText.Replace("@", data.QuestConditionAmount[data.CurrentQuest].ToString());
                        data.slider.maxValue = data.QuestConditionAmount[data.CurrentQuest];
                        data.slider.value = EquipManager.Instance.getEquipNumberByType(Equips.Type.Ring);
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 21: //목걸이 @개 획득하기
                        ResultText = newText.Replace("@", data.QuestConditionAmount[data.CurrentQuest].ToString());
                        data.slider.maxValue = data.QuestConditionAmount[data.CurrentQuest];
                        data.slider.value = EquipManager.Instance.getEquipNumberByType(Equips.Type.Necklace);
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 22: //유물 @개 획득하기
                        ResultText = newText.Replace("@", data.QuestConditionAmount[data.CurrentQuest].ToString());
                        data.slider.maxValue = data.QuestConditionAmount[data.CurrentQuest];
                        data.slider.value = EquipManager.Instance.getEquipNumberByType(Equips.Type.Relics);
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 23: //장신구 @개 획득하기
                        ResultText = newText.Replace("@", data.QuestConditionAmount[data.CurrentQuest].ToString());
                        data.slider.maxValue = data.QuestConditionAmount[data.CurrentQuest];
                        data.slider.value = EquipManager.Instance.getEquipNumberByType(Equips.Type.Accessory);
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 24: //고서 @개 획득하기
                        ResultText = newText.Replace("@", data.QuestConditionAmount[data.CurrentQuest].ToString());
                        data.slider.maxValue = data.QuestConditionAmount[data.CurrentQuest];
                        data.slider.value = EquipManager.Instance.getEquipNumberByType(Equips.Type.Book);
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 25: //@ 레벨 달성하기
                        ResultText = newText.Replace("@", data.QuestConditionAmount[data.CurrentQuest].ToString());
                        data.slider.maxValue = data.QuestConditionAmount[data.CurrentQuest];
                        data.slider.value = Player.instance.Lvl;
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                    case 26: //@ 행성 클리어하기
                        ResultText = newText.Replace("@", StageManager.instance.planets[data.CurrentQuest + 14].PlanetName);
                        data.slider.maxValue = data.QuestConditionAmount[data.CurrentQuest];
                        data.slider.value = Convert.ToInt32(StageManager.instance.planets[data.CurrentQuest + 14].PlanetCleared);
                        data.SliderText.text = data.slider.value + "/" + data.slider.maxValue;
                        break;
                }
            }
   

            data.changeQuestGUI();
            data.QuestText.text = ResultText;
        }
    }

    public void ChangeRewardUI(AchievementData data)
    {
        switch(data.reType) {
            case AchievementData.rewardType.Diamond:
                data.rewardImage.sprite = sprites[0];
                data.rewardImage.rectTransform.sizeDelta = sizeOfSprites[0];
                data.rewardText.text = data.RewardAmount[data.CurrentQuest].ToString() + "개";
                break;
            case AchievementData.rewardType.Money:
                data.rewardImage.sprite = sprites[1];
                data.rewardImage.rectTransform.sizeDelta = sizeOfSprites[1];
                string s = GameManager.instance.MoneyStringForAchievement(data.RewardAmount[data.CurrentQuest]);
                if(s.Length >= 7)
                    s = s.Remove(s.Length - 2, 1);
                data.rewardText.text = s;
                break;
            case AchievementData.rewardType.AutoFarm:
                data.rewardImage.sprite = sprites[2];
                data.rewardImage.rectTransform.sizeDelta = sizeOfSprites[2];
                data.rewardText.text = data.RewardAmount[data.CurrentQuest] / 60 + "분";
                break;
            case AchievementData.rewardType.NormalTicket:
                data.rewardImage.sprite = sprites[3];
                data.rewardImage.rectTransform.sizeDelta = sizeOfSprites[3];
                data.rewardText.text = data.RewardAmount[data.CurrentQuest].ToString() + "개";
                break;
            case AchievementData.rewardType.SpecialTicket:
                data.rewardImage.sprite = sprites[4];
                data.rewardImage.rectTransform.sizeDelta = sizeOfSprites[4];
                data.rewardText.text = data.RewardAmount[data.CurrentQuest].ToString() + "개";
                break;
            case AchievementData.rewardType.StoneTicket:
                data.rewardImage.sprite = sprites[5];
                data.rewardImage.rectTransform.sizeDelta = sizeOfSprites[5];
                data.rewardText.text = data.RewardAmount[data.CurrentQuest].ToString() + "개";
                break;
            case AchievementData.rewardType.UnminedOre:
                data.rewardImage.sprite = sprites[6];
                data.rewardImage.rectTransform.sizeDelta = sizeOfSprites[6];
                data.rewardText.text = data.RewardAmount[data.CurrentQuest].ToString() + "개";
                break;
            case AchievementData.rewardType.Parts:
                data.rewardImage.sprite = sprites[7];
                data.rewardImage.rectTransform.sizeDelta = sizeOfSprites[7];
                data.rewardText.text = data.RewardAmount[data.CurrentQuest].ToString() + "개";
                break;
        }
    }

    public void ReorderQuests(Transform parent, List<AchievementData> data)
    {
        data.Sort((a, b) => {
            int completedComparison = b.isCompleted.CompareTo(a.isCompleted);

            if(completedComparison != 0) {
                return completedComparison;
            }

            int prizeComparison = a.hasEarnedPrize.CompareTo(b.hasEarnedPrize);

            if(prizeComparison != 0) {
                return prizeComparison;
            }

            int progressComparison = (b.slider.value / b.slider.maxValue).CompareTo(a.slider.value / a.slider.maxValue);

            if(progressComparison != 0) {
                return progressComparison;
            }

            return a.QuestNumber.CompareTo(b.QuestNumber);
        });

        for(int i = 1; i < parent.childCount; i++) {
            Transform questTransform = parent.GetChild(i);

            AchievementData quest = data.Find(q => q.id == questTransform.GetComponentInChildren<AchievementData>().id);

            questTransform.SetSiblingIndex(data.IndexOf(quest));
        }
    }

    public void openRewardTab(AchievementData data)
    {
        currentAchieve = data;

        RewardTab.SetActive(true);

        RewardIcon.sprite = data.rewardImage.sprite;
        RewardIcon.rectTransform.sizeDelta = data.rewardImage.rectTransform.sizeDelta;
        RewardAmountText.text = data.rewardText.text;
    }

    public void playerReward()
    {
        RewardTab.SetActive(false);
        currentAchieve.rewardPlayer();
    }




    public static void RewardPlayer(AchievementData.rewardType type, long Amount)
    {
        instance.setSliderValue();

        switch(type) {
            case AchievementData.rewardType.Diamond:
                GameManager.SetDiamond((int)Amount);
                break;
            case AchievementData.rewardType.Money:
                GameManager.SetMoney((int)Amount);
                break;
            case AchievementData.rewardType.AutoFarm:
                Player.instance.AutoShootTime += Amount;
                break;
            case AchievementData.rewardType.NormalTicket:
                GachaManager.Instance.NormalGachaTicket += (int)Amount;
                break;
            case AchievementData.rewardType.SpecialTicket:
                GachaManager.Instance.SpecialGachaTicket += (int)Amount;
                break;
            case AchievementData.rewardType.StoneTicket:
                GachaManager.Instance.StoneGachaTicket += (int)Amount;
                break;
            case AchievementData.rewardType.UnminedOre:
                instance.openOrePrizeTab((int)Amount);
                break;
            case AchievementData.rewardType.Parts:
                GameManager.SetParts((int)Amount);
                break;
        }

        DataPersistenceManager.instance.SaveGameNow();
    }

    public void setSliderValue()
    {
        int count = 0;

        //Daily
        for(int i = 0; i < Daily_Datas.Count; i++) {
            if(Daily_Datas[i].hasEarnedPrize)
                count++;
        }
        DailyQuest_Slider.maxValue = Daily_Datas.Count;
        DailyQuest_Slider.value = count;
        DailyQuest_SliderText.text = DailyQuest_Slider.value + "/" + DailyQuest_Slider.maxValue;
        DailyQuest_CompletedAmountText.text = DailyQuest_Slider.value + "/" + DailyQuest_Slider.maxValue;

        if(!dailyRewarded) {
            DailyQuest_presentBox.interactable = DailyQuest_Slider.value >= DailyQuest_Slider.maxValue;
            DailyQuest_CheckMark.SetActive(false);
        } else {
            DailyQuest_presentBox.interactable = false;
            DailyQuest_CheckMark.SetActive(true);
            dailyRewarded = true;
        }

        count = 0;

        //Weekly
        for(int i = 0; i < Weekly_Datas.Count; i++) {
            if(Weekly_Datas[i].hasEarnedPrize)
                count++;
        }
        WeeklyQuest_Slider.maxValue = Weekly_Datas.Count;
        WeeklyQuest_Slider.value = count;
        WeeklyQuest_SliderText.text = WeeklyQuest_Slider.value + "/" + WeeklyQuest_Slider.maxValue;
        WeeklyQuest_CompletedAmountText.text = WeeklyQuest_Slider.value + "/" + WeeklyQuest_Slider.maxValue;
        if(!weeklyRewarded) {
            WeeklyQuest_presentBox.interactable = WeeklyQuest_Slider.value >= WeeklyQuest_Slider.maxValue;
            WeeklyQuest_CheckMark.SetActive(false);
        } else {
            WeeklyQuest_presentBox.interactable = false;
            WeeklyQuest_CheckMark.SetActive(true);
            weeklyRewarded = true;
        }
    }

    public void getDailyReward()
    {
        DailyBoxRewardTab.SetActive(true);
        //Daily
        GachaManager.Instance.addTicket(GachaData.GachaType.Special, 5);
        Player.instance.addAutoAttackTime_Sec(600);

        DailyQuest_presentBox.interactable = false;
        DailyQuest_CheckMark.SetActive(true);
        dailyRewarded = true;

        SoundManager.Instance.Invoke("playCoinSFX", SoundManager.Instance.click.length);
    }

    public void getWeeklyReward()
    {
        WeeklyBoxRewardTab.SetActive(true);
        //Daily
        GachaManager.Instance.addTicket(GachaData.GachaType.Special, 20);
        Player.instance.addAutoAttackTime_Sec(1800);
        GameManager.SetParts(10);

        WeeklyQuest_presentBox.interactable = false;
        WeeklyQuest_CheckMark.SetActive(true);
        weeklyRewarded = true;

        SoundManager.Instance.Invoke("playCoinSFX", SoundManager.Instance.click.length);

    }

    public void openOrePrizeTab(int n)
    {
        OrePrizeTab.SetActive(true);
        OrePrizeButton.interactable = false;

        if(createdPrizeData.Count > 0) {
            foreach(RailInfoData obj in createdPrizeData) {
                Destroy(obj.gameObject);
            }
            tabgroup.tabButtons.Clear();
            createdPrizeData.Clear();
        }


        prizeAmount = n;
        orePrizeAmount_Text.text = "미확인 광석 " + prizeAmount.ToString() + "개";

        for(int i = 0; i < Railmanager.Instance.currentRails.Count; i++) {
            RailInfoData _r = Instantiate(prizeData_pfb, prizeData_parent);
            _r.GetComponent<TabButton>().tabGroup = tabgroup;
            _r.gameObject.SetActive(true);
            _r.rail = Railmanager.Instance.currentRails[i];
            _r.transform.GetChild(0).GetComponent<Image>().sprite = Railmanager.Instance.currentRails[i].stonePrefab.StoneSprite;
            createdPrizeData.Add(_r);
        }
    }

    public void setOrePrize(Rail r)
    {
        currentOrePrizeRail = r;
        OrePrizeButton.interactable = true;
    }

    public void getOrePrize()
    {
        if(currentOrePrizeRail != null)
            currentOrePrizeRail.unMinedOre += prizeAmount;
        OrePrizeTab.SetActive(false);
    }

    public void resetDailyAchievement()
    {
        dailyRewarded = false;
        PlayedTimeDaily = 0;
        killedEnemyDaily = 0;
        killedBossDaily = 0;
        GachaDaily = 0;
        TowerAdventureDaily = 0;
        IronworkTradeDaily = 0;
        OremineDaily = 0;
        UnminedOreDaily = 0;
        InfiniteStageDaily = 0;

        for(int i = 0; i < Daily_Datas.Count; i++) {
            Daily_Datas[i].hasEarnedPrize = false;
            Daily_Datas[i].isCompleted = false;
            Daily_Datas[i].changeQuestGUI();
        }

        setSliderValue();
    }

    public void resetWeeklyAchievement()
    {
        weeklyRewarded = false;
        PlayedTimeWeekly = 0;
        killedEnemyWeekly = 0;
        killedBossWeekly = 0;
        GachaWeekly = 0;
        OremineWeekly = 0;
        UnminedOreWeekly = 0;
        DailyQuestClearWeekly = 0;
        InfiniteStageWeekly = 0;

        for(int i = 0; i < Weekly_Datas.Count; i++) {
            Weekly_Datas[i].hasEarnedPrize = false;
            Weekly_Datas[i].isCompleted = false;
            Weekly_Datas[i].changeQuestGUI();
        }

        setSliderValue();
    }

    public void LoadData(GameData data)
    {
        if(data == null) return;
        if(AchievementDatas == null || AchievementDatas.Count == 0) {
            Debug.LogWarning("[Achievement Load] AchievementDatas is empty");
            return;
        }

        // Daily
        dailyRewarded = data.DailyRewarded;
        if(data.ResetTimeDaily > 0) ResetTimeDaily = data.ResetTimeDaily;
        if(data.PlayedTimeDaily >= 0) PlayedTimeDaily = data.PlayedTimeDaily;
        if(data.killedEnemyDaily >= 0) killedEnemyDaily = data.killedEnemyDaily;
        if(data.killedBossDaily >= 0) killedBossDaily = data.killedBossDaily;
        if(data.GachaDaily >= 0) GachaDaily = data.GachaDaily;
        if(data.TowerAdventureDaily >= 0) TowerAdventureDaily = data.TowerAdventureDaily;
        if(data.IronworkTradeDaily >= 0) IronworkTradeDaily = data.IronworkTradeDaily;
        if(data.OremineDaily >= 0) OremineDaily = data.OremineDaily;
        if(data.UnminedOreDaily >= 0) UnminedOreDaily = data.UnminedOreDaily;
        if(data.InfiniteStageDaily >= 0) InfiniteStageDaily = data.InfiniteStageDaily;

        // Weekly
        weeklyRewarded = data.WeeklyRewarded;
        if(data.ResetTimeWeekly > 0) ResetTimeWeekly = data.ResetTimeWeekly;
        if(data.PlayedTimeWeekly >= 0) PlayedTimeWeekly = data.PlayedTimeWeekly;
        if(data.killedEnemyWeekly >= 0) killedEnemyWeekly = data.killedEnemyWeekly;
        if(data.killedBossWeekly >= 0) killedBossWeekly = data.killedBossWeekly;
        if(data.GachaWeekly >= 0) GachaWeekly = data.GachaWeekly;
        if(data.OremineWeekly >= 0) OremineWeekly = data.OremineWeekly;
        if(data.UnminedOreWeekly >= 0) UnminedOreWeekly = data.UnminedOreWeekly;
        if(data.DailyQuestClearWeekly >= 0) DailyQuestClearWeekly = data.DailyQuestClearWeekly;
        if(data.InfiniteStageWeekly >= 0) InfiniteStageWeekly = data.InfiniteStageWeekly;

        // Quest
        if(data.GachaTimeQuest >= 0) GachaTimeQuest = data.GachaTimeQuest;
        if(data.TowerAdventureQuest >= 0) TowerAdventureQuest = data.TowerAdventureQuest;
        if(data.OreMinedQuest >= 0) OreMinedQuest = data.OreMinedQuest;
        if(data.UnminedOreQuest >= 0) UnminedOreQuest = data.UnminedOreQuest;

        if(data.currentQuest != null) {
            for(int i = 0; i < AchievementDatas.Count; i++) {
                if(data.currentQuest.TryGetValue(AchievementDatas[i].id, out int amount)) {
                    AchievementDatas[i].CurrentQuest = amount;
                }
            }
        }

        if(data.AchieveCondition != null) {
            for(int i = 0; i < AchievementDatas.Count; i++) {
                if(data.AchieveCondition.TryGetValue(AchievementDatas[i].id, out bool amount)) {
                    AchievementDatas[i].hasEarnedPrize = amount;
                }
            }
        }

        if(data.CompletedTab != null) {
            for(int i = 0; i < AchievementDatas.Count; i++) {
                if(data.CompletedTab.TryGetValue(AchievementDatas[i].id, out bool amount)) {
                    AchievementDatas[i].CompletedTab.SetActive(amount);
                }
            }
        }

        for(int i = 0; i < AchievementDatas.Count; i++) {
            ChangeRewardUI(AchievementDatas[i]);
            AchievementDatas[i].changeQuestGUI();
        }

        SetQuestText();
        setSliderValue();
    }


    public void SaveData(GameData data)
    {
        if(data == null) return;

        // Daily Time and Weekly Time
        if(data.ResetTimeDaily != 0) {
            if(DateTime.Now.DayOfYear - data.ResetTimeDaily != 0) {
                resetDailyAchievement();
                ResetTimeDaily = DateTime.Now.DayOfYear;
            }
        } else {
            resetDailyAchievement();
            ResetTimeDaily = DateTime.Now.DayOfYear;
        }

        if(data.ResetTimeWeekly != 0) {
            if(DateTime.Now.DayOfYear - data.ResetTimeWeekly >= 7) {
                resetWeeklyAchievement();
                ResetTimeWeekly = DateTime.Now.DayOfYear;
            }
        } else {
            resetWeeklyAchievement();
            ResetTimeWeekly = DateTime.Now.DayOfYear - (int)DateTime.Now.DayOfWeek;
        }

        // Daily
        data.DailyRewarded = dailyRewarded;
        data.ResetTimeDaily = ResetTimeDaily;
        data.PlayedTimeDaily = PlayedTimeDaily;
        data.killedEnemyDaily = killedEnemyDaily;
        data.killedBossDaily = killedBossDaily;
        data.GachaDaily = GachaDaily;
        data.TowerAdventureDaily = TowerAdventureDaily;
        data.IronworkTradeDaily = IronworkTradeDaily;
        data.OremineDaily = OremineDaily;
        data.UnminedOreDaily = UnminedOreDaily;
        data.InfiniteStageDaily = InfiniteStageDaily;

        // Weekly
        data.WeeklyRewarded = weeklyRewarded;
        data.ResetTimeWeekly = ResetTimeWeekly;
        data.PlayedTimeWeekly = PlayedTimeWeekly;
        data.killedEnemyWeekly = killedEnemyWeekly;
        data.killedBossWeekly = killedBossWeekly;
        data.GachaWeekly = GachaWeekly;
        data.OremineWeekly = OremineWeekly;
        data.UnminedOreWeekly = UnminedOreWeekly;
        data.DailyQuestClearWeekly = DailyQuestClearWeekly;
        data.InfiniteStageWeekly = InfiniteStageWeekly;

        // Quest
        data.GachaTimeQuest = GachaTimeQuest;
        data.TowerAdventureQuest = TowerAdventureQuest;
        data.OreMinedQuest = OreMinedQuest;
        data.UnminedOreQuest = UnminedOreQuest;

        // 업적 딕셔너리 null 방어
        if(data.currentQuest == null) data.currentQuest = new SerializableDictionary<string, int>();
        if(data.AchieveCondition == null) data.AchieveCondition = new SerializableDictionary<string, bool>();
        if(data.CompletedTab == null) data.CompletedTab = new SerializableDictionary<string, bool>();

        // 기존 값 비우고 다시 저장
        data.currentQuest.Clear();
        data.AchieveCondition.Clear();
        data.CompletedTab.Clear();

        for(int i = 0; i < AchievementDatas.Count; i++) {
            string id = AchievementDatas[i].id;
            data.currentQuest[id] = AchievementDatas[i].CurrentQuest;
            data.AchieveCondition[id] = AchievementDatas[i].hasEarnedPrize;
            data.CompletedTab[id] = AchievementDatas[i].CompletedTab.activeSelf;
        }

        Debug.Log($"[Achievement Save] saved count = {AchievementDatas.Count}");
    }
}
