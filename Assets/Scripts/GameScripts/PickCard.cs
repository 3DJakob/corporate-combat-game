using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickCard : MonoBehaviour
{
    // Start is called before the first frame update
    Ray myRay;      // initializing the ray
    RaycastHit hit; // initializing the raycasthit

    private GameObject cardObject;
    
    static public GameObject tank;
    public GameObject serializedTank;
    static public bool cardSelected = false;
    static public string playerTag;


    void Start()
    {
        cardObject = tank = serializedTank;
    }

    /*
    void Update()
    {
        myRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(myRay, out hit))
            // here I ask : 
            //if myRay hits something, store all the info you can find in the raycasthit varible.
                
        {
            if (Input.GetMouseButtonDown(0))
                {
                    if (hit.collider.gameObject == this.gameObject)
                    {
                        tank = this.cardObject;
                        playerTag = this.gameObject.tag;
                        cardSelected = true;
                    }
                }
        }
    }
    */
}
