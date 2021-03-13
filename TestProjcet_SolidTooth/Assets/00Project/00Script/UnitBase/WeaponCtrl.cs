using UnityEngine;
using System.Collections;
using System.Linq;

public class WeaponCtrl : MonoBehaviour
{
    private UnitCtrl myUnitCtrl;
    [SerializeField]
    private WeaponTrigger basicWeapon;
    [SerializeField]
    private WeaponTrigger[] skillWeaponArr;

    [SerializeField]
    private WeaponTrigger nowSkill; public WeaponTrigger NowWeapon => nowSkill == null ? basicWeapon : nowSkill;//현재 사용 스킬 or 공격
    private WeaponTrigger nextSkill;
    public float NowAttackRange => nowSkill == null ? basicWeapon.AttackRange : nowSkill.AttackRange;//공격사거리
    public virtual bool IsNowAttackOn => nowSkill == null ? basicWeapon.IsAttackOn : nowSkill.IsAttackOn;

    public virtual void setUnitCtrl(UnitCtrl newUnitCtrl)
    {
        myUnitCtrl = newUnitCtrl;
        basicWeapon.setUnitCtrl(myUnitCtrl);
        for (int i = 0; i < skillWeaponArr.Length; i++)
        {
            skillWeaponArr[i].setUnitCtrl(myUnitCtrl);
        }
    }

    public void attackInit()
    {//전체 초기화
        basicWeapon.weaponInit();
        for (int i = 0; i < skillWeaponArr.Length; i++)
        {
            skillWeaponArr[i].weaponInit();
        }
    }
    public void attackCall()
    {//현재 공격 호출
        if (nowSkill != null)
        {
            nowSkill.attackCall();
        }
        else
        {
            basicWeapon.attackCall();
        }
    }
    public void attackEnd()
    {//현재 공격 끝
        if (nowSkill != null)
        {
            nowSkill.attackEnd();
            nowSkill = null;//스킬 사용이 끝났으면 비워줘야지
        }
        else
        {
            basicWeapon.attackEnd();
        }
        //다음 사용 등록된 스킬이있다면 다음공격 교체
        if (nextSkill != null)
        {
            nowSkill = nextSkill;
            nextSkill = null;
        }
    }

    public void attackCoolDown()
    {//현재 공격 쿨타임적용
        if (nowSkill != null)
        {
            nowSkill.coolDownStart();
        }
        basicWeapon.coolDownStart();//스킬을 사용하면 기본공격은 쿨타임적용
    }

    public void attackCalcle()
    {
        if (nowSkill != null)
        {
            if (nowSkill.IsAttacking)
            {
                nowSkill.attackCancle();
                //스킬 사용중에 캔슬 처리할곳
            }
            nowSkill = null;
        }
        basicWeapon.coolDownStart();

    }

    public void setSkillBtn(SkillBtn[] skillBtnArr)
    {
        //데모는 스킬이 고정 3개지만 안전장치 걸어둠
        for (int i = 0; i < skillBtnArr.Length && i < skillWeaponArr.Length; i++)
        {
            skillBtnArr[i].setWeapon(skillWeaponArr[i]);
        }
    }
    public void skillCall(WeaponTrigger weapon)
    {
        if (skillWeaponArr.Contains(weapon))
        {
            //스킬 버튼 눌렀을때 모션 스킵 가능한지 판단 후 빠르게 발동 - 데모에서 구현하지 않음
            //스킬 발동형태 - 즉시발동 AttackRange = 0 , 사거리 확인
            if (weapon.AttackRange <= 0 || weapon.AttackRange <= myUnitCtrl.TargetCtrl.TargetDis)
            {  //스킬 등록당시 적과의 거리가 사거리 안쪽일 경우만 등록
                if (nowSkill == null && basicWeapon.IsAttacking == false)
                {
                    nowSkill = weapon;
                }
                else
                {
                    nextSkill = weapon;
                }
            }
        }
    }

    private void Update()
    {
        if (myUnitCtrl != null && myUnitCtrl.IsLife)
        {
            if (basicWeapon.IsAttackOn == false) basicWeapon.updateCoolTime();
            for (int i = 0; i < skillWeaponArr.Length; i++)
            {
                if (skillWeaponArr[i].IsAttackOn == false) skillWeaponArr[i].updateCoolTime();
            }
        }
    }
}
