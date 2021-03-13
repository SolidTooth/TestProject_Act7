using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponTrigger_MultiAll : WeaponTrigger
{//범위내 모든적 다회 타격 트리거

    [SerializeField]
    private float dotDelay = 0.2f;
    private Dictionary<Collider, float> targetDic = new Dictionary<Collider, float>();
    private Dictionary<Collider, HpCtrl> targetHpCtrlDic = new Dictionary<Collider, HpCtrl>();

    protected void OnTriggerEnter(Collider other)
    {
        if (myColl.enabled && other.tag.Equals("HP"))//myColl.enabled 보험용 - 따로 Bool값만들기엔 낭비
        {
            HpCtrl targetHpCtrl = other.GetComponent<HpCtrl>();
            if (targetHpCtrl != null)
            {
                if (targetDic.ContainsKey(other) == false)
                {
                    targetDic[other] = 0f;
                    targetHpCtrlDic[other] = targetHpCtrl;
                }
                damageSend(targetHpCtrl);
            }
        }
    }
    protected void OnTriggerStay(Collider other)
    {
        if (targetDic.ContainsKey(other))
        {
            targetDic[other] += Time.deltaTime;
            if (targetDic[other] >= dotDelay)
            {
                targetDic[other] = 0;
                damageSend(targetHpCtrlDic[other]);
            }
        }
    }
    public override void attackTriggerOff()
    {
        Debug.Log("호출됨");
        targetDic.Clear();
        targetHpCtrlDic.Clear();
        base.attackTriggerOff();
    }

}
