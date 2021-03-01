using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{
    private PhotonView PV;
    public GameObject myAvatar;
    public int Owner;

    // Start is called before the first frame update
    private void Start()
    {
        PV = GetComponent<PhotonView>();
        Owner = PV.Owner.ActorNumber-1;

        //Spawn set, depending on player who owns the current instance
        int spawnPicker = Owner;

        //If PV is of the current instance, instiate a player avatar
        if (PV.IsMine)
        {
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"), 
                GameSetup.GS.spawnPoints[spawnPicker].position, GameSetup.GS.spawnPoints[spawnPicker].rotation, 0);
            Debug.Log("Avatar spawned");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
