using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ButtonGroup : MonoBehaviour
{
    public List<ButtonData> tabButtons;
    public Color tabIdle;
    public Color tabHover;
    public Color tabActive;
    public ButtonData selectedTab;
    public List<GameObject> objectsToSwap;

    private void Awake()
    {
        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
    }

    public void Subscribe(ButtonData button)
    {
        if(tabButtons == null)
            tabButtons = new List<ButtonData>();

        tabButtons.Add(button);

        tabButtons.Sort((a, b) => a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));
    }

    public void OnTabEnter(ButtonData button)
    {
        ResetTabs();
        if(selectedTab == null || button != selectedTab) {
            button.background.color = tabHover;
        }
    }

    public void OnTabExit(ButtonData button)
    {
        ResetTabs();
    }

    public void OnTabSelected(ButtonData button)
    {
        selectedTab = button;
        ResetTabs();
        button.setGUI(true);
        button.background.color = tabActive;
        int index = button.transform.GetSiblingIndex();
        for(int i = 0; i < objectsToSwap.Count; i++) {
            if(i == index) {
                objectsToSwap[i].SetActive(true);
            } else {
                objectsToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        foreach(ButtonData button in tabButtons) {
            if(selectedTab != null && button == selectedTab) { continue; }
            button.setGUI(false);
            button.background.color = tabIdle;
        }
    }

    public void resetAllTabs()
    {
        foreach(ButtonData button in tabButtons) {
            button.setGUI(false);
            button.background.color = tabIdle;
        }
    }
}
