using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARSelect : MonoBehaviour
{
    public GameObject arCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Select() {
        RaycastHit hit;
        Debug.Log("clicked!");

        if (Physics.Raycast(arCamera.transform.position, arCamera.transform.forward, out hit)) {
            // if (hit.transform.name == "")
            Debug.Log("hiiiiit!");
            Debug.Log(hit.transform.name);

            GameObject theObj = hit.transform.gameObject;

            
            Card card = (Card) theObj.GetComponent(typeof(Card));
            if (card) {
                card.Spawn();
            }


        }
    }
}
