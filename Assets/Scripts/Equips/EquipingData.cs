using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipingData : MonoBehaviour
{
    public EquipFrame equipment;

    [Header("#-----Base Image")]
    public Sprite typeSprite;
    public Sprite BaseEquipImage;

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

    private void Awake()
    {
        if(equipment == null) {
            Image_Equip.sprite = BaseEquipImage;
            Image_Equip.color = new Color(0, 0, 0, 0.3f);
            LevelText_Equip.text = "";
            Type_Image.sprite = typeSprite;
            Background.color = new Color(0.3803922f, 0.4941176f, 0.5411765f);
            Background_BG.color = new Color(0.4431373f, 0.5568628f, 0.6f);
            Glow.color = new Color(0.5137255f, 0.6509804f, 0.7058824f);
            TypeBorder.color = new Color(0.3803922f, 0.4941176f, 0.5411765f);
            Light.color = new Color(0.509804f, 0.627451f, 0.6705883f);
        }
    }

    private void Start()
    {
        InvokeRepeating("setGUI", 0, 0.03f);
    }

    public void setGUI()
    {
        if(equipment != null) {
            if(equipment.equipData.AmountOfEquip <= 0) {
                unEquipItem();
                EquipManager.Instance.unSelectAfterMerge();
                return;
            }
            Background.color = equipment.Background.color;
            Background_BG.color = equipment.Background_BG.color;
            Glow.color = equipment.Glow.color;
            Light.color = equipment.Light.color;
            TypeBorder.color = equipment.TypeBorder.color;
            Type_Image.sprite = equipment.Type_Image.sprite;
            LevelText_Equip.text = "Lv." + equipment.equipData.Level;
            Image_Equip.sprite = equipment.equipData.Sprite_Equip;
            Image_Equip.color = new Color(1, 1, 1, 1f);
        } else {
            Image_Equip.sprite = BaseEquipImage;
            Image_Equip.color = new Color(0, 0, 0, 0.3f);
            LevelText_Equip.text = "";
            Type_Image.sprite = typeSprite;
            Background.color = new Color(0.3803922f, 0.4941176f, 0.5411765f);
            Background_BG.color = new Color(0.4431373f, 0.5568628f, 0.6f);
            Glow.color = new Color(0.5137255f, 0.6509804f, 0.7058824f);
            TypeBorder.color = new Color(0.3803922f, 0.4941176f, 0.5411765f);
            Light.color = new Color(0.509804f, 0.627451f, 0.6705883f);
        }
    }

    public void SetEquipment(EquipFrame data)
    {
        equipment = data;
    }
    public void unEquipItem()
    {
        equipment = null;
    }

    public void EquipUISelect()
    {
        if(equipment != null) {
            equipment.isSelected = true;
            EquipManager.Instance.EquipClicked(equipment);
            EquipManager.Instance.deSelectOthers(equipment);
            equipment.transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
            equipment.Border.color = new Color(0.2649773f, 0.7075472f, 0);
            SoundManager.Instance.playClickSFX();
        }
    }
}
