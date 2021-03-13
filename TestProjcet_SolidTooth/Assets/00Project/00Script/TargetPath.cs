using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetPath : TargetCtrl
{
    [Header("Path")]
    [SerializeField]
    public bool isFirst;
    public Transform pathTran;
    public UnitCtrl[] enemyArr;

    public TargetPath nextPath;//연결 자동화 만들면 로그라이크 맵 형태도 가능

    private void Start()
    {
        if (pathTran == null) pathTran = GetComponent<Transform>();
        pathInit();
    }
    public void pathInit()
    {
        if (isFirst)
        {
            enemyActive();
            setTargetList();
        }
        else
        {
            for (int i = 0; i < enemyArr.Length; i++)
            {
                enemyArr[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("PLAYER"))
        {
            UnitCtrl playerUnitCtrl = other.GetComponent<UnitCtrl>();
            for (int i = 0; i < enemyArr.Length; i++)
            {
                enemyArr[i].lifeOn();
                enemyArr[i].TargetCtrl.setTargetList();
                enemyArr[i].TargetCtrl.findTarget();
            }
            playerUnitCtrl.TargetCtrl.findTarget();
            removeTargetList();
            if (nextPath != null) nextPath.enemyActive();
            this.gameObject.SetActive(false);
        }
    }
    private void enemyActive()
    {
        for (int i = 0; i < enemyArr.Length; i++)
        {
            enemyArr[i].gameObject.SetActive(true);
        }
        setTargetList();
    }


}
