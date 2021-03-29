using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedUnit : MonoBehaviour
{
    PhotonView PV;
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();

        GetComponent<Transform>().SetParent(GameSetup.GS.instanceOfMap.transform, false);

        //this.GetComponent<NavTank>().SetDestination();
    }
    // Update is called once per frame
    void Update()
    {

    }
}