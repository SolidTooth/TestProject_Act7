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
    // 이동속도
    [SerializeField]
    private float moveSpeed; public float MoveSpeed => moveSpeed;
    // 인식거리
    [SerializeField]
    private float lookRange; public float LookRange => lookRange;//0은 무제한
    // 추격제한거리(없음)

}
