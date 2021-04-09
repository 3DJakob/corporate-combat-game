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
        GetComponent<Transform>().localEulerAngles = new Vector3(0, 0, 0);
        //this.GetComponent<NavTank>().SetDestination();
    }
    // Update is called once per frame
    void Update()
    {

    }
}