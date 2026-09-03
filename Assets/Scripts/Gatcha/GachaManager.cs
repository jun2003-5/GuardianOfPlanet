using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using TMPro;

public class GachaManager : MonoBehaviour, IDataPersistence
{
    public static GachaManager Instance;

    [Header("Gacha Animation UI")]
    public GameObject GachaScreen;
    public Transform GachaBox_Transform;
    public Image GachaBox_Image;
    public Sprite[] GachaBox_Sprites;

    public Transform GachaBox_Glow;
    public GameObject gloweffects;

    [Header("Gacha Popup")]
    public GameObject popUp_Chest;
    public Image ChestImage;
    public TextMeshProUGUI chestName_Text;
    GachaData currentGacha;

    [Header("Gacha Result Equip Frame")]
    public GameObject EquipShowTab;
    public EquipingData Frame_EquipResult;
    public GameObject[] Equip_Stats;
    public TextMeshProUGUI EquipResult_Name;
    public TextMeshProUGUI EquipResult_Grade;
    public Slider EquipAmountSlider;
    public TextMeshProUGUI EquipAmountSlider_Text;
    public Image LightEffect;
    public Image LightEffect2;

    [Header("Last Result Tab")]
    public List<EquipingData> resultEquipsData;
    public GameObject Result_Panel;
    public Button EndButton;
    public GameObject TouchScreenText;
    public CanvasGroup ResultEquipGroup;

    [Header("Last Result Tab (Stone)")]
    public List<UpgradeStoneCard> resultStonesData;
    public CanvasGroup ResultStoneGroup;

    [Header("Gauge Bar")]
    int NormalGachaCount;
    int SpecialGachaCount;
    int UpgradeStoneGachaCount;
    [Header("Gauge Bar")]
    public EXPBar NormalSlider;
    public EXPBar SpecialSlider;
    public EXPBar StoneSlider;

    [Header("Ticket")]
    public int NormalGachaTicket;
    public int SpecialGachaTicket;
    public int StoneGachaTicket;

    public GameObject AlertAll;

    public GameObject[] NormalButton;
    public TextMeshProUGUI NormalButton_Text;
    public GameObject NormalAlert;

    public GameObject[] Normalx10Button;
    public TextMeshProUGUI Normalx10Button_Text;

    public GameObject[] SpecialButton;
    public TextMeshProUGUI SpecialButton_Text;
    public GameObject SpecialAlert;

    public GameObject[] Specialx10Button;
    public TextMeshProUGUI Specialx10Button_Text;

    public GameObject[] StoneButton;
    public TextMeshProUGUI StoneButton_Text;
    public GameObject StoneAlert;

    public GameObject[] Stonex10Button;
    public TextMeshProUGUI Stonex10Button_Text;

    [Header("Error Tab")]
    public GameObject ErrorTab;

    Equips.Type randomType;
    //Earned Equip
    public List<EquipFrame> RandomEquip;

    public List<RandomStone> randomStones;
    public int[] StoneNumber;

    int gachaCount;
    bool isEquipGacha;
    float ShakeTime;
    float ShakeScale;
    Equips.MaterialClass m_Class;
    UpgradeStone.TypeOfStone u_Class;
    Color anim_Color;
    bool isFirst;

    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        SetSliderValue();
        InvokeRepeating("SetTicket", 0, 0.5f);
    }
    //One 확정
    public void gachaOneMust(int RingAmount, int NecklaceAmount, int RelicsAmount, int AccessoryAmount, int BookAmount, Equips.MaterialClass minType, Equips.MaterialClass MaxType, Equips.MaterialClass confirmedType, bool moreChance, int boxType)
    {
        //설정
        DOTween.KillAll();
        StopAllCoroutines();

        //랜덤 장비 가지고오기
        if(randomStones != null)
            randomStones.Clear();

        if(RandomEquip != null)
            RandomEquip.Clear();

        int count = RingAmount + NecklaceAmount + RelicsAmount + AccessoryAmount + BookAmount;
        bool confimredCompleted = false;

        gachaCount = count;
        isEquipGacha = true;

        //Box
        GachaBox_Image.sprite = GachaBox_Sprites[boxType];

        //Acheivement
        AchievementManager.instance.GachaDaily += count;
        AchievementManager.instance.GachaWeekly += count;
        AchievementManager.instance.GachaTimeQuest += count;        

        //Ring
        for(int i = 0; i < RingAmount; i++) {
            Equips.MaterialClass equipGrade = minType;

            float random = Random.Range(0.0f, 100.0f);

            if(random <= (moreChance ? 0.5f : 0.25f)) {
                equipGrade = Equips.MaterialClass.Ancient;
            } else if(random <= (moreChance ? 3.75f : 3f)) {
                equipGrade = Equips.MaterialClass.Legendary;
            } else if(random <= (moreChance ? 24.5f : 22f)) {
                equipGrade = Equips.MaterialClass.Unique;
            } else {
                equipGrade = Equips.MaterialClass.Epic;
            }

            if(equipGrade < minType) {
                equipGrade = minType;
            }

            if(equipGrade > MaxType) {
                equipGrade = minType;
            }

            float randomConfirmed = Random.Range(0.0f, 100.0f);

            if(randomConfirmed < (float)RingAmount / (float)count && !confimredCompleted) {
                equipGrade = confirmedType;
                confimredCompleted = true;
            }

            RandomEquip.Add(EquipManager.Instance.getRandomItem_Grade(equipGrade, Equips.Type.Ring));
        }

        //Necklace
        for(int i = 0; i < NecklaceAmount; i++) {
            Equips.MaterialClass equipGrade = minType;

            float random = Random.Range(0.0f, 100.0f);

            if(random <= (moreChance ? 0.5f : 0.25f)) {
                equipGrade = Equips.MaterialClass.Ancient;
            } else if(random <= (moreChance ? 3.75f : 3f)) {
                equipGrade = Equips.MaterialClass.Legendary;
            } else if(random <= (moreChance ? 24.5f : 22f)) {
                equipGrade = Equips.MaterialClass.Unique;
            } else {
                equipGrade = Equips.MaterialClass.Epic;
            }

            if(equipGrade < minType) {
                equipGrade = minType;
            }

            if(equipGrade > MaxType) {
                equipGrade = minType;
            }

            float randomConfirmed = Random.Range(0.0f, 100.0f);

            if(randomConfirmed < (float)(NecklaceAmount + RingAmount) / (float)count && !confimredCompleted) {
                equipGrade = confirmedType;
                confimredCompleted = true;
            }

            RandomEquip.Add(EquipManager.Instance.getRandomItem_Grade(equipGrade, Equips.Type.Necklace));
        }

        //Relics
        for(int i = 0; i < RelicsAmount; i++) {
            Equips.MaterialClass equipGrade = minType;

            float random = Random.Range(0.0f, 100.0f);

            if(random <= (moreChance ? 0.5f : 0.25f)) {
                equipGrade = Equips.MaterialClass.Ancient;
            } else if(random <= (moreChance ? 3.75f : 3f)) {
                equipGrade = Equips.MaterialClass.Legendary;
            } else if(random <= (moreChance ? 24.5f : 22f)) {
                equipGrade = Equips.MaterialClass.Unique;
            } else {
                equipGrade = Equips.MaterialClass.Epic;
            }

            if(equipGrade < minType)
                equipGrade = minType;

            if(equipGrade > MaxType)
                equipGrade = minType;

            float randomConfirmed = Random.Range(0.0f, 100.0f);

            if(randomConfirmed < (float)(RelicsAmount + NecklaceAmount + RingAmount) / (float)count && !confimredCompleted) {
                equipGrade = confirmedType;
                confimredCompleted = true;
            }

            RandomEquip.Add(EquipManager.Instance.getRandomItem_Grade(equipGrade, Equips.Type.Relics));
        }

        //Accessory
        for(int i = 0; i < AccessoryAmount; i++) {
            Equips.MaterialClass equipGrade = minType;

            float random = Random.Range(0.0f, 100.0f);

            if(random <= (moreChance ? 0.5f : 0.25f)) {
                equipGrade = Equips.MaterialClass.Ancient;
            } else if(random <= (moreChance ? 3.75f : 3f)) {
                equipGrade = Equips.MaterialClass.Legendary;
            } else if(random <= (moreChance ? 24.5f : 22f)) {
                equipGrade = Equips.MaterialClass.Unique;
            } else {
                equipGrade = Equips.MaterialClass.Epic;
            }

            if(equipGrade < minType)
                equipGrade = minType;

            if(equipGrade > MaxType)
                equipGrade = minType;

            if(!confimredCompleted) {
                equipGrade = confirmedType;
                confimredCompleted = true;
            }

            RandomEquip.Add(EquipManager.Instance.getRandomItem_Grade(equipGrade, Equips.Type.Accessory));
        }

        //Book
        for(int i = 0; i < BookAmount; i++) {
            Equips.MaterialClass equipGrade = minType;

            float random = Random.Range(0.0f, 100.0f);

            if(random <= (moreChance ? 0.5f : 0.25f)) {
                equipGrade = Equips.MaterialClass.Ancient;
            } else if(random <= (moreChance ? 3.75f : 3f)) {
                equipGrade = Equips.MaterialClass.Legendary;
            } else if(random <= (moreChance ? 27.5f : 25f)) {
                equipGrade = Equips.MaterialClass.Unique;
            } else {
                equipGrade = Equips.MaterialClass.Epic;
            }

            if(equipGrade < minType)
                equipGrade = minType;

            if(equipGrade > MaxType)
                equipGrade = minType;

            RandomEquip.Add(EquipManager.Instance.getRandomItem_Grade(equipGrade, Equips.Type.Book));
        }

        m_Class = Equips.MaterialClass.Normal;

        for(int i = 0; i < RandomEquip.Count; i++) {
            if(((int)RandomEquip[i].equipData.Grade) >= ((int)m_Class)) {
                m_Class = RandomEquip[i].equipData.Grade;
            }
        }

        SetSliderValue();

        for(int i = 0; i < RandomEquip.Count; i++) {
            RandomEquip[i].equipData.AmountOfEquip++;
        }
        for(int i = 0; i < resultEquipsData.Count; i++) {
            resultEquipsData[i].gameObject.SetActive(false);
        }

        for(int i = 1; i <= gachaCount; i++) {
            resultEquipsData[i - 1].gameObject.SetActive(true);
            resultEquipsData[i - 1].SetEquipment(RandomEquip[gachaCount - i]);
            resultEquipsData[i - 1].transform.GetComponentInChildren<Slider>().maxValue = RandomEquip[gachaCount - i].equipData.RequiredAmountForMerge;
            resultEquipsData[i - 1].transform.GetComponentInChildren<Slider>().value = RandomEquip[gachaCount - i].equipData.AmountOfEquip;
            resultEquipsData[i - 1].transform.GetComponentInChildren<Slider>().gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = RandomEquip[gachaCount - i].equipData.AmountOfEquip + "/" + resultEquipsData[i - 1].transform.GetComponentInChildren<Slider>().maxValue;
        }


        StartAnimation();
    }

    public void gachaOneMust(int totalCount, Equips.MaterialClass minType, Equips.MaterialClass MaxType, Equips.MaterialClass confirmedType, bool moreChance, int boxType)
    {
        //설정
        DOTween.KillAll();
        StopAllCoroutines();

        //랜덤 장비 가지고오기
        if(randomStones != null)
            randomStones.Clear();

        if(RandomEquip != null)
            RandomEquip.Clear();

        int count = totalCount;

        gachaCount = count;

        isEquipGacha = true;

        //Box
        GachaBox_Image.sprite = GachaBox_Sprites[boxType];

        //Acheivement
        AchievementManager.instance.GachaDaily += count;
        AchievementManager.instance.GachaWeekly += count;
        AchievementManager.instance.GachaTimeQuest += count;

        //Ring
        for(int i = 0; i < count; i++)
        {
            randomType = getRandomType();

            Equips.MaterialClass equipGrade = minType;

            float random = Random.Range(0.0f, 100.0f);

            if(random <= (moreChance ? 0.5f : 0.25f))
            {
                equipGrade = Equips.MaterialClass.Legendary;
            } else if(random <= (moreChance ? 2.75f : 2f))
            {
                equipGrade = Equips.MaterialClass.Legendary;
            } else if(random <= (moreChance ? 24.5f : 22f))
            {
                equipGrade = Equips.MaterialClass.Unique;
            } else
            {
                equipGrade = Equips.MaterialClass.Epic;
            }

            if(equipGrade < minType)
            {
                equipGrade = minType;
            }

            if(equipGrade > MaxType)
            {
                equipGrade = minType;
            }

            if(i == 0)
            {
                equipGrade = confirmedType;
            }

            RandomEquip.Add(EquipManager.Instance.getRandomItem_Grade(equipGrade, randomType));
        }       

        m_Class = Equips.MaterialClass.Normal;

        for(int i = 0; i < RandomEquip.Count; i++)
        {
            if(((int)RandomEquip[i].equipData.Grade) >= ((int)m_Class))
            {
                m_Class = RandomEquip[i].equipData.Grade;
            }
        }

        SetSliderValue();

        for(int i = 0; i < RandomEquip.Count; i++)
        {
            RandomEquip[i].equipData.AmountOfEquip++;
        }
        for(int i = 0; i < resultEquipsData.Count; i++)
        {
            resultEquipsData[i].gameObject.SetActive(false);
        }

        for(int i = 1; i <= gachaCount; i++)
        {
            resultEquipsData[i - 1].gameObject.SetActive(true);
            resultEquipsData[i - 1].SetEquipment(RandomEquip[gachaCount - i]);
            resultEquipsData[i - 1].transform.GetComponentInChildren<Slider>().maxValue = RandomEquip[gachaCount - i].equipData.RequiredAmountForMerge;
            resultEquipsData[i - 1].transform.GetComponentInChildren<Slider>().value = RandomEquip[gachaCount - i].equipData.AmountOfEquip;
            resultEquipsData[i - 1].transform.GetComponentInChildren<Slider>().gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = RandomEquip[gachaCount - i].equipData.AmountOfEquip + "/" + resultEquipsData[i - 1].transform.GetComponentInChildren<Slider>().maxValue;
        }


        StartAnimation();
    }


    //Real Money Purchase
    public void gachaRealMoney(int RingAmount, int NecklaceAmount, int RelicsAmount, int AccessoryAmount, int BookAmount, Equips.MaterialClass minType, Equips.MaterialClass MaxType, bool moreChance)
    {
        //설정
        DOTween.KillAll();
        StopAllCoroutines();

        //랜덤 장비 가지고오기
        if(randomStones != null)
            randomStones.Clear();

        if(RandomEquip != null)
            RandomEquip.Clear();

        int count = RingAmount + NecklaceAmount + RelicsAmount + AccessoryAmount + BookAmount;

        gachaCount = count;
        isEquipGacha = true;

        //Box
        GachaBox_Image.sprite = GachaBox_Sprites[1];

        //Acheivement
        AchievementManager.instance.GachaDaily += count;
        AchievementManager.instance.GachaWeekly += count;
        AchievementManager.instance.GachaTimeQuest += count;

        //Ring
        for(int i = 0; i < RingAmount; i++) {
            Equips.MaterialClass equipGrade = minType;

            float random = Random.Range(0.0f, 100.0f);

            if(random <= (moreChance ? 0.5f : 0.25f)) {
                equipGrade = Equips.MaterialClass.Ancient;
            } else if(random <= (moreChance ? 3.75f : 3f)) {
                equipGrade = Equips.MaterialClass.Legendary;
            } else if(random <= (moreChance ? 24.5f : 22f)) {
                equipGrade = Equips.MaterialClass.Unique;
            } else {
                equipGrade = Equips.MaterialClass.Epic;
            }

            if(equipGrade < minType) {
                equipGrade = minType;
            }

            if(equipGrade > MaxType) {
                equipGrade = minType;
            }

            RandomEquip.Add(EquipManager.Instance.getRandomItem_Grade(equipGrade, Equips.Type.Ring));
        }

        //Necklace
        for(int i = 0; i < NecklaceAmount; i++) {
            Equips.MaterialClass equipGrade = minType;

            float random = Random.Range(0.0f, 100.0f);

            if(random <= (moreChance ? 0.5f : 0.25f)) {
                equipGrade = Equips.MaterialClass.Ancient;
            } else if(random <= (moreChance ? 3.75f : 3f)) {
                equipGrade = Equips.MaterialClass.Legendary;
            } else if(random <= (moreChance ? 24.5f : 22f)) {
                equipGrade = Equips.MaterialClass.Unique;
            } else {
                equipGrade = Equips.MaterialClass.Epic;
            }

            if(equipGrade < minType) {
                equipGrade = minType;
            }

            if(equipGrade > MaxType) {
                equipGrade = minType;
            }

            RandomEquip.Add(EquipManager.Instance.getRandomItem_Grade(equipGrade, Equips.Type.Necklace));
        }

        //Relics
        for(int i = 0; i < RelicsAmount; i++) {
            Equips.MaterialClass equipGrade = minType;

            float random = Random.Range(0.0f, 100.0f);

            if(random <= (moreChance ? 0.5f : 0.25f)) {
                equipGrade = Equips.MaterialClass.Ancient;
            } else if(random <= (moreChance ? 3.75f : 3f)) {
                equipGrade = Equips.MaterialClass.Legendary;
            } else if(random <= (moreChance ? 24.5f : 22f)) {
                equipGrade = Equips.MaterialClass.Unique;
            } else {
                equipGrade = Equips.MaterialClass.Epic;
            }

            if(equipGrade < minType)
                equipGrade = minType;

            if(equipGrade > MaxType)
                equipGrade = minType;

            RandomEquip.Add(EquipManager.Instance.getRandomItem_Grade(equipGrade, Equips.Type.Relics));
        }

        //Accessory
        for(int i = 0; i < AccessoryAmount; i++) {
            Equips.MaterialClass equipGrade = minType;

            float random = Random.Range(0.0f, 100.0f);

            if(random <= (moreChance ? 0.5f : 0.25f)) {
                equipGrade = Equips.MaterialClass.Ancient;
            } else if(random <= (moreChance ? 3.75f : 3f)) {
                equipGrade = Equips.MaterialClass.Legendary;
            } else if(random <= (moreChance ? 24.5f : 22f)) {
                equipGrade = Equips.MaterialClass.Unique;
            } else {
                equipGrade = Equips.MaterialClass.Epic;
            }

            if(equipGrade < minType)
                equipGrade = minType;

            if(equipGrade > MaxType)
                equipGrade = minType;

            RandomEquip.Add(EquipManager.Instance.getRandomItem_Grade(equipGrade, Equips.Type.Accessory));
        }

        //Book
        for(int i = 0; i < BookAmount; i++) {
            Equips.MaterialClass equipGrade = minType;

            float random = Random.Range(0.0f, 100.0f);

            if(random <= (moreChance ? 0.5f : 0.25f)) {
                equipGrade = Equips.MaterialClass.Ancient;
            } else if(random <= (moreChance ? 3.75f : 3f)) {
                equipGrade = Equips.MaterialClass.Legendary;
            } else if(random <= (moreChance ? 27.5f : 25f)) {
                equipGrade = Equips.MaterialClass.Unique;
            } else {
                equipGrade = Equips.MaterialClass.Epic;
            }

            if(equipGrade < minType)
                equipGrade = minType;

            if(equipGrade > MaxType)
                equipGrade = minType;

            RandomEquip.Add(EquipManager.Instance.getRandomItem_Grade(equipGrade, Equips.Type.Book));
        }

        m_Class = Equips.MaterialClass.Normal;

        for(int i = 0; i < RandomEquip.Count; i++) {
            if(((int)RandomEquip[i].equipData.Grade) >= ((int)m_Class)) {
                m_Class = RandomEquip[i].equipData.Grade;
            }
        }

        SetSliderValue();

        for(int i = 0; i < RandomEquip.Count; i++) {
            RandomEquip[i].equipData.AmountOfEquip++;
        }
        for(int i = 0; i < resultEquipsData.Count; i++) {
            resultEquipsData[i].gameObject.SetActive(false);
        }

        for(int i = 1; i <= gachaCount; i++) {
            resultEquipsData[i - 1].gameObject.SetActive(true);
            resultEquipsData[i - 1].SetEquipment(RandomEquip[gachaCount - i]);
            resultEquipsData[i - 1].transform.GetComponentInChildren<Slider>().maxValue = RandomEquip[gachaCount - i].equipData.RequiredAmountForMerge;
            resultEquipsData[i - 1].transform.GetComponentInChildren<Slider>().value = RandomEquip[gachaCount - i].equipData.AmountOfEquip;
            resultEquipsData[i - 1].transform.GetComponentInChildren<Slider>().gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = RandomEquip[gachaCount - i].equipData.AmountOfEquip + "/" + resultEquipsData[i - 1].transform.GetComponentInChildren<Slider>().maxValue;
        }


        StartAnimation();
    }



    public void openGachaPopup(GachaData data)
    {
        popUp_Chest.SetActive(true);

        currentGacha = data;

        switch(data.gacha) {
            case GachaData.GachaType.Normal:
                ChestImage.sprite = GachaBox_Sprites[0];
                chestName_Text.text = "일반 뽑기 1회";
                break;
            case GachaData.GachaType.Normalx10:
                ChestImage.sprite = GachaBox_Sprites[0]; 
                chestName_Text.text = "일반 뽑기 10회";
                break;
            case GachaData.GachaType.Special:
                ChestImage.sprite = GachaBox_Sprites[1]; 
                chestName_Text.text = "스페셜 뽑기 1회";
                break;
            case GachaData.GachaType.Specialx10:
                ChestImage.sprite = GachaBox_Sprites[1];
                chestName_Text.text = "스페셜 뽑기 10회";
                break;
            case GachaData.GachaType.UpgradeStone:
                ChestImage.sprite = GachaBox_Sprites[2];
                chestName_Text.text = "강화석 뽑기 1회";
                break;
            case GachaData.GachaType.UpgradeStonex10:
                ChestImage.sprite = GachaBox_Sprites[2];
                chestName_Text.text = "강화석 뽑기 20회";
                break;
        }
    }

    public void gacha()
    {
        if(currentGacha.gacha == GachaData.GachaType.Normal) {
            if(NormalGachaTicket > 0) {
                NormalGachaTicket--;
                StartGacha(currentGacha);
            } else {
                if(GameManager.GetDiamond() >= currentGacha.Price) {
                    GameManager.SetDiamond(-currentGacha.Price);
                    StartGacha(currentGacha);
                } else
                    ErrorTab.SetActive(true);
            }
        } else if(currentGacha.gacha == GachaData.GachaType.Normalx10) {
            if(NormalGachaTicket >= 10) {
                NormalGachaTicket -= 10;
                StartGacha(currentGacha);
            } else {
                if(GameManager.GetDiamond() >= currentGacha.Price) {
                    GameManager.SetDiamond(-currentGacha.Price);
                    StartGacha(currentGacha);
                } else
                    ErrorTab.SetActive(true);
            }
        } else if(currentGacha.gacha == GachaData.GachaType.Special) {
            if(SpecialGachaTicket > 0) {
                SpecialGachaTicket--;
                StartGacha(currentGacha);
            } else {
                if(GameManager.GetDiamond() >= currentGacha.Price) {
                    GameManager.SetDiamond(-currentGacha.Price);
                    StartGacha(currentGacha);
                } else
                    ErrorTab.SetActive(true);
            }
        } else if(currentGacha.gacha == GachaData.GachaType.Specialx10) {
            if(SpecialGachaTicket >= 10) {
                SpecialGachaTicket -= 10;
                StartGacha(currentGacha);
            } else {
                if(GameManager.GetDiamond() >= currentGacha.Price) {
                    GameManager.SetDiamond(-currentGacha.Price);
                    StartGacha(currentGacha);
                } else
                    ErrorTab.SetActive(true);
            }
        } else if(currentGacha.gacha == GachaData.GachaType.UpgradeStone) {
            if(StoneGachaTicket > 0) {
                StoneGachaTicket--;
                StartGacha(currentGacha);
            } else {
                if(GameManager.GetDiamond() >= currentGacha.Price) {
                    GameManager.SetDiamond(-currentGacha.Price);
                    StartGacha(currentGacha);
                } else
                    ErrorTab.SetActive(true);
            }
        } else if(currentGacha.gacha == GachaData.GachaType.UpgradeStonex10) {
            if(StoneGachaTicket >= 20) {
                StoneGachaTicket -= 20;
                StartGacha(currentGacha);
            } else {
                if(GameManager.GetDiamond() >= currentGacha.Price) {
                    GameManager.SetDiamond(-currentGacha.Price);
                    StartGacha(currentGacha);
                } else
                    ErrorTab.SetActive(true);
            }
        }
    }

    public void StartGacha(GachaData data)
    {
        //설정
        DOTween.KillAll();
        StopAllCoroutines();


        //랜덤 장비 가지고오기
        if(randomStones != null)
            randomStones.Clear();

        if(RandomEquip != null)
            RandomEquip.Clear();

        for(int i = 0; i < StoneNumber.Length; i++) {
            StoneNumber[i] = 0;
        }


        //카드 설정
        if(data.gacha == GachaData.GachaType.Normal)
            NormalGacha(1);
        else if(data.gacha == GachaData.GachaType.Normalx10)
            NormalGacha(10);
        else if(data.gacha == GachaData.GachaType.UpgradeStone)
            StoneGacha(1);
        else if(data.gacha == GachaData.GachaType.UpgradeStonex10)
            StoneGacha(20);
        else if(data.gacha == GachaData.GachaType.Special)
            SpecialGacha(1);
        else if(data.gacha == GachaData.GachaType.Specialx10)
            SpecialGacha(10);

        if(isEquipGacha) {
            m_Class = Equips.MaterialClass.Normal;

            for(int i = 0; i < RandomEquip.Count; i++) {
                if(((int)RandomEquip[i].equipData.Grade) >= ((int)m_Class)) {
                    m_Class = RandomEquip[i].equipData.Grade;
                }
            }
        } else {
            u_Class = UpgradeStone.TypeOfStone.Normal;

            for(int i = 0; i < randomStones.Count; i++) {
                if(((int)randomStones[i].stone.stoneGrade) <= ((int)u_Class)) {
                    u_Class = randomStones[i].stone.stoneGrade;
                }
            }
        }


        StartAnimation();
    }

    public void NormalGacha(int count)
    {
        gachaCount = count;
        isEquipGacha = true;

        //Box
        GachaBox_Image.sprite = GachaBox_Sprites[0];

        //Achievement Manager
        AchievementManager.instance.GachaDaily += count;
        AchievementManager.instance.GachaWeekly += count;
        AchievementManager.instance.GachaTimeQuest += count;

        for(int i = 0; i < count; i++) {
            NormalGachaCount++;
            randomType = getRandomType();

            float random = Random.Range(0, 100);
            if(NormalGachaCount >= 100) {
                random = 0.05f;
                NormalGachaCount -= 100;
            }
            if(random < 0.05f)
                RandomEquip.Add(EquipManager.Instance.getRandomItem_Grade(Equips.MaterialClass.Unique, randomType));
            else if(random < 5.5f)
                RandomEquip.Add(EquipManager.Instance.getRandomItem_Grade(Equips.MaterialClass.Epic, randomType));
            else if(random < 40f)
                RandomEquip.Add(EquipManager.Instance.getRandomItem_Grade(Equips.MaterialClass.Rare, randomType));
            else if(random < 101)
                RandomEquip.Add(EquipManager.Instance.getRandomItem_Grade(Equips.MaterialClass.Normal, randomType));
        }

        for(int i = 0; i < RandomEquip.Count; i++) {
            RandomEquip[i].equipData.AmountOfEquip++;
        }

        for(int i = 0; i < resultEquipsData.Count; i++) {
            resultEquipsData[i].gameObject.SetActive(false);
        }

        for(int i = 1; i <= gachaCount; i++) {
            resultEquipsData[i-1].gameObject.SetActive(true);
            resultEquipsData[i-1].SetEquipment(RandomEquip[gachaCount - i]);
            resultEquipsData[i-1].transform.GetComponentInChildren<Slider>().maxValue = RandomEquip[gachaCount - i].equipData.RequiredAmountForMerge;
            resultEquipsData[i-1].transform.GetComponentInChildren<Slider>().value = RandomEquip[gachaCount - i].equipData.AmountOfEquip;
            resultEquipsData[i-1].transform.GetComponentInChildren<Slider>().gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = RandomEquip[gachaCount - i].equipData.AmountOfEquip + "/" + resultEquipsData[i-1].transform.GetComponentInChildren<Slider>().maxValue;
        }

        SetSliderValue();
    }
    public void SpecialGacha(int count)
    {
        gachaCount = count;
        isEquipGacha = true;

        //Box
        GachaBox_Image.sprite = GachaBox_Sprites[1];

        //Acheivement
        AchievementManager.instance.GachaDaily += count;
        AchievementManager.instance.GachaWeekly += count;
        AchievementManager.instance.GachaTimeQuest += count;

        for(int i = 0; i < count; i++) {
            SpecialGachaCount++;
            randomType = getRandomType();
            float random = Random.Range(0.0f, 100.0f);
            if(SpecialGachaCount >= 150) {
                random = 0.03f;
                SpecialGachaCount -= 150;
            }
            if(random < 0.025f)
                RandomEquip.Add(EquipManager.Instance.getRandomItem_Grade(Equips.MaterialClass.Legendary, randomType));
            else if(random < 0.9f)
                RandomEquip.Add(EquipManager.Instance.getRandomItem_Grade(Equips.MaterialClass.Unique, randomType));
            else if(random < 50f)
                RandomEquip.Add(EquipManager.Instance.getRandomItem_Grade(Equips.MaterialClass.Epic, randomType));
            else if(random < 101)
                RandomEquip.Add(EquipManager.Instance.getRandomItem_Grade(Equips.MaterialClass.Rare, randomType));
        }

        for(int i = 0; i < RandomEquip.Count; i++) {
            RandomEquip[i].equipData.AmountOfEquip++;
        }
        for(int i = 0; i < resultEquipsData.Count; i++) {
            resultEquipsData[i].gameObject.SetActive(false);
        }

        for(int i = 1; i <= gachaCount; i++) {
            resultEquipsData[i-1].gameObject.SetActive(true);
            resultEquipsData[i-1].SetEquipment(RandomEquip[gachaCount - i]);
            resultEquipsData[i-1].transform.GetComponentInChildren<Slider>().maxValue = RandomEquip[gachaCount - i].equipData.RequiredAmountForMerge;
            resultEquipsData[i-1].transform.GetComponentInChildren<Slider>().value = RandomEquip[gachaCount - i].equipData.AmountOfEquip;
            resultEquipsData[i-1].transform.GetComponentInChildren<Slider>().gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = RandomEquip[gachaCount - i].equipData.AmountOfEquip + "/" + resultEquipsData[i-1].transform.GetComponentInChildren<Slider>().maxValue;
        }
        SetSliderValue();
    }

    public void StoneGacha(int count)
    {
        gachaCount = count;
        isEquipGacha = false;

        //Box
        GachaBox_Image.sprite = GachaBox_Sprites[2];

        for(int i = 0; i < count; i++) {
            UpgradeStoneGachaCount++;

            float random = Random.Range(0.0f, 100f);

            if(UpgradeStoneGachaCount >= 200) {
                random = 0;
                UpgradeStoneGachaCount -= 200;
            }

            RandomStone stoneForAdding = new RandomStone();
            //30 ~ 40
            //20 ~ 30
            //10 ~ 20
            //7 ~ 10
            //3 ~ 7
            //1 ~ 3
            if(random <= 0.1f) {
                stoneForAdding.stone = UpgradeStoneManager.instance.getStone(UpgradeStone.TypeOfStone.Ancient);
                stoneForAdding.stoneNumber = Random.Range(1, 3);
                randomStones.Add(stoneForAdding);
                StoneNumber[5] += stoneForAdding.stoneNumber;
            } else if(random <= 2) {
                stoneForAdding.stone = UpgradeStoneManager.instance.getStone(UpgradeStone.TypeOfStone.Legendary);
                stoneForAdding.stoneNumber = Random.Range(3, 7);
                randomStones.Add(stoneForAdding);
                StoneNumber[4] += stoneForAdding.stoneNumber;
            } else if(random <= 10) {
                stoneForAdding.stone = UpgradeStoneManager.instance.getStone(UpgradeStone.TypeOfStone.Unique);
                stoneForAdding.stoneNumber = Random.Range(7, 10);
                randomStones.Add(stoneForAdding);
                StoneNumber[3] += stoneForAdding.stoneNumber;
            } else if(random <= 23) {
                stoneForAdding.stone = UpgradeStoneManager.instance.getStone(UpgradeStone.TypeOfStone.Epic);
                stoneForAdding.stoneNumber = Random.Range(10, 20);
                randomStones.Add(stoneForAdding);
                StoneNumber[2] += stoneForAdding.stoneNumber;
            } else if(random <= 75) {
                stoneForAdding.stone = UpgradeStoneManager.instance.getStone(UpgradeStone.TypeOfStone.Rare);
                stoneForAdding.stoneNumber = Random.Range(20, 35);
                randomStones.Add(stoneForAdding);
                StoneNumber[1] += stoneForAdding.stoneNumber;
            } else if(random <= 101) {
                stoneForAdding.stone = UpgradeStoneManager.instance.getStone(UpgradeStone.TypeOfStone.Normal);
                stoneForAdding.stoneNumber = Random.Range(30, 50);
                randomStones.Add(stoneForAdding);
                StoneNumber[0] += stoneForAdding.stoneNumber;
            }
        }

        randomStones = randomStones.OrderByDescending(w => ((int)w.stone.stoneGrade)).ToList();

        for(int i = 0; i < randomStones.Count; i++) {
            UpgradeStoneManager.instance.addStone(randomStones[i].stone, randomStones[i].stoneNumber);
        }

        for(int i = 0; i < resultStonesData.Count; i++) {
            resultStonesData[i].gameObject.SetActive(false);
            if(StoneNumber[i] > 0) {
                resultStonesData[i].ItemNumber.text = StoneNumber[i].ToString();
                resultStonesData[i].gameObject.SetActive(true);
            }
        }

        SetSliderValue();
    }

    public void StartAnimation()
    {
        //기본적인 세팅
        Result_Panel.SetActive(false);
        GachaBox_Image.gameObject.SetActive(true);
        EquipShowTab.SetActive(false);

        GachaScreen.SetActive(true);
        isFirst = true;
        StartCoroutine(Animation());
    }

    public IEnumerator Animation()
    {
        setAnimationValues();

        GachaBox_Glow.gameObject.SetActive(false);
        GachaBox_Glow.localScale = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(1);

        GachaBox_Transform.DOShakePosition(ShakeTime, ShakeScale, 10, 90, true)
                  .SetEase(Ease.Linear);


        yield return new WaitForSeconds(ShakeTime * 0.375f);
        GachaBox_Glow.GetComponent<Image>().color = anim_Color;
        GachaBox_Glow.GetComponent<Image>().DOColor(Color.white, 3f);
        GachaBox_Glow.gameObject.SetActive(true);
        GachaBox_Glow.DOScale(new Vector3(13f, 13f, 13f), 3f)
                .SetEase(Ease.Linear);

        yield return new WaitForSeconds(3);
        GachaBox_Image.gameObject.SetActive(false);
        gloweffects.SetActive(false);
        EquipShowTab.SetActive(true);
        gachaCount--;
        EquipOneByOne();
        GachaBox_Glow.DOScale(new Vector3(0, 0, 0), 0.7f)
        .SetEase(Ease.Linear);
        isFirst = false;
    }



    public void setAnimationValues()
    {
        if(isEquipGacha) {
            switch(m_Class) {
                case Equips.MaterialClass.Normal:
                    anim_Color = Color.white;
                    ShakeScale = 5;
                    ShakeTime = 2;
                    break;
                case Equips.MaterialClass.Rare:
                    anim_Color = new Color(0.3960784f, 0.6261988f, 0.9176471f);
                    ShakeScale = 10;
                    ShakeTime = 2;
                    break;
                case Equips.MaterialClass.Epic:
                    anim_Color = new Color(0.8117647f, 0.1960784f, 0.6862745f);
                    ShakeScale = 20;
                    ShakeTime = 2.5f;
                    break;
                case Equips.MaterialClass.Unique:
                    anim_Color = new Color(0.9058824f, 0.7803922f, 0.1098039f);
                    ShakeScale = 30;
                    ShakeTime = 3;
                    break;
                case Equips.MaterialClass.Legendary:
                    anim_Color = new Color(0.9058824f, 0.1490196f, 0.1098039f);
                    ShakeScale = 40;
                    ShakeTime = 3.5f;
                    break;
                case Equips.MaterialClass.Ancient:
                    anim_Color = new Color(1, 0.5019608f, 0);
                    ShakeScale = 50;
                    ShakeTime = 4.5f;
                    break;
            }
        } else {
            switch(u_Class) {
                case UpgradeStone.TypeOfStone.Normal:
                    anim_Color = Color.white;
                    ShakeScale = 5;
                    ShakeTime = 2;
                    break;
                case UpgradeStone.TypeOfStone.Rare:
                    anim_Color = new Color(0.3960784f, 0.6261988f, 0.9176471f);
                    ShakeScale = 10;
                    ShakeTime = 2;
                    break;
                case UpgradeStone.TypeOfStone.Epic:
                    anim_Color = new Color(0.8117647f, 0.1960784f, 0.6862745f);
                    ShakeScale = 20;
                    ShakeTime = 2.5f;
                    break;
                case UpgradeStone.TypeOfStone.Unique:
                    anim_Color = new Color(0.9058824f, 0.7803922f, 0.1098039f);
                    ShakeScale = 30;
                    ShakeTime = 3;
                    break;
                case UpgradeStone.TypeOfStone.Legendary:
                    anim_Color = new Color(0.9058824f, 0.1490196f, 0.1098039f);
                    ShakeScale = 40;
                    ShakeTime = 3.5f;
                    break;
                case UpgradeStone.TypeOfStone.Ancient:
                    anim_Color = new Color(1, 0.5019608f, 0);
                    ShakeScale = 50;
                    ShakeTime = 4.5f;
                    break;
            }
        }
    }
    public void setAnimationValues2()
    {
        if(isEquipGacha) {
            switch(RandomEquip[gachaCount].equipData.Grade) {
                case Equips.MaterialClass.Normal:
                    anim_Color = Color.white;
                    ShakeScale = 5;
                    ShakeTime = 2;
                    break;
                case Equips.MaterialClass.Rare:
                    anim_Color = new Color(0.3960784f, 0.6261988f, 0.9176471f);
                    ShakeScale = 10;
                    ShakeTime = 2;
                    break;
                case Equips.MaterialClass.Epic:
                    anim_Color = new Color(0.8117647f, 0.1960784f, 0.6862745f);
                    ShakeScale = 20;
                    ShakeTime = 2.5f;
                    break;
                case Equips.MaterialClass.Unique:
                    anim_Color = new Color(0.9058824f, 0.7803922f, 0.1098039f);
                    ShakeScale = 30;
                    ShakeTime = 3;
                    break;
                case Equips.MaterialClass.Legendary:
                    anim_Color = new Color(0.9058824f, 0.1490196f, 0.1098039f);
                    ShakeScale = 40;
                    ShakeTime = 3.5f;
                    break;
                case Equips.MaterialClass.Ancient:
                    anim_Color = new Color(1, 0.5019608f, 0);
                    ShakeScale = 50;
                    ShakeTime = 4.5f;
                    break;
            }
        } else {
            switch(randomStones[gachaCount].stone.stoneGrade) {
                case UpgradeStone.TypeOfStone.Normal:
                    anim_Color = Color.white;
                    ShakeScale = 5;
                    ShakeTime = 2;
                    break;
                case UpgradeStone.TypeOfStone.Rare:
                    anim_Color = new Color(0.3960784f, 0.6261988f, 0.9176471f);
                    ShakeScale = 10;
                    ShakeTime = 2;
                    break;
                case UpgradeStone.TypeOfStone.Epic:
                    anim_Color = new Color(0.8117647f, 0.1960784f, 0.6862745f);
                    ShakeScale = 20;
                    ShakeTime = 2.5f;
                    break;
                case UpgradeStone.TypeOfStone.Unique:
                    anim_Color = new Color(0.9058824f, 0.7803922f, 0.1098039f);
                    ShakeScale = 30;
                    ShakeTime = 3;
                    break;
                case UpgradeStone.TypeOfStone.Legendary:
                    anim_Color = new Color(0.9058824f, 0.1490196f, 0.1098039f);
                    ShakeScale = 40;
                    ShakeTime = 3.5f;
                    break;
                case UpgradeStone.TypeOfStone.Ancient:
                    anim_Color = new Color(1, 0.5019608f, 0);
                    ShakeScale = 50;
                    ShakeTime = 4.5f;
                    break;
            }
        }
    }
    public IEnumerator glowEffect()
    {
        setAnimationValues2();
        GachaBox_Glow.GetComponent<Image>().color = anim_Color;
        GachaBox_Glow.GetComponent<Image>().DOColor(Color.white, 3f);
        GachaBox_Glow.gameObject.SetActive(true);
        GachaBox_Glow.DOScale(new Vector3(13f, 18f, 13f), 2f)
                .SetEase(Ease.Linear);
        yield return new WaitForSeconds(3);
        EquipOneByOne();
        GachaBox_Glow.DOScale(new Vector3(0, 0, 0), 0.7f);
    }

    public void NextCard()
    {
        if(isEquipGacha) {
            if(gachaCount > 0) {
                gachaCount--;
                StartCoroutine(glowEffect());
            } else {
                StartCoroutine(showResultOfGacha());
            }
        } else {
            StartCoroutine(showResultOfGacha());
        }
    }

    public IEnumerator showResultOfGacha()
    {
        GachaBox_Glow.DOScale(new Vector3(0, 0, 0), 0);
        if(isEquipGacha) {
            //Stone OFF
            ResultStoneGroup.alpha = 0;
            ResultEquipGroup.alpha = 0;
            TouchScreenText.SetActive(false);
            EndButton.interactable = false;
            Result_Panel.SetActive(true);
            ResultEquipGroup.DOFade(1f, 1)
                .SetEase(Ease.Linear);
            yield return new WaitForSeconds(1.5f);
            TouchScreenText.SetActive(true);
            EndButton.interactable = true;
        } else {
            ResultEquipGroup.alpha = 0;
            ResultStoneGroup.alpha = 0;
            TouchScreenText.SetActive(false);
            EndButton.interactable = false;
            Result_Panel.SetActive(true);
            ResultStoneGroup.DOFade(1f, 1).SetEase(Ease.Linear);
            yield return new WaitForSeconds(1.5f);
            TouchScreenText.SetActive(true);
            EndButton.interactable = true;
        }
    }

    public void skipGlowEffectAnimation()
    {
        if(isFirst) {
            StopAllCoroutines();
            DOTween.KillAll();
            GachaBox_Image.gameObject.SetActive(false);
            gloweffects.SetActive(false);
            EquipShowTab.SetActive(true);
            gachaCount--;
            GachaBox_Glow.DOScale(new Vector3(0, 0, 0), 0);
            EquipOneByOne();
            isFirst = false;
        } else {
            StopAllCoroutines();
            DOTween.KillAll();
            GachaBox_Glow.DOScale(new Vector3(0, 0, 0), 0);
            EquipOneByOne();
        }
    }

    public void SkipCard()
    {
        StopAllCoroutines();
        DOTween.KillAll();
        gachaCount = 0;
        NextCard();
    }

    public void EquipOneByOne()
    {
        if(isEquipGacha) {
            Frame_EquipResult.SetEquipment(RandomEquip[gachaCount]);
            setEquipStatsUI();
            setLightEffect();
            EquipResult_Name.text = RandomEquip[gachaCount].equipData.EquipName;
            EquipResult_Grade.text = RandomEquip[gachaCount].equipData.gradeText;
            EquipAmountSlider.maxValue = RandomEquip[gachaCount].equipData.RequiredAmountForMerge;
            EquipAmountSlider.value = RandomEquip[gachaCount].equipData.AmountOfEquip;
            EquipAmountSlider_Text.text = EquipAmountSlider.value + "/" + EquipAmountSlider.maxValue;
        } else {
            StartCoroutine(showResultOfGacha());
        }
    }

    public void setEquipStatsUI()
    {
        foreach(GameObject g in Equip_Stats) {
            g.SetActive(false);
        }

        if(RandomEquip[gachaCount] != null) {
            if(RandomEquip[gachaCount].equipData.baseOption.option.damage != 0) {
                Equip_Stats[0].SetActive(true);
                Equip_Stats[0].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = RandomEquip[gachaCount].equipData.FinalOption.option.damage.ToString("#.##");
            }
            if(RandomEquip[gachaCount].equipData.baseOption.option.damagePercent != 0) {
                Equip_Stats[1].SetActive(true);
                Equip_Stats[1].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (RandomEquip[gachaCount].equipData.FinalOption.option.damagePercent * 100).ToString("#.##") + "%";
            }
            if(RandomEquip[gachaCount].equipData.baseOption.option.CritChance != 0) {
                Equip_Stats[2].SetActive(true);
                Equip_Stats[2].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (RandomEquip[gachaCount].equipData.FinalOption.option.CritChance).ToString("#.##") + "%";
            }
            if(RandomEquip[gachaCount].equipData.baseOption.option.CritDamage != 0) {
                Equip_Stats[3].SetActive(true);
                Equip_Stats[3].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (RandomEquip[gachaCount].equipData.FinalOption.option.CritDamage * 100).ToString("#.##") + "%";
            }
            if(RandomEquip[gachaCount].equipData.baseOption.option.AttackSpeed != 0) {
                Equip_Stats[4].SetActive(true);
                Equip_Stats[4].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (RandomEquip[gachaCount].equipData.FinalOption.option.AttackSpeed * 100).ToString("#.##") + "%";
            }
            if(RandomEquip[gachaCount].equipData.baseOption.option.BulletSpeed != 0) {
                Equip_Stats[5].SetActive(true);
                Equip_Stats[5].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (RandomEquip[gachaCount].equipData.FinalOption.option.BulletSpeed * 100).ToString("#.##") + "%";
            }
            if(RandomEquip[gachaCount].equipData.baseOption.option.StunPercent != 0) {
                Equip_Stats[6].SetActive(true);
                Equip_Stats[6].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (RandomEquip[gachaCount].equipData.FinalOption.option.StunPercent).ToString("#.##") + "%";
            }
            if(RandomEquip[gachaCount].equipData.baseOption.option.ExtraMoney != 0) {
                Equip_Stats[7].SetActive(true);
                Equip_Stats[7].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (RandomEquip[gachaCount].equipData.FinalOption.option.ExtraMoney * 100).ToString("#.##") + "%";
            }
            if(RandomEquip[gachaCount].equipData.baseOption.option.ExtraEXP != 0) {
                Equip_Stats[8].SetActive(true);
                Equip_Stats[8].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (RandomEquip[gachaCount].equipData.FinalOption.option.ExtraEXP * 100).ToString("#.##") + "%";
            }
        }
    }

    public void setLightEffect()
    {
        switch(RandomEquip[gachaCount].equipData.Grade) {
            case Equips.MaterialClass.Normal:
                LightEffect.color = Color.white;
                LightEffect2.color = Color.white;
                EquipResult_Name.color = Color.white;
                EquipResult_Grade.color = Color.white;
                break;
            case Equips.MaterialClass.Rare:
                LightEffect.color = new Color(0, 1, 0.851826f);
                LightEffect2.color = new Color(0, 1, 0.851826f);
                EquipResult_Name.color = new Color(0.1462264f, 0.9034994f, 1);
                EquipResult_Grade.color = new Color(0.1462264f, 0.9034994f, 1);
                break;
            case Equips.MaterialClass.Epic:
                LightEffect.color = new Color(0.9814101f, 0.3349057f, 1);
                LightEffect2.color = new Color(0.9814101f, 0.3349057f, 1);
                EquipResult_Name.color = new Color(1, 0, 0.989954f);
                EquipResult_Grade.color = new Color(1, 0, 0.989954f);
                break;
            case Equips.MaterialClass.Unique:
                LightEffect.color = new Color(1, 0.7534226f, 0);
                LightEffect2.color = new Color(1, 0.7534226f, 0);
                EquipResult_Name.color = new Color(1, 0.8228641f, 0.259434f);
                EquipResult_Grade.color = new Color(1, 0.8228641f, 0.259434f);
                break;
            case Equips.MaterialClass.Legendary:
                LightEffect.color = new Color(0.990566f, 0.2655566f, 0.1822268f);
                LightEffect2.color = new Color(0.990566f, 0.2655566f, 0.1822268f);
                EquipResult_Name.color = new Color(1, 0.0990566f, 0.0990566f);
                EquipResult_Grade.color = new Color(1, 0.0990566f, 0.0990566f);
                break;
            case Equips.MaterialClass.Ancient:
                LightEffect.color = new Color(0.7735849f, 0.2645572f, 0);
                LightEffect2.color = new Color(0.7735849f, 0.2645572f, 0);
                EquipResult_Name.color = new Color(1, 0.4994942f, 0.09803921f);
                EquipResult_Grade.color = new Color(1, 0.4994942f, 0.09803921f);
                break;
        }
    }

    public void SetTicket()
    {
        //NormalTicket
        if(NormalGachaTicket > 0 && NormalGachaTicket < 10) {
            NormalButton[1].SetActive(false);
            NormalButton[0].SetActive(true);
            NormalButton_Text.text = NormalGachaTicket.ToString();

            Normalx10Button[1].SetActive(true);
            Normalx10Button[0].SetActive(false);
            Normalx10Button_Text.text = "1,000";
        } else if(NormalGachaTicket >= 10) {
            NormalButton[1].SetActive(false);
            NormalButton[0].SetActive(true);
            NormalButton_Text.text = NormalGachaTicket.ToString();

            Normalx10Button[1].SetActive(false);
            Normalx10Button[0].SetActive(true);
            Normalx10Button_Text.text = NormalGachaTicket.ToString();
        } else if(NormalGachaTicket <= 0) {
            NormalButton[1].SetActive(true);
            NormalButton[0].SetActive(false);
            NormalButton_Text.text = "100";

            Normalx10Button[1].SetActive(true);
            Normalx10Button[0].SetActive(false);
            Normalx10Button_Text.text = "1,000";
        }

        //Special Ticket
        if(SpecialGachaTicket > 0 && SpecialGachaTicket < 10) {
            SpecialButton[1].SetActive(false);
            SpecialButton[0].SetActive(true);
            SpecialButton_Text.text = SpecialGachaTicket.ToString();

            Specialx10Button[1].SetActive(true);
            Specialx10Button[0].SetActive(false);
            Specialx10Button_Text.text = "3,000";
        } else if(SpecialGachaTicket >= 10) {
            SpecialButton[1].SetActive(false);
            SpecialButton[0].SetActive(true);
            SpecialButton_Text.text = SpecialGachaTicket.ToString();

            Specialx10Button[1].SetActive(false);
            Specialx10Button[0].SetActive(true);
            Specialx10Button_Text.text = SpecialGachaTicket.ToString();
        } else if(SpecialGachaTicket <= 0) {
            SpecialButton[1].SetActive(true);
            SpecialButton[0].SetActive(false);
            SpecialButton_Text.text = "300";

            Specialx10Button[1].SetActive(true);
            Specialx10Button[0].SetActive(false);
            Specialx10Button_Text.text = "3,000";
        }

        //Stone Ticket
        if(StoneGachaTicket > 0 && StoneGachaTicket < 20) {
            StoneButton[1].SetActive(false);
            StoneButton[0].SetActive(true);
            StoneButton_Text.text = StoneGachaTicket.ToString();

            Stonex10Button[1].SetActive(true);
            Stonex10Button[0].SetActive(false);
            Stonex10Button_Text.text = "2,000";
        } else if(StoneGachaTicket >= 20) {
            StoneButton[1].SetActive(false);
            StoneButton[0].SetActive(true);
            StoneButton_Text.text = StoneGachaTicket.ToString();

            Stonex10Button[1].SetActive(false);
            Stonex10Button[0].SetActive(true);
            Stonex10Button_Text.text = StoneGachaTicket.ToString();
        } else if(StoneGachaTicket <= 0) {
            StoneButton[1].SetActive(true);
            StoneButton[0].SetActive(false);
            StoneButton_Text.text = "100";

            Stonex10Button[1].SetActive(true);
            Stonex10Button[0].SetActive(false);
            Stonex10Button_Text.text = "2,000";
        }

        NormalAlert.SetActive(NormalGachaTicket > 0);
        SpecialAlert.SetActive(SpecialGachaTicket > 0);
        StoneAlert.SetActive(StoneGachaTicket > 0);

        AlertAll.SetActive(NormalAlert.activeSelf || SpecialAlert.activeSelf || StoneAlert.activeSelf);
    }

    public void SetSliderValue()
    {
        if(NormalGachaCount <= 100)
            NormalSlider.setProgress(NormalGachaCount, 100);
        else
            NormalSlider.setProgress(100, 100);

        if(SpecialGachaCount <= 150)
            SpecialSlider.setProgress(SpecialGachaCount, 150);
        else
            SpecialSlider.setProgress(150, 150);

        if(UpgradeStoneGachaCount <= 200)
            StoneSlider.setProgress(UpgradeStoneGachaCount, 200);
        else
            StoneSlider.setProgress(200, 200);
    }

    public Equips.Type getRandomType()
    {
        float random = Random.Range(0, 100);
        if(random < 25)
            return Equips.Type.Ring;
        else if(random < 50)
            return Equips.Type.Necklace;
        else if(random < 72)
            return Equips.Type.Accessory;
        else if(random < 988)
            return Equips.Type.Relics;
        else if(random < 100)
            return Equips.Type.Book;

        return default;
    }

    public void addTicket(GachaData.GachaType type, int n)
    {
        switch(type) {
            case GachaData.GachaType.Normal:
                NormalGachaTicket += n;
                break;
            case GachaData.GachaType.Special:
                SpecialGachaTicket += n;
                break;
            case GachaData.GachaType.UpgradeStone:
                StoneGachaTicket += n;
                break;
        }
    }

    public void LoadData(GameData data)
    {
        NormalGachaCount = data.NormalBoughtCount;
        SpecialGachaCount = data.SpecialBoughtCount;
        UpgradeStoneGachaCount = data.StoneBoughtCount;

        NormalGachaTicket = data.NormalGachaTicket;
        SpecialGachaTicket = data.SpecialGachaTicket;
        StoneGachaTicket = data.StoneGachaTicket;

        SetSliderValue();
    }

    public void SaveData(GameData data)
    {
        data.NormalBoughtCount = this.NormalGachaCount;
        data.SpecialBoughtCount = this.SpecialGachaCount;
        data.StoneBoughtCount = this.UpgradeStoneGachaCount;

        data.NormalGachaTicket = NormalGachaTicket;
        data.SpecialGachaTicket = SpecialGachaTicket;
        data.StoneGachaTicket = StoneGachaTicket;
    }
}


[System.Serializable]
public class RandomStone
{
    public UpgradeStone stone;
    public int stoneNumber;
}
