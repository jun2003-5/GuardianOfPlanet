using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{

    public float Division;
    void Start()
    {
        float width = ScreenSize.GetScreenToWorldWidth;
        transform.localScale = Vector3.one * width / Division;
    }
}
