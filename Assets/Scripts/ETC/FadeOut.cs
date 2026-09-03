using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class FadeOut : MonoBehaviour
{
    public Text[] text;
    public TextMeshProUGUI[] texts;

    private void OnEnable()
    {
        if(text.Length > 0) {
            for(int i = 0; i < text.Length; i++) {
                Fadeout(text[i], i);
            }
        } else if(texts.Length > 0) {
            for(int i = 0; i < texts.Length; i++) {
                Fadeout(texts[i], i);
            }
        }
    }

    async void Fadeout(Text text,int i)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
        await Task.Delay(1000);
        for(float f = 1f; f >= -0.1f; f -= 0.075f) {
            text.color = new Color(text.color.r, text.color.g, text.color.b, f);
            await Task.Delay(50);
        }
        if(i == this.text.Length - 1) {
            gameObject.SetActive(false);
        }
    }
    async void Fadeout(TextMeshProUGUI text, int i)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
        await Task.Delay(1000);
        for(float f = 1f; f >= -0.1f; f -= 0.075f) {
            text.color = new Color(text.color.r, text.color.g, text.color.b, f);
            await Task.Delay(50);
        }
        if(i == this.texts.Length - 1) {
            gameObject.SetActive(false);
        }
    }
}
