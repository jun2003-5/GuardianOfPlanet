using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public TextMeshProUGUI progressNumber;

    private void Start()
    {
        progressNumber.color = Color.black;
    }

    public void setProgress(float Value, float MaxValue)
    {
        slider.maxValue = MaxValue;
        slider.value = Value;

        if(MaxValue == 0) {
            progressNumber.text = Value.ToString();
            return;
        }

        if(progressNumber != null)
            progressNumber.text = Value + "/" + MaxValue;
    }
}
