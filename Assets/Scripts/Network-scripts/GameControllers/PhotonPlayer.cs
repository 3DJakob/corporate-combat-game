using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{
    public static int playerSpawned = 0;
    private PhotonView PV;
    public GameObject myAvatar;

    // Start is called before the first frame update
    private void Start()
    {
        PV = GetComponent<PhotonView>();
        Debug.Log(GameSetup.GS.spawnPoints);
        int spawnPicker = Random.Range(0, GameSetup.GS.spawnPoints.Length);
        if (PV.IsMine)
        {
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"), 
                GameSetup.GS.spawnPoints[spawnPicker].position, GameSetup.GS.spawnPoints[spawnPicker].rotation, 0);
            Debug.Log("Avatar spawned");
        }
        playerSpawned++;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
