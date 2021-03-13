using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBarManager : MonoBehaviour
{
    public static HpBarManager instance;

    [SerializeField]
    private HpBarCtrl hpBarPrefab;

    private Queue<HpBarCtrl> hpBarPool = new Queue<HpBarCtrl>();

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

}
