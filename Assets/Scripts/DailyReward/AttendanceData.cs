using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AttendanceData : MonoBehaviour
{
    public enum AttedanceData { Parts, Diamond, NormalGacha, SpecialGacha, StoneGacha, AutoFarm };

    public AttedanceData _RewardType;

    public Image BG;
    public Image Corner;
    public GameObject glow;

    public TextMeshProUGUI Reward_Text;
    public Image Reward_Image;
    public GameObject Focus;
    public GameObject CheckList;

    public long RewardAmount;

    public void SetDataInfo(Sprite sprite, string rewardText, Vector2 size)
    {
        Reward_Image.sprite = sprite;
        Reward_Image.rectTransform.sizeDelta = size;
        Reward_Text.text = rewardText;
    }

    public void setDimUI()
    {
        BG.color = new Color(0.6588235f, 0.7333333f, 0.8196079f);
        Corner.color = Color.white;
        Reward_Image.color = new Color(1, 1, 1, 0.3843137f);
        CheckList.SetActive(true);
        Focus.SetActive(false);
        glow.SetActive(false);
    }

    public void setRewardingUI()
    {
        BG.color = new Color(0.1411765f, 0.9058824f, 0.08235294f);
        Corner.color = Color.white;
        Reward_Image.color = new Color(1, 1, 1, 1);
        CheckList.SetActive(false);
        Focus.SetActive(true);
        glow.SetActive(false);
    }
}
