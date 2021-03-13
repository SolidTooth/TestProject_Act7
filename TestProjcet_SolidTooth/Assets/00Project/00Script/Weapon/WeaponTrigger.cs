using UnityEngine;
using System.Collections;
using System;

public abstract class WeaponTrigger : MonoBehaviour
{
    protected UnitCtrl myUnitCtrl;
    [SerializeField]
    private int skillNum; public int SkillNum => skillNum;
    [SerializeField]
    [Min(1)]
    protected float skillDamagePercent = 1;//스킬 데미지 증가치
    [SerializeField]
    protected float attackSpeed = 1;
    [SerializeField]
    private float attackRange; public float AttackRange => attackRange;//공격사거리 - 0일경우 제한없음

    [Header("쿨타임 관련")]
    [SerializeField]
    private bool isStartOn;
    [SerializeField]
    private float attackCoolTime; public float AttackCoolTime => attackCoolTime;// 공격 대기 시간
    [SerializeField]//Debug용
    private float attackCoolTemp; public float nowAttackCoolTime => attackCoolTemp;//쿨타임 남은시간
    private bool isAttackOn; public bool IsAttackOn => isAttackOn;
    protected bool isAttacking; public bool IsAttacking => isAttacking;


    [Header("파티클 관련")]
    [SerializeField]
    private ParticleSystem effectParticle;

    [Header("자동입력")]
    [SerializeField]
    protected TargetKind targetKind;
    [SerializeField]
    protected Collider myColl;

    public Action skillOn;//스킬 준비됨
    public Action skillUse;//스킬 사용

    /// <summary>
    /// 모션은 돌려쓸수 있으니까 WeaponTrigger가 이팩트 호출
    /// </summary>

    protected void Start()
    {
        if (myColl == null) myColl = GetComponent<Collider>();
        attackTriggerOff();
    }
    public void weaponInit()
    {
        if (isStartOn == false)
        {
            coolDownStart();
        }
    }

    public void setUnitCtrl(UnitCtrl newUnitCtrl)
    {
        myUnitCtrl = newUnitCtrl;
        targetKind = myUnitCtrl.TargetCtrl.EnemyTargetKind;
    }

    public void attackOn()//공격가능 상태로 ON
    {
        isAttackOn = true;
        attackCoolTemp = 0f;
        skillOn?.Invoke();
    }
    public virtual void attackCall()
    {
        isAttackOn = false;
        isAttacking = true;
        coolDownStart();//Todo 스킬 쿨타임 적용 시점에 대한 기획고려가 필요 공격 (사용즉시/사용후) 쿨타임 적용

        myUnitCtrl.ModelAni.SetInteger("AttackNum", skillNum);
        myUnitCtrl.ModelAni.SetFloat("AttackSpeed", attackSpeed);
        myUnitCtrl.ModelAni.SetTrigger("AttackTrigger");

        //attackTriggerOn(); - 트리거를 켜주는 시점은 애니메이션에서 결정
    }
    public virtual void attackEnd()
    {
        isAttacking = false;
        attackTriggerOff();
    }
    public virtual void attackCancle()
    {
        attackEnd();
        if (isAttackOn)
        {
            isAttackOn = false;
            //공격 사용중 캔슬된 처리 
            attackEffectOff();
        }
    }
    public void coolDownStart()
    {//공격 초기화
        attackCoolTemp = attackCoolTime;
        skillUse?.Invoke();
    }
    public virtual void updateCoolTime()
    {//쿨타임 진행중 - 공격진행중엔 쿨타임 적용없음
        //if(isAttacking==false)//Todo 스킬 쿨타임 적용 시점에 대한 기획고려가 필요 공격 (사용즉시/사용후) 쿨타임 적용
        if (isAttackOn == false)
        {
            attackCoolTemp -= Time.deltaTime;//스킬에 따라 쿨타임감소 적용할수 있도록
            if (attackCoolTemp <= 0)
            {
                attackOn();
            }
        }
    }

    public void attackEffectOn()
    {
        if (effectParticle != null)
        {
            effectParticle.gameObject.SetActive(true);
            effectParticle.time = 0f;
            effectParticle.Play();
        }
    }
    public void attackEffectOff()
    {
        if (effectParticle != null)
        {
            effectParticle.gameObject.SetActive(false);
        }
    }

    public virtual void attackTriggerOn()
    {
        if (myColl != null) myColl.enabled = true;
    }
    public virtual void attackTriggerOff()
    {
        if (myColl != null) myColl.enabled = false;
    }

    protected virtual void damageSend(HpCtrl targetHpCtrl)
    {//상태이상 적용같은건 오버라이드로 처리
        if (targetHpCtrl.IsLife && targetHpCtrl.setDamage(myUnitCtrl.UnitInfo.Damage * skillDamagePercent))
        {
            Debug.Log("타겟 사망");
            myUnitCtrl.TargetCtrl.checkTarget();//타겟 사망 새로운 타겟 탐색
        }
    }
}
