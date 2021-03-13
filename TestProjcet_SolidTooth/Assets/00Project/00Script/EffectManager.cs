using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;
    // Use this for initialization
    void Awake()
    {
        instance = this;
    }
    [SerializeField]
    private List<EffectCtrl> effectList = new List<EffectCtrl>();

    private Dictionary<string, Queue<EffectCtrl>> poolDic = new Dictionary<string, Queue<EffectCtrl>>();

    public EffectCtrl effectCall(string effectTag, Vector3 effectPos, Quaternion effectRot)
    {
        EffectCtrl effectCtrl = effectList.Find(x => x.name.Equals(effectTag));
        if (effectCtrl != null)
        {
            if (poolDic.ContainsKey(effectTag) == false)
            {
                poolDic[effectTag] = new Queue<EffectCtrl>();
            }
            if (poolDic[effectTag].Count > 0)
            {
                effectCtrl = poolDic[effectTag].Dequeue();
            }
            else
            {
                effectCtrl = Instantiate(effectCtrl, this.transform);
                effectCtrl.effectTag = effectTag;
                effectCtrl.gameObject.SetActive(false);
            }
            effectCtrl.effectOn();
            effectCtrl.transform.SetPositionAndRotation(effectPos, effectRot);
        }
        return effectCtrl;
    }
    public void pushEffect(EffectCtrl effectTran)
    {
        if (poolDic.ContainsKey(effectTran.effectTag))
        {
            poolDic[effectTran.effectTag].Enqueue(effectTran);
        }
    }

}
