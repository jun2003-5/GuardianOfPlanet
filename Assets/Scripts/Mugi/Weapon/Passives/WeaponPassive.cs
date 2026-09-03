using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class WeaponPassive : MonoBehaviour, IPointerClickHandler
{
    public PassiveGroup passivegroup;

    public string passiveExplain;

    public GameObject PassiveCallout;

    [Header("#----Cover")]
    public GameObject PassiveCover;

    void Awake()
    {
        passivegroup.Subscribe(this);
        PassiveCallout.GetComponentInChildren<TextMeshProUGUI>().text = passiveExplain;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        passivegroup.activateCallout(this);
    }
}
