using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class CollectionData : MonoBehaviour
{
    public string id;
    public Image Collection_MonsterImage;
    public GameObject Boss_Obj;
    public Image BG;
    public Image Gradient;
    public Image Glow;
    public TextMeshProUGUI Collection_Stage;
    public GameObject[] stars;
    public Slider slider_Killed;
    public TextMeshProUGUI Collection_Killed;

    public TextMeshProUGUI gradeText;

    public Sprite[] masterSprites;

    public enum Grade {Unknown, Bronze, Silver, Gold, Platinum, Diamond, Master}
    public Grade grade;

    public enum typeofEnemy { normal, boss }

    public typeofEnemy _typeofEnemy;

    [Header("업적 보상")]
    public int[] EarnDiamond;


    [Header("등급 업 위한 수")]
    public List<int> RequiredNumberForGrade;

    [Header("죽인 수")]
    public int Killed_Num;
    public Sprite[] Frames;

    private void Start()
    {
        if(_typeofEnemy == typeofEnemy.normal) {
            RequiredNumberForGrade.Add(1);
            RequiredNumberForGrade.Add(100);
            RequiredNumberForGrade.Add(500);
            RequiredNumberForGrade.Add(2500);
            RequiredNumberForGrade.Add(10000);
            RequiredNumberForGrade.Add(50000);

        } else {
            RequiredNumberForGrade.Add(1);
            RequiredNumberForGrade.Add(10);
            RequiredNumberForGrade.Add(50);
            RequiredNumberForGrade.Add(125);
            RequiredNumberForGrade.Add(300);
            RequiredNumberForGrade.Add(1000);
        }

        InvokeRepeating("setSlider", 0, 1f);
    }
    private void setSlider()
    {
        if(grade != Grade.Master) {
            Collection_Killed.text = (Killed_Num - getTotalEnemyNeededToKill()) + "/" + RequiredNumberForGrade[(int)grade];
            slider_Killed.maxValue = RequiredNumberForGrade[(int)grade];
            slider_Killed.value = (Killed_Num - getTotalEnemyNeededToKill());
        } else {
            Collection_Killed.text = "Clear";
            slider_Killed.maxValue = RequiredNumberForGrade[5];
            slider_Killed.value = 0;
        }

        ChangeGrade();
    }
    public void ChangeGrade()
    {
        if(grade == Grade.Unknown) {
            Collection_MonsterImage.color = Color.black;
            gradeText.text = "?";
            BG.color = new Color(0.3843137f, 0.4313726f, 0.5450981f);
            Gradient.color = new Color(0.5647059f, 0.6392157f, 0.7294118f);
            Glow.color = new Color(0.5137255f, 0.6509804f, 0.7058824f);
        } else if(grade == Grade.Bronze) {
            Collection_MonsterImage.color = Color.white;
            gradeText.text = "E";
            BG.color = new Color(0.3843137f, 0.4313726f, 0.5450981f);
            Gradient.color = new Color(0.5647059f, 0.6392157f, 0.7294118f);
            Glow.color = new Color(0.5137255f, 0.6509804f, 0.7058824f);
        } else if (grade == Grade.Silver) {
            Collection_MonsterImage.color = Color.white;
            gradeText.text = "D";
            BG.color = new Color(0.2039216f, 0.6941177f, 0.3254902f);
            Gradient.color = new Color(0.3529412f, 0.8470588f, 0.282353f);
            Glow.color = new Color(0.6117647f, 0.9960784f, 0.3098039f);
        } else if (grade == Grade.Gold) {
            Collection_MonsterImage.color = Color.white;
            gradeText.text = "C";
            BG.color = new Color(0.2352941f, 0.5333334f, 0.9647059f);
            Gradient.color = new Color(0.02352941f, 0.6745098f, 0.9960784f);
            Glow.color = new Color(0.003921569f, 0.8705882f, 1);
        } else if (grade == Grade.Platinum) {
            Collection_MonsterImage.color = Color.white;
            gradeText.text = "B";
            BG.color = new Color(0.4509804f, 0.3019608f, 0.9333333f);
            Gradient.color = new Color(0.5843138f, 0.3333333f, 0.9921569f);
            Glow.color = new Color(0.7254902f, 0.5882353f, 1);
        } else if (grade == Grade.Diamond) {
            Collection_MonsterImage.color = Color.white;
            gradeText.text = "A";
            BG.color = new Color(1, 0.7882353f, 0);
            Gradient.color = new Color(1, 0.9607843f, 0.1333333f);
            Glow.color = new Color(1, 0.8470588f, 0.1254902f);
            Collection_Stage.color = Color.black;
        } else if (grade == Grade.Master) {
            Collection_MonsterImage.color = Color.white;
            gradeText.text = "S";
            BG.sprite = masterSprites[0];
            BG.color = new Color(1, 1, 1);
            Gradient.sprite = masterSprites[1];
            Gradient.color = new Color(1, 1, 1);
            Glow.color = new Color(1, 0.9960784f, 0.8235294f);
            Collection_Stage.color = Color.black;
        }
        setStarObj();
    }

    public int getTotalEnemyNeededToKill()
    {
        int sum = 0;
        for(int i = 0; i < ((int)grade); i++) {
            sum += RequiredNumberForGrade[i];
        }

        return sum;
    }

    public void checkEarnDiamond()
    {
        int sum = 0;
        for(int i = 0; i <= RequiredNumberForGrade.Count; i++) {
            sum = 0;
            for(int j = 0; j < i; j++) {
                sum += RequiredNumberForGrade[j];
            }

            if(Killed_Num >= sum) {
                grade = (Grade)i;
            } else {
                break;
            }
        }

        for(int i = 0; i < ((int)grade); i++) {
            if(EarnDiamond[i] != 2)
                EarnDiamond[i] = 1;
        }
    }

    public void setStarObj()
    {
        for(int i = 0; i < stars.Length; i++) {
            stars[i].SetActive(false);
        }

        for(int i = 0; i < ((int)grade); i++) {
            stars[i].SetActive(true);
        }
    }
}
