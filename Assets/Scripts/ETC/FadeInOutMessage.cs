using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class FadeInOutMessage : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    private Tween fadeTween;

    private void OnEnable()
    {
        startFading();
    }

   

    private void Fade(float endValue, float duration, TweenCallback onEnd)
    {
        if(fadeTween != null) {
            fadeTween.Kill(false);
        }

        fadeTween = canvasGroup.DOFade(endValue, duration).SetUpdate(true);
        fadeTween.onComplete += onEnd;
    }

    public void FadeIn(float duration)
    {
        Fade(1f, duration, () => {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        });
    }

    public void FadeOut(float duration)
    {
        Fade(0f, duration, () => {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        });
    }

    public IEnumerator FadeInandOut()
    {
        FadeIn(1f);
        yield return new WaitForSecondsRealtime(2f);
        FadeOut(1f);
        this.gameObject.SetActive(false);
    }


    public void startFading()
    {
        StartCoroutine(FadeInandOut());
    }
}
