using UnityEngine;
using System.Collections;

public abstract class WeaponTrigger : MonoBehaviour
{
    protected UnitCtrl myUnitCtrl;
    [SerializeField]
    private int attackMotionNum;
    [SerializeField]
    [Min(1)]
    protected float skillDamagePercent = 1;//스킬 데미지 증가치
    [SerializeField]
    private float attackRange; public float AttackRange => attackRange;//공격사거리 - 0일경우 제한없음
                                                                       // 공격 대기 시간
    [SerializeField]
    private float attackCoolTime; public float AttackCoolTime => attackCoolTime;
    [SerializeField]//Debug용
    private float attackCoolTemp; public bool IsAttackOn => attackCoolTemp >= attackCoolTime;
    protected bool isAttacking; public bool IsAttacking => isAttacking;

    [Header("자동입력")]
    [SerializeField]
    protected TargetKind targetKind;
    [SerializeField]
    protected Collider myColl;

    /// <summary>
    /// 모션은 돌려쓸수 있으니까 WeaponTrigger가 이팩트 호출
    /// </summary>

    protected void Start()
    {
        if (myColl == null) myColl = GetComponent<Collider>();
        myColl.enabled = false;
    }

    public void setUnitCtrl(UnitCtrl newUnitCtrl)
    {
        myUnitCtrl = newUnitCtrl;
        targetKind = myUnitCtrl.TargetCtrl.EnemyTargetKind;
    }

    public void attackOn()//공격가능 상태로 ON
    {
        attackCoolTemp = attackCoolTime;
    }
    public virtual void attackCall()
    {
        isAttacking = true;
        coolDownStart();//Todo 스킬 쿨타임 적용 시점에 대한 기획고려가 필요 공격 (사용즉시/사용후) 쿨타임 적용
        myUnitCtrl.ModelAni.SetInteger("AttackNum", attackMotionNum);
        myUnitCtrl.ModelAni.SetTrigger("AttackTrigger");
        //attackTriggerOn(); - 트리거를 켜주는 시점은 애니메이션에서 결정
    }
    public virtual void attackEnd()
    {
        isAttacking = false;
        if (myColl.enabled == true)
        {
            attackTriggerOff();
        }
    }
    public void coolDownStart()
    {//공격 초기화
        attackCoolTemp = 0f;
    }
    public virtual void updateCoolTime()
    {//쿨타임 진행중 - 공격진행중엔 쿨타임 적용없음
        //if(isAttacking==false)//Todo 스킬 쿨타임 적용 시점에 대한 기획고려가 필요 공격 (사용즉시/사용후) 쿨타임 적용
        attackCoolTemp += Time.deltaTime;//스킬에 따라 쿨타임감소 적용할수 있도록
    }

    public void attackEffectOn()
    {

    }
    public void attackEffectOff()
    {

    }

    public void attackTriggerOn()
    {
        myColl.enabled = true;
    }
    public void attackTriggerOff()
    {
        myColl.enabled = false;
    }

    protected virtual void damageSend(HpCtrl targetHpCtrl)
    {//상태이상 적용같은건 오버라이드로 처리
        if (targetHpCtrl.setDamage(myUnitCtrl.UnitInfo.Damage * skillDamagePercent))
        {
            myUnitCtrl.TargetCtrl.checkTarget();//타겟 사망 새로운 타겟 탐색
        }
    }
}
