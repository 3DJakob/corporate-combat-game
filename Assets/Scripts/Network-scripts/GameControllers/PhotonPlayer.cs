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
        Owner = PlayerInfo.PI.mySelectedTeam;
        Debug.Log("owner is " + Owner);

        //Spawn set, depending on player who owns the current instance
        //int spawnPicker = Owner;

        //If PV is of the current instance, instiate a player avatar
        if (PV.IsMine)
        {
            PV.RPC("RPC_CreatePlayer", RpcTarget.All);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //T = GetComponent<Transform>();
        //if (PV.IsMine)
        //{
        //    myAvatar.transform.position = new Vector3(0, 0, GameSync.GSync.syncVariable);
        //}
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    {
        myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"),
                GameSetup.GS.spawnPoints[Owner].position, GameSetup.GS.spawnPoints[Owner].rotation, 0);
        Debug.Log("Avatar spawned at spawnpoint" + Owner);
    }
}
