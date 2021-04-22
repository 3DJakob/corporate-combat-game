// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.AI;

// public class NavTank : MonoBehaviour
// {
//     public float tankHealth = 50f;
//     public static NavMeshAgent meshAgent;
//     private Rigidbody rb;

//     [SerializeField] Transform destination;
//     // Start is called before the first frame update
//     void Start()
//     {
//         meshAgent = this.GetComponent<NavMeshAgent>();
//         rb = GetComponent<Rigidbody>();

//         if (meshAgent == null)
//         {
//             Debug.LogError("The nav mesh agent is not attached to " + gameObject.name);
//         }
//         else
//         {
//             SetDestination();
//         }
//     }

//     private void SetDestination()
//     {
//         Vector3 target = destination.transform.position;
//         meshAgent.SetDestination(target);
//     }

//     public void StopMove()
//     {
//         Debug.Log(meshAgent.isStopped + "Stop " + this.name);
//         meshAgent.isStopped = true;
//     }


//     public void StartMove()
//     {
//         meshAgent.isStopped = false;
//         Debug.Log("Moving");
//     }

//     /*public bool TakeDamage(float amount)
//     {
//         tankHealth -= amount;
//         if (tankHealth <= 0 && gameObject.tag != "Finish")
//         {
//             Die();
//             return true;
//         }
//         else if(tankHealth <= 0 && gameObject.tag == "Finish"){
//             Debug.Log("You WIN!!!");
//             Die();
//             return false;
//         }
//         else{
//             return false;
//         }
//     }

//     void Die()
//     {
//         Destroy(gameObject);
//     }*/

// }
