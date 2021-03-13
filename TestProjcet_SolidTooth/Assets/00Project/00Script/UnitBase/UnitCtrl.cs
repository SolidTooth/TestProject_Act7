using UnityEngine;
using System.Collections;
using System;

public class UnitCtrl : MonoBehaviour
{
    private static readonly int aniTag_Idle = Animator.StringToHash("Idle");
    private static readonly int aniTag_Move = Animator.StringToHash("Move");
    private static readonly int aniTag_Attack = Animator.StringToHash("Attack");
    private static readonly int aniTag_Death = Animator.StringToHash("Death");
    private static readonly int aniTag_Hit = Animator.StringToHash("Hit");

    [SerializeField]
    private UnitInfo unitInfo; public UnitInfo UnitInfo => unitInfo;
    [SerializeField]
    private bool isAutoLife;

    [SerializeField]
    private Animator modelAni; public Animator ModelAni => modelAni;
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


    [SerializeField]
    private WeaponCtrl weaponCtrl; public WeaponCtrl WeaponCtrl => weaponCtrl;

    private void OnEnable()
    {
        if (isAutoLife) lifeOn();
        navAgentCtrl.posNavInit();//네비 위로 위치 초기화
    }
    private void OnDisable()
    {
        lifeOff();
    }
    private void Awake()
    {
        if (hpCtrl == null) hpCtrl = GetComponent<HpCtrl>();
        hpCtrl.setUnitCtrl(this);
        hpCtrl.setMaxHp(unitInfo.MaxHp);

        if (navAgentCtrl == null) navAgentCtrl = GetComponent<NavAgentCtrl>();
        navAgentCtrl?.setUnitCtrl(this);

        if (targetCtrl == null) targetCtrl = GetComponent<TargetCtrl>();
        targetCtrl?.setUnitCtrl(this);

        if (weaponCtrl != null) weaponCtrl = GetComponent<WeaponCtrl>();
        weaponCtrl.setUnitCtrl(this);

    }
    private void Update()
    {
        checkState();
        updateState();
    }
    public void lifeOn()
    {
        changeState(UnitState.Init);
    }
    public void lifeOff()
    {
        weaponCtrl.attackCalcle();//공격중단
        targetCtrl.removeTargetList();
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
                navAgentCtrl.chaseOff();//이동에서 빠져나오면 네비이동 꺼줌
                break;
            case UnitState.Attack:
                //공격의 의한 이동, 회전 잠금 해제
                if (nextState != UnitState.Condition)
                {//다음 행동이 상태이상이 아니라면
                    weaponCtrl.attackEnd();
                    isLookLock = false;
                    isMoveLock = false;
                }
                break;
            case UnitState.Condition:
                break;
        }
        nowState = nextState;
        switch (nextState)
        {//상태 들어올때 행동
            case UnitState.Death:
                lifeOff();
                modelAni.SetTrigger("Death");//사망 모션 호출
                break;
            case UnitState.Init://초기화
                hpCtrl.resetLife();//체력 초기화
                navAgentCtrl.posNavInit();//위치 초기화
                targetCtrl.setTargetList();//타겟시스템 초기화
                weaponCtrl.attackInit();//무기 초기화
                //애니메이션 초기화
                modelAni.SetFloat("Forward", 0f);
                modelAni.SetTrigger("Reset");
                changeState(UnitState.Idle);//초기화 완료 상태변경
                break;
            case UnitState.Idle:
                modelAni.SetFloat("Forward", 0f);
                lookAtPos();
                break;
            case UnitState.Move:
                navAgentCtrl.posTranInit();
                modelAni.SetFloat("Forward", 1f);
                navAgentCtrl.chaseOn();
                break;
            case UnitState.Attack:
                Debug.Log("공격 요청");
                modelAni.SetFloat("Forward", 0f);
                lookAtPos();
                weaponCtrl.attackCall();
                isLookLock = true;
                isMoveLock = true;
                break;
            case UnitState.Condition:
                weaponCtrl.attackCalcle();
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
                targetCtrl.checkTarget();//타겟의 상태를 확인해서 변경할지
                //targetCtrl.findAutoTargeting();//항상 새로운 타겟을 찾을지는 기획의도가 필요
                if (targetCtrl.TargetDis == 0) return;//타겟 없음 제자리에서 대기
                if (weaponCtrl.NowAttackRange > 0 && targetCtrl.TargetDis > weaponCtrl.NowAttackRange)
                {
                    //공격범위밖 이동시작
                    changeState(UnitState.Move);
                }
                else if (weaponCtrl.IsNowAttackOn && (targetCtrl.TargetTran != null || weaponCtrl.NowAttackRange <= 0))
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
                else if (weaponCtrl.NowAttackRange <= 0)
                {
                    changeState(UnitState.Attack);
                }
                break;
            case UnitState.Attack:
                if (checkAniTagChange(aniTag_Attack, aniTag_Idle))
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
            case UnitState.Condition:
                break;
        }
    }




    public void changeState(UnitState newState)
    {
        if (nextState == UnitState.Death && newState != UnitState.Init) return;//사망은 변경불가 - 초기화로만 가능
        nextState = newState;
    }

    public bool checkAniTagChange(int agoTag, int nextTag)//애니메이션 넘어간것 체크
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

    public bool checkAniTag(int nextTag)//애니메이션 현재를 체크
    {
        aniStateInfo = modelAni.GetCurrentAnimatorStateInfo(0);
        nowAniTag = aniStateInfo.tagHash;
        return nowAniTag == nextTag;
    }

    public float getChaseRange()
    {
        //현재 타겟이 없거나 타겟팅불가 적이 아니면 가까이 이동만, 적일 경우 공격 사거리까지 접근
        return targetCtrl.IsTargetEnemy ? weaponCtrl.NowAttackRange : 0.2f;
    }

    public void moveNav(Vector3 nextMove)
    {
        if (isMoveLock) return;
        Vector3 nextDir = nextMove - transform.position;//이동은 네비기준이니까 Dir 따로 계산
        lookAtPos(nextDir);
        transform.Translate(nextDir, Space.World);
        //Debug.Log(nextDir.ToString("F3"));
    }

    public void lookAtPos()
    {
        lookAtPos(targetCtrl.TargetDir);
    }

    private void lookAtPos(Vector3 lookDir)
    {
        if (isLookLock) return;
        float nextRotY = (Mathf.Atan2(lookDir.x, lookDir.z) * Mathf.Rad2Deg) - transform.eulerAngles.y;
        //transform.eulerAngles = nextRot;
        transform.Rotate(Vector3.up, nextRotY);
    }

    public void hitMotion()
    {
        if (nowState == nextState && (nowState == UnitState.Idle || nowState == UnitState.Move))//대기나 이동,Hit 상태에서만 hit모션
        {
            modelAni.SetTrigger("Hit");
        }
    }

    //애니메이션 이벤트키 호출
    public void attackEffectOn()
    {
        weaponCtrl.NowWeapon.attackEffectOn();
    }
    public void attackEffectOff()
    {
        weaponCtrl.NowWeapon.attackEffectOn();
    }
    public void attackTriggerOn()
    {
        weaponCtrl.NowWeapon.attackTriggerOn();
    }
    public void attackTriggerOff()
    {
        weaponCtrl.NowWeapon.attackTriggerOff();
    }

    private void OnGUI()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            lifeOn();//부활
        }
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
    Condition = 4,//상태이상 - 주로 기절같은 그로기 상태
}