using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBtn : MonoBehaviour
{
    [SerializeField]
    private Text coolTimeText;
    [SerializeField]
    private Image iconImage; public Image IconImage => iconImage;
    [SerializeField]
    private WeaponTrigger weapon;

    private void Start()
    {
        coolTimeText.gameObject.SetActive(false);
    }

    public void setWeapon(WeaponTrigger newWeapon)
    {
        weapon = newWeapon;
        weapon.skillOn = skillOn;
        weapon.skillUse = useSkill;
        if (weapon.IsAttackOn == false) useSkill();//등록당시 스킬 쿨타임 확인
    }
    public void skillBtn()
    {
        if (weapon == null || weapon.IsAttackOn == false) return;
        PlayerCtrl.instance.skillCall(weapon);
    }
    public void useSkill()
    {
        iconImage.color = Color.gray;
        coolTimeText.gameObject.SetActive(true);
        StartCoroutine(cor_CoolTime());
    }
    private IEnumerator cor_CoolTime()
    {
        while (weapon != null && weapon.IsAttackOn == false)
        {
            coolTimeText.text = $"{weapon.nowAttackCoolTime + 1:N0}";//소수점 없이 표현
            yield return null;
        }
    }
    public void skillOn()
    {
        iconImage.color = Color.white;
        coolTimeText.gameObject.SetActive(false);
    }
}
