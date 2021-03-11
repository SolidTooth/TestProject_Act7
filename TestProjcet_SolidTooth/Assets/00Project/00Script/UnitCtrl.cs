using UnityEngine;
using System.Collections;
using System;

public class UnitCtrl : MonoBehaviour
{
    private static readonly int aniTag_Idle = Animator.StringToHash("Idle");
    private static readonly int aniTag_Move = Animator.StringToHash("Move");
    private static readonly int aniTag_Attack = Animator.StringToHash("Attack");

    [SerializeField]
    private UnitInfo unitInfo; public UnitInfo UnitInfo => unitInfo;
    [SerializeField]
    private bool isAutoLife;

    [SerializeField]
    private Animator modelAni;
    private AnimatorStateInfo aniStateInfo;
    private int nowAniTag;

    [Header("상태관련")]
    [SerializeField]
    private HpCtrl hpCtrl; public bool IsLife => hpCtrl.IsLife;
    [SerializeField]
    private UnitState nowState;
    [SerializeField]
    private UnitState nextState;
    [SerializeField]
    private bool isMoveLock;
    [SerializeField]
    private bool isLookLock;

    [Header("타겟관련")]
    [SerializeField]
    private NavAgentCtrl navAgentCtrl;
    [SerializeField]
    private TargetCtrl targetCtrl; public TargetCtrl TargetCtrl => targetCtrl;
    private float targetDis;
    private Vector3 targetDir;

    [SerializeField]
    private WeaponCtrl weaponCtrl;



    private void Start()
    {
        if (hpCtrl == null) hpCtrl = GetComponent<HpCtrl>();
        hpCtrl.setUnitCtrl(this);
        hpCtrl.setMaxHp(unitInfo.MaxHp);

        if (navAgentCtrl == null) navAgentCtrl = GetComponent<NavAgentCtrl>();
        navAgentCtrl?.setUnitCtrl(this);

        if (targetCtrl == null) targetCtrl = GetComponent<TargetCtrl>();
        targetCtrl?.setUnitCtrl(this);

        if (weaponCtrl != null) weaponCtrl.setUnitCtrl(this);

        if (isAutoLife) lifeOn();
    }
    public void lifeOn()
    {
        changeState(UnitState.Init);
    }

    private void Update()
    {
        calcTargetData();
        lookAtPos();
        checkState();
        updateState();
    }
    private void checkState()
    {
        if (nowState == nextState) return;
        switch (nowState)
        {//상태에서 나갈때 행동
            case UnitState.Death:
                break;
            case UnitState.Init:
                break;
            case UnitState.Idle:
                break;
            case UnitState.Move:
                break;
            case UnitState.Attack:
                //공격의 의한 이동, 회전 잠금 해제
                isLookLock = false;
                isMoveLock = false;
                break;
            case UnitState.Skill:
                break;
            case UnitState.Condition:
                break;
        }
        nowState = nextState;
        switch (nextState)
        {//상태 들어올때 행동
            case UnitState.Death:
                targetCtrl.lifeOff();
                break;
            case UnitState.Init://초기화
                hpCtrl.resetLife();
                targetCtrl.lifeOn();
                unitInfo.attackInit();
                modelAni.SetFloat("Forward", 0f);
                changeState(UnitState.Idle);
                break;
            case UnitState.Idle:
                modelAni.SetFloat("Forward", 0f);
                break;
            case UnitState.Move:
                modelAni.SetFloat("Forward", 1f);
                navAgentCtrl.chaseOn();
                break;
            case UnitState.Attack:
                unitInfo.attackInit();
                isLookLock = true;
                isMoveLock = true;
                //lookAtPos();공격호출시 바라보기 < 항상바라보는 형태로 변경함
                modelAni.SetInteger("AttackNum", 0);
                modelAni.SetTrigger("AttackTrigger");
                break;
            case UnitState.Skill:
                break;
            case UnitState.Condition:
                break;
        }

    }
    private void updateState()
    {

        switch (nowState)
        {//상태 업데이트
            case UnitState.Death:
                break;
            case UnitState.Init:
                break;
            case UnitState.Idle:
                if (targetDis > unitInfo.AttackRange)
                {
                    //공격범위밖 이동시작
                    changeState(UnitState.Move);
                }
                else if (targetCtrl.TargetTran != null && unitInfo.isAttackOn())
                {
                    //공격범위안 - 타겟있음
                    changeState(UnitState.Attack);
                }
                break;
            case UnitState.Move:
                if (navAgentCtrl.IsChase == false)
                {
                    changeState(UnitState.Idle);
                }
                break;
            case UnitState.Attack:
                if (checkAniTag(aniTag_Attack, aniTag_Idle))
                {
                    //if (AttackCountCheck()) //연타 공격에 대한 구상
                    //{//어택횟수가 남음
                    //    if (checkTarget() == false)//타겟변경 확인(타겟사망,거리초과)
                    //    {//유지시 추가어택
                    //     //attackCall
                    //     //return;
                    //    }
                    //}
                    changeState(UnitState.Idle);//공격완료후 대기로
                }
                break;
            case UnitState.Skill:
                break;
            case UnitState.Condition:
                break;
        }
    }
    private void calcTargetData()
    {
        if (targetCtrl.TargetTran != null)//타겟있음
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = targetCtrl.TargetTran.position;
            myPos.y = targetPos.y = 0;//Y축을 통일해서 평면상의 거리와 방향을 계산

            targetDis = Vector3.Distance(myPos, targetPos);
            targetDir = targetPos - myPos;

        }
        else
        {
            targetCtrl.findAutoTargeting();
            if (targetDis > 0)//타겟없음 대기
            {
                targetDis = -1;
                targetDir = Vector3.zero;
            }
        }
    }



    public void changeState(UnitState newState)
    {
        if (nextState == UnitState.Death && newState != UnitState.Init) return;//사망은 변경불가 - 초기화로만 가능
        nextState = newState;
    }

    public bool checkAniTag(int agoTag, int nextTag)//애니메이션 넘어간것 체크
    {
        aniStateInfo = modelAni.GetCurrentAnimatorStateInfo(0);
        if (agoTag != nowAniTag)
        {
            nowAniTag = aniStateInfo.tagHash;
            return false;
        }
        nowAniTag = aniStateInfo.tagHash;
        return nowAniTag == nextTag;
    }

    public float getChaseRange()
    {
        return unitInfo.AttackRange;
    }

    public void moveNav(Vector3 nextMove)
    {
        if (isMoveLock) return;
        Vector3 nextDir = nextMove - transform.position;//이동은 네비기준이니까 Dir 따로 계산
        nextDir = transform.InverseTransformPoint(transform.position + nextDir);
        transform.Translate(nextDir * unitInfo.MoveSpeed);
        //modelAni.SetFloat("Forward", 1f);//Root 모션등 이동값을 따로 입력해줘야할경우
        //Debug.Log(nextDir.ToString("F3"));
    }

    public void lookAtPos()
    {
        lookAtPos(targetDir);
    }
    private void lookAtPos(Vector3 lookDir)
    {
        if (isLookLock) return;
        float nextRotY = (Mathf.Atan2(lookDir.x, lookDir.z) * Mathf.Rad2Deg) - transform.eulerAngles.y;
        //transform.eulerAngles = nextRot;
        transform.Rotate(Vector3.up, nextRotY);
    }
}

public enum UnitState
{
    Init = -10,//초기화
    Death = -1,//사망
    None = 0,//사용불가 - 리셋용
    Idle = 1,//대기
    Move = 2,//이동
    Attack = 3,//공격
    Skill = 4,//스킬 - 스킬 사용후 외부적으로 풀어줘야함
    Condition = 5,//상태이상
}