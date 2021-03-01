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
    public FloatVariable playersSpawned;

    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log(playersSpawned.Value);
        PV = GetComponent<PhotonView>();
        Debug.Log(GameSetup.GS.spawnPoints);
        int spawnPicker = (int)(playersSpawned.Value);
        if (PV.IsMine)
        {
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"), 
                GameSetup.GS.spawnPoints[spawnPicker].position, GameSetup.GS.spawnPoints[spawnPicker].rotation, 0);
            Debug.Log("Avatar spawned");
        }
        //Debug.Log(playerSpawned);
        playersSpawned.ApplyChange(1);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
