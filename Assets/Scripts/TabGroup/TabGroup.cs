using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class TabGroup : MonoBehaviour
{

    public List<TabButton> tabButtons;
    public Color tabIdle;
    public Color tabHover;
    public Color tabActive;
    public TabButton selectedTab;
    public List<GameObject> objectsToSwap;

    public float IdleSize;
    public float ScalingSize;

    private void Awake()
    {
        if(ScalingSize == 0)
            ScalingSize = 1f;
        if(IdleSize == 0)
            IdleSize = 1f;

        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
    }

    public void Subscribe(TabButton button)
    {
        if(tabButtons == null)
            tabButtons = new List<TabButton>();

        tabButtons.Add(button);

        tabButtons.Sort((a, b) => a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if(selectedTab == null || button != selectedTab) {
            button.background.color = tabHover;
        }
    }

    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButton button)
    {
        selectedTab = button;
        ResetTabs();
        button.transform.DOScale(ScalingSize, 0.5f).SetDelay(0).SetEase(Ease.OutBack).SetUpdate(true);
        int index = button.transform.GetSiblingIndex();
        for(int i = 0; i < objectsToSwap.Count; i++) {
            if(i == index) {
                objectsToSwap[i].SetActive(true);
            } else {
                objectsToSwap[i].SetActive(false);
            }
        }
        ChangeOnButton(index);
    }

    public void SelectTabbyIndex(TabButton data)
    {
        selectedTab = data;
        foreach(TabButton button in tabButtons) {
            if(selectedTab != null && button == selectedTab) { continue; }
            button.transform.localScale = new Vector3(IdleSize, IdleSize, IdleSize);
            button.background.color = tabIdle;
        }
        data.transform.localScale = new Vector3(ScalingSize, ScalingSize, ScalingSize);
        int index = data.transform.GetSiblingIndex();
        for(int i = 0; i < objectsToSwap.Count; i++) {
            if(i == index) {
                objectsToSwap[i].SetActive(true);
            } else {
                objectsToSwap[i].SetActive(false);
            }
        }
        ChangeOnButton(index);
    }
    public void SelectTabbyIndex(int a)
    {
        selectedTab = tabButtons[a];
        foreach(TabButton button in tabButtons) {
            if(selectedTab != null && button == selectedTab) { continue; }
            button.transform.localScale = new Vector3(IdleSize, IdleSize, IdleSize);
            button.background.color = tabIdle;
        }
        tabButtons[a].transform.localScale = new Vector3(ScalingSize, ScalingSize, ScalingSize);
        int index = tabButtons[a].transform.GetSiblingIndex();
        for(int i = 0; i < objectsToSwap.Count; i++) {
            if(i == index) {
                objectsToSwap[i].SetActive(true);
            } else {
                objectsToSwap[i].SetActive(false);
            }
        }
        ChangeOnButton(index);
    }


    public void ChangeOnButton(int index)
    {
        for(int i = 0; i < tabButtons.Count; i++) {
            if(i == index)
                tabButtons[i].background.color = tabActive;
            else
                tabButtons[i].background.color = tabIdle;
        }
    }

    public void ResetTabs()
    {
        foreach(TabButton button in tabButtons) {
            if(selectedTab != null && button == selectedTab) { continue; }
            button.transform.DOScale(IdleSize, 0.5f).SetDelay(0).SetEase(Ease.OutBack).SetUpdate(true);
            button.background.color = tabIdle;
        }
    }

    public void ResetEveryTabToNormal()
    {
        foreach(TabButton button in tabButtons) {
            button.transform.DOScale(IdleSize, 0.5f).SetDelay(0).SetEase(Ease.OutBack).SetUpdate(true);
            button.background.color = tabIdle;
        }
    }

    private void LateUpdate()
    {
        // Call ForceRebuildLayoutImmediate only when needed
        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
    }
}
