using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class Card : MonoBehaviour {

    public float fireRate = 1.0f;
    public float damage = 1.0f;
    public float speed = 1.0f;
    public float range = 1.0f;

    public int cost = 50;
    private PhotonView PV;

    // public GameObject ObjectToSpawn;
    public string nameOfObjectToSpawn;

    public void Spawn () {

        Debug.Log("trying to get money...");
        // Debug.Log(GameObject.Find("EnergyController").GetComponent<EnergyController>());
        // GameObject EnergyDoHicky = GameObject.Find("EnergyController");
        // Debug.Log(EnergyDoHicky);
        // if (GameObject.Find("EnergyController").GetComponent<EnergyController>()) {
        //     Debug.Log("Found th econtroller");
        // } else {
        //                 Debug.Log("CANT find th econtroller");
        // }

        if (GameObject.Find("EnergyController").GetComponent<EnergyController>().Buy(cost)) {
            GameObject.Find("PhotonNetworkPlayer(Clone)").GetComponent<PhotonPlayer>().SpawnTank(fireRate, damage, speed, range, nameOfObjectToSpawn);
        } else {
            Debug.Log("sry...");
        }

    }

    // Start is called before the first frame update
    void Start() {
        //Debug.Log("getting photonview...");
        PV = GetComponent<PhotonView>();
        //Debug.Log(PV);
    }

    // Update is called once per frame
    void Update() {
        
    }
}
