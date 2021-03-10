using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class NavManager : MonoBehaviour
{
    /// 네비존 관리와 타겟리스트 관리
    [SerializeField]
    public NavMeshSurface navSurface;
    private void Start()
    {
        if (navSurface == null) navSurface = GetComponent<NavMeshSurface>();
        navSurface.RemoveData();
        navSurface.BuildNavMesh();
    }
}
