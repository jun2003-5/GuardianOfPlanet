using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipManager : MonoBehaviour, IDataPersistence
{
    public static EquipManager Instance;

    public EquipUpgradeManager upgradeManager;

    public EquipFrame equipFrame_pfb;

    [Header("아이템 등급별")]
    public EquipSorting Sorting_Ring;
    public EquipSorting Sorting_Necklace;
    public EquipSorting Sorting_Book;
    public EquipSorting Sorting_Accessory;
    public EquipSorting Sorting_Relics;
    [HideInInspector]
    public List<EquipFrame> All_Equips;

    [HideInInspector]
    public EquipFrame SelectedEquip;
    [HideInInspector]
    public UpgradeStone SelectedStone;

    [Header("정보창")]
    [Space(5)]
    public Image Background;
    public Image Background_BG;
    public Image Border;
    public Image Light;
    public Image Glow;
    public Image TypeBorder;
    public Image Type;
    public Image EquipImage;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI GradeText;
    public List<GameObject> equipStats_obj;
    public TextMeshProUGUI EquipNameText;

    //버튼
    public GameObject EquipButton;
    public GameObject unEquipButton;
    public GameObject UpgradeButton;
    public GameObject JinhwaButton;

    [Header("star prefab")]
    public Transform StarParent;
    public Image Star;
    public List<Image> star_List;

    [Header("Set")]
    public SetScript prefab_Set;
    public Transform SetParent;

    [HideInInspector]
    public List<SetScript> setDatas;

    [Header("#----1세트2세트")]
    public int CurrentSet;
    public List<EquipSet> Equip_Sets;
    public TabGroup tabGroup;
    public List<TabButton> Buttons;

    public Sprite[] EquipType_Sprites;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Background.gameObject.SetActive(false);
        Background_BG.gameObject.SetActive(false);
        Glow.gameObject.SetActive(false);
        Border.gameObject.SetActive(false);
        Light.gameObject.SetActive(false);
        TypeBorder.gameObject.SetActive(false);
        EquipImage.gameObject.SetActive(false);
        GradeText.gameObject.SetActive(false);
        LevelText.gameObject.SetActive(false);
        EquipNameText.gameObject.SetActive(false);
        Type.gameObject.SetActive(false);
        foreach(GameObject g in equipStats_obj) {
            g.SetActive(false);
        }

        createAllItems();
        CreateStar();
    }

    private void Update()
    {
        if(Input.GetKeyDown("x")) {
            foreach(EquipFrame e in All_Equips) {
                e.equipData.AmountOfEquip++;
            }
        }

        ChangeEquipsUI();
    }

    //Create Methods
    public void createAllItems()
    {
        //반지
        for(int i = 0; i < Sorting_Ring.allItems.Count; i++) {
            CreateEquip(Sorting_Ring.allItems[i]);
        }

        //목걸이
        for(int i = 0; i < Sorting_Necklace.allItems.Count; i++) {
            CreateEquip(Sorting_Necklace.allItems[i]);
        }

        //책
        for(int i = 0; i < Sorting_Book.allItems.Count; i++) {
            CreateEquip(Sorting_Book.allItems[i]);
        }

        //장신구
        for(int i = 0; i < Sorting_Accessory.allItems.Count; i++) {
            CreateEquip(Sorting_Accessory.allItems[i]);
        }

        //장신구
        for(int i = 0; i < Sorting_Relics.allItems.Count; i++) {
            CreateEquip(Sorting_Relics.allItems[i]);
        }
    }
    public void CreateEquip(Equips equip)
    {
        EquipFrame e = Instantiate(equipFrame_pfb);
        e.equipData = equip;
        e.equipData.setEquipInfo();
        if(equip.TypeOfEquip == Equips.Type.Ring) {
            e.setEquipFrameUI(equip, EquipType_Sprites[0]);
            Sorting_Ring.Have_Items.Add(e);
            e.transform.SetParent(Sorting_Ring.parentObject);
        } else if(equip.TypeOfEquip == Equips.Type.Necklace) {
            e.setEquipFrameUI(equip, EquipType_Sprites[1]);
            Sorting_Necklace.Have_Items.Add(e);
            e.transform.SetParent(Sorting_Necklace.parentObject);
        } else if(equip.TypeOfEquip == Equips.Type.Book) {
            e.setEquipFrameUI(equip, EquipType_Sprites[4]);
            Sorting_Book.Have_Items.Add(e);
            e.transform.SetParent(Sorting_Book.parentObject);
        } else if(equip.TypeOfEquip == Equips.Type.Accessory) {
            e.setEquipFrameUI(equip, EquipType_Sprites[3]);
            Sorting_Accessory.Have_Items.Add(e);
            e.transform.SetParent(Sorting_Accessory.parentObject);
        } else if(equip.TypeOfEquip == Equips.Type.Relics) {
            e.setEquipFrameUI(equip, EquipType_Sprites[2]);
            Sorting_Relics.Have_Items.Add(e);
            e.transform.SetParent(Sorting_Relics.parentObject);
        }
        All_Equips.Add(e);
        e.transform.localScale = new Vector3(0.95f, 0.95f, 0.95f);
        e.name = e.equipData.EquipName + e.equipData.Grade;
    }

    public void setEquipAvailablilty()
    {
        //반지
        for(int i = 0; i < Sorting_Ring.Have_Items.Count; i++) {
            Sorting_Ring.Have_Items[i].gameObject.SetActive(Sorting_Ring.Have_Items[i].equipData.AmountOfEquip > 0);
        }

        //목걸이
        for(int i = 0; i < Sorting_Necklace.Have_Items.Count; i++) {
            Sorting_Necklace.Have_Items[i].gameObject.SetActive(Sorting_Necklace.Have_Items[i].equipData.AmountOfEquip > 0);
        }

        //유물
        for(int i = 0; i < Sorting_Relics.Have_Items.Count; i++) {
            Sorting_Relics.Have_Items[i].gameObject.SetActive(Sorting_Relics.Have_Items[i].equipData.AmountOfEquip > 0);
        }

        //장신구
        for(int i = 0; i < Sorting_Accessory.Have_Items.Count; i++) {
            Sorting_Accessory.Have_Items[i].gameObject.SetActive(Sorting_Accessory.Have_Items[i].equipData.AmountOfEquip > 0);
        }

        //책
        for(int i = 0; i < Sorting_Book.Have_Items.Count; i++) {
            Sorting_Book.Have_Items[i].gameObject.SetActive(Sorting_Book.Have_Items[i].equipData.AmountOfEquip > 0);
        }
    }

    public void CreateStar()
    {
        for(int i = 0; i < 10; i++) {
            Image _star = Instantiate(Star, StarParent);
            star_List.Add(_star);
            star_List[i].color = new Color(0, 0, 0, 0.6f);
        }
    }
    /*
    public void CreateSetInformation()
    {
        for(int i = 1; i < System.Enum.GetValues(typeof(Equips.SetType)).Length; i++) {
            SetScript _s = Instantiate(prefab_Set, SetParent);
            setDatas.Add(_s);
        }
    }
    */
    public void ReorderSets()
    {
        setDatas.Sort((a, b) => {
            int ancientComparison = a.getNumberOfGradeEquip(Equips.MaterialClass.Ancient).CompareTo(b.getNumberOfGradeEquip(Equips.MaterialClass.Ancient));

            if(ancientComparison != 0) {
                return ancientComparison;
            }

            int legendaryComparison = a.getNumberOfGradeEquip(Equips.MaterialClass.Legendary).CompareTo(b.getNumberOfGradeEquip(Equips.MaterialClass.Legendary));

            if(legendaryComparison != 0) {
                return legendaryComparison;
            }

            int uniqueComparison = a.getNumberOfGradeEquip(Equips.MaterialClass.Unique).CompareTo(b.getNumberOfGradeEquip(Equips.MaterialClass.Unique));

            if(uniqueComparison != 0) {
                return uniqueComparison;
            }

            int epicComparison = a.getNumberOfGradeEquip(Equips.MaterialClass.Epic).CompareTo(b.getNumberOfGradeEquip(Equips.MaterialClass.Epic));

            if(epicComparison != 0) {
                return epicComparison;
            }

            int rareComparison = a.getNumberOfGradeEquip(Equips.MaterialClass.Rare).CompareTo(b.getNumberOfGradeEquip(Equips.MaterialClass.Rare));

            if(rareComparison != 0) {
                return rareComparison;
            }

            int normalComparison = a.getNumberOfGradeEquip(Equips.MaterialClass.Normal).CompareTo(b.getNumberOfGradeEquip(Equips.MaterialClass.Normal));

            return normalComparison;
        });

        for(int i = 0; i < setDatas.Count; i++) {
            setDatas[i].gameObject.transform.SetSiblingIndex(i);
        }
    }
    public void ChangeStarSetting(int n)
    {
        for(int i = 0; i < 10 + n; i++) {
            if(i < 10) {
                star_List[i].color = new Color(0, 0, 0, 0.6f);
            } else if(i >= 10 && i < 20) {
                star_List[i - 10].color = new Color(1, 1, 1, 1f);
            } else {
                star_List[i - 20].color = new Color(1, 0, 0.085289f);
            }
        }
    }

    //Equip Related
    public void UpgradeEquip(Equips equip, int n)
    {
        for(int i = 0; i < All_Equips.Count; i++) {
            if(equip.id == All_Equips[i].equipData.id) {
                All_Equips[i].equipData.EquipEarned(n);
            }
        }

        ChangeEquipsUI();
    }
    public void ChangeEquipsUI()
    {
        //UI
        if(SelectedEquip != null) {
            unEquipButton.SetActive(SelectedEquip == EquipingManager.Instance.Ring.equipment || SelectedEquip == EquipingManager.Instance.Necklace.equipment || SelectedEquip == EquipingManager.Instance.Book.equipment || SelectedEquip == EquipingManager.Instance.Accessory.equipment || SelectedEquip == EquipingManager.Instance.Relics.equipment);
            EquipButton.SetActive(!(SelectedEquip == EquipingManager.Instance.Ring.equipment || SelectedEquip == EquipingManager.Instance.Necklace.equipment || SelectedEquip == EquipingManager.Instance.Book.equipment || SelectedEquip == EquipingManager.Instance.Accessory.equipment || SelectedEquip == EquipingManager.Instance.Relics.equipment));
            UpgradeButton.SetActive(SelectedEquip.equipData.Level < 20);
            JinhwaButton.SetActive(SelectedEquip.equipData.AmountOfEquip >= SelectedEquip.equipData.RequiredAmountForMerge && SelectedEquip.equipData.Grade != Equips.MaterialClass.Ancient);
            ChangeStarSetting(SelectedEquip.equipData.Level);
            setEquipInfoGUI();
        }

        //SetButton
        tabGroup.SelectTabbyIndex(Buttons[CurrentSet]);

        foreach(EquipFrame e in All_Equips) {
            e.setGUI();
        }

        setEquipStatsUI();
        setEquipAvailablilty();
    }

    public void setEquipStatsUI()
    {
        foreach(GameObject g in equipStats_obj) {
            g.SetActive(false);
        }

        if(SelectedEquip != null) {
            if(SelectedEquip.equipData.baseOption.option.damage != 0) {
                equipStats_obj[0].SetActive(true);
                equipStats_obj[0].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = SelectedEquip.equipData.FinalOption.option.damage.ToString("#.##");
            }
            if(SelectedEquip.equipData.baseOption.option.damagePercent != 0) {
                equipStats_obj[1].SetActive(true);
                equipStats_obj[1].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (SelectedEquip.equipData.FinalOption.option.damagePercent * 100).ToString("#.##") + "%";
            }
            if(SelectedEquip.equipData.baseOption.option.CritChance != 0) {
                equipStats_obj[2].SetActive(true);
                equipStats_obj[2].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (SelectedEquip.equipData.FinalOption.option.CritChance).ToString("#.##") + "%";
            }
            if(SelectedEquip.equipData.baseOption.option.CritDamage != 0) {
                equipStats_obj[3].SetActive(true);
                equipStats_obj[3].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (SelectedEquip.equipData.FinalOption.option.CritDamage * 100).ToString("#.##") + "%";
            }
            if(SelectedEquip.equipData.baseOption.option.AttackSpeed != 0) {
                equipStats_obj[4].SetActive(true);
                equipStats_obj[4].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (SelectedEquip.equipData.FinalOption.option.AttackSpeed * 100).ToString("#.##") + "%";
            }
            if(SelectedEquip.equipData.baseOption.option.BulletSpeed != 0) {
                equipStats_obj[5].SetActive(true);
                equipStats_obj[5].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (SelectedEquip.equipData.FinalOption.option.BulletSpeed * 100).ToString("#.##") + "%";
            }
            if(SelectedEquip.equipData.baseOption.option.StunPercent != 0) {
                equipStats_obj[6].SetActive(true);
                equipStats_obj[6].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (SelectedEquip.equipData.FinalOption.option.StunPercent).ToString("#.##") + "%";
            }
            if(SelectedEquip.equipData.baseOption.option.ExtraMoney != 0) {
                equipStats_obj[7].SetActive(true);
                equipStats_obj[7].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (SelectedEquip.equipData.FinalOption.option.ExtraMoney * 100).ToString("#.##") + "%";
            }
            if(SelectedEquip.equipData.baseOption.option.ExtraEXP != 0) {
                equipStats_obj[8].SetActive(true);
                equipStats_obj[8].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (SelectedEquip.equipData.FinalOption.option.ExtraEXP * 100).ToString("#.##") + "%";
            }
        }
    }

    public void unSelectAfterMerge()
    {
        SelectedEquip = null;
        Background.gameObject.SetActive(false);
        Background_BG.gameObject.SetActive(false);
        Glow.gameObject.SetActive(false);
        Light.gameObject.SetActive(false);
        TypeBorder.gameObject.SetActive(false);
        EquipImage.gameObject.SetActive(false);
        GradeText.gameObject.SetActive(false);
        LevelText.gameObject.SetActive(false);
        EquipNameText.gameObject.SetActive(false);
        Type.gameObject.SetActive(false);
        Border.gameObject.SetActive(false);
        EquipButton.SetActive(false);
        unEquipButton.SetActive(false);
        UpgradeButton.SetActive(false);
        JinhwaButton.SetActive(false);
    }

    public void EquipClicked(EquipFrame data)
    {
        SelectedEquip = data;

        Background.gameObject.SetActive(true);
        Background_BG.gameObject.SetActive(true);
        Glow.gameObject.SetActive(true);
        Light.gameObject.SetActive(true);
        TypeBorder.gameObject.SetActive(true);
        EquipImage.gameObject.SetActive(true);
        GradeText.gameObject.SetActive(true);
        LevelText.gameObject.SetActive(true);
        EquipNameText.gameObject.SetActive(true);
        Type.gameObject.SetActive(true);
        Border.gameObject.SetActive(true);

        Background.color = data.Background.color;
        Background_BG.color = data.Background_BG.color;
        Glow.color = data.Glow.color;
        Light.color = data.Light.color;
        TypeBorder.color = data.TypeBorder.color;
        Type.sprite = data.Type_Image.sprite;
        LevelText.text = "Lv." + data.equipData.Level;
        GradeText.text = data.equipData.gradeText;
        GradeText.color = data.Background_BG.color;
        EquipNameText.text = data.equipData.EquipName;
        EquipImage.sprite = data.equipData.Sprite_Equip;
        setEquipStatsUI();

        ChangeEquipsUI();
    }

    public void setEquipInfoGUI()
    {
        Background.color = SelectedEquip.Background.color;
        Background_BG.color = SelectedEquip.Background_BG.color;
        Glow.color = SelectedEquip.Glow.color;
        Light.color = SelectedEquip.Light.color;
        TypeBorder.color = SelectedEquip.TypeBorder.color;
        Type.sprite = SelectedEquip.Type_Image.sprite;
        LevelText.text = "Lv." + SelectedEquip.equipData.Level;
        GradeText.text = SelectedEquip.equipData.gradeText;
        GradeText.color = SelectedEquip.Background_BG.color;
        EquipNameText.text = SelectedEquip.equipData.EquipName;
        EquipImage.sprite = SelectedEquip.equipData.Sprite_Equip;
    }

    public void deSelectOthers(EquipFrame eq)
    {
        foreach(EquipFrame e in All_Equips) {
            if(e != eq) {
                e.transform.localScale = new Vector3(0.95f, 0.95f, 0.95f);
                e.Border.color = Color.black;
            }
        }

        ChangeEquipsUI();
    }
    public void EquipItem()
    {
        EquipingManager.Instance.SetEquipingItem(SelectedEquip);
        for(int i = 0; i < All_Equips.Count; i++) {
            if(All_Equips[i].equipData.TypeOfEquip == SelectedEquip.equipData.TypeOfEquip) {
                if(All_Equips[i] == SelectedEquip) {
                    All_Equips[i].isEquiped = true;
                } else {
                    All_Equips[i].isEquiped = false;
                }
            }
        }

        ChangeEquipsUI();
    }
    public void UnequipItem()
    {
        EquipingManager.Instance.UnequipItem(SelectedEquip.equipData.TypeOfEquip);
        for(int i = 0; i < All_Equips.Count; i++) {
            All_Equips[i].isEquiped = false;
        }

        ChangeEquipsUI();
    }
    public void UnequipAllItem()
    {
        EquipingManager.Instance.UnequipItem(Equips.Type.Ring);
        EquipingManager.Instance.UnequipItem(Equips.Type.Accessory);
        EquipingManager.Instance.UnequipItem(Equips.Type.Book);
        EquipingManager.Instance.UnequipItem(Equips.Type.Relics);
        EquipingManager.Instance.UnequipItem(Equips.Type.Necklace);
        for(int i = 0; i < All_Equips.Count; i++) {
            All_Equips[i].isEquiped = false;
        }

        ChangeEquipsUI();
    }

    //Add Methods
    public void addMergeEquip(EquipFrame data)
    {
        data.equipData.AmountOfEquip++;
    }

    public int getEquipNumber(EquipFrame data)
    {
        return All_Equips.Find(x => x.equipData.id == data.equipData.id).equipData.AmountOfEquip;
    }

    //Get Methods
    public EquipFrame getRandomItem_Grade(Equips.MaterialClass materialClass, Equips.Type EquipType)
    {
        List<EquipFrame> RandomList = new List<EquipFrame>();
        for(int i = 0; i < All_Equips.Count; i++) {
            if(All_Equips[i].equipData.Grade == materialClass && All_Equips[i].equipData.TypeOfEquip == EquipType) {
                RandomList.Add(All_Equips[i]);
            }
        }
        int randomIndex = Random.Range(0, RandomList.Count);

        return RandomList[randomIndex];
    }
    public int getEquipNumberByType(Equips.Type data)
    {
        int sum = 0;
        for(int i = 0; i < All_Equips.Count; i++) {
            if(All_Equips[i].equipData.TypeOfEquip == data && All_Equips[i].equipData.AmountOfEquip > 0)
                sum++;
        }
        return sum;
    }

    //Set Methods
    public void setSetTabInterace()
    {
        /*
        for(int i = 1; i < System.Enum.GetValues(typeof(Equips.SetType)).Length; i++) {
            datas.Clear();
            equipState.Clear();
            for(int j = 0; j < All_Equips.Count; j++) {
                if((Equips.SetType)i == All_Equips[j].setType) {
                    datas.Add(All_Equips[j]);

                    name = All_Equips[j].setNameInKorean;

                    if(All_Equips[j].isEquiped) {
                        equipState.Add(3);
                    } else if(All_Equips[j].AmountOfEquip > 0) {
                        equipState.Add(2);
                    } else {
                        equipState.Add(1);
                    }
                }
            }
            setDatas[i-1].setInterface(setSetNameInKorean((Equips.SetType)i), datas, equipState);
        }

        ReorderSets();
        */
    }
    public string setSetNameInKorean(Equips.SetType type)
    {
        switch(type) {
            case Equips.SetType.Aphrodite_Set:
                return "아프로디테 세트";
            case Equips.SetType.Ares_Set:
                return "아레스 세트";
            case Equips.SetType.Argos_Set:
                return "아르고스 세트";
            case Equips.SetType.Artemis_Set:
                return "아르테미스 세트";
            case Equips.SetType.Atena_Set:
                return "아테나 세트";
            case Equips.SetType.Beginner_Set:
                return "초보자 세트";
            case Equips.SetType.Blood_Set:
                return "혈의 세트";
            case Equips.SetType.Cerberus_Set:
                return "케르베로스 세트";
            case Equips.SetType.Dark_Set:
                return "어둠 세트";
            case Equips.SetType.Demeter_Set:
                return "****** 세트";
            case Equips.SetType.Devil_Set:
                return "악마 세트";
            case Equips.SetType.Evil_Set:
                return "악의 세트";
            case Equips.SetType.Gaea_Set:
                return "가이아 세트";
            case Equips.SetType.Gold_Set:
                return "황금 세트";
            case Equips.SetType.Hades_Set:
                return "하데스 세트";
            case Equips.SetType.Hephaistus_Set:
                return "헤파이스토스 세트";
            case Equips.SetType.Hercules_Set:
                return "헤라클레스 세트";
            case Equips.SetType.Hestia_Set:
                return "헤스티아 세트";
            case Equips.SetType.Honor_Set:
                return "정복자 세트";
            case Equips.SetType.Hunter_Set:
                return "사냥꾼 세트";
            case Equips.SetType.Hydra_Set:
                return "히드라 세트";
            case Equips.SetType.Ice_Set:
                return "서리 세트";
            case Equips.SetType.Iron_Set:
                return "철 세트";
            case Equips.SetType.Jungle_Set:
                return "정글 세트";
            case Equips.SetType.Magician_Set:
                return "마법사 세트";
            case Equips.SetType.ME_Set:
                return "ME 세트";
            case Equips.SetType.Pan_Set:
                return "판 세트";
            case Equips.SetType.Poseidon_Set:
                return "포세이돈 세트";
            case Equips.SetType.Sea_Set:
                return "바다 세트";
            case Equips.SetType.Warrior_Set:
                return "전사 세트";
            case Equips.SetType.Wood_Set:
                return "나무 세트";
            case Equips.SetType.Zeus_Set:
                return "제우스 세트";
        }
        return "";
    }
    public void setSetEffect(Equips.SetType type, EquipsOption option)
    {
        switch(type) {
            case Equips.SetType.Aphrodite_Set:
                option.option.CritChance = 15;
                option.option.damage = 30;
                break;
            case Equips.SetType.Ares_Set:
                option.option.CritChance = 15;
                break;
            case Equips.SetType.Argos_Set:
                option.option.CritChance = 15;
                break;
            case Equips.SetType.Artemis_Set:
                option.option.CritChance = 15;
                break;
            case Equips.SetType.Atena_Set:
                option.option.CritChance = 15;
                break;
            case Equips.SetType.Beginner_Set:
                option.option.CritChance = 15;
                break;
            case Equips.SetType.Blood_Set:
                option.option.CritChance = 15;
                break;
            case Equips.SetType.Cerberus_Set:
                option.option.CritChance = 15;
                break;
            case Equips.SetType.Dark_Set:
                option.option.CritChance = 15;
                break;
            case Equips.SetType.Demeter_Set:
                option.option.CritChance = 15;
                break;
            case Equips.SetType.Devil_Set:
                option.option.CritChance = 15;
                break;
            case Equips.SetType.Evil_Set:
                option.option.CritChance = 15;
                break;
            case Equips.SetType.Gaea_Set:
                option.option.CritChance = 15;
                break;
            case Equips.SetType.Gold_Set:
                option.option.CritChance = 15;
                break;
            case Equips.SetType.Hades_Set:
                option.option.CritChance = 15;
                break;
            case Equips.SetType.Hephaistus_Set:
                option.option.CritChance = 15;
                break;
            case Equips.SetType.Hercules_Set:
                option.option.CritChance = 15;
                break;
            case Equips.SetType.Hestia_Set:
                option.option.CritChance = 15;
                break;
            case Equips.SetType.Honor_Set:
                option.option.CritChance = 15;
                break;
            case Equips.SetType.Hunter_Set:
                option.option.CritChance = 15;
                break;
            case Equips.SetType.Hydra_Set:
                option.option.CritChance = 15;
                break;
            case Equips.SetType.Ice_Set:
                option.option.CritChance = 15;
                break;
            case Equips.SetType.Iron_Set:
                option.option.CritChance = 15;
                break;
            case Equips.SetType.Jungle_Set:
                option.option.CritChance = 15;
                break;
            case Equips.SetType.Magician_Set:
                option.option.CritChance = 15;
                break;
            case Equips.SetType.ME_Set:
                option.option.CritChance = 15;
                break;
            case Equips.SetType.Pan_Set:
                option.option.CritChance = 15;
                break;
            case Equips.SetType.Poseidon_Set:
                option.option.CritChance = 15;
                break;
            case Equips.SetType.Sea_Set:
                option.option.CritChance = 15;
                break;
            case Equips.SetType.Warrior_Set:
                option.option.CritChance = 15;
                break;
            case Equips.SetType.Wood_Set:
                option.option.CritChance = 15;
                break;
            case Equips.SetType.Zeus_Set:
                option.option.CritChance = 15;
                break;
        }
    }

    public void unSetEquipSet(int index)
    {
        Equip_Sets[CurrentSet].equipSet[index] = null;
    }
    public void setEquipSet(int index, EquipFrame data)
    {
        Equip_Sets[CurrentSet].equipSet[index] = data;
    }

    public void setEquipSetItem(int index)
    {
        CurrentSet = index;
        for(int i = 0; i < 5; i++) {
            if(Equip_Sets[index].equipSet[i] != null) {
                EquipingManager.Instance.SetEquipingItem(Equip_Sets[index].equipSet[i]);
            } else {
                if(i == 0)
                    EquipingManager.Instance.UnequipItem(Equips.Type.Ring);
                else if(i == 1)
                    EquipingManager.Instance.UnequipItem(Equips.Type.Necklace);
                else if(i == 2)
                    EquipingManager.Instance.UnequipItem(Equips.Type.Relics);
                else if(i == 3)
                    EquipingManager.Instance.UnequipItem(Equips.Type.Accessory);
                else if(i == 4)
                    EquipingManager.Instance.UnequipItem(Equips.Type.Book);
            }
        }

        ChangeEquipsUI();
    }

    public void addEquipByID(string id)
    {
        foreach(EquipFrame e in All_Equips) {
            if(e.equipData.id == id) {
                e.equipData.AmountOfEquip++;
            }
        }
    }

    //Data Save
    public void LoadData(GameData data)
    {
        for(int i = 0; i < All_Equips.Count; i++) {
            data.Equip_Amount.TryGetValue(All_Equips[i].equipData.id, out int amount);
            All_Equips[i].equipData.AmountOfEquip = amount;
            if(amount >= 1) {
                All_Equips[i].equipData.setEquipInfo();
            }
        }

        //레벨
        for(int i = 0; i < All_Equips.Count; i++) {
            data.Equip_Level.TryGetValue(All_Equips[i].equipData.id, out int level);
            All_Equips[i].equipData.Level = level;
        }

        for(int i = 0; i < All_Equips.Count; i++) {
            data.Equip_IsEquiped.TryGetValue(All_Equips[i].equipData.id, out bool value);
            if(value) {
                EquipingManager.Instance.SetEquipingItem(All_Equips[i]);
                All_Equips[i].isEquiped = true;
            }
        }

        //스탯
        for(int i = 0; i < All_Equips.Count; i++) {
            for(int j = 0; j < 20; j++) {
                data.Equip_UpgradeIndex.TryGetValue(All_Equips[i].equipData.id + "UpgradeIndex" + j, out string value);
                All_Equips[i].equipData.UpgradeIndex[j] = value;
            }

            All_Equips[i].equipData.setExtraOptionStats();
            All_Equips[i].equipData.setFinalOptionStats();
        }

        for(int i = 0; i < Equip_Sets.Count; i++) {
            for(int j = 0; j < Equip_Sets[i].equipSet.Count; j++) {
                data.Equip_SetData.TryGetValue("setofthedatas" + i + j, out string EquipID);

                if(EquipID != "null") {
                    Equip_Sets[i].equipSet[j] = All_Equips.Find(x => x.equipData.id == EquipID);
                }
            }
        }

        CurrentSet = data.currentSetIndex;

        setEquipAvailablilty();
        setEquipSetItem(CurrentSet);
    }
    public void SaveData(GameData data)
    {
        //레벨
        for(int i = 0; i < All_Equips.Count; i++) {
            if(data.Equip_Level.ContainsKey(All_Equips[i].equipData.id))
                data.Equip_Level.Remove(All_Equips[i].equipData.id);

            data.Equip_Level.Add(All_Equips[i].equipData.id, All_Equips[i].equipData.Level);
        }
        for(int i = 0; i < All_Equips.Count; i++) {
            if(data.Equip_Amount.ContainsKey(All_Equips[i].equipData.id))
                data.Equip_Amount.Remove(All_Equips[i].equipData.id);

            data.Equip_Amount.Add(All_Equips[i].equipData.id, All_Equips[i].equipData.AmountOfEquip);
        }

        //장착했는지
        for(int i = 0; i < All_Equips.Count; i++) {
            if(data.Equip_IsEquiped.ContainsKey(All_Equips[i].equipData.id))
                data.Equip_IsEquiped.Remove(All_Equips[i].equipData.id);

            data.Equip_IsEquiped.Add(All_Equips[i].equipData.id, All_Equips[i].isEquiped);
        }

        //스탯
        for(int i = 0; i < All_Equips.Count; i++) {
            for(int j = 0; j < 20; j++) {
                if(data.Equip_UpgradeIndex.ContainsKey(All_Equips[i].equipData.id + "UpgradeIndex" + j))
                    data.Equip_UpgradeIndex.Remove(All_Equips[i].equipData.id + "UpgradeIndex" + j);

                data.Equip_UpgradeIndex.Add(All_Equips[i].equipData.id + "UpgradeIndex" + j, All_Equips[i].equipData.UpgradeIndex[j]);
            }
        }

        //Set 
        for(int i = 0; i < Equip_Sets.Count; i++) {
            for(int j = 0; j < Equip_Sets[i].equipSet.Count; j++) {
                if(data.Equip_SetData.ContainsKey("setofthedatas" + i + j))
                    data.Equip_SetData.Remove("setofthedatas" + i + j);

                if(Equip_Sets[i].equipSet[j] != null) {
                    data.Equip_SetData.Add("setofthedatas" + i + j, Equip_Sets[i].equipSet[j].equipData.id);
                } else {
                    data.Equip_SetData.Add("setofthedatas" + i + j, "null");
                }
            }
        }

        data.currentSetIndex = CurrentSet;
    }
}

[System.Serializable]
public class EquipSet
{
    public List<EquipFrame> equipSet;
}
