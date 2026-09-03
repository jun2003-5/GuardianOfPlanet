using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CardData : MonoBehaviour
{
    [Header("Borders")]
    public Image EquipBorder;

    [Header("Equip Result")]
    public Image EquipImage;
    public TextMeshProUGUI EquipName;
    public TextMeshProUGUI EquipStat;
    public TextMeshProUGUI EquipGrade;

    public GameObject newText;

    public CanvasGroup cg;
    private Tween fadeTween;
    public Button SkipButton;

    public void SetCardData(EquipFrame data)
    {
        cg.alpha = 0;
        SkipButton.interactable = false;
     //   EquipImage.sprite = data.ItemImage.sprite;
        EquipName.text = data.equipData.EquipName;
        EquipGrade.text = data.equipData.gradeText;
    //    EquipBorder.color = data.Border.color;
//        EquipGrade.color = data.Border.color;
        newText.SetActive(EquipManager.Instance.All_Equips.Find(x => x == data).equipData.AmountOfEquip <= 1);
        StartCoroutine(StartFading(EquipManager.Instance.All_Equips.Find(x => x == data).equipData.AmountOfEquip > 1));
    }

    public void Skip()
    {
        cg.alpha = 1;
        fadeTween.Kill();
        SkipButton.interactable = true;
    }

    private void Fade(float endValue, float duration, TweenCallback onEnd)
    {
        if(fadeTween != null) {
            fadeTween.Kill(false);
        }

        fadeTween = cg.DOFade(endValue, duration).SetUpdate(true);
        fadeTween.onComplete += onEnd;
    }

    public void FadeIn(float duration)
    {
        Fade(1f, duration, () => {
            cg.interactable = false;
            cg.blocksRaycasts = false;
        });
    }

    public IEnumerator StartFading(bool isFirst)
    {
        SkipButton.interactable = isFirst;
        FadeIn(2f);
        yield return new WaitForSeconds(2);
        SkipButton.interactable = true;
    }
}
