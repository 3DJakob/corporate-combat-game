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

    public void Spawn (string lane) {
        Debug.Log("trying to get money...");
        if (GameObject.Find("EnergyController").GetComponent<EnergyController>().Buy(cost)) {
            GameObject.Find("PhotonNetworkPlayer(Clone)").GetComponent<PhotonPlayer>().SpawnTank(fireRate, damage, speed, range, nameOfObjectToSpawn, lane);
        }
    }

    // Start is called before the first frame update
    void Start() {
        Debug.Log("getting photonview...");
        PV = GetComponent<PhotonView>();
        Debug.Log(PV);
    }

    // Update is called once per frame
    void Update() {
        
    }
}
