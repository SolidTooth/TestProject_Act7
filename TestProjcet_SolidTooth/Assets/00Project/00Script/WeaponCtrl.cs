using UnityEngine;
using System.Collections;

public class WeaponCtrl : MonoBehaviour
{
    private UnitCtrl myUnitCtrl;
    [SerializeField]
    private WeaponTrigger basicWeapon;

    [SerializeField]
    private WeaponTrigger nowSkill;
    public float NowAttackRange => nowSkill == null ? basicWeapon.AttackRange : nowSkill.AttackRange;

    public virtual void setUnitCtrl(UnitCtrl newUnitCtrl)
    {
        myUnitCtrl = newUnitCtrl;
        basicWeapon.setUnitCtrl(myUnitCtrl);
    }
}
