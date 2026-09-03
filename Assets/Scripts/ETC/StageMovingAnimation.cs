using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StageMovingAnimation : MonoBehaviour
{
    public Image planetImage;

    private void OnEnable()
    {
        StartCoroutine(AnimationStart());
    }

    IEnumerator AnimationStart()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        planetImage.transform.localScale = new Vector3(1, 1, 1);
        planetImage.transform.DOScale(10, 50).SetDelay(0).SetEase(Ease.OutBack).SetUpdate(true);
    }
}
