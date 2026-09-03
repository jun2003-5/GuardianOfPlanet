using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UserIconData : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public int Index;

    public Image Border;
    public Image Deco;
    public Image Icon;
    public GameObject Lock;

    public TabButton button;

    public bool unLocked;
    public bool isSelected;

    public bool isActual;

    [Header("#-----Is Basic")]
    public bool isBasic;

    RectTransform rect;

    public void setIconData(UserIconData data)
    {
        if(data.Deco.gameObject.activeSelf) {
            //Deco
            rect = data.Deco.gameObject.GetComponent<RectTransform>();
            Deco.rectTransform.sizeDelta = new Vector2(rect.rect.width, rect.rect.height);
            Deco.rectTransform.anchoredPosition = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y);

            Deco.gameObject.SetActive(true);
            Deco.sprite = data.Deco.sprite;
        } else {
            Deco.gameObject.SetActive(false);
        }
        //Border
        Border.color = data.Border.color;

        //Icon
        rect = data.Icon.gameObject.GetComponent<RectTransform>();

        Icon.rectTransform.sizeDelta = new Vector2(rect.rect.width, rect.rect.height);
        Icon.rectTransform.anchoredPosition = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y);

        Icon.color = data.Icon.color;
        Icon.sprite = data.Icon.sprite;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!isActual && unLocked)
            UserIconManager.Instance.ClickIcon(this);
    }
}
