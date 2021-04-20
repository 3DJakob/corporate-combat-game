using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedFactory :  MonoBehaviour
{
    // Start is called before the first frame update
    PhotonView PV;
    void Start()
    {
        PV = GetComponent<PhotonView>();
        GetComponent<Transform>().SetParent(GameSetup.GS.instanceOfMap.transform.Find("Spelplan 1").transform, false);
        GetComponent<Transform>().localEulerAngles = new Vector3(-90, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
