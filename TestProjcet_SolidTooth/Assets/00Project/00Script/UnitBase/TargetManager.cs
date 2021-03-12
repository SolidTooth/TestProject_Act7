using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public static TargetManager instance;

    private Dictionary<TargetKind, List<TargetCtrl>> targetListDic = new Dictionary<TargetKind, List<TargetCtrl>>();

    private void Awake()
    {
        instance = this;
    }

    public void addTarget(TargetCtrl targetCtrl)
    {
        if (targetListDic.ContainsKey(targetCtrl.MyKind) == false)
        {
            targetListDic[targetCtrl.MyKind] = new List<TargetCtrl>();
        }
        targetListDic[targetCtrl.MyKind].Add(targetCtrl);
    }

    public void removeTarget(TargetCtrl targetCtrl)
    {
        if (targetListDic.ContainsKey(targetCtrl.MyKind))
        {
            targetListDic[targetCtrl.MyKind].Remove(targetCtrl);
        }
    }

    public TargetCtrl findTarget(TargetCtrl finder, TargetKind findTargetKind)
    {//자신과 가장 가까운 타겟을 찾아오기
        if (targetListDic.ContainsKey(findTargetKind))
        {
            List<TargetCtrl> findList = targetListDic[findTargetKind].FindAll(x => x.IsTargeting);//찾을 목록 추림
            float minDis = -1;
            int minIndex = -1;
            for (int i = 0; i < findList.Count; i++)
            {
                //if (findList[i].isFindable == false) continue;//비활성화된 상태 > FindAll로 해결 
                float dis = Vector3.Distance(finder.transform.position, findList[i].transform.position);
                //Debug.Log("타겟" + findList[i].name + " : " + dis);
                if (minDis > dis || minIndex < 0)
                {
                    minIndex = i;
                    minDis = dis;
                }
            }
            return minIndex < 0 ? null : findList[minIndex];

        }
        return null;
    }
}
