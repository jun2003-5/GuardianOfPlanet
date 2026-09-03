using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WarningSign : MonoBehaviour
{
    public Image WarningImage;
    private Tween fadeTween;

    private void OnEnable()
    {
        WarningImage.color = new Color(1, 1, 1, 0);
        StartCoroutine(StartFading());
    }

    private void Fade(float endValue, float duration)
    {
        if(fadeTween != null) {
            fadeTween.Kill(false);
        }

        fadeTween = WarningImage.DOFade(endValue, duration).SetUpdate(true);
    }

    public void FadeIn(float duration)
    {
        Fade(1f, duration);
    }

    public void FadeOut(float duration)
    {
        Fade(0f, duration);
    }

    IEnumerator StartFading()
    {
        for(int i = 0; i < 3; i++) {
            FadeIn(1f);
            yield return new WaitForSecondsRealtime(1);
            FadeOut(1f);
            yield return new WaitForSecondsRealtime(1);
        }
        this.gameObject.SetActive(false);
    }
}
