using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class notification : MonoBehaviour
{
    [SerializeField] public string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    [Header("#----UI")]
    [Space(7)]
    public GameObject checkMark;

    public bool neverShowAgain;

    private void OnEnable()
    {
        checkMark.SetActive(neverShowAgain);
    }

    public void checkNeverSee()
    {
        checkMark.SetActive(!checkMark.activeSelf);
        neverShowAgain = checkMark.activeSelf;
    }
}
