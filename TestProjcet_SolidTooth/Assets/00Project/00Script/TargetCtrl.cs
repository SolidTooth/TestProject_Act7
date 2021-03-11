using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCtrl : MonoBehaviour
{
    private UnitCtrl myUnitCtrl;

    [SerializeField]
    private TargetKind myKind; public TargetKind MyKind => myKind;
    [SerializeField]
    private TargetKind enemyTargetKind; public TargetKind EnemyTargetKind => enemyTargetKind;
    [SerializeField]
    private bool isAutoTargeting; public bool IsAutoTargeting => isAutoTargeting;//자동으로 타겟을 찾는 경우

    [SerializeField]
    private TargetCtrl targetCtrl; public Transform TargetTran => targetCtrl != null ? targetCtrl.transform : null;

    [SerializeField]
    private bool isTargeting; public bool IsTargeting => isTargeting;//Life 체크와 다름, 살아있지만 타겟팅 안될수있음 - 지금은 굳이 만들필요없음

    private void OnDisable()
    {
        lifeOff();
    }

    public void setUnitCtrl(UnitCtrl newUnitCtrl)
    {
        myUnitCtrl = newUnitCtrl;
    }

    public void lifeOn()
    {
        TargetManager.instance.addTarget(this);
        isTargeting = true;
    }
    public void lifeOff()
    {
        isTargeting = false;
        TargetManager.instance.removeTarget(this);
    }

    public void findTarget()
    {
        targetCtrl = TargetManager.instance.findTarget(this, enemyTargetKind);
    }
    public void findAutoTargeting()
    {
        if (IsAutoTargeting)
        {
            findTarget();
        }
        if (targetCtrl != null && targetCtrl.isTargeting == false)
        {//타겟이 있는 상태에서 호출된거면 타겟 상태를 확인해봐야함
            targetCtrl = null;
        }
    }
    public void checkTarget()
    {
        if (targetCtrl == null || targetCtrl.isTargeting == false || targetCtrl.gameObject.activeSelf == false)
        {//씬에서 안쓰거나 타겟팅이 불가능할때 다른 타겟 탐색
            targetCtrl = null;
            findAutoTargeting();
        }
    }

}
public enum TargetKind
{
    Path = -1,//이동경로
    None = 0,//미설정
    Player = 1,//플레이어
    Enemy = 2,//적
}