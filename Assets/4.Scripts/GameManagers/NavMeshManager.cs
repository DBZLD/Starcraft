using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshManager : MonoBehaviour
{
    public NavMeshSurface m_NavMeshSurfaceS;
    public NavMeshSurface m_NavMeshSurfaceM;
    public NavMeshSurface m_NavMeshSurfaceL;
    private void Awake()
    {
        m_NavMeshSurfaceS = GetComponent<NavMeshSurface>();
        m_NavMeshSurfaceM = GetComponent<NavMeshSurface>();
        m_NavMeshSurfaceL = GetComponent<NavMeshSurface>();
    }
    private void Start()
    {
        NavMeshBake();
    }

    public void NavMeshBake()
    {
        m_NavMeshSurfaceS.BuildNavMesh();
        m_NavMeshSurfaceM.BuildNavMesh();
        m_NavMeshSurfaceL.BuildNavMesh();
    }
}
