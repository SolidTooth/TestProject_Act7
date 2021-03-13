using UnityEngine;
using System.Collections;

public class EffectCtrl : MonoBehaviour
{
    [SerializeField]
    public string effectTag;
    [SerializeField]
    private bool isPool;

    public void effectOn()
    {
        isPool = true;
    }

    public void effectOff()
    {
        if (isPool == false)
        {
            isPool = true;
            EffectManager.instance.pushEffect(this);
        }
    }
    private void OnDisable()
    {
        effectOff();
    }
}
