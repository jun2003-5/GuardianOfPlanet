using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("#-----Button Menu")]
    [Space(6)]
    public GameObject ButtonMenu;

    [Header("#-----Equip Focus Menu")]
    [Space(6)]
    public GameObject EquipFocusMenu;

    [Header("#----Resources")]
    public TextMeshProUGUI[] gold_Text;
    public TextMeshProUGUI[] diamond_Text;
    public TextMeshProUGUI[] ore_Text;
    public TextMeshProUGUI[] parts_Text;

    [Header("#---GameSpeed and AutoAttack")]
    public Button[] AutoFarmButton;
    public TextMeshProUGUI[] GameSpeedText;
    public Button noLimitAutoFarm;


    [Header("#----Top Menu Bar Exclamation Mark")]
    public GameObject TopBarExclamationMark;
    public GameObject[] menuExclamationMark;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        setGoldandDiamondText();
        setGameSpeedandAuto();
        setExclamationMarkTopMenu();
    }

    public void setGameSpeedandAuto()
    {
        for(int i = 0; i < AutoFarmButton.Length; i++) {
            if(Player.instance.AutoShootTime > 0) {
                AutoFarmButton[i].interactable = true;
                if(WeaponManager.instance.shootType == WeaponManager.ShootType.AutoShoot) {
                    TimeSpan t = TimeSpan.FromSeconds(Player.instance.AutoShootTime);
                    AutoFarmButton[i].GetComponentInChildren<TextMeshProUGUI>().text = string.Format("{0:#0}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
                } else {
                    AutoFarmButton[i].GetComponentInChildren<TextMeshProUGUI>().text = "AUTO";
                }
            } else {
                AutoFarmButton[i].interactable = false;
                AutoFarmButton[i].GetComponentInChildren<TextMeshProUGUI>().text = "AUTO";
            }

            AutoFarmButton[i].GetComponent<Image>().color = WeaponManager.instance.shootType == WeaponManager.ShootType.AutoShoot ? new Color(0.2378687f, 0.8490566f, 0) : Color.white;
        }

        for(int i = 0; i < GameSpeedText.Length; i++) {
            if(Time.timeScale >= 1) 
                GameSpeedText[i].text = "x" + Time.timeScale;
        }

        noLimitAutoFarm.GetComponent<Image>().color = WeaponManager.instance.shootType == WeaponManager.ShootType.AutoShoot ? new Color(0.2378687f, 0.8490566f, 0) : Color.white;
    }

    public void setGoldandDiamondText()
    {
        for(int i = 0; i < gold_Text.Length; i++) gold_Text[i].text = GameManager.MoneyStringForGamemanager(GameManager.instance.money);
        for(int i = 0; i < diamond_Text.Length; i++) diamond_Text[i].text = string.Format("{0:#,##0}", GameManager.instance.Diamond);
        for(int i = 0; i < ore_Text.Length; i++) ore_Text[i].text = GameManager.MoneyString(GameManager.instance.Ore);
        for(int i = 0; i < parts_Text.Length; i++) parts_Text[i].text = GameManager.MoneyString(GameManager.instance.Parts);
    }


    public void setEquipFocus(int position)
    {
        EquipFocusMenu.transform.localPosition = new Vector3(position, EquipFocusMenu.transform.localPosition.y, EquipFocusMenu.transform.localPosition.z);
    }

    public void buttonMenuOpen()
    {
        ButtonMenu.SetActive(!ButtonMenu.activeSelf);
    }

    public void setExclamationMarkTopMenu()
    {
        TopBarExclamationMark.SetActive(false);
        for(int i = 0; i < menuExclamationMark.Length;i++) {
            if(menuExclamationMark[i].activeSelf) {
                TopBarExclamationMark.SetActive(true);
                break;
            }
        }
    }
}
