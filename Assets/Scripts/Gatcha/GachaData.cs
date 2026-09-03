using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaData: MonoBehaviour
{
    public enum GachaType {Normal, Normalx10, Special, Specialx10, UpgradeStone, UpgradeStonex10};

    public GachaType gacha;

    public int Price;
}
