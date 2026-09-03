using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour
{
    public static Drill instance;

    [Header("#-----DrillDamage")]
    public int damage;

    [Header("#-----Reload")]
    [Space(10)]
    public float MaximumAttackSpeed;
    public float attackSpeed;

    [Header("#---Luck")]
    public float luck;

    [Header("#-----Shooting Point")]
    [Space(10)]
    public Transform ShootingPoint;

    [HideInInspector]
    public bool IsShooting;
    float TimerAs;

    private void Awake()
    {
        instance = this;
    }
    
    public void setDrillStat(drillBasicInfo data, int damageLevel, int ASLevel, int luckLevel)
    {
        damage = (int)((data.baseDamage + (data.increasePerLevel_Damage * damageLevel)) * (MineManager.instance.drillStatsIncrease + 1));
        attackSpeed = Mathf.Max(MaximumAttackSpeed, data.baseAttackSpeed - (data.increasePerLevel_AttackSpeed * ASLevel));
        luck = (data.baseLuck + (data.increasePerLevel_Luck * luckLevel)) * (MineManager.instance.drillStatsIncrease + 1);
    }

    private void Update()
    {
        if(IsShooting) {
            TimerAs += Time.deltaTime;
            if(TimerAs >= attackSpeed) {
                CreateBullet();
                TimerAs = 0;
            }
        }
    }

    public void CreateBullet()
    {
        DrillBullet bullet_A = ObjectPoolBullet.Instance.GetPoolObject(PoolObjectTypeBullet.Mining).GetComponent<DrillBullet>();
        bullet_A.bulletDamage = damage;
        bullet_A.transform.position = ShootingPoint.position;
        bullet_A.transform.rotation = transform.rotation;
        bullet_A.gameObject.SetActive(true);
    }

    private void OnMouseDown()
    {
        if(!Railmanager.Instance.StoneBuyTab.activeSelf && !Railmanager.Instance.ShopTab.activeSelf && !Railmanager.Instance.AutoExchangeTab.activeSelf && !Railmanager.Instance.ClickBuyTab.activeSelf)
            Railmanager.Instance.openRailInfoTab(this.transform.parent.GetComponent<Rail>());
    }
}
