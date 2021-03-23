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
        PV.transform.SetParent(GameSetup.GS.instanceOfMap.transform, true);

    }
    // Update is called once per frame
    void Update()
    {

    }
}