using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UpgradeStone : MonoBehaviour
{
    [SerializeField] public string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public enum TypeOfStone { Ancient, Legendary, Unique, Epic, Rare, Normal };

    public TypeOfStone stoneGrade;

    public int StoneAmount;
    public int EXPamount;  

    private void Start()
    {
        SetStoneData();
    }



    public void SetStoneData()
    {
        switch(stoneGrade) {
            case TypeOfStone.Normal:
                EXPamount = 5;
                break;
            case TypeOfStone.Rare:
                EXPamount = 20;
                break;
            case TypeOfStone.Epic:
                EXPamount = 50;
                break;
            case TypeOfStone.Unique:
                EXPamount = 350;
                break;
            case TypeOfStone.Legendary:
                EXPamount = 1500;
                break;
            case TypeOfStone.Ancient:
                EXPamount = 5000;
                break;
        }
    }   
}
