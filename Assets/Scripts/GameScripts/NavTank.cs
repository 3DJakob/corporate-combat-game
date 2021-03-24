using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavTank : MonoBehaviour
{
    public static NavMeshAgent meshAgent;
    private Rigidbody rb;

    [SerializeField] Transform destination;
    // Start is called before the first frame update
    void Start()
    {
        meshAgent = this.GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        if (meshAgent == null)
        {
            Debug.LogError("The nav mesh agent is not attached to " + gameObject.name);
        }
        else
        {
            SetDestination();
 
        }
    }

    public void SetDestination()
    {
        GameObject target = new GameObject();
        target.transform.SetParent(GameSetup.GS.instanceOfMap.transform, false);
        destination = target.transform;
        

        this.gameObject.GetComponent<NavMeshAgent>().destination = destination.localPosition;
        //meshAgent.SetDestination(target);
    }

    public void SetDestination(Vector3 target)
    {
        meshAgent = this.GetComponent<NavMeshAgent>(); //Don't know why this is needed here aswell but did not work without it
        //Debug.Log(meshAgent);
        meshAgent.SetDestination(target);
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

        meshAgent.speed = 50f;
    }

}
