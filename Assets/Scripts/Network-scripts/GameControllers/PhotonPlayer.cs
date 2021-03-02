using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PhotonPlayer : MonoBehaviour
{
    private PhotonView PV;
    public GameObject myAvatar;
    public Button rightButton;
    public Button leftButton;

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
            //PV.RPC("RPC_CreatePlayer", RpcTarget.AllViaServer);
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"),
                GameSetup.GS.spawnPoints[Owner].position, GameSetup.GS.spawnPoints[Owner].rotation, 0);
            Debug.Log("Avatar spawned at spawnpoint" + Owner);

            rightButton = GameObject.Find("MoveRight").GetComponent<Button>();
            rightButton.onClick.AddListener(OnRightButtonClicked);
            leftButton = GameObject.Find("MoveLeft").GetComponent<Button>();
            leftButton.onClick.AddListener(OnLeftButtonClicked);
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

    public void OnRightButtonClicked()
    {
        
        Debug.Log("Moves right");
        myAvatar.transform.position += new Vector3(0.2f, 0, 0);
        
    }
    public void OnLeftButtonClicked()
    {
        
        Debug.Log("Moves right");
        myAvatar.transform.position += new Vector3(-0.2f, 0, 0);

    }
}
