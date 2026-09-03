using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipCalloutGroup : MonoBehaviour
{
    public List<EquipCallout> callOuts;

    [HideInInspector]
    public bool isCalloutClick;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && isCalloutClick) {
            deactivateAll();
        }
    }

    public void activateCallout(EquipCallout data)
    {
        for(int i = 0; i < callOuts.Count; i++) {
            if(callOuts[i] != data) {
                if(callOuts[i].CalloutObject.activeSelf)
                    callOuts[i].CalloutObject.SetActive(false);
            } else {
                isCalloutClick = !callOuts[i].CalloutObject.activeSelf;
                callOuts[i].CalloutObject.SetActive(!callOuts[i].CalloutObject.activeSelf);
            }
        }
    }

    public void deactivateAll()
    {
        for(int i = 0; i < callOuts.Count; i++) {
            callOuts[i].CalloutObject.SetActive(false);
        }
    }
}
