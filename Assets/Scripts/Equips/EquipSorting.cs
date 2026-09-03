using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipSorting : MonoBehaviour
{
    public Transform parentObject;

    public List<Equips> allItems;
    public Equips[] LegendaryEquips;
    public Equips[] UniqueEquips;
    public Equips[] EpicEquips;
    public Equips[] RareEquips;
    public Equips[] NormalEquips;

    public List<EquipFrame> Have_Items;

    public string resourcePath;

    private void Awake()
    {
        //¹ÝÁö
        allItems.AddRange(Resources.LoadAll<Equips>("Prefab/Equips/" + resourcePath));
        LegendaryEquips = Resources.LoadAll<Equips>("Prefab/Equips/"+ resourcePath  +"/Legendary");
        UniqueEquips = Resources.LoadAll<Equips>("Prefab/Equips/"+resourcePath+"/Unique");
        EpicEquips = Resources.LoadAll<Equips>("Prefab/Equips/"+resourcePath+"/Epic");
        RareEquips = Resources.LoadAll<Equips>("Prefab/Equips/"+resourcePath+"/Rare");
        NormalEquips = Resources.LoadAll<Equips>("Prefab/Equips/"+resourcePath+"/Normal");
    }
    private void Update()
    {
        for(var i = Have_Items.Count - 1; i > -1; i--) {
            if(Have_Items[i] == null)
                Have_Items.RemoveAt(i);
        }
        SortOrder();
    }

    public void SortOrder()
    {
        var sortedList = Have_Items.OrderByDescending(x => x.equipData.id).ToList();
        Have_Items = sortedList.OrderByDescending(x => x.equipData.AmountOfEquip).ToList();
        Have_Items = Have_Items.OrderByDescending(x => (int)x.equipData.Grade).ToList();

        List<EquipFrame> Ancient = setListbyGrade(Equips.MaterialClass.Ancient);
        List<EquipFrame> Legendary = setListbyGrade(Equips.MaterialClass.Legendary);
        List<EquipFrame> Unique = setListbyGrade(Equips.MaterialClass.Unique);
        List<EquipFrame> Epic = setListbyGrade(Equips.MaterialClass.Epic);
        List<EquipFrame> Rare = setListbyGrade(Equips.MaterialClass.Rare);
        List<EquipFrame> Normal = setListbyGrade(Equips.MaterialClass.Normal);
        Have_Items.Clear();
        Have_Items.AddRange(Ancient);
        Have_Items.AddRange(Legendary);
        Have_Items.AddRange(Unique);
        Have_Items.AddRange(Epic);
        Have_Items.AddRange(Rare);
        Have_Items.AddRange(Normal);

        
        for(int i = 0; i < sortedList.Count; i++) {
            Have_Items[i].gameObject.transform.SetSiblingIndex(i);
        }
    }

    public List<EquipFrame> setListbyGrade(Equips.MaterialClass clas)
    {
        List<EquipFrame> ret = new List<EquipFrame>();
        for(int i = 0; i < Have_Items.Count; i++) {
            if(Have_Items[i].equipData.Grade == clas) {
                ret.Add(Have_Items[i]);
            }
        }
        ret.Sort(SortByscore);
        return ret;
    }
    static int SortByscore(EquipFrame e1, EquipFrame e2)
    {
        if(e1.equipData.id == e2.equipData.id)
            return -e1.equipData.Level.CompareTo(e2.equipData.Level);

        return 0;
    }

    public List<Equips> getAvailableEquip()
    {
        List<Equips> equipList = new List<Equips>();
        foreach(Equips e in allItems) {
            if(e.AmountOfEquip > 0) {
                equipList.Add(e);
            }
        }
        return equipList;
    }

    public void RemoveFromList(Equips equip)
    {
        for(int i = 0; i < Have_Items.Count; i++) {
            if(Have_Items[i] == equip) {
                Have_Items.RemoveAt(i);
            }
        }
    }
}
