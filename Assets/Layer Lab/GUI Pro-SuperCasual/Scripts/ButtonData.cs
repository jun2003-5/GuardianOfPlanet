using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonData : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public ButtonGroup tabGroup;

    public GameObject Light;
    [HideInInspector]
    public Image background;

    public Sprite FocusSprite;
    public Sprite IdleSprite;
    void Awake()
    {
        background = GetComponent<Image>();
        tabGroup.Subscribe(this);
    }

    public void setGUI(bool active)
    {
        if(active) {
            Light.SetActive(true);
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(gameObject.GetComponent<RectTransform>().sizeDelta.x, 189);
            background.sprite = FocusSprite;
        } else {
            Light.SetActive(false);
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(gameObject.GetComponent<RectTransform>().sizeDelta.x, 179.7f);
            background.sprite = IdleSprite;
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnTabExit(this);
    }
}
