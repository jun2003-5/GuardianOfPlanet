using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour, IDataPersistence
{
    public static ShopManager Instance;

    [Header("#-----Datas")]
    [Space(5)]
    public List<ShopData> shop_datas;
    public TimeAttackShopData planetClearPackage;

    public List<TimeAttackShopData> package_Datas;

    public GameObject ErrorMesseage;
    public GameObject BuyTab;

    [Header("UIs")]
    ShopData currentShoppingProduct;

    [Header("#---Scroll Rect")]
    public ScrollRect scrollRect;
    public RectTransform contentPanel;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        for(int i = 0; i < shop_datas.Count; i++) {
            shop_datas[i].setGUI();
        }
    }

    public void openBuyTab(ShopData data)
    {
        if(GameManager.GetDiamond() >= data.Price) {
            BuyTab.SetActive(true);
            currentShoppingProduct = data;
        } else {
            ErrorMesseage.SetActive(true);
        }
    }
    public void buyProductWithDiamond()
    {
        if(GameManager.GetDiamond() >= currentShoppingProduct.Price) {
            switch(currentShoppingProduct._typeOfShop) {
                case ShopData.TypeOfShop.AutoFarm:
                    GameManager.SetDiamond(-currentShoppingProduct.Price);
                    Player.instance.addAutoAttackTime_Sec(currentShoppingProduct.Value * 60);
                    break;
                case ShopData.TypeOfShop.Gold:
                    GameManager.SetDiamond(-currentShoppingProduct.Price);
                    GameManager.SetMoney(currentShoppingProduct.Value);
                    break;
            }
        } else {
            ErrorMesseage.SetActive(true);
        }
    }

    public void snapTo(RectTransform data)
    {
        Canvas.ForceUpdateCanvases();

        Vector2 dataPosition = new Vector2(data.position.x, data.position.y + 35);

        contentPanel.anchoredPosition =
                (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position)
                - (Vector2)scrollRect.transform.InverseTransformPoint(dataPosition);
    }

    public void setisBoughtBool(string id)
    {
        for(int i = 0; i < shop_datas.Count; i++) {
            if(shop_datas[i].id == id) {
                shop_datas[i].isBought = true;
                break;
            }
        }
        setShopGUIs();
    }

    public void setisBoughtBoolPackage(string id)
    {
        for(int i = 0; i < package_Datas.Count; i++) {
            if(package_Datas[i].id == id) {
                package_Datas[i].isBought = true;
                break;
            }
        }
        setShopGUIs();

    }

    public void setShopGUIs()
    {
        for(int i = 0; i < shop_datas.Count; i++) {
            shop_datas[i].setGUI();
        }
    }

    public void LoadData(GameData data)
    {
        for(int i = 0; i < shop_datas.Count; i++) {
            data.Shop_isBought.TryGetValue(shop_datas[i].id, out bool value);
            shop_datas[i].isBought = value;
        }
        setShopGUIs();

        //Time Attack Packages
        for(int i = 0; i < package_Datas.Count; i++) {
            data.Shop_isBought.TryGetValue(package_Datas[i].id, out bool value);
            package_Datas[i].isBought = value;

            if(!value) {
                data.Shop_isActivate.TryGetValue(package_Datas[i].id, out bool value2);
                package_Datas[i].isActivate = value2;
                if(!value2) {
                    package_Datas[i].startEvent();
                } else {
                    data.Shop_leftTime.TryGetValue(package_Datas[i].id, out string time);
                    package_Datas[i].startTimeSpan = time;
                }
            } else {
                Destroy(package_Datas[i].gameObject);
                package_Datas.Remove(package_Datas[i]);
                i--;
            }
        }

        //Planet Package
        data.Shop_isBought.TryGetValue(planetClearPackage.id, out bool bought);
        planetClearPackage.isBought = bought;

        if(!bought) {
            data.Shop_isActivate.TryGetValue(planetClearPackage.id, out bool active);
            planetClearPackage.isActivate = active;

            if(active) {
                planetClearPackage.gameObject.SetActive(true);

                data.Shop_leftTime.TryGetValue(planetClearPackage.id, out string time2);
                planetClearPackage.startTimeSpan = time2;
            }
        }
        planetClearPackage.setGUI();
    }

    public void SaveData(GameData data)
    {
        for(int i = 0; i < shop_datas.Count; i++) {
            if(data.Shop_isBought.ContainsKey(shop_datas[i].id))
                data.Shop_isBought.Remove(shop_datas[i].id);

            data.Shop_isBought.Add(shop_datas[i].id, shop_datas[i].isBought);
        }

        for(int i = 0; i < package_Datas.Count; i++) {
            if(data.Shop_isBought.ContainsKey(package_Datas[i].id))
                data.Shop_isBought.Remove(package_Datas[i].id);

            data.Shop_isBought.Add(package_Datas[i].id, package_Datas[i].isBought);

            if(data.Shop_isActivate.ContainsKey(package_Datas[i].id))
                data.Shop_isActivate.Remove(package_Datas[i].id);

            data.Shop_isActivate.Add(package_Datas[i].id, package_Datas[i].isActivate);

            if(package_Datas[i].isActivate) {
                if(data.Shop_leftTime.ContainsKey(package_Datas[i].id))
                    data.Shop_leftTime.Remove(package_Datas[i].id);

                data.Shop_leftTime.Add(package_Datas[i].id, package_Datas[i].startTimeSpan);
            }
        }

        if(data.Shop_isBought.ContainsKey(planetClearPackage.id))
            data.Shop_isBought.Remove(planetClearPackage.id);

        data.Shop_isBought.Add(planetClearPackage.id, planetClearPackage.isBought);

        if(data.Shop_isActivate.ContainsKey(planetClearPackage.id))
            data.Shop_isActivate.Remove(planetClearPackage.id);

        data.Shop_isActivate.Add(planetClearPackage.id, planetClearPackage.isActivate);

        if(planetClearPackage.isActivate) {
            if(data.Shop_leftTime.ContainsKey(planetClearPackage.id))
                data.Shop_leftTime.Remove(planetClearPackage.id);

            data.Shop_leftTime.Add(planetClearPackage.id, planetClearPackage.startTimeSpan);
        }
    }
}
