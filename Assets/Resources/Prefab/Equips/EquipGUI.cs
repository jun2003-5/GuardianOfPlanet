using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipGUI : MonoBehaviour
{

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

    public void setGUI(EquipFrame equipment)
    {
        Background.color = equipment.Background.color;
        Background_BG.color = equipment.Background_BG.color;
        Glow.color = equipment.Glow.color;
        Light.color = equipment.Light.color;
        TypeBorder.color = equipment.TypeBorder.color;
        Type_Image.sprite = equipment.Type_Image.sprite;
        LevelText_Equip.text = "Lv." + equipment.equipData.Level;
        Image_Equip.sprite = equipment.equipData.Sprite_Equip;
        Image_Equip.color = new Color(1, 1, 1, 1f);
    }

    public void setGUIFixedLevel(EquipFrame equipment, int level)
    {
        Background.color = equipment.Background.color;
        Background_BG.color = equipment.Background_BG.color;
        Glow.color = equipment.Glow.color;
        Light.color = equipment.Light.color;
        TypeBorder.color = equipment.TypeBorder.color;
        Type_Image.sprite = equipment.Type_Image.sprite;
        LevelText_Equip.text = "Lv." + level;
        Image_Equip.sprite = equipment.equipData.Sprite_Equip;
        Image_Equip.color = new Color(1, 1, 1, 1f);
    }
}