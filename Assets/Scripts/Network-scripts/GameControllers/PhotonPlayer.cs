using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{
    //public static int playerSpawned = 0;
    private PhotonView PV;
    public GameObject myAvatar;
    public int Owner;

    // Start is called before the first frame update
    private void Start()
    {
        
        PV = GetComponent<PhotonView>();
        Debug.Log(GameSetup.GS.spawnPoints);
        Owner = PV.Owner.ActorNumber-1;
        int spawnPicker = Owner;
        if (PV.IsMine)
        {
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"), 
                GameSetup.GS.spawnPoints[spawnPicker].position, GameSetup.GS.spawnPoints[spawnPicker].rotation, 0);
            Debug.Log("Avatar spawned");
        }
        Debug.Log(Owner);
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
