using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public static PlayerCtrl instance;
    [SerializeField]
    private UnitCtrl playerUnitCtrl;
    [SerializeField]
    private SkillBtn[] skillBtnArr;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        playerUnitCtrl.WeaponCtrl.setSkillBtn(skillBtnArr);
    }

    public void skillCall(WeaponTrigger weapon)
    {
        playerUnitCtrl.WeaponCtrl.skillCall(weapon);
    }

}
