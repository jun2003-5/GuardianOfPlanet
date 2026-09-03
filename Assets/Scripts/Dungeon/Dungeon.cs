using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dungeon : MonoBehaviour
{
    [Header("테스트")]
    [SerializeField]
    public DungeonManager dungeonManager;
    [SerializeField]
    public DungeonData dungeonData;

    [SerializeField] 
    Text Boss_Name;
    [SerializeField]
    TextMeshProUGUI Boss_Lvl;
    [SerializeField]
    TextMeshProUGUI Least_Damage_For_Boss;
    public Image DungeonTitleImage;

    public Text Cooldown;

    [Space(10)]
    [Header("쿨타임")] 
    public float coolTime;

    float coolTimeStartTime;

    bool coolTimeStart;
    
    public enum BossLvl {lvl1,lvl2,lvl3,lvl4,lvl5};

    public BossLvl _bossLvl;

    public int LeastDamage;

    public int ForNextStage_Kills;

    public Color ColorForTitle;

    public string getBossLevel(BossLvl bo)
    {
        switch(bo) {
            case BossLvl.lvl1:
                return "★";
            case BossLvl.lvl2:
                return "★★";
            case BossLvl.lvl3:
                return "★★★";
            case BossLvl.lvl4:
                return "★★★★";
            case BossLvl.lvl5:
                return "★★★★★";
            default:
                Debug.Log("Error: this bo enum value is invalid");
                return null;
        }
    }

    private void Start()
    {
        coolTimeStartTime = coolTime;
        Boss_Lvl.text = getBossLevel(_bossLvl);
        Least_Damage_For_Boss.text = string.Format("{0:#,###0}", LeastDamage);
        Boss_Name.text = dungeonData.DungeonName;
        DungeonTitleImage.color = ColorForTitle;
    }

    public void startDungeon()
    {
        Cooldown.transform.parent.GetComponent<Button>().interactable = false;
    }
    public void endDungeon()
    {
        coolTimeStart = true;
    }

    private void Update()
    {      
        if(coolTimeStart) {
            Cooldown.transform.parent.GetComponent<Button>().interactable = false;
            coolTime -= Time.deltaTime;
            Cooldown.text = ((int)coolTime).ToString();
            if(coolTime <= 0) {
                coolTimeStart = false;
                Cooldown.transform.parent.GetComponent<Button>().interactable = true;
                Cooldown.text = "소환";
                coolTime = coolTimeStartTime;
            }
        }
    }
}
