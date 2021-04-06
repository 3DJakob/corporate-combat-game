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
        // theParent.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        string pathOfPrefabDirectory = "Cards/";

        float place = (-CardNames.Length + 1.0f) / 2.0f;

        foreach (string prefabName in CardNames) {
            var prefabInstance = Resources.Load(pathOfPrefabDirectory + prefabName) as GameObject;
            GameObject theObj = GameObject.Instantiate(prefabInstance, theParent.transform, false);
            theObj.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f); // ITS HUGE!
            theObj.transform.localPosition = new Vector3(theObj.transform.localPosition.x  + 0.1f * place, 0, 0);
            place = place + 1.0f;
        }
    }

    // Update is called once per frame
    void Update() {
        
    }
}
