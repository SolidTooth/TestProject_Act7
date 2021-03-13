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
    private TargetKind pathTargetKind;
    [SerializeField]
    private bool isAutoFind; public bool IsAutoFind => isAutoFind;//자동으로 타겟을 찾는 경우

    [SerializeField]
    private TargetCtrl enemyTargetCtrl; public Transform TargetTran => enemyTargetCtrl != null ? enemyTargetCtrl.transform : null;
    public bool IsTargetEnemy => enemyTargetCtrl != null ? enemyTargetCtrl.myKind == enemyTargetKind : false;
    [SerializeField]
    private bool isTargeting; public bool IsTargeting => isTargeting;//자신이 타겟팅이 되는지 Life 체크와 다름, 살아있지만 타겟팅 안될수있음 - 지금은 굳이 만들필요없음

    private float targetDis; public float TargetDis => targetDis;
    private Vector3 targetDir; public Vector3 TargetDir => targetDir;

    private void OnDisable()
    {
        removeTargetList();
    }

    public void setUnitCtrl(UnitCtrl newUnitCtrl)
    {
        myUnitCtrl = newUnitCtrl;
    }

    public void setTargetList()
    {
        TargetManager.instance.addTarget(this);
        isTargeting = true;
    }

    public void removeTargetList()
    {
        isTargeting = false;
        TargetManager.instance.removeTarget(this);
    }

    public void findTarget()
    {
        enemyTargetCtrl = TargetManager.instance.findTarget(this, enemyTargetKind);
        //적이없으면 경로를 타겟으로
        if (enemyTargetCtrl == null && pathTargetKind != TargetKind.None) enemyTargetCtrl = TargetManager.instance.findTarget(this, pathTargetKind);
    }
    public void findTargetAuto()
    {
        if (IsAutoFind)
        {
            findTarget();
        }
        if (enemyTargetCtrl != null && enemyTargetCtrl.isTargeting == false)
        {//타겟이 있는 상태에서 호출된거면 타겟 상태를 확인해봐야함
            enemyTargetCtrl = null;
        }
    }
    public void checkTarget()
    {
        if (enemyTargetCtrl == null || enemyTargetCtrl.isTargeting == false || enemyTargetCtrl.gameObject.activeInHierarchy == false)
        {//씬에서 안쓰거나 타겟팅이 불가능할때 다른 타겟 탐색
            enemyTargetCtrl = null;
            findTargetAuto();
            calcTargetData();
            //Debug.Log("타겟을 변경합니다" + (enemyTargetCtrl != null ? enemyTargetCtrl.name : "없음"));
        }
    }
    private void Update()
    {
        calcTargetData();
    }
    public void calcTargetData()
    {
        if (enemyTargetCtrl != null)//타겟있음
        {
            Vector3 myPos = myUnitCtrl.transform.position;
            Vector3 targetPos = TargetTran.position;
            myPos.y = targetPos.y = 0;//Y축을 통일해서 평면상의 거리와 방향을 계산

            targetDis = Vector3.Distance(myPos, targetPos);
            targetDir = targetPos - myPos;
        }
        else
        {
            if (targetDis > 0)//타겟없음 대기
            {
                targetDis = -1;
                targetDir = Vector3.zero;
            }
        }
    }
}
public enum TargetKind
{
    Ctrl = -2,//직접 이동
    Path = -1,//이동경로
    None = 0,//미설정
    Player = 1,//플레이어
    Enemy = 2,//적
}