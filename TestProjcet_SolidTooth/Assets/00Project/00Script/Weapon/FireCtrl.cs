using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FireCtrl : MonoBehaviour
{
    [SerializeField]
    private Transform firePivot;//발사 시작위치
    [SerializeField]
    private Transform bulletPrefab;
    [SerializeField]
    private Vector2 bulletSpeed;
    [SerializeField]
    private Vector2 shotDelay;
    [SerializeField]
    private ParticleSystem shotEffect;
    [SerializeField]
    private string hitEffectTag;

    private Queue<Transform> bulletPool = new Queue<Transform>();

    public void shotAuto(TargetCtrl targetTran, Action<HpCtrl> damageAction)
    {
        Transform bulletTran = null;
        if (bulletPool.Count > 0)
        {
            bulletTran = bulletPool.Dequeue();
        }
        else
        {
            bulletTran = Instantiate<Transform>(bulletPrefab);//탄환의 부모는 null

        }
        StartCoroutine(cor_ShotAuto(bulletTran, targetTran, damageAction));
    }
    private IEnumerator cor_ShotAuto(Transform bulletTran, TargetCtrl targetCtrl, Action<HpCtrl> damageAction)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(shotDelay.x, shotDelay.y));
        float bulletSpeed = this.bulletSpeed.y <= 0 ? this.bulletSpeed.x : UnityEngine.Random.Range(this.bulletSpeed.x, this.bulletSpeed.y);
        shotEffect.time = 0;
        shotEffect.Play();
        bulletTran.SetParent(null);
        bulletTran.SetPositionAndRotation(firePivot.position, firePivot.rotation);
        bulletTran.gameObject.SetActive(true);
        while (targetCtrl.IsTargeting)//타겟팅이 가능한 상태에서만 공격가능
        {
            bulletTran.Translate((targetCtrl.transform.position - bulletTran.position).normalized * bulletSpeed, Space.World);
            //Debug.Log(Vector3.Distance(targetCtrl.transform.position, bulletTran.position));
            if (Vector3.Distance(targetCtrl.transform.position, bulletTran.position) < 1.05f * bulletSpeed)
            {//근접하면 타격완료
                Debug.Log("탄환 도착");
                break;
            }
            yield return null;
        }
        EffectManager.instance.effectCall(hitEffectTag, bulletTran.position, bulletTran.rotation).gameObject.SetActive(true);
        damageAction(targetCtrl.UnitCtrl.HpCtrl);
        bulletTran.gameObject.SetActive(false);
        bulletTran.SetParent(this.transform);
        bulletPool.Enqueue(bulletTran);
    }
}
