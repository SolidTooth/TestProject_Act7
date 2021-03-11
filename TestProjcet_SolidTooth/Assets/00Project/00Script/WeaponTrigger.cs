using UnityEngine;
using System.Collections;

public abstract class WeaponTrigger : MonoBehaviour
{
    protected UnitCtrl myUnitCtrl;
    [SerializeField]
    [Min(1)]
    protected float skillDamagePercent = 1;//스킬 데미지 증가치
    [SerializeField]
    private float attackRange; public float AttackRange => attackRange;//공격사거리 - 0일경우 제한없음

    [Header("자동입력")]
    [SerializeField]
    protected TargetKind targetKind;
    protected Collider myColl;
    protected void Start()
    {
        myColl = GetComponent<Collider>();
        myColl.enabled = false;
    }

    public void setUnitCtrl(UnitCtrl newUnitCtrl)
    {
        myUnitCtrl = newUnitCtrl;
        targetKind = myUnitCtrl.TargetCtrl.EnemyTargetKind;
    }
    public void attackOn()
    {
        myColl.enabled = true;
    }
    public void attackOff()
    {
        myColl.enabled = false;
    }
}
