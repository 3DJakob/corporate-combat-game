using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour
{
    // Start is called before the first frame update
    public void Bake()
    {
        this.transform.GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    void Start()
    {
        Bake();
        //surface.BuildNavMesh();
    }

    // Update is called once per frame
}
