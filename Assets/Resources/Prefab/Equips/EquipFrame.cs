using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipFrame : MonoBehaviour, IPointerClickHandler, IPointerExitHandler
{
    [HideInInspector]
    public Equips equipData;
    [Header("#-----Equip Image&Text")]
    [Space(5)]
    public Image Image_Equip;
    public TextMeshProUGUI LevelText_Equip;
    public Image Type_Image;

    [Header("#-----Equip UI")]
    [Space(5)]
    public Image Background;
    public Image Background_BG;
    public Image Border;
    public Image Light;
    public Image Glow;
    public Image TypeBorder;

    [Header("#-----Equip Slider")]
    [Space(5)]
    public Slider Slider_Amount;
    public TextMeshProUGUI Slider_Text;

    [HideInInspector]
    public bool isSelected;
    [HideInInspector]
    public bool isEquiped;


    public void setEquipFrameUI(Equips equipData, Sprite EquipType)
    {
        this.equipData = equipData;
        Image_Equip.sprite = equipData.Sprite_Equip;
        Type_Image.sprite = EquipType;
        LevelText_Equip.text = "Lv." + equipData.Level;
        getEquipFrameColor(equipData.Grade);

        setSliderValue();
    }

    public void setGUI()
    {
        equipData.setFinalOptionStats();
        Image_Equip.sprite = equipData.Sprite_Equip;
        LevelText_Equip.text = "Lv." + equipData.Level;
        getEquipFrameColor(equipData.Grade);
        setSliderValue();
    }

    public void getEquipFrameColor(Equips.MaterialClass grade)
    {
        switch(grade) {
            case Equips.MaterialClass.Normal:
                Background.color = new Color(0.3803922f, 0.4941176f, 0.5411765f);
                Background_BG.color = new Color(0.4431373f, 0.5568628f, 0.6f);
                Glow.color = new Color(0.5137255f, 0.6509804f, 0.7058824f);
                TypeBorder.color = new Color(0.3803922f, 0.4941176f, 0.5411765f);
                Light.color = new Color(0.509804f, 0.627451f, 0.6705883f);
                break;
            case Equips.MaterialClass.Rare:
                Background.color = new Color(0, 0.6588235f, 1);
                Background_BG.color = new Color(0.172549f, 0.7450981f, 1);
                Glow.color = new Color(0.03137255f, 0.9372549f, 1);
                TypeBorder.color = new Color(0.05490196f, 0.509804f, 0.9764706f);
                Light.color = new Color(0.2078431f, 0.9843137f, 1);
                break;
            case Equips.MaterialClass.Epic:
                Background.color = new Color(0.6980392f, 0.3764706f, 0.9921569f);
                Background_BG.color = new Color(0.7843137f, 0.5019608f, 0.9960784f);
                Glow.color = new Color(0.7254902f, 0.5882353f, 1);
                TypeBorder.color = new Color(0.6392157f, 0.2431373f, 1);
                Light.color = new Color(1, 0.5411765f, 1);
                break;
            case Equips.MaterialClass.Unique:
                Background.color = new Color(0.8679245f, 0.6819407f, 0);
                Background_BG.color = new Color(0.9433962f, 0.8213097f, 0);
                Glow.color = new Color(0.8884411f, 0.9433962f, 0);
                TypeBorder.color = new Color(1, 0.8705882f, 0);
                Light.color = new Color(1, 0.9960785f, 0);
                break;
            case Equips.MaterialClass.Legendary:
                Background.color = new Color(0.8745098f, 0.1843137f, 0.2196078f);
                Background_BG.color = new Color(0.9921569f, 0.2784314f, 0.2941177f);
                Glow.color = new Color(1, 0.6156863f, 0.6431373f);
                TypeBorder.color = new Color(1, 0.1882353f, 0.3019608f);
                Light.color = new Color(1, 0.5607843f, 0.6117647f);
                break;
            case Equips.MaterialClass.Ancient:
                Background.color = new Color(0.8679245f, 0.4799112f, 0);
                Background_BG.color = new Color(0.8113208f, 0.3971962f, 0);
                Glow.color = new Color(0.8773585f, 0.2940994f, 0);
                TypeBorder.color = new Color(1, 0.4712134f, 0.0235849f);
                Light.color = new Color(1, 0.5529412f, 0);
                break;
        }
    }

    public void setSliderValue()
    {
        Slider_Amount.maxValue = equipData.RequiredAmountForMerge;
        Slider_Amount.value = equipData.AmountOfEquip;

        Slider_Text.text = equipData.AmountOfEquip + "/" + Slider_Amount.maxValue;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left) {
            isSelected = true;
            EquipManager.Instance.EquipClicked(this);
            EquipManager.Instance.deSelectOthers(this);
            transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
            Border.color = new Color(0.2649773f, 0.7075472f, 0);
        }
        SoundManager.Instance.playClickSFX();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!isSelected)
            transform.localScale = new Vector3(0.95f, 0.95f, 0.95f);
    }
}
