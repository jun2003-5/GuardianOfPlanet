using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Purchasing;


public class IAPManager : MonoBehaviour, IStoreListener
{
    public static IAPManager instance;

    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    [Header("Non-Consumable Gameobjects")]
    public GameObject GameSpeedx2_Obj;

    public notification PlanetClearTab;

    //Step 1 create your products
    [Header("#----Diamond Products")]
    [Space(7)]
    public string Diamond_300;
    public string Diamond_500;
    public string Diamond_1000;
    public string Diamond_2000;
    public string Diamond_3000;
    public string Diamond_5000;
    public string Diamond_10000;
    public string Diamond_30000;
    public string Diamond_50000;
    public string Diamond_3000_50sale;
    [Header("#----Diamond Package")]
    public string Diamond_Package_1;
    public string Diamond_Package_2;
    public string Diamond_Package_3;
    public string Diamond_Package_4;

    [Header("#----Space Stone")]
    [Space(7)]
    public string SpaceStone_10;
    public string SpaceStone_30;
    public string SpaceStone_50;
    public string SpaceStone_100;
    public string SpaceStone_300;
    public string SpaceStone_500;


    [Header("#-----Auto Attack Products")]
    [Space(7)]
    public string GameSpeedx2;
    public string AutoAttack_1hours;
    public string AutoAttack_2hours;
    public string AutoAttack_3hours;
    public string AutoAttack_5hours;
    public string AutoAttack_7hours;
    public string AutoAttack_10hours;
    public string AutoAttack_3hours_Sale;
    [Header("#-----Auto Attack Package")]
    public string AutoAttack_Package_1;
    public string AutoAttack_Package_2;
    public string AutoAttack_Package_3;

    [Header("#-----Equip Products")]
    [Space(7)]
    public string allEquip_Gacha;
    public string Ring_Gacha;
    public string Necklace_Gacha;
    public string Relics_Gacha;
    public string Accessory_Gacha;
    public string Book_Gacha;
    [Header("#-----Equip Package")]
    public string Ring_Gacha_Package;
    public string Necklace_Gacha_Package;
    public string Relics_Gacha_Package;
    public string Accessory_Gacha_Package;
    public string Book_Gacha_Package;

    [Header("#----Starter Package")]
    [Space(7)]
    public string StarterPackage_1;
    public string StarterPackage_2;
    public string StarterPackage_3;

    [Header("#-----Event Packages")]
    public string releaseEventPackage;
    public string earthSupplyPackage;
    public string bestQualityEquipPackage;
    public string planetClearPackage;

    [Header("#----Auto Exchange Tab")]
    public string autoExchangeProduct;

    [Header("----No ads")]
    public string noAdsProduct;

    //************************** Adjust these methods **************************************
    public void InitializePurchasing()
    {
        if(IsInitialized()) { return; }
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        //Diamond Products
        builder.AddProduct(Diamond_300, ProductType.Consumable);
        builder.AddProduct(Diamond_500, ProductType.Consumable);
        builder.AddProduct(Diamond_1000, ProductType.Consumable);
        builder.AddProduct(Diamond_2000, ProductType.Consumable);
        builder.AddProduct(Diamond_3000, ProductType.Consumable);
        builder.AddProduct(Diamond_5000, ProductType.Consumable);
        builder.AddProduct(Diamond_10000, ProductType.Consumable);
        builder.AddProduct(Diamond_30000, ProductType.Consumable);
        builder.AddProduct(Diamond_50000, ProductType.Consumable);
        builder.AddProduct(Diamond_3000_50sale, ProductType.Consumable);
        builder.AddProduct(Diamond_Package_1, ProductType.Consumable);
        builder.AddProduct(Diamond_Package_2, ProductType.Consumable);
        builder.AddProduct(Diamond_Package_3, ProductType.Consumable);
        builder.AddProduct(Diamond_Package_4, ProductType.Consumable);

        //SpaceStone
        builder.AddProduct(SpaceStone_10, ProductType.Consumable);
        builder.AddProduct(SpaceStone_30, ProductType.Consumable);
        builder.AddProduct(SpaceStone_50, ProductType.Consumable);
        builder.AddProduct(SpaceStone_100, ProductType.Consumable);
        builder.AddProduct(SpaceStone_300, ProductType.Consumable);
        builder.AddProduct(SpaceStone_500, ProductType.Consumable);

        //Auto Attack Product
        builder.AddProduct(GameSpeedx2, ProductType.Consumable);
        builder.AddProduct(AutoAttack_1hours, ProductType.Consumable);
        builder.AddProduct(AutoAttack_2hours, ProductType.Consumable);
        builder.AddProduct(AutoAttack_3hours, ProductType.Consumable);
        builder.AddProduct(AutoAttack_5hours, ProductType.Consumable);
        builder.AddProduct(AutoAttack_7hours, ProductType.Consumable);
        builder.AddProduct(AutoAttack_10hours, ProductType.Consumable);
        builder.AddProduct(AutoAttack_3hours_Sale, ProductType.Consumable);
        builder.AddProduct(AutoAttack_Package_1, ProductType.Consumable);
        builder.AddProduct(AutoAttack_Package_2, ProductType.Consumable);
        builder.AddProduct(AutoAttack_Package_3, ProductType.Consumable);

        //Equip Product
        builder.AddProduct(allEquip_Gacha, ProductType.Consumable);
        builder.AddProduct(Ring_Gacha, ProductType.Consumable);
        builder.AddProduct(Necklace_Gacha, ProductType.Consumable);
        builder.AddProduct(Relics_Gacha, ProductType.Consumable);
        builder.AddProduct(Accessory_Gacha, ProductType.Consumable);
        builder.AddProduct(Book_Gacha, ProductType.Consumable);
        builder.AddProduct(Ring_Gacha_Package, ProductType.Consumable);
        builder.AddProduct(Necklace_Gacha_Package, ProductType.Consumable);
        builder.AddProduct(Relics_Gacha_Package, ProductType.Consumable);
        builder.AddProduct(Accessory_Gacha_Package, ProductType.Consumable);
        builder.AddProduct(Book_Gacha_Package, ProductType.Consumable);

        //Starter Package
        builder.AddProduct(StarterPackage_1, ProductType.Consumable);
        builder.AddProduct(StarterPackage_2, ProductType.Consumable);
        builder.AddProduct(StarterPackage_3, ProductType.Consumable);

        //Event Package
        builder.AddProduct(releaseEventPackage, ProductType.Consumable);
        builder.AddProduct(earthSupplyPackage, ProductType.Consumable);
        builder.AddProduct(bestQualityEquipPackage, ProductType.Consumable);
        builder.AddProduct(planetClearPackage, ProductType.Consumable);

        //AutoExchange Tab
        builder.AddProduct(autoExchangeProduct, ProductType.Consumable);

        //No Ads
        builder.AddProduct(noAdsProduct, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    //Diamond Products
    public void buyDiamond300() { BuyProductID(Diamond_300); }
    public void buyDiamond500() { BuyProductID(Diamond_500); }
    public void buyDiamond1000() { BuyProductID(Diamond_1000); }
    public void buyDiamond2000() { BuyProductID(Diamond_2000); }
    public void buyDiamond3000() { BuyProductID(Diamond_3000); }
    public void buyDiamond5000() { BuyProductID(Diamond_5000); }
    public void buyDiamond10000() { BuyProductID(Diamond_10000); }
    public void buyDiamond30000() { BuyProductID(Diamond_30000); }
    public void buyDiamond50000() { BuyProductID(Diamond_50000); }
    public void buyDiamond300050Sale() { BuyProductID(Diamond_3000_50sale); }
    public void buyDiamond_Package_1() { BuyProductID(Diamond_Package_1); }
    public void buyDiamond_Package_2() { BuyProductID(Diamond_Package_2); }
    public void buyDiamond_Package_3() { BuyProductID(Diamond_Package_3); }
    public void buyDiamond_Package_4() { BuyProductID(Diamond_Package_4); }

    //Space Stone
    public void buySpaceStone10() { BuyProductID(SpaceStone_10); }
    public void buySpaceStone30() { BuyProductID(SpaceStone_30); }
    public void buySpaceStone50() { BuyProductID(SpaceStone_50); }
    public void buySpaceStone100() { BuyProductID(SpaceStone_100); }
    public void buySpaceStone300() { BuyProductID(SpaceStone_300); }
    public void buySpaceStone500() { BuyProductID(SpaceStone_500); }


    //Auto Attack Products
    public void buyGamespeedx2() { BuyProductID(GameSpeedx2); }
    public void buyAutoAttack1hour() { BuyProductID(AutoAttack_1hours); }
    public void buyAutoAttack2hour() { BuyProductID(AutoAttack_2hours); }
    public void buyAutoAttack3hour() { BuyProductID(AutoAttack_3hours); }
    public void buyAutoAttack5hour() { BuyProductID(AutoAttack_5hours); }
    public void buyAutoAttack7hour() { BuyProductID(AutoAttack_7hours); }
    public void buyAutoAttack10hour() { BuyProductID(AutoAttack_10hours); }
    public void buyAutoAttack3hourSale() { BuyProductID(AutoAttack_3hours_Sale); }
    public void buyAutoAttackPackage1() { BuyProductID(AutoAttack_Package_1); }
    public void buyAutoAttackPackage2() { BuyProductID(AutoAttack_Package_2); }
    public void buyAutoAttackPackage3() { BuyProductID(AutoAttack_Package_3); }

    //Equip Gacha Products
    public void buyAllEquipGacha() { BuyProductID(allEquip_Gacha); }
    public void buyRingGacha() { BuyProductID(Ring_Gacha); }
    public void buyNecklaceGacha() { BuyProductID(Necklace_Gacha); }
    public void buyRelicsGacha() { BuyProductID(Relics_Gacha); }
    public void buyAccessoryGacha() { BuyProductID(Accessory_Gacha); }
    public void buyBookGacha() { BuyProductID(Book_Gacha); }
    public void buyRingGachaPackage() { BuyProductID(Ring_Gacha_Package); }
    public void buyNecklaceGachaPackage() { BuyProductID(Necklace_Gacha_Package); }
    public void buyRelicsGachaPackage() { BuyProductID(Relics_Gacha_Package); }
    public void buyAccessoryGachaPackage() { BuyProductID(Accessory_Gacha_Package); }
    public void buyBookGachaPackage() { BuyProductID(Book_Gacha_Package); }

    //Starter Package
    public void buyStarterPackage1() { BuyProductID(StarterPackage_1); }
    public void buyStarterPackage2() { BuyProductID(StarterPackage_2); }
    public void buyStarterPackage3() { BuyProductID(StarterPackage_3); }

    //Event Packages
    public void buyReleaseEventPackage() { BuyProductID(releaseEventPackage); }
    public void buyearthSupplyPackage() { BuyProductID(earthSupplyPackage); }
    public void buybestQualityEquipPackage() { BuyProductID(bestQualityEquipPackage); }
    public void buyplanetClearPackage() { BuyProductID(planetClearPackage); }

    //Auto Exchange Tab
    public void buyAutoExchangeProduct() {BuyProductID(autoExchangeProduct); }

    //No Ads
    public void buynoAdsProduct() { BuyProductID(noAdsProduct); }

    //Step 4 modify purchasing
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        //Diamond Products
        if(String.Equals(purchaseEvent.purchasedProduct.definition.id, Diamond_300, StringComparison.Ordinal)) GameManager.SetDiamond(300);
        else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, Diamond_500, StringComparison.Ordinal)) GameManager.SetDiamond(500);
        else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, Diamond_1000, StringComparison.Ordinal)) GameManager.SetDiamond(1000);
        else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, Diamond_2000, StringComparison.Ordinal)) GameManager.SetDiamond(2000);
        else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, Diamond_3000, StringComparison.Ordinal)) GameManager.SetDiamond(3000);
        else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, Diamond_5000, StringComparison.Ordinal)) GameManager.SetDiamond(5000);
        else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, Diamond_10000, StringComparison.Ordinal)) GameManager.SetDiamond(10000);
        else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, Diamond_30000, StringComparison.Ordinal)) GameManager.SetDiamond(30000);
        else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, Diamond_50000, StringComparison.Ordinal)) GameManager.SetDiamond(50000);
        else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, Diamond_3000_50sale, StringComparison.Ordinal)) {
            GameManager.SetDiamond(3000);
            ShopManager.Instance.setisBoughtBool("464a4257-6d73-4073-8195-27beb0077dab");
        }

        //Diamond Package
        else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, Diamond_Package_1, StringComparison.Ordinal)) {
            GameManager.SetDiamond(5000);
            GachaManager.Instance.addTicket(GachaData.GachaType.UpgradeStone, 50);
            Player.instance.addAutoAttackTime_Sec(1800);
            //IsBought
            ShopManager.Instance.setisBoughtBool("525a5159-f97b-4367-9b32-276da8d80ca8");

        } else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, Diamond_Package_2, StringComparison.Ordinal)) {
            GameManager.SetDiamond(7500);
            GachaManager.Instance.gachaOneMust(5, Equips.MaterialClass.Epic, Equips.MaterialClass.Legendary, Equips.MaterialClass.Unique, false, 1);
            UpgradeStoneManager.instance.addStone(UpgradeStone.TypeOfStone.Epic, 500);
            //IsBought
            ShopManager.Instance.setisBoughtBool("7d8b8aa7-e1d5-4657-9ab1-0322f2a3ac27");

        } else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, Diamond_Package_3, StringComparison.Ordinal)) {
            GameManager.SetDiamond(10000);
            GachaManager.Instance.gachaOneMust(5, Equips.MaterialClass.Epic, Equips.MaterialClass.Legendary, Equips.MaterialClass.Legendary, false, 3);
            UpgradeStoneManager.instance.addStone(UpgradeStone.TypeOfStone.Unique, 200);
            Player.instance.addAutoAttackTime_Sec(7200);
            //IsBought
            ShopManager.Instance.setisBoughtBool("287e2808-620b-4882-8647-f16815cabc53");

        } else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, Diamond_Package_4, StringComparison.Ordinal)) {
            GameManager.SetDiamond(20000);
            GachaManager.Instance.gachaOneMust(10, Equips.MaterialClass.Epic, Equips.MaterialClass.Legendary, Equips.MaterialClass.Legendary, false, 3);
            UpgradeStoneManager.instance.addStone(UpgradeStone.TypeOfStone.Legendary, 100);
            Player.instance.addAutoAttackTime_Sec(18000);
            //IsBought
            ShopManager.Instance.setisBoughtBool("7c0df313-e56d-482b-ab71-49dae21376ff");

        }

        //SpaceStone
        else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, SpaceStone_10, StringComparison.Ordinal)) GameManager.SetParts(10);
        else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, SpaceStone_30, StringComparison.Ordinal)) GameManager.SetParts(30);
        else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, SpaceStone_50, StringComparison.Ordinal)) GameManager.SetParts(50);
        else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, SpaceStone_100, StringComparison.Ordinal)) GameManager.SetParts(100);
        else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, SpaceStone_300, StringComparison.Ordinal)) GameManager.SetParts(300);
        else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, SpaceStone_500, StringComparison.Ordinal)) GameManager.SetParts(500);


        //Auto Attack Products
        else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, GameSpeedx2, StringComparison.Ordinal)) { GameManager.instance.GameSpeedBought = true; ShopManager.Instance.setisBoughtBool("5ad46b67-3d28-444d-b9f6-bc67ceeac5ec");
        } else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, AutoAttack_1hours, StringComparison.Ordinal)) Player.instance.addAutoAttackTime_Sec(3600);
        else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, AutoAttack_2hours, StringComparison.Ordinal)) Player.instance.addAutoAttackTime_Sec(7200);
        else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, AutoAttack_3hours, StringComparison.Ordinal)) Player.instance.addAutoAttackTime_Sec(10800);
        else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, AutoAttack_5hours, StringComparison.Ordinal)) Player.instance.addAutoAttackTime_Sec(18000);
        else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, AutoAttack_7hours, StringComparison.Ordinal)) Player.instance.addAutoAttackTime_Sec(25200);
        else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, AutoAttack_10hours, StringComparison.Ordinal)) Player.instance.addAutoAttackTime_Sec(36000);
        else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, AutoAttack_3hours_Sale, StringComparison.Ordinal)) {
            Player.instance.addAutoAttackTime_Sec(10800);
            ShopManager.Instance.setisBoughtBool("cc0b2338-5d10-4153-bdb1-623c4c94e52c");
        }

        //Auto Attack Package
        else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, AutoAttack_Package_1, StringComparison.Ordinal)) {
            GameManager.SetDiamond(3000);
            Player.instance.addAutoAttackTime_Sec(7200);
            GachaManager.Instance.addTicket(GachaData.GachaType.UpgradeStone, 100);

            //IsBought
            ShopManager.Instance.setisBoughtBool("382c92c2-85b8-4a92-ad22-41451c42dd9b");

        } else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, AutoAttack_Package_2, StringComparison.Ordinal)) {
            GameManager.SetDiamond(5000);
            Player.instance.addAutoAttackTime_Sec(36000);
            GachaManager.Instance.addTicket(GachaData.GachaType.UpgradeStone, 200);
            //IsBought
            ShopManager.Instance.setisBoughtBool("4d3d98dc-a704-4eca-a3d8-f4088158f1aa");

        } else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, AutoAttack_Package_3, StringComparison.Ordinal)) {
            GameManager.SetDiamond(10000);
            Player.instance.addAutoAttackTime_Sec(180000);
            GachaManager.Instance.addTicket(GachaData.GachaType.UpgradeStone, 500);

            //IsBought
            ShopManager.Instance.setisBoughtBool("7f4f708f-f7ed-4902-8b2c-3ba091209ef7");

        }

        //***************************************Equips Gacha Products************************************************************************
        else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, allEquip_Gacha, StringComparison.Ordinal)) {
            GachaManager.Instance.gachaRealMoney(2, 2, 3, 2, 1, Equips.MaterialClass.Unique, Equips.MaterialClass.Legendary, false);
            //IsBought
            ShopManager.Instance.setisBoughtBool("be7bce9a-9887-4d5d-903c-90a4a0dd9ef6");

        } else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, Ring_Gacha, StringComparison.Ordinal)) {
            GachaManager.Instance.gachaRealMoney(5, 0, 0, 0, 0, Equips.MaterialClass.Unique, Equips.MaterialClass.Legendary, false);
        } else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, Necklace_Gacha, StringComparison.Ordinal)) {
            GachaManager.Instance.gachaRealMoney(0, 5, 0, 0, 0, Equips.MaterialClass.Unique, Equips.MaterialClass.Legendary, false);
        } else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, Relics_Gacha, StringComparison.Ordinal)) {
            GachaManager.Instance.gachaRealMoney(0, 0, 5, 0, 0, Equips.MaterialClass.Unique, Equips.MaterialClass.Legendary, false);
        } else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, Accessory_Gacha, StringComparison.Ordinal)) {
            GachaManager.Instance.gachaRealMoney(0, 0, 0, 5, 0, Equips.MaterialClass.Unique, Equips.MaterialClass.Legendary, false);
        } else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, Book_Gacha, StringComparison.Ordinal)) {
            GachaManager.Instance.gachaRealMoney(0, 0, 0, 0, 3, Equips.MaterialClass.Unique, Equips.MaterialClass.Legendary, false);
        }

        //***************************************Equips Gacha Package************************************************************************
        else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, Ring_Gacha_Package, StringComparison.Ordinal)) {
            GachaManager.Instance.gachaRealMoney(10, 0, 0, 0, 0, Equips.MaterialClass.Epic, Equips.MaterialClass.Legendary, true);
            UpgradeStoneManager.instance.addStone(UpgradeStone.TypeOfStone.Unique, 100);

            //IsBought
            ShopManager.Instance.setisBoughtBool("84a7bfca-bc41-4d03-a4e5-5b04ca8af00c");

        } else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, Necklace_Gacha_Package, StringComparison.Ordinal)) {
            GachaManager.Instance.gachaRealMoney(0, 10, 0, 0, 0, Equips.MaterialClass.Epic, Equips.MaterialClass.Legendary, true);
            UpgradeStoneManager.instance.addStone(UpgradeStone.TypeOfStone.Unique, 100);

            //IsBought
            ShopManager.Instance.setisBoughtBool("de788cd2-d775-4589-90ee-f3dc8d73a2bf");

        } else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, Relics_Gacha_Package, StringComparison.Ordinal)) {
            GachaManager.Instance.gachaRealMoney(0, 0, 10, 0, 0, Equips.MaterialClass.Epic, Equips.MaterialClass.Legendary, true);
            UpgradeStoneManager.instance.addStone(UpgradeStone.TypeOfStone.Unique, 100);

            //IsBought
            ShopManager.Instance.setisBoughtBool("606a0a4d-f13d-4164-a545-a95e76a239a7");

        } else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, Accessory_Gacha_Package, StringComparison.Ordinal)) {
            GachaManager.Instance.gachaRealMoney(0, 0, 0, 10, 0, Equips.MaterialClass.Epic, Equips.MaterialClass.Legendary, true);
            UpgradeStoneManager.instance.addStone(UpgradeStone.TypeOfStone.Unique, 100);

            //IsBought
            ShopManager.Instance.setisBoughtBool("1fdf8051-9689-4585-918b-eb82464ed71f");

        } else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, Book_Gacha_Package, StringComparison.Ordinal)) {
            GachaManager.Instance.gachaRealMoney(0, 0, 0, 0, 5, Equips.MaterialClass.Epic, Equips.MaterialClass.Legendary, true);
            UpgradeStoneManager.instance.addStone(UpgradeStone.TypeOfStone.Unique, 100);

            //IsBought
            ShopManager.Instance.setisBoughtBool("ebf42321-e797-49cd-8d30-378f3fcb5efb");

        }

        //***************************************Starter Package************************************************************************
        else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, StarterPackage_1, StringComparison.Ordinal)) {
            GameManager.SetDiamond(1000);
            GachaManager.Instance.gachaOneMust(3, Equips.MaterialClass.Epic, Equips.MaterialClass.Legendary, Equips.MaterialClass.Legendary, false, 3);
            UpgradeStoneManager.instance.addStone(UpgradeStone.TypeOfStone.Rare, 300);
            Player.instance.addAutoAttackTime_Sec(600);

            //IsBought
            ShopManager.Instance.setisBoughtBool("48e37d28-80ee-4417-b208-ad48e0d3fc46");

        } else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, StarterPackage_2, StringComparison.Ordinal)) {
            GameManager.SetDiamond(3000);
            GachaManager.Instance.gachaOneMust(5, Equips.MaterialClass.Epic, Equips.MaterialClass.Legendary, Equips.MaterialClass.Legendary, false, 3);
            UpgradeStoneManager.instance.addStone(UpgradeStone.TypeOfStone.Epic, 400);
            Player.instance.addAutoAttackTime_Sec(1800);

            //IsBought
            ShopManager.Instance.setisBoughtBool("496a1c13-9e02-437c-9775-062da625d559");

        } else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, StarterPackage_3, StringComparison.Ordinal)) {
            GameManager.SetDiamond(10000);
            GachaManager.Instance.gachaOneMust(10, Equips.MaterialClass.Epic, Equips.MaterialClass.Legendary, Equips.MaterialClass.Legendary, false, 3);
            UpgradeStoneManager.instance.addStone(UpgradeStone.TypeOfStone.Unique, 500);
            Player.instance.addAutoAttackTime_Sec(3600);

            //IsBought
            ShopManager.Instance.setisBoughtBool("61731c18-66fa-4639-a376-2e961f9c781e");

        }

        //***************************************Notification************************************************************************
        else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, releaseEventPackage, StringComparison.Ordinal)) {
            GameManager.SetDiamond(5000);
            GachaManager.Instance.gachaOneMust(10, Equips.MaterialClass.Epic, Equips.MaterialClass.Legendary, Equips.MaterialClass.Legendary, false, 3);
            Player.instance.addAutoAttackTime_Sec(1800);
            GameManager.SetParts(30);

            //IsBought
            ShopManager.Instance.setisBoughtBoolPackage("83698cb4-88bb-40bd-bc2e-0019240dadcd");

        } else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, earthSupplyPackage, StringComparison.Ordinal)) {
            GameManager.SetMoney(10000000000);
            Player.instance.addAutoAttackTime_Sec(3600);
            GameManager.SetDiamond(5000);

            //IsBought
            ShopManager.Instance.setisBoughtBoolPackage("a0f9a995-6715-44c7-9de9-bfb7f3352f27");

        } else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, bestQualityEquipPackage, StringComparison.Ordinal)) {
            GachaManager.Instance.gachaRealMoney(1, 1, 1, 1, 0, Equips.MaterialClass.Unique, Equips.MaterialClass.Legendary, false);

            //IsBought
            ShopManager.Instance.setisBoughtBoolPackage("77b4b625-6fb1-4d07-83aa-c97cfba85bec");

        } else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, planetClearPackage, StringComparison.Ordinal)) {
            GameManager.SetDiamond(3000);
            GachaManager.Instance.gachaOneMust(10, Equips.MaterialClass.Epic, Equips.MaterialClass.Legendary, Equips.MaterialClass.Legendary, false, 3);
            Player.instance.addAutoAttackTime_Sec(900);
            UpgradeStoneManager.instance.addStone(UpgradeStone.TypeOfStone.Unique, 200);

            //IsBought
            ShopManager.Instance.planetClearPackage.isBought = true;
            ShopManager.Instance.planetClearPackage.setGUI();

            if(PlanetClearTab.gameObject.activeSelf) {
                PlanetClearTab.checkNeverSee();
                PlanetClearTab.gameObject.SetActive(false);
            }

        } 
        //***********************auto exchange Tab******************************
        else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, autoExchangeProduct, StringComparison.Ordinal)){
            Railmanager.Instance.autoExchangeBought();

            ShopManager.Instance.setisBoughtBool("b79f06b6-b9c9-4e52-afbb-ff19bed0d002");
        }

        //***********************auto exchange Tab******************************
        else if(String.Equals(purchaseEvent.purchasedProduct.definition.id, noAdsProduct, StringComparison.Ordinal)) {
            GameManager.instance.noAdsBought = true;

            ShopManager.Instance.setisBoughtBool("337b3f8d-25b8-4206-b3f0-04c77aa098cb");
        } 
        
        
        else Debug.Log("Purchase Failed");

        return PurchaseProcessingResult.Complete;
    }

    //**************************** Dont worry about these methods ***********************************
    private void Awake()
    {
        TestSingleton();
    }

    void Start()
    {
        if(m_StoreController == null) { InitializePurchasing(); }
    }

    private void TestSingleton()
    {
        if(instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void BuyProductID(string productId)
    {
        if(IsInitialized()) {
            Product product = m_StoreController.products.WithID(productId);
            if(product != null && product.availableToPurchase) {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            } else {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        } else {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public void RestorePurchases()
    {
        if(!IsInitialized()) {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        if(Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer) {
            Debug.Log("RestorePurchases started ...");

            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) => {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        } else {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new NotImplementedException();
    }    
}
