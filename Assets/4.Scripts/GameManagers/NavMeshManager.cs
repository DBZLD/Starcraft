using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshManager : MonoBehaviour
{
    public NavMeshSurface m_NavMeshSurface;

    private void Awake()
    {
        m_NavMeshSurface = GetComponent<NavMeshSurface>();
    }
    private void Start()
    {
        NavMeshBake();
    }

    public void NavMeshBake()
    {
        m_NavMeshSurface.BuildNavMesh();
    }
}
