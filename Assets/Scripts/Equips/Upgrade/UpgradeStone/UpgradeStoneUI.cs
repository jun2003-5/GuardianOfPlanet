using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UpgradeStoneUI : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public UpgradeStone stoneData;

    [Header("UIs")]
    public TextMeshProUGUI ItemNumber;
    public GameObject Empty_Tab;

    float timerAs;
    float waitTimer;
    bool addTime;

    public int SelectedAmount;

    public void setStoneAmountText()
    {
        if(stoneData.StoneAmount > 0) {
            ItemNumber.text = (stoneData.StoneAmount - SelectedAmount).ToString();
            Empty_Tab.SetActive(false);
        } else {
            Empty_Tab.SetActive(true);
        }
    }

    public void resetStoneAmount()
    {
        SelectedAmount = 0;
        setStoneAmountText();
    }

    public void setStoneAmount()
    {
        stoneData.StoneAmount -= SelectedAmount;
        resetStoneAmount();
    }

    private void Update()
    {
        if(addTime && stoneData.StoneAmount > SelectedAmount) {
            timerAs += Time.deltaTime;
            if(timerAs > 0.5f) {
                waitTimer += Time.deltaTime;
                if(waitTimer > 0.05f) {
                    EquipUpgradeManager.Instance.UpgradeStoneClicked(this);
                    waitTimer = 0;
                }
            }

            if(stoneData.StoneAmount <= 0) {
                timerAs = 0;
                waitTimer = 0;
                addTime = false;
            }
        } else {
            timerAs = 0;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(stoneData.StoneAmount > SelectedAmount) {
            EquipUpgradeManager.Instance.UpgradeStoneClicked(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(stoneData == null)
            Debug.Log(this.name);
        EquipUpgradeManager.Instance.ChangeObjectSize(stoneData, new Vector3(0.8f, 0.8f, 0.8f));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        addTime = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        addTime = false;
    }
}
