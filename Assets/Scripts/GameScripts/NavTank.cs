using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class NavTank : MonoBehaviour
{
    public static NavMeshAgent meshAgent;
    public int team;
    private Rigidbody rb;
    //private PhotonView PV;


    [SerializeField] Transform destination;
    // Start is called before the first frame update
    void Start()
    {
        //PV = GetComponent<PhotonView>();
        meshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        
        if (meshAgent == null)
        {
            Debug.LogError("The nav mesh agent is not attached to " + gameObject.name);
        }
        
        if (!meshAgent.isOnNavMesh)
        {
            //Set to position you want to warp to
            meshAgent.Warp(transform.localPosition);
            meshAgent.enabled = false;
            meshAgent.enabled = true;
        }
        SetDestination();
    }


    public void WarpToPosition(Vector3 pos) {

        //Set to position you want to warp to
        if (!meshAgent.isOnNavMesh)
        {
            meshAgent.Warp(pos);
            meshAgent.enabled = false;
            meshAgent.enabled = true;
        }
        SetDestination();
    }

    public void SetDestination()
    {
        if (meshAgent.isOnNavMesh)
        {
            if (team == 0)
            {
                meshAgent.SetDestination(GameSetup.GS.instanceOfMap.transform.Find("SpawnPoint t2").transform.position);
            }
            else 
            {
                meshAgent.SetDestination(GameSetup.GS.instanceOfMap.transform.Find("SpawnPoint t1").transform.position);
            }
        }
    }

    //private void OnTriggerEnter(Collider other){
    private void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.tag == "Tank")
        {
            stopMove();
            //DestroyGameObject();
        }
    }

    private void OnCollisionExit(Collision collider)
    {
        startMove();
    }

    void stopMove()
    {
        Debug.Log("Stop Move kallas");
        meshAgent.speed = 0;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    void DestroyGameObject()
    {
        Destroy(gameObject);
    }

    void startMove()
    {
        rb.constraints = RigidbodyConstraints.FreezePosition;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        meshAgent.speed = 20f;
    }

    private void Update()
    {
        //This is to prevent a unity bug from occurring
        if (!meshAgent.isOnNavMesh)
        {
            //By reenabling the navMeshAgent it is able to find the navMesh
            meshAgent.enabled = false;
            meshAgent.enabled = true;
            SetDestination();
            Debug.Log("Sätter destination efter ReEnable i Update()");
        }
    }

}
