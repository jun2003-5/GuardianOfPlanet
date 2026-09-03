using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserIconManager : MonoBehaviour, IDataPersistence
{
    public static UserIconManager Instance;

    public GameObject UserIconTab;

    public List<UserIconData> AllData;

    public List<UserIconData> planet_Icon_Data;
    public List<UserIconData> Boss_Icon_Data;
    public List<UserIconData> Stone_Icon_Data;
    public List<UserIconData> Equip_Icon_Data;

    [Header("#-----Tab Group && Scroll Rect")]
    public TabGroup tabgroup;
    public ScrollRect scrollRect;
    public RectTransform contentPanel;

    [Header("#---User Icon")]
    [Space(6)]
    public UserIconData Actual_User_Icon;

    [Header("#---GUI")]
    [Space(6)]
    public GameObject ExclamationMark_Obj;
    public Button ApplyButton;
    public TextMeshProUGUI PossessedAmountText;

    public UserIconData selectedData;

    bool isNewIcon;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        /*
        ApplyButton.interactable = selectedData != null;
        ExclamationMark_Obj.SetActive(isNewIcon);
        */
    }

    public void openUserIconTab()
    {
        UserIconTab.SetActive(true);

        //TabGroup
        tabgroup.SelectTabbyIndex(selectedData.button);

        //Reorder
        ReorderQuests();

        //Snap To
        for(int i = 0; i < AllData.Count; i++) {
            if(AllData[i].id == selectedData.id) {
                SnapTo(AllData[i].gameObject.GetComponent<RectTransform>());
                break;
            }
        }
        int sum = 0;
        for(int i = 0; i < AllData.Count; i++) {
            if(AllData[i].unLocked)
                sum++;
        }

        //Text
        PossessedAmountText.text = "º¸À¯ Áß: " + sum + "/" + AllData.Count;

        //Order

        isNewIcon = false;
    }

    public void unlockPlanetIcon(int index)
    {
        /*
        if(!planet_Icon_Data[index].unLocked)
            isNewIcon = true;

        planet_Icon_Data[index].Lock.SetActive(false);
        planet_Icon_Data[index].unLocked = true;
        */
    }

    public void unlockBossIcon(string id)
    {
        /*
        int index = -1;

        if(id == "0ff9f6c6-848f-4ade-80e8-123026ca7b2d")
            index = 0;
        else if(id == "54e8bc60-7838-4bb0-9fde-fb05f9150017")
            index = 1;
        else if(id == "9511dc34-2a69-4bb2-9956-1824b02f1365")
            index = 2;
        else if(id == "9702df5b-be41-4bf2-b7c8-f6a8bcfc7141")
            index = 3;
        else if(id == "3a824f3a-7b92-4150-8ce8-e45071b325c7")
            index = 4;
        else if(id == "9897d6bd-f7d3-427c-9caa-a41af147e1e5")
            index = 5;
        else if(id == "4529ce13-44b2-450a-86ff-bd5316c48f24")
            index = 6;
        else if(id == "cdee5555-3f4f-402b-b3b7-d1876cd65dda")
            index = 7;
        else if(id == "8b5d55fe-508c-426e-935d-c540a287f5ea")
            index = 8;
        else if(id == "c330c290-c6aa-4ddd-af2a-6a0c9176d7b3")
            index = 9;
        else if(id == "2419c747-750c-4625-a6af-60c2c59db74d")
            index = 10;
        else if(id == "5de334fe-aec1-4c3f-9e0e-27e3048300b5")
            index = 11;
        else if(id == "26e4ff7f-6f6e-4fa4-89a4-58e7e19c674a")
            index = 12;

        if(index > -1) {
            if(!Boss_Icon_Data[index].unLocked)
                isNewIcon = true;

            Boss_Icon_Data[index].Lock.SetActive(false);
            Boss_Icon_Data[index].unLocked = true;
        }
        */
    }

    public void unlockStoneIcon(int index)
    {
        /*
        if(!Stone_Icon_Data[index].unLocked)
            isNewIcon = true;

        Stone_Icon_Data[index].Lock.SetActive(false);
        Stone_Icon_Data[index].unLocked = true;
        */
    }

    public void unlockAncientIcon(int index)
    {
        /*
        if(!Equip_Icon_Data[index].unLocked)
            isNewIcon = true;

        Equip_Icon_Data[index].Lock.SetActive(false);
        Equip_Icon_Data[index].unLocked = true;
        */
    }

    public void ClickIcon(UserIconData data)
    {
        selectedData = data;
    }

    public void ApplyIcon()
    {
        for(int i = 0; i < AllData.Count; i++) {
            AllData[i].isSelected = AllData[i].id == selectedData.id;
        }

        Actual_User_Icon.setIconData(selectedData);
    }

    public void SnapTo(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        Vector2 targetLocalPosition = scrollRect.transform.InverseTransformPoint(target.position);
        Vector2 contentPanelLocalPosition = scrollRect.transform.InverseTransformPoint(contentPanel.position);

        // Only modify the Y position
        contentPanel.anchoredPosition = new Vector2(contentPanel.anchoredPosition.x, contentPanelLocalPosition.y - targetLocalPosition.y - 45);
    }

    public void ReorderQuests()
    {
        AllData.Sort((a, b) => {
            bool aUnlocked = a.unLocked;
            bool bUnlocked = b.unLocked;

            // Compare unlocked status first
            int unlockedComparison = bUnlocked.CompareTo(aUnlocked);

            if(unlockedComparison != 0) {
                return unlockedComparison;
            }

            // If both are unlocked, maintain the original order based on index
            if(aUnlocked && bUnlocked) {
                return a.Index.CompareTo(b.Index);
            }

            // If both are locked, or if one is unlocked and the other is locked,
            // then compare by original order based on index
            return a.Index.CompareTo(b.Index);
        });

        for(int i = 0; i < AllData.Count; i++) {
            AllData[i].gameObject.transform.SetSiblingIndex(i);
        }

    }


    public void LoadData(GameData data)
    {
        /*
        for(int i = 0; i < AllData.Count; i++) {
            data.isUnlocked_Icon.TryGetValue(AllData[i].id, out bool unlock);
            AllData[i].unLocked = unlock;
            AllData[i].Lock.SetActive(!unlock);

            //isBaisc
            if(AllData[i].isBasic) {
                AllData[i].unLocked = true;
                AllData[i].Lock.SetActive(false);
            }

            data.isSelected_Icon.TryGetValue(AllData[i].id, out bool select);
            AllData[i].isSelected = select;
            if(select) {
                ClickIcon(AllData[i]);
                ApplyIcon();
            }
        }

        if(selectedData == null)
            selectedData = AllData[0];
        */
    }

    public void SaveData(GameData data)
    {
        /*
        for(int i = 0; i < AllData.Count; i++) {
            if(data.isUnlocked_Icon.ContainsKey(AllData[i].id))
                data.isUnlocked_Icon.Remove(AllData[i].id);

            data.isUnlocked_Icon.Add(AllData[i].id, AllData[i].unLocked);

            if(data.isSelected_Icon.ContainsKey(AllData[i].id))
                data.isSelected_Icon.Remove(AllData[i].id);

            data.isSelected_Icon.Add(AllData[i].id, AllData[i].isSelected);
        }
        */
    }
}
