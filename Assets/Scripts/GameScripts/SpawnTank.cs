using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTank : MonoBehaviour
{
    Ray myRay;      // initializing the ray
    RaycastHit hit; // initializing the raycasthit
    Vector3 spawnPoint = new Vector3(-22f, -12f, 42f);
    Vector3 spawnPoint_p2 = new Vector3(-22f, -11f, -231f);
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
                        Instantiate(PickCard.tank, spawnPoint, Quaternion.identity);
                        PickCard.cardSelected = false;

                    }
                    else if (PickCard.playerTag == "Player2")
                    {
                        Instantiate(PickCard.tank, spawnPoint_p2, Quaternion.identity);
                        PickCard.cardSelected = false;

                    }
                }

            }
        }

    }
}