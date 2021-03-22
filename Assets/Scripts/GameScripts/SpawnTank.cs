using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnTank : MonoBehaviour
{
    Ray myRay;      // initializing the ray
    RaycastHit hit; // initializing the raycasthit

    public GameObject SpawnPoint1;
    public GameObject SpawnPoint2;
    public NavMeshAgent surface;
   
    void Update()
    {
        myRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(myRay, out hit))
        {
            // here I ask : 
            //if myRay hits something, store all the info you can find in the raycasthit varible.
            if (Input.GetMouseButtonDown(0) && PickCard.cardSelected)
            {

                if (hit.collider.gameObject == this.gameObject)
                {
                    if (PickCard.playerTag == "Player")
                    {
                        Instantiate(PickCard.tank, SpawnPoint1.gameObject.transform.position, Quaternion.identity);
                        PickCard.cardSelected = false;

                    }
                    else if (PickCard.playerTag == "Player2")
                    {
                        Instantiate(PickCard.tank, SpawnPoint2.gameObject.transform.position, Quaternion.identity);
                        PickCard.cardSelected = false;

                    }
                }

            }
        }

    }
}