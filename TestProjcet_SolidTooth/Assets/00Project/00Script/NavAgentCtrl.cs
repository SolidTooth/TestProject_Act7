using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentCtrl : MonoBehaviour
{
    private NavMeshAgent agent;
    private UnitCtrl unitCtrl;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        unitCtrl = GetComponent<UnitCtrl>();
        agent.updatePosition = false;
        agent.updateRotation = false;
    }

    [SerializeField]
    private bool isChase; public bool IsChase => isChase;//추격중



    public void chaseOn()
    {
        isChase = true;
        agent.SetDestination(unitCtrl.TargetTran.position);
        StartCoroutine(cor_ChaseUpdate());
    }
    private IEnumerator cor_ChaseUpdate()
    {
        while (agent != null && unitCtrl.TargetTran != null && isChase)
        {
            yield return null;
            unitCtrl.moveNav(agent.nextPosition);
            if (agent.remainingDistance < unitCtrl.getChaseRange())
            {
                isChase = false;
                agent.ResetPath();
                Debug.Log("도착" + agent.remainingDistance);
                break;
            }
            agent.SetDestination(unitCtrl.TargetTran.position);

        }
    }

    public void chaseOff()
    {
        agent.ResetPath();
    }

}
