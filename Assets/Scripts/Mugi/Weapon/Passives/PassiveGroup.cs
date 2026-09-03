using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveGroup : MonoBehaviour
{

    List<WeaponPassive> passives;

    [HideInInspector]
    public bool isPassiveClick;

    public void Subscribe(WeaponPassive wp)
    {
        if(passives == null) {
            passives = new List<WeaponPassive>();
        }

        passives.Add(wp);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && isPassiveClick) {
            deactivateAll();
        }
    }

    public void activateCallout(WeaponPassive data)
    {
        for(int i = 0; i < passives.Count; i++) {
            if(passives[i] != data) {
                if(passives[i].PassiveCallout.activeSelf)
                    passives[i].PassiveCallout.SetActive(false);
            } else {
                isPassiveClick = !passives[i].PassiveCallout.activeSelf;
                passives[i].PassiveCallout.SetActive(!passives[i].PassiveCallout.activeSelf);
            }
        }
    }

    public void deactivateAll()
    {
        for(int i = 0; i < passives.Count; i++) {
            passives[i].PassiveCallout.SetActive(false);
        }
    }
}
