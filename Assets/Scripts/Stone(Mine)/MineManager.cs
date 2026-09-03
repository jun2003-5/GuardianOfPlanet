using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MineManager : MonoBehaviour, IDataPersistence
{
    public Sprite MatSprite;
    public Sprite partsSprite;

    public void StartMoveToMine()
    {
        StartCoroutine(MoveToMine());
    }

    public IEnumerator MoveToMine()
    {
        TransformTab.instance.gameObject.SetActive(true);
        TransformTab.instance.startFading("±¤»ê");

        InfiniteStage.Instance.PauseInfiniteStage();

        yield return new WaitForSecondsRealtime(1.5f);
        GridManager.instance.ToMine();

        //Sound
        SoundManager.Instance.playMusic(SoundManager.MusicType.MineBGM);
    }

    public static MineManager instance;

    [HideInInspector]
    public int clickPower;
    public int ClickLevel;
    [Header("Click UI")]
    public TextMeshProUGUI clickText;
    public TextMeshProUGUI ClickPriceText;
    public TextMeshProUGUI ClickLevelText;

    public List<WeaponPassive> passiveObjects;

    public Image ClickBuyButton;

    [Header("Passive Buff")]
    public bool extraOrePerTouch;
    public float ExtraRailOre;
    public float tradeSalePercent;
    public bool partsPerTouch;
    public float drillStatsIncrease;
    public bool getSpaceStoneFromOre;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void Update()
    {
        //Button
        if(GameManager.GetParts() >= GetClickPrice()) {
            ClickBuyButton.color = new Color(0.5299332f, 0.9056604f, 0.3545746f);
        } else {
            ClickBuyButton.color = Color.white;
        }
    }

    public long GetClickPrice()
    {
        if(ClickLevel < 15)
            return 1;
        else if(ClickLevel < 30)
            return 2;
        else if(ClickLevel < 50)
            return 3;
        else if(ClickLevel < 75)
            return 5;
        else if(ClickLevel < 100)
            return 10;
        else if(ClickLevel < 200)
            return 25;
        else
            return 30;
    }

    public void UpgradeClickPower()
    {
        if(GameManager.GetParts() >= GetClickPrice()) {
            GameManager.SetParts(-GetClickPrice());
            ClickLevel++;

            SoundManager.Instance.Invoke("playCoinSFX", SoundManager.Instance.click.length);
        }
        setClickPower();
        setPassiveValues();
    }

    public void setClickPower()
    {
        clickPower = 0;

        clickPower += ClickLevel;

        if(ClickLevel > 50) {
            clickPower += (ClickLevel - 50);
        }


        if(clickPower == 0)
            clickPower = 1;
        //UI
        //Text
        ClickPriceText.text = GameManager.MoneyString(GetClickPrice());
        clickText.text = "È­¸é Å¬¸¯ ½Ã ±¤¼® " + GameManager.MoneyString(clickPower) + "°³ È¹µæ";
        ClickLevelText.text = "Lv." + ClickLevel;
    }

    public void setPassiveValues()
    {
        ExtraRailOre = 0;
        tradeSalePercent = 0;
        drillStatsIncrease = 0;


        //Passive 1
        if(ClickLevel >= 15) {
            extraOrePerTouch = true;
            passiveObjects[0].PassiveCover.SetActive(false);
        }

        //Passive 2
        if(ClickLevel >= 30) {
            ExtraRailOre += 0.25f;
            passiveObjects[1].PassiveCover.SetActive(false);
        }

        //Passive 3
        if(ClickLevel >= 50) {
            passiveObjects[2].PassiveCover.SetActive(false);
        }

        //Passive 4
        if(ClickLevel >= 75) {
            partsPerTouch = true;
            passiveObjects[3].PassiveCover.SetActive(false);
        }

        //Passive 5
        if(ClickLevel >= 100) {
            tradeSalePercent += 0.15f;
            passiveObjects[4].PassiveCover.SetActive(false);
        }

        //Passive 6
        if(ClickLevel >= 300) {
            drillStatsIncrease += 0.25f;
            passiveObjects[5].PassiveCover.SetActive(false);
        }

        //Passive 6
        if(ClickLevel >= 500) {
            getSpaceStoneFromOre = true;
            passiveObjects[6].PassiveCover.SetActive(false);
        }
    }


    public void MineTouchScreen()
    {
        if(extraOrePerTouch) {
            float random = Random.Range(0.0f, 101.0f);
            if(random <= 2.5f) {
                GameManager.SetOre(clickPower + 500);
                MaterialPopUp.Create(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0), MatSprite, clickPower + 500);
                return;
            }
        }

        if(partsPerTouch) {
            float random = Random.Range(0.0f, 101.0f);
            if(random <= 0.5f) {
                GameManager.SetParts(1);
                MaterialPopUp.Create(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0), partsSprite, 1);
                return;
            }
        }

        GameManager.SetOre(clickPower);
        MaterialPopUp.Create(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0), MatSprite, clickPower);

    }

    public void getStoneParts()
    {
        if(getSpaceStoneFromOre) {
            float random = Random.Range(0.0f, 101.0f);
            if(random <= 1) {
                GameManager.SetParts(1);
                MaterialPopUp.Create(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0), partsSprite, 1);
                return;
            }
        }
    }

    public void LoadData(GameData data)
    {
        ClickLevel = data.Clicklevel;

        setClickPower();
        setPassiveValues();
    }
    public void SaveData(GameData data)
    {
        data.Clicklevel = ClickLevel;
    }
}
