using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveEasyWay : MonoBehaviour
{
    public Vector3 CurrentLoc;

    public Vector3 TargetLoc;


    private void Update()
    {
        if(CurrentLoc == TargetLoc) {
            Destroy(this.gameObject);
        }
    }
}
