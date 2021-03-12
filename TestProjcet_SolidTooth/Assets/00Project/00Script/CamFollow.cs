using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField]
    private Transform playerTran;
    [SerializeField]
    private Transform camPivotTran;

    // Update is called once per frame
    void Update()
    {
        camPivotTran.position = Vector3.Lerp(camPivotTran.position, playerTran.position, 0.1f);
    }
}
