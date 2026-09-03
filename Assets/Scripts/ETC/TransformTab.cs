using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class TransformTab : MonoBehaviour
{
    public static TransformTab instance;

    [SerializeField] private CanvasGroup canvasGroup;
    private Tween fadeTween;

    public TextMeshProUGUI text1;
    public TextMeshProUGUI TipText;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        RandomTips();
    }

    public void RandomTips()
    {
        int r = Random.Range(0, 11);
        switch(r) {
            case 1:
                TipText.text = "*Tip 도감을 통해 다이아를 획득할 수 있다";
                break;
            case 2:
                TipText.text = "*Tip 모험에서 행성정보를 클릭하면 몬스터의 정보를 알 수 있다";
                break;
            case 3:
                TipText.text = "*Tip 모험은 난이도에 따라 보상이 달라진다";
                break;
            case 4:
                TipText.text = "*Tip 1인 개발 게임이다";
                break;
            case 5:
                TipText.text = "*Tip 광산을 통해 부족한 능력치를 향상 시킬 수 있다";
                break;
            case 6:
                TipText.text = "*Tip 자동사냥은 보스보다 가까운 적부터 공격한다";
                break;
            case 7:
                TipText.text = "*Tip 상태창의 스탯 강화를 통해 더욱 강해질 수 있다";
                break;
            case 8:
                TipText.text = "*Tip 똑같은 장비를 모으면 다음 등급의 장비를 획득할 수 있다";
                break;
            case 9:
                TipText.text = "*Tip 레벨이 오를때 마다 데미지가 증가한다";
                break;
            case 10:
                TipText.text = "*Tip 강화는 10성부터 실패 시 장비 레벨이 하락한다";
                break;
        }
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
    }


    public void startFading(string text)
    {
        instance.text1.text = text;
        instance.RandomTips();
        StartCoroutine(FadeInandOut());
    }
}
