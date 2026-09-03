using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Railmanager : MonoBehaviour, IDataPersistence
{
    public static Railmanager Instance;

    public Transform railParent;

    public Transform firstRailLoc;
    public Transform bottomRailLoc;

    [Header("#---Rail")]
    public Rail rail_pfb;
    public List<Rail> currentRails;

    [Header("#----StoneShops")]
    public List<railShop> shops;

    [Header("#---Rail Info Tab")]
    //Tab group
    public TabGroup tabGroup;
    public TabButton tabButton;
    public GameObject StoneBuyTab;
    public GameObject ShopTab;
    public GameObject ClickBuyTab;

    //GameObject
    public GameObject RailInfo_Tab;

    public Image railStone_Image;
    public TextMeshProUGUI stone_Level_Text;
    public TextMeshProUGUI spawnTime_Text;
    public TextMeshProUGUI value_Text;
    public TextMeshProUGUI ore_Health_Text;
    public TextMeshProUGUI possessedOre_Text;
    //Drill
    public TextMeshProUGUI damage_Upgrade_Cost;
    public TextMeshProUGUI attackSpeed_Upgrade_Cost;
    public TextMeshProUGUI luck_Upgrade_Cost;
    public TextMeshProUGUI Damage_Text;
    public TextMeshProUGUI AttackSpeed_Text;
    public TextMeshProUGUI Luck_Text;


    //Upgrade Buttons
    public Button DamageUpgradeButton;
    public Button AttackSpeedUpgradeButton;
    public Button LuckUpgradeButton;

    //Image for Ore Upgrade
    public Image DamageupgradeOreImage;
    public Image AttackSpeedUpgradeOreImage;
    public Image LuckUpgradeOreImage;


    [Header("#-----Iron Work Tab")]
    public Image unMinedStone_Image;
    public Image unMinedStoneButton1_Image;
    public Image unMinedStoneButton2_Image;
    public Image unMinedStoneButton3_Image;
    public Image oreButton1_Image;
    [Space(5)]
    public TextMeshProUGUI unMinedStone1_AmountText;
    public TextMeshProUGUI unMinedStone2_AmountText;
    public TextMeshProUGUI unMinedStone3_AmountText;

    [Space(5)]
    public Image CheckBox1;
    public Image CheckBox2;
    public Image CheckBox3;
    [Space(5)]
    public Sprite goldSprite;
    public Sprite partsSprite;

    [Space(5)]
    public Image tradingOreImage;
    public Image resultImage;

    [Space(5)]
    public TextMeshProUGUI tradingOreText;
    public TextMeshProUGUI resultAmountText;
    public TextMeshProUGUI unMinedOreText;

    [Space(5)]
    public GameObject chooseAmountTab;
    public Slider tradeAmountSlider;
    public TextMeshProUGUI MinText;
    public TextMeshProUGUI MaxText;
    public TextMeshProUGUI tradingAmount_Text;
    public Image TradingItem_Image;

    [Space(5)]
    public Button UnminedSliderButton;
    public Button MAX_Button;
    public Button addOne_Button;
    public Button addTen_Button;
    public Button addHundred_Button;

    private int orePerItem;
    private int tradeCost;
    private int tradingAmount;

    [Header("ExchangeButtonandTab")]
    public Button exchangeButton;
    public GameObject ExchangeTab;

    public GameObject ErrorTab;

    private Vector3 dragOrigin;
    private Camera mainCamera;

    [Header("Auto Exchange Tab")]
    public bool autoExchangerBought;
    public GameObject buyAutoProductTab;
    public GameObject AutoExchangeTab;
    public Transform railButton_Parent;
    public GameObject railButton_pfb;
    public List<GameObject> railButtonsData;

    public bool autoExchange_PureOre;
    public bool autoExchange_Gold;
    public bool autoExchange_SpaceStone;
    public Image CheckBox1_autoExchange;
    public Image CheckBox2_autoExchange;
    public Image CheckBox3_autoExchange;

    public Transform selectedRailGUI_Parent;
    public GameObject selectRailGUI_pfb;

    [Header("Select Rail Tab")]
    public GameObject SelectRailTab;
    public List<bool> railAutoExchangeCondition;

    [Header("Scroll")]
    [Space(5)]
    public float dragSpeed = 2.0f;
    public float minY = 0f; //아래로 카메라 이동 범위 확장
    public float maxY = 0f; //위로 카메라 이동 범위 확장

    float railFrameTopY;
    float railFrameBottomY;

    public float intervalY; //간격만큼 카메라 확장
    int countToScroll;

    public bool isInMine = false;
    public bool moveCamera = false;

    public Rail selectedRail;

    private int currentTradeIndex;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        mainCamera = Camera.main;
        railFrameTopY = mainCamera.ScreenToWorldPoint(firstRailLoc.position).y;
        railFrameBottomY = mainCamera.ScreenToWorldPoint(bottomRailLoc.position).y;

        InvokeRepeating("changeGUI", 0, 0.03f);
    }

    void Update()
    {
        if(autoExchangerBought) {
            if(railAutoExchangeCondition.Count > 0) {
                autoExchangeItems();
            }
        }

        if(isInMine) {
            //Camera Move
            if(moveCamera && !StoneBuyTab.activeSelf && !AutoExchangeTab.activeSelf && !ShopTab.activeSelf && !ClickBuyTab.activeSelf) {
                if(Input.GetMouseButtonDown(0)) {
                    dragOrigin = Input.mousePosition;
                    return;
                }

                if(!Input.GetMouseButton(0)) return;

                Vector3 pos = mainCamera.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
                Vector3 move = new Vector3(0, -pos.y * dragSpeed, 0);

                Vector3 newPosition = mainCamera.transform.position + move;
                newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
                mainCamera.transform.position = newPosition;
            }
        }    
    }

    public void changeGUI()
    {
        //UpgradeTab
        if(selectedRail != null && RailInfo_Tab.activeSelf) {

            DamageUpgradeButton.interactable = selectedRail.possessedOre >= selectedRail.getTotalPrice(1);
            AttackSpeedUpgradeButton.interactable = selectedRail.possessedOre >= selectedRail.getTotalPrice(2) && selectedRail.attackSpeedLevel < 150;
            LuckUpgradeButton.interactable = selectedRail.possessedOre >= selectedRail.getTotalPrice(3);

            setRailInfoTabData();

            //UnMined Ore Text
            unMinedOreText.text = GameManager.instance.moneyStringForMine(selectedRail.unMinedOre);

            if(currentTradeIndex == 1 || currentTradeIndex == 3) {
                MAX_Button.interactable = selectedRail.unMinedOre >= orePerItem;
                exchangeButton.interactable = selectedRail.unMinedOre >= orePerItem && tradingAmount > 0;
                UnminedSliderButton.interactable = selectedRail.unMinedOre >= orePerItem;
                addOne_Button.interactable = selectedRail.unMinedOre >= ((tradingAmount + 1) * orePerItem);
                addTen_Button.interactable = selectedRail.unMinedOre >= ((tradingAmount + 10) * orePerItem);
                addHundred_Button.interactable = selectedRail.unMinedOre >= ((tradingAmount + 100) * orePerItem);
            } else {
                MAX_Button.interactable = selectedRail.unMinedOre >= 1;
                exchangeButton.interactable = selectedRail.unMinedOre >= 1 && tradingAmount > 0;
                UnminedSliderButton.interactable = selectedRail.unMinedOre >= 1;
                addOne_Button.interactable = selectedRail.unMinedOre >= tradingAmount + 1;
                addTen_Button.interactable = selectedRail.unMinedOre >= tradingAmount + 10;
                addHundred_Button.interactable = selectedRail.unMinedOre >= tradingAmount + 100;
            }

            if(chooseAmountTab.activeSelf) {
                tradeAmountSlider.minValue = 1;
                tradeAmountSlider.maxValue = currentTradeIndex == 1 || currentTradeIndex == 3 ? selectedRail.unMinedOre / orePerItem : selectedRail.unMinedOre;

                if(currentTradeIndex == 2) {
                    MinText.text = (tradeAmountSlider.minValue).ToString();
                    MaxText.text = (selectedRail.unMinedOre).ToString();
                } else {
                    MinText.text = (tradeAmountSlider.minValue * orePerItem).ToString();
                    MaxText.text = (Mathf.FloorToInt(selectedRail.unMinedOre / orePerItem) * orePerItem).ToString();
                }
            }
        }

        //Auto Exchange
        CheckBox1_autoExchange.color = autoExchange_PureOre ? new Color(1, 1, 1, 1) : new Color(0, 0, 0, 0.4f);
        CheckBox2_autoExchange.color = autoExchange_Gold ? new Color(1, 1, 1, 1) : new Color(0, 0, 0, 0.4f);
        CheckBox3_autoExchange.color = autoExchange_SpaceStone ? new Color(1, 1, 1, 1) : new Color(0, 0, 0, 0.4f);
    }

    public Rail activateRail(railShop data)
    {
        Rail _r = Instantiate(rail_pfb, railParent);
        _r.shop = data;
        data.railUpgradeButton.rail = _r;
        _r.stonePrefab = data.stone;
        _r.transform.position = new Vector3(0, railFrameTopY - 1.0f - (intervalY * currentRails.Count), 0);
        if(railFrameTopY - (intervalY * (currentRails.Count+1)) < railFrameBottomY) {
            countToScroll++;
            MoveCameraInitialize(countToScroll);
        }
        currentRails.Add(_r);
        railAutoExchangeCondition.Add(true);
        SetautoExchangeTab();
        setSelectedRailGUI();
        data.railUpgradeButton.gameObject.SetActive(true);

        return _r;
    }

    public void MoveCameraInitialize(int count)
    {
        moveCamera = true;

        minY = -intervalY * count - 1;
    }

    public void openRailInfoTab(Rail data)
    {
        selectedRail = data;
        tabGroup.SelectTabbyIndex(tabButton);
        changeGUI();
        RailInfo_Tab.SetActive(true);
    }

    public void setRailInfoTabData()
    {
        //Image
        railStone_Image.sprite = selectedRail.stonePrefab.StoneSprite;

        //Stats Info
        stone_Level_Text.text = "Lv." + selectedRail.shop.ShopLevel.ToString();
        spawnTime_Text.text = selectedRail.shop.getSpawnTime().ToString("F2") + "s";
        value_Text.text = selectedRail.shop.getStoneValue().ToString("N0");
        ore_Health_Text.text = string.Format("{0:#,###0}", selectedRail.getStoneHealth());
        Damage_Text.text = selectedRail.drill.damage.ToString();
        AttackSpeed_Text.text = (1.0f / selectedRail.drill.attackSpeed).ToString("F2");
        Luck_Text.text = "x" + (1 + selectedRail.drill.luck).ToString("F2");
        possessedOre_Text.text = GameManager.instance.moneyStringForMine(selectedRail.possessedOre);

        //GUI
        DamageupgradeOreImage.sprite = selectedRail.stonePrefab.StoneSprite;
        AttackSpeedUpgradeOreImage.sprite = selectedRail.stonePrefab.StoneSprite;
        LuckUpgradeOreImage.sprite = selectedRail.stonePrefab.StoneSprite;

        //Upgrade Text
        damage_Upgrade_Cost.text = GameManager.instance.moneyStringForMine(selectedRail.getTotalPrice(1));

        if(selectedRail.attackSpeedLevel >= 150)
            attackSpeed_Upgrade_Cost.text = "MAX";
        else
            attackSpeed_Upgrade_Cost.text = GameManager.instance.moneyStringForMine(selectedRail.getTotalPrice(2));
        luck_Upgrade_Cost.text = GameManager.instance.moneyStringForMine(selectedRail.getTotalPrice(3));
    }

    public void UpgradeRail()
    {
        selectedRail.shop.Upgrade();
    }

    public void UpgradeDamage()
    {
        selectedRail.UpgradeDrillDamage();
    }
    public void UpgradeAS()
    {
        selectedRail.UpgradeDrillSpeed();
    }
    public void UpgradeLuck()
    {
        selectedRail.UpgradeDrillLuck();
    }

    //Iron Work
    public void setIronWorkTab()
    {
        unMinedStone_Image.sprite = selectedRail.stonePrefab.StoneSprite;
        unMinedStoneButton1_Image.sprite = selectedRail.stonePrefab.StoneSprite;
        unMinedStoneButton2_Image.sprite = selectedRail.stonePrefab.StoneSprite;
        unMinedStoneButton3_Image.sprite = selectedRail.stonePrefab.StoneSprite;
        oreButton1_Image.sprite = selectedRail.stonePrefab.StoneSprite;

        unMinedStone1_AmountText.text = selectedRail.getTradingAmountForUnminedOre(1).ToString();
        unMinedStone2_AmountText.text = selectedRail.getTradingAmountForUnminedOre(2).ToString("N0");
        unMinedStone3_AmountText.text = selectedRail.getTradingAmountForUnminedOre(3).ToString();

        //Result
        tradingOreImage.sprite = selectedRail.stonePrefab.StoneSprite;
        resultImage.sprite = selectedRail.stonePrefab.StoneSprite;

        tradingAmount = 0;

        tradingOreText.text = "수량 선택";
        resultAmountText.text = "0";
    }

    public void setTradeAmount()
    {
        if(currentTradeIndex == 2) {
            if(selectedRail.unMinedOre >= 0) {
                chooseAmountTab.SetActive(true);
                TradingItem_Image.sprite = selectedRail.stonePrefab.StoneSprite;
                tradingAmount_Text.text = (tradeAmountSlider.value).ToString();
            } else {
                ErrorTab.SetActive(true);
            }
        } else {
            if(selectedRail.unMinedOre >= orePerItem) {
                chooseAmountTab.SetActive(true);
                TradingItem_Image.sprite = selectedRail.stonePrefab.StoneSprite;
                tradingAmount_Text.text = (tradeAmountSlider.value).ToString();
            } else {
                ErrorTab.SetActive(true);
            }
        }
    }

    public void onValueChangeSlider()
    {
        if(currentTradeIndex == 2) {
            tradingAmount_Text.text = (tradeAmountSlider.value).ToString();
        } else 
            tradingAmount_Text.text = (tradeAmountSlider.value * orePerItem).ToString();
    }

    public void addTradingAmount(int n)
    {
        if(currentTradeIndex == 1 || currentTradeIndex == 3) {
            if(n == 0) {
                tradingAmount = (int)(selectedRail.unMinedOre / orePerItem);
                setResultText();
                return;
            }
            if(n == -1) {
                tradingAmount = 0;
                setResultText();
                return;
            }
            tradingAmount += n;
            setResultText();
        } else {
            if(n == 0) {
                tradingAmount = (int)selectedRail.unMinedOre;
                setResultText();
                return;
            }
            if(n == -1) {
                tradingAmount = 0;
                setResultText();
                return;
            }
            tradingAmount += n;
            setResultText();
        }
    }

    public void setResultText()
    {
        if(currentTradeIndex == 2) {
            tradingOreText.text = tradingAmount != 0 ? (tradingAmount).ToString() : "수량 선택";
            resultAmountText.text = (tradingAmount * orePerItem).ToString();
        } else {
            tradingOreText.text = tradingAmount != 0 ? (tradingAmount * orePerItem).ToString() : "수량 선택";
            resultAmountText.text = (tradingAmount).ToString();
        }
    }

    public void setSliderValue()
    {
        chooseAmountTab.SetActive(false);

        tradingAmount = (int)tradeAmountSlider.value;
        setResultText();
    }


    public void setCatergoryGUI(int n)
    {
        currentTradeIndex = n;
        //Default Values
        setIronWorkTab();


        CheckBox1.color = currentTradeIndex == 1 ? new Color(1, 1, 1, 1) : new Color(0, 0, 0, 0.4f);
        CheckBox2.color = currentTradeIndex == 2 ? new Color(1, 1, 1, 1) : new Color(0, 0, 0, 0.4f);
        CheckBox3.color = currentTradeIndex == 3 ? new Color(1, 1, 1, 1) : new Color(0, 0, 0, 0.4f);

        if(currentTradeIndex == 1) { 
            resultImage.sprite = selectedRail.stonePrefab.StoneSprite;
            resultImage.rectTransform.sizeDelta = new Vector2(50, 50);
        } else if(currentTradeIndex == 2) {
            resultImage.sprite = goldSprite;
            resultImage.rectTransform.sizeDelta = new Vector2(50, 50);
        } else if(currentTradeIndex == 3) {
            resultImage.sprite = partsSprite;
            resultImage.rectTransform.sizeDelta = new Vector2(35, 45);
        }
        

        orePerItem = selectedRail.getTradingAmountForUnminedOre(currentTradeIndex);
        addTradingAmount(-1);
    }

    //Exchange Tab
    public void openExchangeTab()
    {
        if(GameManager.GetOre() >= tradeCost * tradingAmount) {
            ExchangeTab.SetActive(true);
        } else {
            ErrorTab.SetActive(true);
        }
    }

    public void exchangeItem()
    {
        //Achievement
        AchievementManager.instance.IronworkTradeDaily++;

        selectedRail.unMinedOre -= currentTradeIndex == 2 ? tradingAmount : tradingAmount * orePerItem;
        switch(currentTradeIndex) {
            case 1:
                selectedRail.possessedOre += tradingAmount;
                break;
            case 2:
                GameManager.SetMoney(tradingAmount * orePerItem);
                break;
            case 3:
                GameManager.SetParts(tradingAmount);
                break;
        }

        setIronWorkTab();
    }

    public bool getLockTabCondition(railShop data)
    {
        if(shops.IndexOf(data) != 0)
            return shops[shops.IndexOf(data) - 1].ShopLevel < 30;
        else
            return false;
    }

    public void checkShopInfo()
    {
        for(int i = 0; i < shops.Count; i++) {
            shops[i].SetShopInfo();
        }
    }

    public void SetautoExchangeTab() {
        for(int i = 0; i < railButton_Parent.childCount; i++) {
            Destroy(railButton_Parent.GetChild(i).gameObject);
        }
        railButtonsData.Clear();
        for(int i = 0; i < currentRails.Count; i ++) {
            GameObject _g = Instantiate(railButton_pfb, railButton_Parent);
            _g.transform.GetChild(0).GetComponent<Image>().sprite = currentRails[i].stonePrefab.StoneSprite;
            railButtonsData.Add(_g);
        }
    }

    public void openAutoExchangeTab() {
        AutoExchangeTab.SetActive(true);
        buyAutoProductTab.SetActive(!autoExchangerBought);
    }

    public void toggleAutoExchange(int index) {
        switch(index){
            case 1:
                autoExchange_PureOre = !autoExchange_PureOre;
                autoExchange_Gold = false;
                autoExchange_SpaceStone = false;
                break;
            case 2:
                autoExchange_PureOre = false;
                autoExchange_Gold = !autoExchange_Gold;
                autoExchange_SpaceStone = false;
                break;
            case 3:
                autoExchange_PureOre = false;
                autoExchange_Gold = false;
                autoExchange_SpaceStone = !autoExchange_SpaceStone;
                break;
        }
    }

    public void changeSelectedRail() {
        SelectRailTab.SetActive(true);
        SetautoExchangeTab();
        for(int i = 0; i < railAutoExchangeCondition.Count; i++) {
            if(railAutoExchangeCondition[i]) {
                railButtonsData[i].GetComponent<railButton>().frame.sprite = railButtonsData[i].GetComponent<railButton>().twoFrame[1];
            } else {
                railButtonsData[i].GetComponent<railButton>().frame.sprite = railButtonsData[i].GetComponent<railButton>().twoFrame[0];
            }
        }
    }

    public void selectAllRail() {
        for(int i = 0; i < railButtonsData.Count; i++) {
            railButtonsData[i].GetComponent<railButton>().frame.sprite = railButtonsData[i].GetComponent<railButton>().twoFrame[1];
        }
    }
    public void unselectAllRail() {
        for(int i = 0; i < railButtonsData.Count; i++) {
            railButtonsData[i].GetComponent<railButton>().frame.sprite = railButtonsData[i].GetComponent<railButton>().twoFrame[0];
        }
    }

    public void applyCondition() {
        railAutoExchangeCondition.Clear();
        for(int i = 0; i < railButtonsData.Count; i++) {
            railAutoExchangeCondition.Add(railButtonsData[i].GetComponent<railButton>().frame.sprite == railButtonsData[i].GetComponent<railButton>().twoFrame[1]);
        }

        setSelectedRailGUI();
    }

    public void setSelectedRailGUI() {
        for(int i = 0; i < selectedRailGUI_Parent.childCount; i++) {
            Destroy(selectedRailGUI_Parent.GetChild(i).gameObject);
        }
        for(int i = 0; i < railAutoExchangeCondition.Count; i++) {
            if(railAutoExchangeCondition[i]) {
                GameObject _g = Instantiate(selectRailGUI_pfb, selectedRailGUI_Parent);
                _g.GetComponent<Image>().sprite = railButtonsData[i].GetComponent<railButton>().railButton_oreImage.sprite;
            }
        }
    }

    public void autoExchangeItems() {
        int tradeAmount = 0;
        int oreItem = 0;
        for(int i = 0; i < currentRails.Count; i++) {
            if(autoExchange_PureOre && railAutoExchangeCondition[i]) {
                oreItem = currentRails[i].getTradingAmountForUnminedOre(1);
                tradeAmount = (int)(currentRails[i].unMinedOre / oreItem);
                currentRails[i].unMinedOre -= tradeAmount * oreItem;
                currentRails[i].possessedOre += tradeAmount;
            }

            if(autoExchange_Gold && railAutoExchangeCondition[i]) {
                oreItem = currentRails[i].getTradingAmountForUnminedOre(2);
                tradeAmount = (int)currentRails[i].unMinedOre;
                currentRails[i].unMinedOre -= tradeAmount;
                GameManager.SetMoney(tradeAmount * oreItem);
            }

            if(autoExchange_SpaceStone && railAutoExchangeCondition[i]) {
                oreItem = currentRails[i].getTradingAmountForUnminedOre(3);
                tradeAmount = (int)(currentRails[i].unMinedOre / oreItem);
                currentRails[i].unMinedOre -= tradeAmount * oreItem;
                GameManager.SetParts(tradeAmount);
            }
        }
    }

    public void autoExchangeBought() {
        autoExchangerBought = true;
        SetautoExchangeTab();
        openAutoExchangeTab();
        selectAllRail();
        applyCondition();
        setSelectedRailGUI();
        toggleAutoExchange(1);
    }



    public void LoadData(GameData data)
    {
        autoExchangerBought = data.autoExchangerBought;

        for(int i = 0; i < shops.Count; i++) {
            data.railShopLevel.TryGetValue(shops[i].id, out int shoplevel);
            shops[i].ShopLevel = shoplevel;

            if(shoplevel > 0) {
                Rail r = activateRail(shops[i]);

                //Mine Stone Amount
                data.minedStoneAmount.TryGetValue(shops[i].id, out long value);
                r.minedStoneAmount = value;

                //Possessed Amount
                data.possessedOre.TryGetValue(shops[i].id, out long value2);
                r.possessedOre = value2;

                //Unmined Ore
                data.unminedOre.TryGetValue(shops[i].id, out long data1);
                r.unMinedOre = data1;

                //Damage Level
                data.railDrill_DamageLevel.TryGetValue(shops[i].id, out int value3);
                r.damageLevel = value3;

                //AS Level
                data.railDrill_AttackSpeedLevel.TryGetValue(shops[i].id, out int value4);
                if(value4 > 150) {
                    value4 = 150;
                }
                r.attackSpeedLevel = value4;

                //Luck Level
                data.railDrill_luckLevel.TryGetValue(shops[i].id, out int value5);
                r.luckLevel = value5;
            }
        }
        if(data.autoExchangerBought) {
            SetautoExchangeTab();

            if(data.firstTimeGame) {
                selectAllRail();
                applyCondition();
                data.firstTimeGame = false;
            } else {
                for(int i = 0; i < railButtonsData.Count; i++) {
                    data.autoSelectedRailBool.TryGetValue("autorail" + i, out bool value);
                    railAutoExchangeCondition[i] = value;
                }
            }
            setSelectedRailGUI();
            toggleAutoExchange(data.autoExchangeIndexBool);
        }
    }
    public void SaveData(GameData data) {

        data.autoExchangerBought = autoExchangerBought;


        for(int i = 0; i < currentRails.Count; i++) {
            //ShopLevel
            if(data.railShopLevel.ContainsKey(currentRails[i].shop.id))
                data.railShopLevel.Remove(currentRails[i].shop.id);

            data.railShopLevel.Add(currentRails[i].shop.id, currentRails[i].shop.ShopLevel);

            //minedStoneAmount
            if(data.minedStoneAmount.ContainsKey(currentRails[i].shop.id))
                data.minedStoneAmount.Remove(currentRails[i].shop.id);

            data.minedStoneAmount.Add(currentRails[i].shop.id, currentRails[i].minedStoneAmount);

            //Possessed Ore
            if(data.possessedOre.ContainsKey(currentRails[i].shop.id))
                data.possessedOre.Remove(currentRails[i].shop.id);

            data.possessedOre.Add(currentRails[i].shop.id, currentRails[i].possessedOre);

            //Unmined Ore
            if(data.unminedOre.ContainsKey(currentRails[i].shop.id))
                data.unminedOre.Remove(currentRails[i].shop.id);

            data.unminedOre.Add(currentRails[i].shop.id, currentRails[i].unMinedOre);

            //DamagLevel
            if(data.railDrill_DamageLevel.ContainsKey(currentRails[i].shop.id))
                data.railDrill_DamageLevel.Remove(currentRails[i].shop.id);

            data.railDrill_DamageLevel.Add(currentRails[i].shop.id, currentRails[i].damageLevel);

            //ASLevel
            if(data.railDrill_AttackSpeedLevel.ContainsKey(currentRails[i].shop.id))
                data.railDrill_AttackSpeedLevel.Remove(currentRails[i].shop.id);

            data.railDrill_AttackSpeedLevel.Add(currentRails[i].shop.id, currentRails[i].attackSpeedLevel);

            //LuckLevel
            if(data.railDrill_luckLevel.ContainsKey(currentRails[i].shop.id))
                data.railDrill_luckLevel.Remove(currentRails[i].shop.id);

            data.railDrill_luckLevel.Add(currentRails[i].shop.id, currentRails[i].luckLevel);
        }

        if(data.autoExchangerBought) {
            if(autoExchange_PureOre)
                data.autoExchangeIndexBool = 1;
            else if(autoExchange_Gold)
                data.autoExchangeIndexBool = 2;
            else if(autoExchange_SpaceStone)
                data.autoExchangeIndexBool = 3;

            for(int i = 0; i < railAutoExchangeCondition.Count; i++) {

                if(data.autoSelectedRailBool.ContainsKey("autorail" + i)) {
                    data.autoSelectedRailBool.Remove("autorail" + i);
                }

                data.autoSelectedRailBool.Add("autorail" + i, railAutoExchangeCondition[i]);
            }

            data.firstTimeGame = false;
        }
    }
}
