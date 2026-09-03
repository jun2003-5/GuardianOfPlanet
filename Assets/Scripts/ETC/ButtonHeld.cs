using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHeld : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Slider slider;

    bool isPressed;
    bool isDrag;

    float TimerAs;

    private void Awake()
    {
        slider.maxValue = 3;
    }

    public void ResetGame()
    {
        DataPersistenceManager.instance.DeleteGame();
    }

    private void Update()
    {
        if(isPressed) {
            TimerAs += Time.deltaTime;
            if(TimerAs >= 3)
                ResetGame();
        }
        slider.value = TimerAs;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        TimerAs = 0;
    }

    public void OnDrag(PointerEventData eventData)
    {
    }
}
