using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarManager : MonoBehaviour
{
    public static HpBarManager instance;

    [SerializeField]
    private HpBarCtrl hpBarPrefab;
    [SerializeField]
    private Text damageTextPrefab;

    private Queue<HpBarCtrl> hpBarPool = new Queue<HpBarCtrl>();
    private Queue<Text> damageTextPool = new Queue<Text>();

    private void Awake()
    {
        instance = this;
        hpBarPrefab.gameObject.SetActive(false);
    }

    public HpBarCtrl settingHpBar(HpCtrl newHpCtrl)
    {
        HpBarCtrl newHpBar = null;
        if (hpBarPool.Count > 0)
        {
            newHpBar = hpBarPool.Dequeue();
        }
        else
        {
            newHpBar = Instantiate<HpBarCtrl>(hpBarPrefab, hpBarPrefab.transform.parent);
        }
        newHpBar.setTarget(newHpCtrl);
        newHpBar.gameObject.SetActive(true);
        return newHpBar;
    }
    public void pushHpBar(HpBarCtrl newHpBarCtrl)
    {
        if (newHpBarCtrl == null) return;
        hpBarPool.Enqueue(newHpBarCtrl);
        newHpBarCtrl.gameObject.SetActive(false);
    }
    public void damageText(float damage, Transform hpBarTran)
    {
        Text newDamageText = null;
        if (damageTextPool.Count > 0)
        {
            newDamageText = damageTextPool.Dequeue();
        }
        else
        {
            newDamageText = Instantiate(damageTextPrefab.transform.parent.gameObject, this.transform).GetComponentInChildren<Text>();
        }
        newDamageText.text = $"{damage * 10:N0}";
        newDamageText.transform.parent.SetParent(hpBarTran);
        newDamageText.transform.parent.gameObject.SetActive(true);
    }
    public void pushDamageText(Text newDamageText)
    {
        if (newDamageText == null) return;
        newDamageText.transform.parent.SetParent(this.transform);
        newDamageText.transform.parent.gameObject.SetActive(false);
        damageTextPool.Enqueue(newDamageText);
    }

}
