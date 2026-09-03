using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dummy : MonoBehaviour
{
    public TextMeshPro DPSText;

    public long DamagerPS;
    public long MAX;

    public float AttackedTime;

    public int count;

    public bool isPoisoned;

    float time;

    void Update()
    {
        if(count > 0) {
            if((int)(DamagerPS / Mathf.Max(1, Mathf.CeilToInt(time))) > MAX) {
                MAX = (int)(DamagerPS / Mathf.Max(1, Mathf.CeilToInt(time)));
            }
        }

        if(DamagerPS != 0 && count > 0) {
            DPSText.text = "DPS: " + (int)(DamagerPS / Mathf.Max(1, Mathf.CeilToInt(time))) + "\nÃÖ´ë: " + MAX;
        } else
            DPSText.text = "";

        AttackedTime += Time.deltaTime;
        if(AttackedTime > 2.5f) {
            DamagerPS = 0;
            MAX = 0;
            count = 0;
            time = 0;
        } else {
            time += Time.deltaTime;
        }
    }

    public void addDamage(float damage, bool Crit, float CritDamage)
    {
        AttackedTime = 0;
        count++;
        if(Crit) {
            DamagerPS += (long)(damage * (2 + CritDamage));
            DamagePopup.Create(this.transform.position, (long)(damage * (2 + CritDamage)), true);
        } else {
            DamagerPS += (long)damage;
            DamagePopup.Create(this.transform.position, (long)damage, false);
        }
    }
}
