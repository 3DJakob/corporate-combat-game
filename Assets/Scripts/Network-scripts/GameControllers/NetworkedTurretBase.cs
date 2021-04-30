using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NetworkedTurretBase : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(GameSetup.GS.instanceOfMap.transform, false);
    }
}
 
