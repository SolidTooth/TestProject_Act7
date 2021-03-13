using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponTrigger_Archer : WeaponTrigger
{
    [SerializeField]
    private Transform skillPivot;
    [SerializeField]
    private float skillRange;
    [SerializeField]
    private int bulletCount = 0;//0은 범위내 모두
    [SerializeField]
    private FireCtrl[] fireCtrlArr;
    public override void attackTriggerOn()
    {
        List<TargetCtrl> targetList = TargetManager.instance.findRangeTarget(skillPivot, targetKind, skillRange);

        if (targetList != null && targetList.Count > 0)
        {
            for (int i = 0; bulletCount > 0 ? i < bulletCount : i < targetList.Count; i++)
            {
                fireCtrlArr[i % fireCtrlArr.Length].shotAuto(targetList[i % targetList.Count], damageSend);//타겟 위치로 탄환 발사
            }
        }
    }

}
