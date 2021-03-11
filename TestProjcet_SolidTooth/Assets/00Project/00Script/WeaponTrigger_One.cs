using UnityEngine;
using System.Collections;

public class WeaponTrigger_One : WeaponTrigger
{//1회 타격 트리거
    protected void OnTriggerEnter(Collider other)
    {
        if (myColl.enabled && other.tag.Equals("HP"))//myColl.enabled 보험용 - 따로 Bool값만들기엔 낭비
        {
            HpCtrl targetHpCtrl = other.GetComponent<HpCtrl>();
            if (targetHpCtrl != null)
            {
                attackOff();//공격성공
                if (targetHpCtrl.setDamage(myUnitCtrl.UnitInfo.Damage * skillDamagePercent))
                {
                    myUnitCtrl.TargetCtrl.findAutoTargeting();//타겟 사망 새로운 타겟 탐색
                }
            }
        }
    }
}
