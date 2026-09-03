using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementData : MonoBehaviour
{
    [SerializeField] public string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    [Header("#----------Basic Settings----------#")]
    [Space(5)]
    public string QuestName;
    public List<int> QuestConditionAmount;
    public int QuestNumber;

    public enum TypeofQuest { Daily, Weekly, Quest };

    [Header("#----------Information----------#")]
    [Space(5)]
    public TypeofQuest _type;
    public int QuestAmount;
    public int CurrentQuest;
    public enum rewardType { Diamond, Money, AutoFarm, NormalTicket, SpecialTicket, StoneTicket, UnminedOre, Parts };
    public rewardType reType;
    public List<long> RewardAmount;

    [Header("#----------UIs----------#")]
    [Space(5)]
    public Button AchieveButton;
    public TextMeshProUGUI QuestText;
    public TextMeshProUGUI rewardText;
    public Image rewardImage;
    public GameObject CompletedTab;
    public Slider slider;
    public TextMeshProUGUI SliderText;

    public bool isCompleted;
    public bool hasEarnedPrize;

    private bool isClaiming = false;

    public async void Achieved()
    {
        if(isClaiming) return;
        if(hasEarnedPrize) return;
        if(!isCompleted) return;

        isClaiming = true;
        AchieveButton.interactable = false;

        try {
            SoundManager.Instance.Invoke("playAchieveSFX", SoundManager.Instance.click.length);

            if(CurrentQuest < 0 || CurrentQuest >= RewardAmount.Count) {
                Debug.LogError($"Reward index error: {CurrentQuest}");
                return;
            }

            long rewardValue = RewardAmount[CurrentQuest];

            // 보상 팝업은 보여주되, 상태 확정은 지금 해버림
            AchievementManager.instance.openRewardTab(this);

            // 보상 지급
            AchievementManager.RewardPlayer(reType, rewardValue);

            // 업적 상태 갱신
            if(_type == TypeofQuest.Daily)
                AchievementManager.instance.DailyQuestClearWeekly++;

            if(CurrentQuest == QuestAmount - 1) {
                hasEarnedPrize = true;
                CompletedTab.SetActive(true);
            } else {
                CurrentQuest++;
                AchievementManager.instance.ChangeRewardUI(this);
            }

            isCompleted = false;

            AchievementManager.instance.SetQuestText();
            AchievementManager.instance.setSliderValue();
            changeQuestGUI();

            await DataPersistenceManager.instance.SaveGame();
        } finally {
            isClaiming = false;
        }
    }

    public async void rewardPlayer()
    {
        await RewardPlayerInternal();
    }

    private async Task RewardPlayerInternal()
    {
        if(isClaiming) return;
        if(hasEarnedPrize) return;

        if(CurrentQuest < 0 || CurrentQuest >= RewardAmount.Count) {
            Debug.LogError($"[Achievement] RewardAmount index error. CurrentQuest={CurrentQuest}, Count={RewardAmount.Count}");
            return;
        }

        isClaiming = true;
        AchieveButton.interactable = false;

        // 업적 상태를 먼저 확정
        if(_type == TypeofQuest.Daily)
            AchievementManager.instance.DailyQuestClearWeekly++;

        if(CurrentQuest >= QuestAmount - 1) {
            hasEarnedPrize = true;
            CompletedTab.SetActive(true);
            isCompleted = false;
        } else {
            CurrentQuest++;
            hasEarnedPrize = false;
            CompletedTab.SetActive(false);
            isCompleted = false;
        }

        AchievementManager.instance.SetQuestText();
        AchievementManager.instance.setSliderValue();
        AchievementManager.instance.ChangeRewardUI(this);
        changeQuestGUI();

        // 그 다음 보상 지급
        AchievementManager.RewardPlayer(reType, RewardAmount[Mathf.Clamp(CurrentQuest - (hasEarnedPrize ? 0 : 1), 0, RewardAmount.Count - 1)]);

        // 즉시 저장
        bool saveSuccess = false;
        if(DataPersistenceManager.instance != null) {
            saveSuccess = await DataPersistenceManager.instance.SaveGame();
        }

        if(!saveSuccess) {
            Debug.LogWarning("[Achievement] Save failed after reward claim. There is a risk of duplicate claim after restart.");
        }

        isClaiming = false;
    }

    public void changeQuestGUI()
    {
        if(!hasEarnedPrize) {
            isCompleted = slider.value >= slider.maxValue;
            CompletedTab.SetActive(false);
        } else {
            isCompleted = false;
            CompletedTab.SetActive(true);
        }

        AchieveButton.interactable = isCompleted && !hasEarnedPrize && !isClaiming;
    }
}