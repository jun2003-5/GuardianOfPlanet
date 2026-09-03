using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopData : MonoBehaviour
{
    [SerializeField] public string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public enum TypeOfShop {Package, AutoFarm, GameSpeed, Diamond, Gold, SpaceStone};
    public TypeOfShop _typeOfShop;

    public enum PurchaseType {Diamond, RealMoney};
    public PurchaseType _TypeOfPurchase;

    public int Value;
    public int Price;

    [Header("Can Buy Multiple")]
    public bool islimitBuy;
    public bool isBought;

    public void setGUI()
    {
        if(islimitBuy) {
            if(isBought) {
                gameObject.SetActive(false);
            }
        }
    }
}
