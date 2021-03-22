using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavTank : MonoBehaviour
{
    public float tankHealth = 50f;
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

    private void SetDestination()
    {
        Vector3 target = destination.transform.position;
        meshAgent.SetDestination(target);
    }

    //private void OnTriggerEnter(Collider other){
    private void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.tag == "Tank")
        {

            //StopMove();

            //DestroyGameObject();

        }

    }
    private void OnCollisionExit(Collision collider)
    {
        //StartMove();
    }

    public void StopMove()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
        meshAgent.isStopped = true;
        //Debug.Log("Stopped!");
        //shoot();



    }

    void DestroyGameObject()
    {
        Destroy(gameObject);
    }

    public void StartMove()
    {

        //rb.constraints = RigidbodyConstraints.FreezePosition;
        //rb.constraints = RigidbodyConstraints.FreezeRotation;
        meshAgent.isStopped = false;
        Debug.Log("Moving");
    }

    public void TakeDamage(float amount)
    {
        tankHealth -= amount;
        if (tankHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

}
