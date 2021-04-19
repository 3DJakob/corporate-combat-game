using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEnergySource : MonoBehaviour
{
    public string nameOfObjectToSpawn;

    public int cost = 100;
    //private PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        //PV = GetComponent<PhotonView>();
    }

    public void Spawn()
    {

        Debug.Log("trying to get money...");
        // Debug.Log(GameObject.Find("EnergyController").GetComponent<EnergyController>());
        // GameObject EnergyDoHicky = GameObject.Find("EnergyController");
        // Debug.Log(EnergyDoHicky);
        // if (GameObject.Find("EnergyController").GetComponent<EnergyController>()) {
        //     Debug.Log("Found th econtroller");
        // } else {
        //                 Debug.Log("CANT find th econtroller");
        // }

        if (GameObject.Find("EnergyController").GetComponent<EnergyController>().Buy(cost))
        {
            GameObject.Find("PhotonNetworkPlayer(Clone)").GetComponent<PhotonPlayer>().SpawnEnergySource(PlayerInfo.PI.mySelectedTeam, nameOfObjectToSpawn);
        
        }
        else
        {
            Debug.Log("Not enough energy..");
        }

    }
}
