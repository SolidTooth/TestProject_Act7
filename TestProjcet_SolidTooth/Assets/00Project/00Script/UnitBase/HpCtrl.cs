using UnityEngine;
using System.Collections;

public class HpCtrl : MonoBehaviour
{
    private UnitCtrl myUnitCtrl; public UnitCtrl UnitCtrl => myUnitCtrl;
    private float maxHp;
    [SerializeField]
    private float nowHp; public float NowHp => nowHp;
    public float hpFill => nowHp / maxHp;
    private bool isLife; public bool IsLife => isLife;
    [SerializeField]
    private Transform hpBarPivot; public Transform HpBarPivot => hpBarPivot;
    [SerializeField]
    private bool isHpBarUnActive;//hpbar 제거 안함
    [SerializeField]
    private HpBarCtrl hpBarCtrl;

    //private void Start()
    //{
    //    if(hpBarCtrl!=null)hpBarCtrl.
    //}
    private void OnDisable()
    {
        if (isHpBarUnActive) return;
        hpBarCtrl?.removeTarget();//Hpbar 제거 시점 - 시체제거
        hpBarCtrl = null;
    }
    public void setUnitCtrl(UnitCtrl newUnitCtrl)
    {
        myUnitCtrl = newUnitCtrl;
    }

    public void setMaxHp(float newMaxHp, bool isRecovery = true)
    {
        maxHp = newMaxHp;
        if (isRecovery)
        {
            nowHp = maxHp;
        }
        hpBarCtrl?.setBarValue(hpFill);
    }
    public bool setDamage(float damage)//true 죽음, false 생존
    {
        if (isLife == false) return true;//죽은자에게 칼을...
        nowHp -= damage;
        if (hpBarCtrl != null)
        {
            hpBarCtrl.setBarValue(hpFill);
            HpBarManager.instance.damageText(damage, hpBarCtrl.transform);
        }
        if (nowHp <= 0)
        {
            //사망처리
            isLife = false;
            myUnitCtrl.changeState(UnitState.Death);
            //hpBarCtrl?.removeTarget(); //Hpbar 제거 시점 - 사망즉시
            //hpBarCtrl = null;
            return true;
        }
        myUnitCtrl.hitMotion();//피격당했을때 hit모션
        return false;
    }
    public void resetLife()
    {
        isLife = true;
        nowHp = maxHp;
        if (hpBarPivot != null && hpBarCtrl == null)
        {
            hpBarCtrl = HpBarManager.instance.settingHpBar(this);
        }
        hpBarCtrl?.setBarValue(hpFill);
    }
}

