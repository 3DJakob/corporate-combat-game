using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour
{
    [SerializeField]
    NavMeshSurface surface;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.GetComponent<NavMeshSurface>().BuildNavMesh();
        //surface.BuildNavMesh();
    }

    // Update is called once per frame
}
