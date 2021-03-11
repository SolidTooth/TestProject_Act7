using UnityEngine;
using System.Collections;

public class WeaponTrigger_OneAll : WeaponTrigger
{//범위내 모든적 1회 타격 트리거
    protected void OnTriggerEnter(Collider other)
    {
        if (myColl.enabled && other.tag.Equals("HP"))//myColl.enabled 보험용 - 따로 Bool값만들기엔 낭비
        {
            HpCtrl targetHpCtrl = other.GetComponent<HpCtrl>();
            if (targetHpCtrl != null)
            {
                damageSend(targetHpCtrl);
            }
        }
    }
}
