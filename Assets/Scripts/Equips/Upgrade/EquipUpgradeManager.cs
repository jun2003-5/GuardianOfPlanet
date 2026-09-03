using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public class EquipUpgradeManager : MonoBehaviour
{
    public static EquipUpgradeManager Instance;

    public GameObject tab;
    public Button UpgradingButton;

    EquipFrame UpgradingEquip;
    [Header("Upgrading Item GUI")]
    [Space(7)]
    public EquipingData equipGUI;
    public TextMeshProUGUI UpgradeLevelText;
    public TextMeshProUGUI UpgradePercentageText;
    public Image EquipGradeFrame;
    public TextMeshProUGUI EquipGradeText;
    public TextMeshProUGUI EquipNameText;

    [Header("stats Prefab")]
    public List<Upgrade_StatsPrefab> ListOfUSP;
    public Transform USPParent;

    [Header("Stones")]
    [Space(6)]
    public Transform stoneParent;
    public List<UpgradeStoneUI> ListedStones;

    public Slider SPBar;
    public TextMeshProUGUI SPBar_Text;

    [Header("Upgrade Result")]
    public EquipingData ResultPanel_Equip;
    public TextMeshProUGUI ResultPanel_TextStar;
    public GameObject UpgradeResultTab;
    public List<Upgrade_StatsPrefab> UpgradedListOfUSP;
    public TextMeshProUGUI UpgradeResultText;
    public GameObject CantUpgrade;
    public GameObject WantToUpgradeTab;

    int CurrentEXPAdded;
    float SuccessPercent;
    float FailPercent;

    public void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InvokeRepeating("setStoneAmountGUI", 0, 0.05f);
    }

    public void OpenUpgradeTab()
    {
        if(EquipManager.Instance.SelectedEquip.equipData.Level >= 20) {
            CantUpgrade.SetActive(true);
            return;
        } else
            gameObject.SetActive(true);

        //Default Values
        UpgradingEquip = EquipManager.Instance.SelectedEquip;
        CurrentEXPAdded = 0;

        //업그레이드 확률
        UpgradePercentageText.text = "성공 확률: " + SuccessPercent + "%\n실패 확률: " + FailPercent + "%";


        //장비 GUI
        equipGUI.SetEquipment(UpgradingEquip);

        //Set Upgrade Tab GUI
        SPBar.value = 0;
        SPBar.maxValue = UpgradingEquip.equipData.getUpgradeXP();

        for(int i = 0; i < ListedStones.Count; i++) {
            ListedStones[i].setStoneAmountText();
        }

        //Reset Prefab
        for(int i = 0; i < ListOfUSP.Count; i++) {
            ListOfUSP[i].gameObject.SetActive(false);
        }

        //StatsPrefab Create and Setting
        if(UpgradingEquip.equipData.FinalOption.option.damage != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.Damage, UpgradingEquip.equipData.FinalOption.option.damage);
        if(UpgradingEquip.equipData.FinalOption.option.damagePercent != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.DamagePercent, UpgradingEquip.equipData.FinalOption.option.damagePercent);
        if(UpgradingEquip.equipData.FinalOption.option.AttackSpeed != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.AttackSpeed, UpgradingEquip.equipData.FinalOption.option.AttackSpeed);
        if(UpgradingEquip.equipData.FinalOption.option.BulletSpeed != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.BulletSpeed, UpgradingEquip.equipData.FinalOption.option.BulletSpeed);
        if(UpgradingEquip.equipData.FinalOption.option.CritChance != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.CritChance, UpgradingEquip.equipData.FinalOption.option.CritChance);
        if(UpgradingEquip.equipData.FinalOption.option.CritDamage != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.CritDamage, UpgradingEquip.equipData.FinalOption.option.CritDamage);
        if(UpgradingEquip.equipData.FinalOption.option.StunPercent != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.StunPercent, UpgradingEquip.equipData.FinalOption.option.StunPercent);
        if(UpgradingEquip.equipData.FinalOption.option.ExtraEXP != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.ExtraEXP, UpgradingEquip.equipData.FinalOption.option.ExtraEXP);
        if(UpgradingEquip.equipData.FinalOption.option.ExtraMoney != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.ExtraMoney, UpgradingEquip.equipData.FinalOption.option.ExtraMoney);

        //Stone UI
        stoneUIOrder();

        //Equip Grade
        EquipGradeFrame.color = UpgradingEquip.Background_BG.color;
        EquipGradeText.text = UpgradingEquip.equipData.gradeText;
        EquipNameText.text = UpgradingEquip.equipData.EquipName;
    }

    public void setStoneAmountGUI()
    {
        for(int i = 0; i < ListedStones.Count; i++) {
            ListedStones[i].setStoneAmountText();
        }
    }

    public void setUpgradeTabGUI()
    {
        getUpgradeProbability(UpgradingEquip.equipData.Level);

        //레벨
        UpgradeLevelText.text = UpgradingEquip.equipData.Level + "성 > " + (UpgradingEquip.equipData.Level + 1) + "성";

        //Slider
        SPBar.value = CurrentEXPAdded;
        SPBar.maxValue = UpgradingEquip.equipData.getUpgradeXP();
        SPBar_Text.text = SPBar.value + "/" + UpgradingEquip.equipData.getUpgradeXP() + " (" + (int)(SPBar.value / UpgradingEquip.equipData.getUpgradeXP() * 100) + "%)";

        //Percent
        if(SPBar.value > 0) {
            UpgradePercentageText.text = "성공 확률: " + (SuccessPercent * (SPBar.value / UpgradingEquip.equipData.getUpgradeXP())).ToString("F1") + "%\n실패 확률: " + (FailPercent + (SuccessPercent - (SuccessPercent * (SPBar.value / UpgradingEquip.equipData.getUpgradeXP())))).ToString("F1") + "%";
        } else {
            UpgradePercentageText.text = "성공 확률: 0%\n실패 확률: 0%";
        }

        //Upgrade Button
        UpgradingButton.interactable = CurrentEXPAdded > 0;

         //StatsPrefab Create and Setting
        if(UpgradingEquip.equipData.FinalOption.option.damage != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.Damage, UpgradingEquip.equipData.FinalOption.option.damage);
        if(UpgradingEquip.equipData.FinalOption.option.damagePercent != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.DamagePercent, UpgradingEquip.equipData.FinalOption.option.damagePercent);
        if(UpgradingEquip.equipData.FinalOption.option.AttackSpeed != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.AttackSpeed, UpgradingEquip.equipData.FinalOption.option.AttackSpeed);
        if(UpgradingEquip.equipData.FinalOption.option.BulletSpeed != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.BulletSpeed, UpgradingEquip.equipData.FinalOption.option.BulletSpeed);
        if(UpgradingEquip.equipData.FinalOption.option.CritChance != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.CritChance, UpgradingEquip.equipData.FinalOption.option.CritChance);
        if(UpgradingEquip.equipData.FinalOption.option.CritDamage != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.CritDamage, UpgradingEquip.equipData.FinalOption.option.CritDamage);
        if(UpgradingEquip.equipData.FinalOption.option.StunPercent != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.StunPercent, UpgradingEquip.equipData.FinalOption.option.StunPercent);
        if(UpgradingEquip.equipData.FinalOption.option.ExtraEXP != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.ExtraEXP, UpgradingEquip.equipData.FinalOption.option.ExtraEXP);
        if(UpgradingEquip.equipData.FinalOption.option.ExtraMoney != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.ExtraMoney, UpgradingEquip.equipData.FinalOption.option.ExtraMoney);
    }

    //CreateStatsPrefab
    public void CreateStatsPrefab(Upgrade_StatsPrefab.StatsType type, float baseStat)
    {
        for(int i = 0; i < ListOfUSP.Count; i++) {
            if(ListOfUSP[i]._type == type) {
                ListOfUSP[i].BaseStat = baseStat;
                setStatsPrefab(ListOfUSP[i], UpgradingEquip.equipData.Level);
                ListOfUSP[i].gameObject.SetActive(true);
                break;
            }
        }
    }

    public void setStatsPrefab(Upgrade_StatsPrefab _U, int level)
    {
        _U.StatsNumberRange.Clear();
        if(_U._type == Upgrade_StatsPrefab.StatsType.DamagePercent) {
            switch(UpgradingEquip.equipData.Grade) {
                case Equips.MaterialClass.Normal:
                    _U.StatsNumberRange.Add(0.15f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.16f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.17f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.18f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.19f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Rare:
                    _U.StatsNumberRange.Add(0.12f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.13f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.135f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.14f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.15f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Epic:
                    _U.StatsNumberRange.Add(0.1f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.105f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.11f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.115f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.12f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Unique:
                    _U.StatsNumberRange.Add(0.095f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.1f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.105f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.11f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.12f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Legendary:
                    _U.StatsNumberRange.Add(0.09f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.10f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.105f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.11f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.12f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Ancient:
                    _U.StatsNumberRange.Add(0.085f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.09f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.095f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.10f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.105f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
            }
        } else if(_U._type == Upgrade_StatsPrefab.StatsType.Damage) {
            switch(UpgradingEquip.equipData.Grade) {
                case Equips.MaterialClass.Normal:
                    _U.StatsNumberRange.Add(0.15f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.175f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.20f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.225f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.25f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Rare:
                    _U.StatsNumberRange.Add(0.14f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.16f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.18f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.20f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.22f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Epic:
                    _U.StatsNumberRange.Add(0.13f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.14f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.15f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.16f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.17f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Unique:
                    _U.StatsNumberRange.Add(0.12f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.13f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.14f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.15f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.16f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Legendary:
                    _U.StatsNumberRange.Add(0.11f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.115f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.12f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.125f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.13f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Ancient:
                    _U.StatsNumberRange.Add(0.10f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.105f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.11f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.115f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.12f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
            }
        } else if(_U._type == Upgrade_StatsPrefab.StatsType.AttackSpeed) {
            switch(UpgradingEquip.equipData.Grade) {
                case Equips.MaterialClass.Normal:
                    _U.StatsNumberRange.Add(0.06f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0625f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.065f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0675f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.07f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Rare:
                    _U.StatsNumberRange.Add(0.05f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0525f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.055f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0575f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.06f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Epic:
                    _U.StatsNumberRange.Add(0.04f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0425f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.045f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0475f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.05f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Unique:
                    _U.StatsNumberRange.Add(0.03f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0325f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.035f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0375f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.04f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Legendary:
                    _U.StatsNumberRange.Add(0.02f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.025f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.025f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0275f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.03f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Ancient:
                    _U.StatsNumberRange.Add(0.01f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0125f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.015f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0175f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.02f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
            }
        } else if(_U._type == Upgrade_StatsPrefab.StatsType.BulletSpeed) {
            switch(UpgradingEquip.equipData.Grade) {
                case Equips.MaterialClass.Normal:
                    _U.StatsNumberRange.Add(0.06f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0625f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.065f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0675f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.07f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Rare:
                    _U.StatsNumberRange.Add(0.05f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0525f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.055f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0575f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.06f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Epic:
                    _U.StatsNumberRange.Add(0.04f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0425f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.045f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0475f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.05f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Unique:
                    _U.StatsNumberRange.Add(0.03f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0325f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.035f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0375f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.04f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Legendary:
                    _U.StatsNumberRange.Add(0.02f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.025f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.025f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0275f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.03f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Ancient:
                    _U.StatsNumberRange.Add(0.01f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0125f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.015f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0175f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.02f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
            }
        } else if(_U._type == Upgrade_StatsPrefab.StatsType.CritChance) {
            switch(UpgradingEquip.equipData.Grade) {
                case Equips.MaterialClass.Normal:
                    _U.StatsNumberRange.Add(0.06f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0625f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.065f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0675f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.07f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Rare:
                    _U.StatsNumberRange.Add(0.05f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0525f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.055f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0575f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.06f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Epic:
                    _U.StatsNumberRange.Add(0.04f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0425f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.045f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0475f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.05f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Unique:
                    _U.StatsNumberRange.Add(0.03f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0325f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.035f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0375f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.04f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Legendary:
                    _U.StatsNumberRange.Add(0.02f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.025f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.025f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0275f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.03f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Ancient:
                    _U.StatsNumberRange.Add(0.01f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0125f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.015f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0175f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.02f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
            }
        } else if(_U._type == Upgrade_StatsPrefab.StatsType.CritDamage) {
            switch(UpgradingEquip.equipData.Grade) {
                case Equips.MaterialClass.Normal:
                    _U.StatsNumberRange.Add(0.11f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.1125f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.115f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.1175f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.12f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Rare:
                    _U.StatsNumberRange.Add(0.10f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.1025f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.105f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.1075f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.11f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Epic:
                    _U.StatsNumberRange.Add(0.09f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0925f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.095f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0975f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.10f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Unique:
                    _U.StatsNumberRange.Add(0.08f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0825f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.085f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0875f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.09f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Legendary:
                    _U.StatsNumberRange.Add(0.07f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.075f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.075f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0775f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.08f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Ancient:
                    _U.StatsNumberRange.Add(0.06f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0625f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.065f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0675f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.07f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
            }
        } else if(_U._type == Upgrade_StatsPrefab.StatsType.StunPercent) {
            switch(UpgradingEquip.equipData.Grade) {
                case Equips.MaterialClass.Normal:
                    _U.StatsNumberRange.Add(0.06f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0625f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.065f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0675f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.07f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Rare:
                    _U.StatsNumberRange.Add(0.05f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0525f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.055f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0575f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.06f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Epic:
                    _U.StatsNumberRange.Add(0.04f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0425f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.045f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0475f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.05f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Unique:
                    _U.StatsNumberRange.Add(0.03f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0325f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.035f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0375f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.04f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Legendary:
                    _U.StatsNumberRange.Add(0.02f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.025f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.025f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0275f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.03f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Ancient:
                    _U.StatsNumberRange.Add(0.01f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0125f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.015f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0175f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.02f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
            }
        } else if(_U._type == Upgrade_StatsPrefab.StatsType.ExtraEXP) {
            switch(UpgradingEquip.equipData.Grade) {
                case Equips.MaterialClass.Normal:
                    _U.StatsNumberRange.Add(0.09f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0925f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.095f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0975f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.10f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Rare:
                    _U.StatsNumberRange.Add(0.08f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0825f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.085f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0875f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.09f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Epic:
                    _U.StatsNumberRange.Add(0.07f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0725f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.075f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0775f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.08f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Unique:
                    _U.StatsNumberRange.Add(0.06f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0625f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.065f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0675f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.07f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Legendary:
                    _U.StatsNumberRange.Add(0.05f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.055f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.055f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0575f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.06f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Ancient:
                    _U.StatsNumberRange.Add(0.04f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0425f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.045f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0475f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.05f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
            }
        } else if(_U._type == Upgrade_StatsPrefab.StatsType.ExtraMoney) {
            switch(UpgradingEquip.equipData.Grade) {
                case Equips.MaterialClass.Normal:
                    _U.StatsNumberRange.Add(0.09f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0925f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.095f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0975f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.10f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Rare:
                    _U.StatsNumberRange.Add(0.08f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0825f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.085f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0875f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.09f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Epic:
                    _U.StatsNumberRange.Add(0.07f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0725f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.075f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0775f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.08f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Unique:
                    _U.StatsNumberRange.Add(0.06f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0625f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.065f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0675f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.07f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Legendary:
                    _U.StatsNumberRange.Add(0.05f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.055f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.055f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0575f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.06f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Ancient:
                    _U.StatsNumberRange.Add(0.04f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0425f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.045f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.0475f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.StatsNumberRange.Add(0.05f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
            }
        }
    }

    public List<float> getStatsPrefab(Upgrade_StatsPrefab.StatsType type, Equips equip, int level)
    {
        List<float> _U = new List<float>();
        if(type == Upgrade_StatsPrefab.StatsType.DamagePercent) {
            switch(equip.Grade) {
                case Equips.MaterialClass.Normal:
                    _U.Add(0.15f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.16f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.17f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.18f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.19f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Rare:
                    _U.Add(0.12f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.13f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.135f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.14f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.15f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Epic:
                    _U.Add(0.1f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.105f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.11f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.115f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.12f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Unique:
                    _U.Add(0.095f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.1f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.105f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.11f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.12f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Legendary:
                    _U.Add(0.09f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.10f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.105f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.11f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.12f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Ancient:
                    _U.Add(0.085f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.09f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.095f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.10f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.105f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
            }
        } else if(type == Upgrade_StatsPrefab.StatsType.Damage) {
            switch(equip.Grade) {
                case Equips.MaterialClass.Normal:
                    _U.Add(0.15f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.175f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.20f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.225f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.25f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Rare:
                    _U.Add(0.14f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.16f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.18f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.20f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.22f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Epic:
                    _U.Add(0.13f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.14f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.15f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.16f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.17f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Unique:
                    _U.Add(0.12f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.13f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.14f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.15f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.16f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Legendary:
                    _U.Add(0.11f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.115f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.12f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.125f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.13f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Ancient:
                    _U.Add(0.10f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.105f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.11f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.115f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.12f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
            }
        } else if(type == Upgrade_StatsPrefab.StatsType.AttackSpeed) {
            switch(equip.Grade) {
                case Equips.MaterialClass.Normal:
                    _U.Add(0.06f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0625f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.065f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0675f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.07f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Rare:
                    _U.Add(0.05f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0525f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.055f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0575f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.06f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Epic:
                    _U.Add(0.04f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0425f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.045f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0475f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.05f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Unique:
                    _U.Add(0.03f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0325f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.035f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0375f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.04f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Legendary:
                    _U.Add(0.02f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.025f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.025f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0275f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.03f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Ancient:
                    _U.Add(0.01f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0125f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.015f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0175f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.02f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
            }
        } else if(type == Upgrade_StatsPrefab.StatsType.BulletSpeed) {
            switch(equip.Grade) {
                case Equips.MaterialClass.Normal:
                    _U.Add(0.06f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0625f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.065f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0675f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.07f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Rare:
                    _U.Add(0.05f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0525f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.055f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0575f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.06f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Epic:
                    _U.Add(0.04f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0425f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.045f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0475f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.05f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Unique:
                    _U.Add(0.03f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0325f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.035f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0375f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.04f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Legendary:
                    _U.Add(0.02f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.025f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.025f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0275f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.03f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Ancient:
                    _U.Add(0.01f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0125f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.015f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0175f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.02f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
            }
        } else if(type == Upgrade_StatsPrefab.StatsType.CritChance) {
            switch(equip.Grade) {
                case Equips.MaterialClass.Normal:
                    _U.Add(0.06f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0625f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.065f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0675f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.07f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Rare:
                    _U.Add(0.05f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0525f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.055f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0575f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.06f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Epic:
                    _U.Add(0.04f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0425f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.045f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0475f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.05f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Unique:
                    _U.Add(0.03f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0325f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.035f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0375f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.04f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Legendary:
                    _U.Add(0.02f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.025f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.025f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0275f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.03f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Ancient:
                    _U.Add(0.01f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0125f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.015f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0175f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.02f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
            }
        } else if(type == Upgrade_StatsPrefab.StatsType.CritDamage) {
            switch(equip.Grade) {
                case Equips.MaterialClass.Normal:
                    _U.Add(0.11f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.1125f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.115f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.1175f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.12f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Rare:
                    _U.Add(0.10f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.1025f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.105f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.1075f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.11f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Epic:
                    _U.Add(0.09f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0925f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.095f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0975f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.10f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Unique:
                    _U.Add(0.08f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0825f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.085f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0875f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.09f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Legendary:
                    _U.Add(0.07f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.075f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.075f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0775f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.08f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Ancient:
                    _U.Add(0.06f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0625f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.065f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0675f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.07f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
            }
        } else if(type == Upgrade_StatsPrefab.StatsType.StunPercent) {
            switch(equip.Grade) {
                case Equips.MaterialClass.Normal:
                    _U.Add(0.06f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0625f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.065f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0675f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.07f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Rare:
                    _U.Add(0.05f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0525f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.055f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0575f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.06f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Epic:
                    _U.Add(0.04f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0425f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.045f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0475f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.05f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Unique:
                    _U.Add(0.03f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0325f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.035f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0375f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.04f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Legendary:
                    _U.Add(0.02f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.025f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.025f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0275f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.03f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Ancient:
                    _U.Add(0.01f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0125f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.015f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0175f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.02f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
            }
        } else if(type == Upgrade_StatsPrefab.StatsType.ExtraEXP) {
            switch(equip.Grade) {
                case Equips.MaterialClass.Normal:
                    _U.Add(0.09f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0925f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.095f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0975f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.10f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Rare:
                    _U.Add(0.08f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0825f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.085f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0875f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.09f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Epic:
                    _U.Add(0.07f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0725f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.075f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0775f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.08f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Unique:
                    _U.Add(0.06f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0625f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.065f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0675f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.07f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Legendary:
                    _U.Add(0.05f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.055f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.055f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0575f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.06f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Ancient:
                    _U.Add(0.04f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0425f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.045f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0475f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.05f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
            }
        } else if(type == Upgrade_StatsPrefab.StatsType.ExtraMoney) {
            switch(equip.Grade) {
                case Equips.MaterialClass.Normal:
                    _U.Add(0.09f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0925f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.095f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0975f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.10f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Rare:
                    _U.Add(0.08f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0825f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.085f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0875f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.09f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Epic:
                    _U.Add(0.07f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0725f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.075f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0775f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.08f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Unique:
                    _U.Add(0.06f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0625f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.065f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0675f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.07f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Legendary:
                    _U.Add(0.05f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.055f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.055f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0575f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.06f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
                case Equips.MaterialClass.Ancient:
                    _U.Add(0.04f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0425f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.045f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.0475f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    _U.Add(0.05f * (1 + (level >= 10 ? level % 10 * 0.2f : 0)));
                    break;
            }
        }
        return _U;
    }

    public void UpgradeTab()
    {
        if(CurrentEXPAdded > 0)
            WantToUpgradeTab.SetActive(true);
    }

    public void UpgradeEquip()
    {
        float random = Random.Range(0.0f, 101.0f);
        UpgradeResultTab.SetActive(true);
        if(random <= SuccessPercent) {
            UpgradeSuccessed();
        } else {
            UpgradeFailed();
        }

        CurrentEXPAdded = 0;
        for(int i = 0; i < ListedStones.Count; i++) {
            ListedStones[i].setStoneAmount();
            stoneUIOrder();
        }
    }

    public void UpgradeSuccessed()
    {
        UpgradeResultText.text = "강화 성공";
        UpgradeResultText.color = new Color(0.2049017f, 0.8f, 0.1019608f);
        for(int i = 0; i < UpgradedListOfUSP.Count; i++) {
            UpgradedListOfUSP[i].gameObject.SetActive(false);
        }

        for(int i = 0; i < ListOfUSP.Count; i++) {
            if(ListOfUSP[i].gameObject.activeSelf) {
                UpgradedListOfUSP[i].gameObject.SetActive(true);
                UpgradedListOfUSP[i].BaseStat = ListOfUSP[i].BaseStat;
                UpgradedListOfUSP[i].StatsNumberRange = ListOfUSP[i].StatsNumberRange;
            }
        }

        for(int i = 0; i < UpgradedListOfUSP.Count; i++) {
            if(UpgradedListOfUSP[i].gameObject.activeSelf) {
                EquipManager.Instance.SelectedEquip.equipData.LevelUpStats(UpgradedListOfUSP[i].StartSlotMachine());
            }
        }

        for(int i = 0; i < UpgradedListOfUSP.Count; i++) {
            if(UpgradedListOfUSP[i].gameObject.activeSelf)
                UpgradedListOfUSP[i].ChangeTextSuccess();
        }

        EquipManager.Instance.SelectedEquip.equipData.setExtraOptionStats();

        EquipManager.Instance.SelectedEquip.equipData.Level++;

        //Equip Image
        ResultPanel_Equip.SetEquipment(UpgradingEquip);
        ResultPanel_TextStar.text = (UpgradingEquip.equipData.Level-1) + "성 > " + UpgradingEquip.equipData.Level + "성";
        equipGUI.SetEquipment(UpgradingEquip);

        UpgradingEquip.equipData.setFinalOptionStats();

        //Reset Prefab
        for(int i = 0; i < ListOfUSP.Count; i++) {
            ListOfUSP[i].gameObject.SetActive(false);
        }

        //StatsPrefab Create and Setting
        if(UpgradingEquip.equipData.FinalOption.option.damage != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.Damage, UpgradingEquip.equipData.FinalOption.option.damage);
        if(UpgradingEquip.equipData.FinalOption.option.damagePercent != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.DamagePercent, UpgradingEquip.equipData.FinalOption.option.damagePercent);
        if(UpgradingEquip.equipData.FinalOption.option.AttackSpeed != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.AttackSpeed, UpgradingEquip.equipData.FinalOption.option.AttackSpeed);
        if(UpgradingEquip.equipData.FinalOption.option.BulletSpeed != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.BulletSpeed, UpgradingEquip.equipData.FinalOption.option.BulletSpeed);
        if(UpgradingEquip.equipData.FinalOption.option.CritChance != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.CritChance, UpgradingEquip.equipData.FinalOption.option.CritChance);
        if(UpgradingEquip.equipData.FinalOption.option.CritDamage != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.CritDamage, UpgradingEquip.equipData.FinalOption.option.CritDamage);
        if(UpgradingEquip.equipData.FinalOption.option.StunPercent != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.StunPercent, UpgradingEquip.equipData.FinalOption.option.StunPercent);
        if(UpgradingEquip.equipData.FinalOption.option.ExtraEXP != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.ExtraEXP, UpgradingEquip.equipData.FinalOption.option.ExtraEXP);
        if(UpgradingEquip.equipData.FinalOption.option.ExtraMoney != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.ExtraMoney, UpgradingEquip.equipData.FinalOption.option.ExtraMoney);
    }

    public void UpgradeFailed()
    {
        UpgradeResultText.text = "강화 실패";
        UpgradeResultText.color = new Color(0.8584906f, 0.1579299f, 0.1936449f);

        for(int i = 0; i < UpgradedListOfUSP.Count; i++) {
            UpgradedListOfUSP[i].gameObject.SetActive(false);
        }

        for(int i = 0; i < ListOfUSP.Count; i++) {
            if(ListOfUSP[i].gameObject.activeSelf) {
                UpgradedListOfUSP[i].gameObject.SetActive(true);
                UpgradedListOfUSP[i].BaseStat = ListOfUSP[i].BaseStat;
                setStatsPrefab(UpgradedListOfUSP[i], EquipManager.Instance.SelectedEquip.equipData.Level - 1);
                UpgradedListOfUSP[i].noChangeText();
            }
        }

        if(EquipManager.Instance.SelectedEquip.equipData.Level > 10) {
            int index = 0;
            for(int i = 0; i < UpgradedListOfUSP.Count; i++) {
                if(UpgradedListOfUSP[i].gameObject.activeSelf) {
                    UpgradedListOfUSP[i].ChangeTextFailed(EquipManager.Instance.SelectedEquip.equipData.getUpgradeIndex()[index]);
                    index++;
                }
            }

            EquipManager.Instance.SelectedEquip.equipData.removeLevelUpStats();
            ResultPanel_TextStar.text = (EquipManager.Instance.SelectedEquip.equipData.Level + 1) + "성 > " + EquipManager.Instance.SelectedEquip.equipData.Level + "성";
        } else {
            ResultPanel_TextStar.text = EquipManager.Instance.SelectedEquip.equipData.Level + "성 > " + EquipManager.Instance.SelectedEquip.equipData.Level + "성";
        }

        //Equip Image
        ResultPanel_Equip.SetEquipment(EquipManager.Instance.SelectedEquip);

        //Reset        
        for(int i = 0; i < ListOfUSP.Count; i++) {
            ListOfUSP[i].gameObject.SetActive(false);
        }

        //StatsPrefab Create and Setting
        if(UpgradingEquip.equipData.FinalOption.option.damage != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.Damage, UpgradingEquip.equipData.FinalOption.option.damage);
        if(UpgradingEquip.equipData.FinalOption.option.damagePercent != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.DamagePercent, UpgradingEquip.equipData.FinalOption.option.damagePercent);
        if(UpgradingEquip.equipData.FinalOption.option.AttackSpeed != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.AttackSpeed, UpgradingEquip.equipData.FinalOption.option.AttackSpeed);
        if(UpgradingEquip.equipData.FinalOption.option.BulletSpeed != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.BulletSpeed, UpgradingEquip.equipData.FinalOption.option.BulletSpeed);
        if(UpgradingEquip.equipData.FinalOption.option.CritChance != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.CritChance, UpgradingEquip.equipData.FinalOption.option.CritChance);
        if(UpgradingEquip.equipData.FinalOption.option.CritDamage != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.CritDamage, UpgradingEquip.equipData.FinalOption.option.CritDamage);
        if(UpgradingEquip.equipData.FinalOption.option.StunPercent != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.StunPercent, UpgradingEquip.equipData.FinalOption.option.StunPercent);
        if(UpgradingEquip.equipData.FinalOption.option.ExtraEXP != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.ExtraEXP, UpgradingEquip.equipData.FinalOption.option.ExtraEXP);
        if(UpgradingEquip.equipData.FinalOption.option.ExtraMoney != 0)
            CreateStatsPrefab(Upgrade_StatsPrefab.StatsType.ExtraMoney, UpgradingEquip.equipData.FinalOption.option.ExtraMoney);
    }

    public void getUpgradeProbability(int star)
    {
        switch(star) {
            case 0:
                SuccessPercent = 100;
                FailPercent = 0;
                break;
            case 1:
                SuccessPercent = 90;
                FailPercent = 10;
                break;
            case 2:
                SuccessPercent = 80;
                FailPercent = 20;
                break;
            case 3:
                SuccessPercent = 75;
                FailPercent = 25;
                break;
            case 4:
                SuccessPercent = 70;
                FailPercent = 30;
                break;
            case 5:
                SuccessPercent = 65;
                FailPercent = 35;
                break;
            case 6:
                SuccessPercent = 60;
                FailPercent = 40;
                break;
            case 7:
                SuccessPercent = 60;
                FailPercent = 40;
                break;
            case 8:
                SuccessPercent = 60;
                FailPercent = 40;
                break;
            case 9:
                SuccessPercent = 50;
                FailPercent = 50;
                break;
            case 10:
                SuccessPercent = 50;
                FailPercent = 50;
                break;
            case 11:
                SuccessPercent = 50;
                FailPercent = 50;
                break;
            case 12:
                SuccessPercent = 50;
                FailPercent = 50;
                break;
            case 13:
                SuccessPercent = 45;
                FailPercent = 55;
                break;
            case 14:
                SuccessPercent = 45;
                FailPercent = 55;
                break;
            case 15:
                SuccessPercent = 45;
                FailPercent = 55;
                break;
            case 16:
                SuccessPercent = 40;
                FailPercent = 60;
                break;
            case 17:
                SuccessPercent = 30;
                FailPercent = 70;
                break;
            case 18:
                SuccessPercent = 20;
                FailPercent = 80;
                break;
            case 19:
                SuccessPercent = 10;
                FailPercent = 90;
                break;
        }
    }

    public void ChangeObjectSize(UpgradeStone stone, Vector3 size)
    {
        for(int i = 0; i < ListedStones.Count; i++) {
            if(ListedStones[i] == stone) {
                ListedStones[i].gameObject.transform.localScale = size;
            }
        }
    }

    public void DeSelectAll()
    {
        for(int i = 0; i < ListedStones.Count; i++) {
            ListedStones[i].resetStoneAmount();
        }
        CurrentEXPAdded = 0;
    }

    public void AutoSelect()
    {
        DeSelectAll();

        int TotalEXP = CurrentEXPAdded;

        for(int i = 0; i < ListedStones.Count; i++) {
            for(int j = 0; j < ListedStones[i].stoneData.StoneAmount; j++) {
                if(TotalEXP >= UpgradingEquip.equipData.getUpgradeXP())
                    break;
                TotalEXP += ListedStones[i].stoneData.EXPamount;
                ListedStones[i].SelectedAmount++;
            }
            if(TotalEXP >= UpgradingEquip.equipData.getUpgradeXP())
                break;
        }

        for(int i = 0; i < ListedStones.Count; i++) {
            ListedStones[i].setStoneAmountText();
        }
        CurrentEXPAdded = TotalEXP;
    }

    public void UpgradeStoneClicked(UpgradeStoneUI stone)
    {
        if(CurrentEXPAdded < UpgradingEquip.equipData.getUpgradeXP()) {
            stone.SelectedAmount++;
            CurrentEXPAdded += stone.stoneData.EXPamount;
            stone.setStoneAmountText();
        }
    }

    public void stoneUIOrder()
    {
        ListedStones.Sort((a, b) => {
            int stoneAmount = a.Empty_Tab.activeSelf.CompareTo(b.Empty_Tab.activeSelf);

            if(stoneAmount != 0) {
                return stoneAmount;
            }

            int stoneGrade = b.stoneData.stoneGrade.CompareTo(a.stoneData.stoneGrade);

            if(stoneGrade != 0) {
                return stoneGrade;
            }

            return 0;
        });

        for(int i = 0; i < ListedStones.Count; i++) {
            UpgradeStoneUI stoneUI = ListedStones[i];
            int childIndex = stoneUI.transform.GetSiblingIndex();
            if(childIndex != i) {
                stoneUI.transform.SetSiblingIndex(i);
            }
        }
    }

    private void Update()
    {
        setUpgradeTabGUI();
    }
}
