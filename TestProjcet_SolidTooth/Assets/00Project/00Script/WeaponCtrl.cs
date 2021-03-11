using UnityEngine;
using System.Collections;

public class WeaponCtrl : MonoBehaviour
{
    private UnitCtrl myUnitCtrl;
    [SerializeField]
    private WeaponTrigger basicWeapon;
    [SerializeField]
    private WeaponTrigger[] skillWeapon;

    [SerializeField]
    private WeaponTrigger nowSkill; public WeaponTrigger NowWeapon => nowSkill == null ? basicWeapon : nowSkill;
    public float NowAttackRange => nowSkill == null ? basicWeapon.AttackRange : nowSkill.AttackRange;
    public bool IsNowAttackOn => nowSkill == null ? basicWeapon.IsAttackOn : nowSkill.IsAttackOn;

    public virtual void setUnitCtrl(UnitCtrl newUnitCtrl)
    {
        myUnitCtrl = newUnitCtrl;
        basicWeapon.setUnitCtrl(myUnitCtrl);
        for (int i = 0; i < skillWeapon.Length; i++)
        {
            skillWeapon[i].setUnitCtrl(myUnitCtrl);
        }
    }

    public void attackInit()
    {//전체 초기화
        basicWeapon.coolDownStart();
        for (int i = 0; i < skillWeapon.Length; i++)
        {
            skillWeapon[i].coolDownStart();
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
        }
        else
        {
            basicWeapon.attackEnd();
        }
        nowSkill = null;//스킬 사용이 끝났으면 비워줘야지
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
                nowSkill.attackEnd();
                //스킬 사용중에 캔슬 처리할곳
            }
            nowSkill = null;
        }
        basicWeapon.coolDownStart();
      
    }
    private void Update()
    {
        if (myUnitCtrl != null && myUnitCtrl.IsLife)
        {
            if (basicWeapon.IsAttackOn == false) basicWeapon.updateCoolTime();
            for (int i = 0; i < skillWeapon.Length; i++)
            {
                if (skillWeapon[i].IsAttackOn == false) skillWeapon[i].updateCoolTime();
            }
        }
    }
}
