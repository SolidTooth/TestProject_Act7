using UnityEngine;
using System.Collections;
[System.Serializable]
public class UnitInfo
{
    // 체력
    [SerializeField]
    private float maxHp; public float MaxHp => maxHp;
    // 공격력
    [SerializeField]
    private float damage; public float Damage => damage;
    // 공격 대기 시간
    [SerializeField]
    private float attackDelay; public float AttackDelay => attackDelay;
    private float attackDelayTemp;
    public void attackInit()
    {
        attackDelayTemp = 0f;
    }
    public bool isAttackOn()
    {
        attackDelayTemp += Time.deltaTime;
        return attackDelayTemp >= attackDelay;
    }
    // 이동속도
    [SerializeField]
    private float moveSpeed; public float MoveSpeed => moveSpeed;
    // 인식거리
    [SerializeField]
    private float lookRange; public float LookRange => lookRange;//0은 무제한
    // 공격사거리 - 무기에 따라 달라지도록 모듈화 하면 좋음
    [SerializeField]
    private float attackRange; public float AttackRange => attackRange;//0은 무제한
    // 추격제한거리(없음)

}
