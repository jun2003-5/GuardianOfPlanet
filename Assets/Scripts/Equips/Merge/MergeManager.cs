using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MergeManager : MonoBehaviour
{
    EquipFrame MergingEquip;

    public EquipUpgradeManager upgradeManager;

    [Header("UIs")]
    [Space(5)]
    public EquipGUI MergeEquip_UI;
    public UpgradeStoneCard Stone_UI;
    public TextMeshProUGUI stoneAmount_Text;
    public TextMeshProUGUI equipAmount_Text;

    [Header("Stone")]
    public Sprite[] stoneSprites;
    public Image stoneImage;
    public Image Background;
    public Image Background_BG;
    public Image Light;
    public Image Glow;
    public Image TypeBorder;

    [Header("Button")]
    [Space(5)]
    public Button MergeButton;

    [Header("ResultUIs")]
    [Space(5)]
    public EquipGUI ResultItem;
    public GameObject QuestionMark_Equip;
    public TextMeshProUGUI ResultItemLevel_Text;

    [Header("#----Animation")]
    public RectMask2D topBar;
    public RectMask2D middleBar;

    [Header("ItemEarned")]
    [Space(5)]
    public GameObject ItemEarnedTab;
    public Image Background_Top;
    public Image Background_Glow;
    public Image TypeIcon;
    public TextMeshProUGUI typeofEquip;
    public TextMeshProUGUI nameofEquip;
    public TextMeshProUGUI gradeofEquip;
    public EquipingData ResultTab_EquipData;
    public List<Upgrade_StatsPrefab> ResultTab_Stats;
    public Slider ResultItemAmountSlider;
    public TextMeshProUGUI ResultSlider_Text;

    [Header("#Give Stone If Have")]
    public GameObject StoneRewardTab;
    public GameObject[] stone_Obj;


    [Header("Requiredmoney to merge")]
    public TextMeshProUGUI MergePriceText;
    public TextMeshProUGUI RequiredGoldPrice;

    public int[] mergeStoneRequirement;
    long MergePrice;

    EquipFrame RandomEquip;
    EquipFrame ResultEquip;

    UpgradeStone.TypeOfStone typeStone;
    bool isMerging;

    float[] waitTimes = {
        0.03f, 0.03f, 0.03f, 0.03f, 0.03f, // 5 steps with 0.03s delay
        0.05f, 0.05f, 0.05f, 0.05f, 0.05f, // 5 steps with 0.05s delay
        0.1f, 0.1f, 0.1f, 0.1f, 0.1f,       // 5 steps with 0.1s delay
        0.2f, 0.2f, 0.2f, 0.2f, 0.2f,       // 5 steps with 0.2s delay
        0.23f, 0.25f, 0.26f, 0.28f, 0.3f,       // 5 steps with 0.3s delay
        0.4f, 0.43f, 0.46f, 0.49f, 0.5f,       // 5 steps with 0.5s delay
        0.55f, 0.65f, 0.75f, 0.85f, 1.0f,     // 5 steps with 1.0s delay
    };

    private void Start()
    {
        InvokeRepeating("UIChange", 0, 0.05f);
    }

    public void getEquipFrameColor(Equips.MaterialClass grade)
    {
        switch(grade) {
            case Equips.MaterialClass.Normal:
                stoneImage.sprite = stoneSprites[0];
                Background.color = new Color(0.3803922f, 0.4941176f, 0.5411765f);
                Background_BG.color = new Color(0.4431373f, 0.5568628f, 0.6f);
                Glow.color = new Color(0.5137255f, 0.6509804f, 0.7058824f);
                TypeBorder.color = new Color(0.3803922f, 0.4941176f, 0.5411765f);
                Light.color = new Color(0.509804f, 0.627451f, 0.6705883f);
                break;
            case Equips.MaterialClass.Rare:
                stoneImage.sprite = stoneSprites[1];
                Background.color = new Color(0, 0.6588235f, 1);
                Background_BG.color = new Color(0.172549f, 0.7450981f, 1);
                Glow.color = new Color(0.03137255f, 0.9372549f, 1);
                TypeBorder.color = new Color(0.05490196f, 0.509804f, 0.9764706f);
                Light.color = new Color(0.2078431f, 0.9843137f, 1);
                break;
            case Equips.MaterialClass.Epic:
                stoneImage.sprite = stoneSprites[2];
                Background.color = new Color(0.6980392f, 0.3764706f, 0.9921569f);
                Background_BG.color = new Color(0.7843137f, 0.5019608f, 0.9960784f);
                Glow.color = new Color(0.7254902f, 0.5882353f, 1);
                TypeBorder.color = new Color(0.6392157f, 0.2431373f, 1);
                Light.color = new Color(1, 0.5411765f, 1);
                break;
            case Equips.MaterialClass.Unique:
                stoneImage.sprite = stoneSprites[3];
                Background.color = new Color(0.8679245f, 0.6819407f, 0);
                Background_BG.color = new Color(0.9433962f, 0.8213097f, 0);
                Glow.color = new Color(0.8884411f, 0.9433962f, 0);
                TypeBorder.color = new Color(1, 0.8705882f, 0);
                Light.color = new Color(1, 0.9960785f, 0);
                break;
            case Equips.MaterialClass.Legendary:
                stoneImage.sprite = stoneSprites[4];
                Background.color = new Color(0.8745098f, 0.1843137f, 0.2196078f);
                Background_BG.color = new Color(0.9921569f, 0.2784314f, 0.2941177f);
                Glow.color = new Color(1, 0.6156863f, 0.6431373f);
                TypeBorder.color = new Color(1, 0.1882353f, 0.3019608f);
                Light.color = new Color(1, 0.5607843f, 0.6117647f);
                break;
        }
    }

    public void OpenMergeTab() //Tab
    {
        MergingEquip = EquipManager.Instance.SelectedEquip;
        gameObject.SetActive(true);

        //Question Mark
        QuestionMark_Equip.SetActive(true);

        //UI Setting
        MergeEquip_UI.setGUI(MergingEquip);

        //Result
        ResultItem.setGUI(EquipManager.Instance.getRandomItem_Grade(MergingEquip.equipData.Grade + 1, MergingEquip.equipData.TypeOfEquip));
        ResultItem.Image_Equip.gameObject.SetActive(false);
        QuestionMark_Equip.SetActive(true);
        ResultItemLevel_Text.text = "Lv.0";

        //Animation Default
        topBar.padding = new Vector4(330, 0, 0, 0);
        middleBar.padding = new Vector4(middleBar.padding.x, 730, middleBar.padding.z, 0);


        getEquipFrameColor(MergingEquip.equipData.Grade);

        if(MergingEquip.equipData.Grade == Equips.MaterialClass.Normal)
            MergePrice = 1000000;
        else if(MergingEquip.equipData.Grade == Equips.MaterialClass.Rare)
            MergePrice = 5000000;
        else if(MergingEquip.equipData.Grade == Equips.MaterialClass.Epic)
            MergePrice = 100000000;
        else if(MergingEquip.equipData.Grade == Equips.MaterialClass.Unique)
            MergePrice = 1000000000;
        else if(MergingEquip.equipData.Grade == Equips.MaterialClass.Legendary)
            MergePrice = 10000000000;

        UIChange();
    }

    public void CloseMergeTab()
    {
        this.gameObject.SetActive(false);
    }

    public void UIChange()
    {
        if(gameObject.activeSelf && !isMerging) {
            //Button
            MergeButton.interactable = GameManager.GetMoney() >= MergePrice && MergingEquip.equipData.AmountOfEquip >= MergingEquip.equipData.RequiredAmountForMerge && UpgradeStoneManager.instance.stonesData[(int)MergingEquip.equipData.Grade].StoneAmount >= mergeStoneRequirement[(int)MergingEquip.equipData.Grade];

            stoneAmount_Text.text = UpgradeStoneManager.instance.stonesData[(int)MergingEquip.equipData.Grade].StoneAmount.ToString() + "/" + mergeStoneRequirement[(int)MergingEquip.equipData.Grade];
            equipAmount_Text.text = MergingEquip.equipData.AmountOfEquip + "/" + MergingEquip.equipData.RequiredAmountForMerge;
            MergePriceText.text = GameManager.MoneyString(MergePrice);
            RequiredGoldPrice.text = GameManager.MoneyStringForGamemanager(GameManager.instance.money) + "/" + GameManager.MoneyString(MergePrice);

            if(MergingEquip.equipData.AmountOfEquip >= MergingEquip.equipData.RequiredAmountForMerge)
                equipAmount_Text.color = new Color(0, 0.8396226f, 0.06517614f);
            else
                equipAmount_Text.color = new Color(0.8490566f, 0, 0.05251745f);

            if(UpgradeStoneManager.instance.stonesData[(int)MergingEquip.equipData.Grade].StoneAmount >= mergeStoneRequirement[(int)MergingEquip.equipData.Grade])
                stoneAmount_Text.color = new Color(0, 0.8396226f, 0.06517614f);
            else
                stoneAmount_Text.color = new Color(0.8490566f, 0, 0.05251745f);

            if(GameManager.GetMoney() >= MergePrice)
                RequiredGoldPrice.color = new Color(0, 0.8396226f, 0.06517614f);
            else
                RequiredGoldPrice.color = new Color(0.8490566f, 0, 0.05251745f);
        }
    }

    public void MergeEquipment()
    {
        isMerging = true;
        MergeButton.interactable = false;
        GameManager.SetMoney(-MergePrice);
        MergingEquip.equipData.AmountOfEquip -= MergingEquip.equipData.RequiredAmountForMerge;
        MergingEquip.equipData.Level = 0;
        UpgradeStoneManager.instance.stonesData[(int)MergingEquip.equipData.Grade].StoneAmount -= mergeStoneRequirement[(int)MergingEquip.equipData.Grade];
        ResultEquip = EquipManager.Instance.getRandomItem_Grade(MergingEquip.equipData.Grade + 1, MergingEquip.equipData.TypeOfEquip);

        for(int i = 0; i < ResultTab_Stats.Count; i++) {
            ResultTab_Stats[i].gameObject.SetActive(false);
        }

        //Level Up
        if(ResultEquip.equipData.FinalOption.option.damage != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.Damage, ResultEquip.equipData.FinalOption.option.damage);
        if(ResultEquip.equipData.FinalOption.option.damagePercent != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.DamagePercent, ResultEquip.equipData.FinalOption.option.damagePercent);
        if(ResultEquip.equipData.FinalOption.option.AttackSpeed != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.AttackSpeed, ResultEquip.equipData.FinalOption.option.AttackSpeed);
        if(ResultEquip.equipData.FinalOption.option.BulletSpeed != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.BulletSpeed, ResultEquip.equipData.FinalOption.option.BulletSpeed);
        if(ResultEquip.equipData.FinalOption.option.CritChance != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.CritChance, ResultEquip.equipData.FinalOption.option.CritChance);
        if(ResultEquip.equipData.FinalOption.option.CritDamage != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.CritDamage, ResultEquip.equipData.FinalOption.option.CritDamage);
        if(ResultEquip.equipData.FinalOption.option.StunPercent != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.StunPercent, ResultEquip.equipData.FinalOption.option.StunPercent);
        if(ResultEquip.equipData.FinalOption.option.ExtraEXP != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.ExtraEXP, ResultEquip.equipData.FinalOption.option.ExtraEXP);
        if(ResultEquip.equipData.FinalOption.option.ExtraMoney != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.ExtraMoney, ResultEquip.equipData.FinalOption.option.ExtraMoney);

        ResultEquip.equipData.setFinalOptionStats();

        ResultEquip.equipData.AmountOfEquip++;
        StartCoroutine(MergingEffect());
    }
    public void MergingEffectUIs(bool Real)
    {
        if(!Real)
            RandomEquip = EquipManager.Instance.getRandomItem_Grade(MergingEquip.equipData.Grade + 1, MergingEquip.equipData.TypeOfEquip);
        else {
            RandomEquip = ResultEquip;
        }
        QuestionMark_Equip.SetActive(false);
        ResultItem.Image_Equip.gameObject.SetActive(true);
        ResultItem.setGUIFixedLevel(RandomEquip, RandomEquip.equipData.Level);
    }
    IEnumerator MergingEffect()
    {
        int n = (int)topBar.padding.x;
        while(topBar.padding.x > 0) {
            n -= 3;
            topBar.padding = new Vector4(n, 0, 0, 0);
            yield return new WaitForEndOfFrame();
        }

        n = (int)middleBar.padding.y;
        while(middleBar.padding.y > 0) {
            n -= 3;
            middleBar.padding = new Vector4(middleBar.padding.x, n, middleBar.padding.z, 0);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1);

        for(int i = 0; i < waitTimes.Length; i++) {
            MergingEffectUIs(i == waitTimes.Length - 1);
            yield return new WaitForSeconds(waitTimes[i]);
        }

        isMerging = false;
        setResultTabUI();
        OpenMergeTab();
        MergeButton.interactable = GameManager.GetMoney() >= MergePrice && MergingEquip.equipData.AmountOfEquip >= MergingEquip.equipData.RequiredAmountForMerge && UpgradeStoneManager.instance.stonesData[(int)MergingEquip.equipData.Grade].StoneAmount >= mergeStoneRequirement[(int)MergingEquip.equipData.Grade];
        UIChange();
    }

    public void setResultTabUI()
    {
        ItemEarnedTab.SetActive(true);

        switch(RandomEquip.equipData.Grade) {
            case Equips.MaterialClass.Normal:
                Background_Top.color = new Color(0.227451f, 0.2941177f, 0.3529412f);
                Background_Glow.color = new Color(0.231373f, 0.478431f, 0.552941f);
                gradeofEquip.text = "일반";
                break;
            case Equips.MaterialClass.Rare:
                Background_Top.color = new Color(0.301961f, 0.329412f, 0.87451f);
                Background_Glow.color = new Color(0.168627f, 0.619608f, 1.0f);
                gradeofEquip.text = "레어";
                break;
            case Equips.MaterialClass.Epic:
                Background_Top.color = new Color(0.517647f, 0.0f, 1.0f);
                Background_Glow.color = new Color(0.662745f, 0.294118f, 1.0f);
                gradeofEquip.text = "에픽";
                break;
            case Equips.MaterialClass.Unique:
                Background_Top.color = new Color(1.0f, 0.690196f, 0.2f);
                Background_Glow.color = new Color(1.0f, 0.847058f, 0.164706f);
                gradeofEquip.text = "유니크";
                break;
            case Equips.MaterialClass.Legendary:
                Background_Top.color = new Color(0.600000f, 0.086275f, 0.145098f);
                Background_Glow.color = new Color(0.960784f, 0.200000f, 0.298039f);
                gradeofEquip.text = "레전더리";
                break;
            case Equips.MaterialClass.Ancient:
                Background_Top.color = new Color(0.831373f, 0.478431f, 0.0509804f);
                Background_Glow.color = new Color(0.811765f, 0.627451f, 0.121569f);
                gradeofEquip.text = "고대";
                break;
        }

        TypeIcon.sprite = RandomEquip.Type_Image.sprite;

        switch(RandomEquip.equipData.TypeOfEquip) {
            case Equips.Type.Ring:
                typeofEquip.text = "반지";
                break;
            case Equips.Type.Necklace:
                typeofEquip.text = "목걸이";
                break;
            case Equips.Type.Relics:
                typeofEquip.text = "유물";
                break;
            case Equips.Type.Accessory:
                typeofEquip.text = "장신구";
                break;
            case Equips.Type.Book:
                typeofEquip.text = "고서";
                break;
        }

        nameofEquip.text = RandomEquip.equipData.EquipName;
        ResultTab_EquipData.SetEquipment(RandomEquip);

        if(RandomEquip.equipData.baseOption.option.damage != 0) {
            ResultTab_Stats[0].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = RandomEquip.equipData.FinalOption.option.damage.ToString("#.##");
        }
        if(RandomEquip.equipData.baseOption.option.damagePercent != 0) {
            ResultTab_Stats[1].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (RandomEquip.equipData.FinalOption.option.damagePercent * 100).ToString("#.##") + "%";
        }
        if(RandomEquip.equipData.baseOption.option.CritChance != 0) {
            ResultTab_Stats[2].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (RandomEquip.equipData.FinalOption.option.CritChance).ToString("#.##") + "%";
        }
        if(RandomEquip.equipData.baseOption.option.CritDamage != 0) {
            ResultTab_Stats[3].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (RandomEquip.equipData.FinalOption.option.CritDamage * 100).ToString("#.##") + "%";
        }
        if(RandomEquip.equipData.baseOption.option.AttackSpeed != 0) {
            ResultTab_Stats[4].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (RandomEquip.equipData.FinalOption.option.AttackSpeed * 100).ToString("#.##") + "%";
        }
        if(RandomEquip.equipData.baseOption.option.BulletSpeed != 0) {
            ResultTab_Stats[5].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (RandomEquip.equipData.FinalOption.option.BulletSpeed * 100).ToString("#.##") + "%";
        }
        if(RandomEquip.equipData.baseOption.option.StunPercent != 0) {
            ResultTab_Stats[6].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (RandomEquip.equipData.FinalOption.option.StunPercent).ToString("#.##") + "%";
        }
        if(RandomEquip.equipData.baseOption.option.ExtraMoney != 0) {
            ResultTab_Stats[7].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (RandomEquip.equipData.FinalOption.option.ExtraMoney * 100).ToString("#.##") + "%";
        }
        if(RandomEquip.equipData.baseOption.option.ExtraEXP != 0) {
            ResultTab_Stats[8].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (RandomEquip.equipData.FinalOption.option.ExtraEXP * 100).ToString("#.##") + "%";
        }

        //Slider
        ResultItemAmountSlider.maxValue = RandomEquip.equipData.RequiredAmountForMerge;
        ResultItemAmountSlider.value = RandomEquip.equipData.AmountOfEquip;
        ResultSlider_Text.text = RandomEquip.equipData.AmountOfEquip + "/" + RandomEquip.equipData.RequiredAmountForMerge;
    }

    public void checkIfRewardStone()
    {
        if(RandomEquip.equipData.AmountOfEquip > 1) {
            StoneRewardTab.SetActive(true);
            for(int i = 0; i < stone_Obj.Length; i++) {
                stone_Obj[i].SetActive(false);
                stone_Obj[i].SetActive(((int)RandomEquip.equipData.Grade) == i);
                if(stone_Obj[i].activeSelf) {
                    stone_Obj[i].transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = RewardingStoneAmount().ToString();
                    UpgradeStoneManager.instance.addStone(typeStone, RewardingStoneAmount());
                }
            }
        }
    }

    public int RewardingStoneAmount()
    {
        switch(RandomEquip.equipData.Grade) {
            case Equips.MaterialClass.Normal:
                typeStone = UpgradeStone.TypeOfStone.Normal;
                return 50;
            case Equips.MaterialClass.Rare:
                typeStone = UpgradeStone.TypeOfStone.Rare;
                return 35;
            case Equips.MaterialClass.Epic:
                typeStone = UpgradeStone.TypeOfStone.Epic;
                return 20;
            case Equips.MaterialClass.Unique:
                typeStone = UpgradeStone.TypeOfStone.Unique;
                return 10;
            case Equips.MaterialClass.Legendary:
                typeStone = UpgradeStone.TypeOfStone.Legendary;
                return 5;
            case Equips.MaterialClass.Ancient:
                typeStone = UpgradeStone.TypeOfStone.Ancient;
                return 1;
            default:
                return 0;
        }
    }

    public void CreateStatsPrefab(Upgrade_StatsPrefab.StatsType type, float baseStat)
    {
        for(int i = 0; i < ResultTab_Stats.Count; i++) {
            if(ResultTab_Stats[i]._type == type) {
                ResultTab_Stats[i].BaseStat = baseStat;
                ResultTab_Stats[i].StatsNumberRange = upgradeManager.getStatsPrefab(ResultTab_Stats[i]._type, ResultEquip.equipData, ResultEquip.equipData.Level);
                ResultTab_Stats[i].gameObject.SetActive(true);
                break;
            }
        }
    }
}
