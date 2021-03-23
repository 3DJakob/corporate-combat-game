using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour {
    public GameObject Card;

    // Start is called before the first frame update
    void Start() {
        Debug.Log("Woho Im running!!");
    }

    public void initiate(Transform placement, string[] CardNames) {
        Debug.Log("Im initiated!");
        Debug.Log(placement.position);

        GameObject Empty = new GameObject();
        GameObject theParent = GameObject.Instantiate(Empty, placement.position, placement.rotation);

        string prefabName = "FastTank";
        string pathOfPrefabDirectory = "Cards/";
        var prefabInstance = Resources.Load(pathOfPrefabDirectory + prefabName) as GameObject;


        for(int i = 0; i < 8; i++) {
            

            GameObject theObj = GameObject.Instantiate(Card, theParent.transform, false);
            theObj.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            theObj.transform.localPosition = new Vector3(theObj.transform.position.x  + 0.1f * i, 0, 0);
        }

    }

    // Update is called once per frame
    void Update() {
        
    }
}
