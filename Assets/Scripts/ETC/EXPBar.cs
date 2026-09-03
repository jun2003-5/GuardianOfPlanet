using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EXPBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public TextMeshProUGUI progressNumber;
    public TextMeshProUGUI progressNumber2;

    public Button button;

    public void setProgress(float Value, float MaxValue)
    {
        slider.maxValue = MaxValue;
        slider.value = Value;

        if(progressNumber != null)
            progressNumber.text = (long)Mathf.Round(Value) + "/" + (long)Mathf.Round(MaxValue) + " (" + string.Format("{0:#,###0.##}", Value / MaxValue * 100) + "%)";

        if(progressNumber2 != null) {
            if(Value >= MaxValue) {
                progressNumber2.text = (long)Mathf.Round(MaxValue) + "/" + (long)Mathf.Round(MaxValue);
            } else {
                progressNumber2.text = (long)Mathf.Round(Value) + "/" + (long)Mathf.Round(MaxValue);
            }
        }
    }

    public void SetText(string s)
    {
        progressNumber.text = s;
        slider.maxValue = 1;
        slider.value = 1;
    }

    private void Update()
    {
        if(button != null)
            button.gameObject.SetActive(slider.value >= slider.maxValue);
    }
}
