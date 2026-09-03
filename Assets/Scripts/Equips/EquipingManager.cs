using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EquipingManager : MonoBehaviour
{
    public static EquipingManager Instance;

    //Equip 1
    public EquipingData Ring;

    //Equip 2
    public EquipingData Necklace;

    //Equip 3
    public EquipingData Accessory;

    //Equip 4
    public EquipingData Relics;

    //Equip 5
    public EquipingData Book;

    public EquipingData[] StatEquips;

    public float Attack_Damage_Equip;
    public float Attack_DamagePercent_Equip;
    public float Attack_Speed_Equip;
    public float Bullet_Speed_Equip;
    public float Critical_Chance_Equip;
    public float Critical_Damage_Equip;
    public float StunPower_Equip;
    public float ExtraGoldByNumber_Equip;
    public float ExtraEXPByNumber_Equip;
    public float ExtraGold_Equip;
    public float ExtraEXP_Equip;

    private void Awake()
    {
        Instance = this;
    }

    
    public void SetEquipingItem(EquipFrame equip)
    {
        switch(equip.equipData.TypeOfEquip) {
            case Equips.Type.Ring:
                EquipManager.Instance.setEquipSet(0, equip);
                Ring.SetEquipment(equip);
                break;
            case Equips.Type.Necklace:
                EquipManager.Instance.setEquipSet(1, equip);
                Necklace.SetEquipment(equip);
                break;
            case Equips.Type.Accessory:
                EquipManager.Instance.setEquipSet(3, equip);
                Accessory.SetEquipment(equip);
                break;
            case Equips.Type.Relics:
                EquipManager.Instance.setEquipSet(2, equip);
                Relics.SetEquipment(equip);
                break;
            case Equips.Type.Book:
                EquipManager.Instance.setEquipSet(4, equip);
                Book.SetEquipment(equip);
                break;
        }
    }

    public void UnequipItem(Equips.Type type)
    {
        if(type == Equips.Type.Ring) {
            EquipManager.Instance.unSetEquipSet(0);
            Ring.unEquipItem();
        } else if(type == Equips.Type.Necklace) {
            EquipManager.Instance.unSetEquipSet(1);
            Necklace.unEquipItem();
        } else if(type == Equips.Type.Accessory) {
            EquipManager.Instance.unSetEquipSet(3);
            Accessory.unEquipItem();
        } else if(type == Equips.Type.Relics) {
            EquipManager.Instance.unSetEquipSet(2);
            Relics.unEquipItem();
        } else if(type == Equips.Type.Book) {
            EquipManager.Instance.unSetEquipSet(4);
            Book.unEquipItem();
        }
    }

    public void CopyEquips()
    {
        StatEquips[0].SetEquipment(Ring.equipment);
        StatEquips[1].SetEquipment(Necklace.equipment);
        StatEquips[2].SetEquipment(Accessory.equipment);
        StatEquips[3].SetEquipment(Relics.equipment);
        StatEquips[4].SetEquipment(Book.equipment);
    }

    private void Update()
    {
        setEquipDamage();
    }

    public void setEquipDamage()
    {
        Attack_Damage_Equip = 0;
        Attack_DamagePercent_Equip = 0;
        Bullet_Speed_Equip = 0;
        Attack_Speed_Equip = 0;
        Critical_Chance_Equip = 0;
        Critical_Damage_Equip = 0;
        StunPower_Equip = 0;
        ExtraEXP_Equip = 0;
        ExtraGold_Equip = 0;

        // For Ring
        if(Ring.equipment != null) {
            Attack_Damage_Equip += Ring.equipment.equipData.FinalOption.option.damage;
            Attack_DamagePercent_Equip += Ring.equipment.equipData.FinalOption.option.damagePercent;
            Bullet_Speed_Equip += Ring.equipment.equipData.FinalOption.option.BulletSpeed;
            Attack_Speed_Equip += Ring.equipment.equipData.FinalOption.option.AttackSpeed;
            Critical_Chance_Equip += Ring.equipment.equipData.FinalOption.option.CritChance;
            Critical_Damage_Equip += Ring.equipment.equipData.FinalOption.option.CritDamage;
            StunPower_Equip += Ring.equipment.equipData.FinalOption.option.StunPercent;
            ExtraEXP_Equip += Ring.equipment.equipData.FinalOption.option.ExtraEXP;
            ExtraGold_Equip += Ring.equipment.equipData.FinalOption.option.ExtraMoney;
        }

        // For Necklace
        if(Necklace.equipment != null) {
            Attack_Damage_Equip += Necklace.equipment.equipData.FinalOption.option.damage;
            Attack_DamagePercent_Equip += Necklace.equipment.equipData.FinalOption.option.damagePercent;
            Bullet_Speed_Equip += Necklace.equipment.equipData.FinalOption.option.BulletSpeed;
            Attack_Speed_Equip += Necklace.equipment.equipData.FinalOption.option.AttackSpeed;
            Critical_Chance_Equip += Necklace.equipment.equipData.FinalOption.option.CritChance;
            Critical_Damage_Equip += Necklace.equipment.equipData.FinalOption.option.CritDamage;
            StunPower_Equip += Necklace.equipment.equipData.FinalOption.option.StunPercent;
            ExtraEXP_Equip += Necklace.equipment.equipData.FinalOption.option.ExtraEXP;
            ExtraGold_Equip += Necklace.equipment.equipData.FinalOption.option.ExtraMoney;
        }

        // For Accessory
        if(Accessory.equipment != null) {
            Attack_Damage_Equip += Accessory.equipment.equipData.FinalOption.option.damage;
            Attack_DamagePercent_Equip += Accessory.equipment.equipData.FinalOption.option.damagePercent;
            Bullet_Speed_Equip += Accessory.equipment.equipData.FinalOption.option.BulletSpeed;
            Attack_Speed_Equip += Accessory.equipment.equipData.FinalOption.option.AttackSpeed;
            Critical_Chance_Equip += Accessory.equipment.equipData.FinalOption.option.CritChance;
            Critical_Damage_Equip += Accessory.equipment.equipData.FinalOption.option.CritDamage;
            StunPower_Equip += Accessory.equipment.equipData.FinalOption.option.StunPercent;
            ExtraEXP_Equip += Accessory.equipment.equipData.FinalOption.option.ExtraEXP;
            ExtraGold_Equip += Accessory.equipment.equipData.FinalOption.option.ExtraMoney;
        }

        // For Relics
        if(Relics.equipment != null) {
            Attack_Damage_Equip += Relics.equipment.equipData.FinalOption.option.damage;
            Attack_DamagePercent_Equip += Relics.equipment.equipData.FinalOption.option.damagePercent;
            Bullet_Speed_Equip += Relics.equipment.equipData.FinalOption.option.BulletSpeed;
            Attack_Speed_Equip += Relics.equipment.equipData.FinalOption.option.AttackSpeed;
            Critical_Chance_Equip += Relics.equipment.equipData.FinalOption.option.CritChance;
            Critical_Damage_Equip += Relics.equipment.equipData.FinalOption.option.CritDamage;
            StunPower_Equip += Relics.equipment.equipData.FinalOption.option.StunPercent;
            ExtraEXP_Equip += Relics.equipment.equipData.FinalOption.option.ExtraEXP;
            ExtraGold_Equip += Relics.equipment.equipData.FinalOption.option.ExtraMoney;
        }

        // For Book
        if(Book.equipment != null) {
            Attack_Damage_Equip += Book.equipment.equipData.FinalOption.option.damage;
            Attack_DamagePercent_Equip += Book.equipment.equipData.FinalOption.option.damagePercent;
            Bullet_Speed_Equip += Book.equipment.equipData.FinalOption.option.BulletSpeed;
            Attack_Speed_Equip += Book.equipment.equipData.FinalOption.option.AttackSpeed;
            Critical_Chance_Equip += Book.equipment.equipData.FinalOption.option.CritChance;
            Critical_Damage_Equip += Book.equipment.equipData.FinalOption.option.CritDamage;
            StunPower_Equip += Book.equipment.equipData.FinalOption.option.StunPercent;
            ExtraEXP_Equip += Book.equipment.equipData.FinalOption.option.ExtraEXP;
            ExtraGold_Equip += Book.equipment.equipData.FinalOption.option.ExtraMoney;
        }
    }

}

