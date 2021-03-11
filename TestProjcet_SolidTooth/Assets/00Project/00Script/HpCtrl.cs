using UnityEngine;
using System.Collections;

public class HpCtrl : MonoBehaviour
{
    private UnitCtrl myUnitCtrl;
    private float maxHp;
    [SerializeField]
    private float nowHp; public float NowHp => nowHp;
    private bool isLife; public bool IsLife => isLife;

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
    }
    public bool setDamage(float damage)//true 죽음, false 생존
    {
        if (isLife == false) return true;//죽은자에게 칼을...
        nowHp -= damage;
        if (nowHp <= 0)
        {
            //사망처리
            isLife = false;
            myUnitCtrl.changeState(UnitState.Death);
            return true;
        }
        return false;
    }
    public void resetLife()
    {
        isLife = true;
        nowHp = maxHp;
    }
}

