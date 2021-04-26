using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class Card : MonoBehaviour
{

    //TankProperties
    public float fireRate = 1.0f;
    public float damage = 1.0f;
    public float speed = 1.0f;
    public float range = 1.0f;
    public float hp;

    //EnergyProperties
    public int generationRate = 1;
    public float lifetime = 10f;

    //General properties
    public int cost = 50;
    public string nameOfObjectToSpawn;
    public string type;

    public void Spawn(string lane)
    {
        Debug.Log("trying to get money...");

        if (GameObject.Find("EnergyController").GetComponent<EnergyController>().Buy(cost))
        {

            GameObject.Find("PhotonNetworkPlayer(Clone)").GetComponent<PhotonPlayer>().SpawnTank(fireRate, damage, speed, range, nameOfObjectToSpawn, lane);
        }
    }
    public void Spawn(Transform pos, string nameOfPlatform)
    {
        Debug.Log("trying to get money...");
        if (GameObject.Find("EnergyController").GetComponent<EnergyController>().Buy(cost))
        {
            if (type == "EnergySource")
            {
                GameObject.Find("PhotonNetworkPlayer(Clone)").GetComponent<PhotonPlayer>().SpawnEnergySource(generationRate, lifetime, nameOfObjectToSpawn, pos.localPosition, nameOfPlatform);
            }
            else if (type == "Turret")
            {
                GameObject.Find("PhotonNetworkPlayer(Clone)").GetComponent<PhotonPlayer>().SpawnTurret(fireRate, damage, pos.localPosition, range, nameOfObjectToSpawn, nameOfPlatform);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("getting photonview...");

        //Debug.Log(PV);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
