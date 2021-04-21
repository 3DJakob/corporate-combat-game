using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedFactory :  MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    public int team;
    PhotonView PV;
    void Start()
    {
        string factory = "RED FACTORY";
        if(team == 1) factory = "BLUE FACTORY";

        PV = GetComponent<PhotonView>();
        GetComponent<Transform>().SetParent(GameSetup.GS.instanceOfMap.transform.Find("Spelplan 1").Find(factory).transform, false);
        GetComponent<Transform>().localEulerAngles = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
