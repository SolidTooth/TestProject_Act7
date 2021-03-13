using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentCtrl : MonoBehaviour
{
    private UnitCtrl unitCtrl;
    private NavMeshAgent agent;


    [SerializeField]
    private bool isChase; public bool IsChase => isChase;//추격중
    [SerializeField]
    private float dis;
    public void setUnitCtrl(UnitCtrl newUnitCtrl)
    {
        unitCtrl = newUnitCtrl;
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = false;
    }
    public void posNavInit()
    {
        unitCtrl.transform.position = agent.nextPosition;
    }
    public void posTranInit()
    {
        agent.nextPosition = unitCtrl.transform.position;
    }

    public void chaseOn()
    {
        isChase = true;
        agent.speed = unitCtrl.UnitInfo.MoveSpeed;
        agent.nextPosition = unitCtrl.transform.position;
        agent.SetDestination(unitCtrl.TargetCtrl.TargetTran.position);
        StartCoroutine(cor_ChaseUpdate());
    }
    private IEnumerator cor_ChaseUpdate()
    {
        while (agent != null && unitCtrl.TargetCtrl.TargetTran != null && isChase)
        {
            yield return null;
            unitCtrl.moveNav(agent.nextPosition);
            dis = agent.remainingDistance;
            if (agent.remainingDistance < unitCtrl.getChaseRange() || unitCtrl.getChaseRange() <= 0)
            {
                isChase = false;
                agent.ResetPath();
                Debug.Log("도착" + agent.remainingDistance);
                break;
            }
            agent.SetDestination(unitCtrl.TargetCtrl.TargetTran.position);
        }
    }

    public void chaseOff()
    {
        agent.ResetPath();
    }

}
