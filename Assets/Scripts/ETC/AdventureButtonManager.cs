using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdventureButtonManager : MonoBehaviour
{
    [Header("UIs")]
    public GameObject DungeonCover;
    public GameObject InfiniteStageCover;
    public GameObject LabCover;

    public Button DungeonButton;
    public Button InfiniteStageButton;
    public Button LabButton;
    public void OnEnable()
    {
        InfiniteStageCover.SetActive(!StageManager.instance.planets[0].PlanetCleared);
        DungeonCover.SetActive(!StageManager.instance.planets[1].PlanetCleared);
        LabCover.SetActive(CollectionManager.Instance.getHigherGradeCollectionFoundAmount(CollectionData.Grade.Silver) < 5);

        InfiniteStageButton.interactable = !InfiniteStageCover.activeSelf;
        DungeonButton.interactable = !DungeonCover.activeSelf;
        LabButton.interactable = !LabCover.activeSelf;
    }
}
