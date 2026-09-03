using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SetEquip
{
    [HideInInspector]
    public Equips equip;
    public GameObject Game_Object;
    public Image Border_Image;
    public GameObject Background_Ancient;
    public GameObject Background_Legendary;
    public GameObject Background_Unique;
    public GameObject Background_Epic;
    public GameObject Background_Rare;
    public Image EquipImage;
}
