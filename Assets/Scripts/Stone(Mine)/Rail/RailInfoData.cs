using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RailInfoData : MonoBehaviour
{
    [HideInInspector]
    public Rail rail;

    public void setInfoData()
    {
        Railmanager.Instance.openRailInfoTab(rail);
    }

    public void setOrePrizeData()
    {
        AchievementManager.instance.setOrePrize(rail);
    }
}
