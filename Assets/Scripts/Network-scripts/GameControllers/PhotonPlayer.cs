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

    //public Button tankSpawnButton;
    //public Button startButton;
    //public Button rightButton;
    //public Button leftButton;

    public int spawnPicker;

    // Start is called before the first frame update
    private void Start()
    {
        PV = GetComponent<PhotonView>();

        //Spawn set, depending on player who owns the current instance 
        //---Selected based on teams for now-----
        spawnPicker = PlayerInfo.PI.mySelectedTeam;
        Debug.Log("Spawn is at " + spawnPicker);

        //If PV is of the current instance, instantiate a player avatar and add onClick-events to the UI-buttons
        if (PV.IsMine)
        {
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"),
                GameSetup.GS.spawnPoints[spawnPicker].position, GameSetup.GS.spawnPoints[spawnPicker].rotation, 0);
            Debug.Log("Avatar spawned at spawnpoint" + spawnPicker);
            Debug.Log("Player created");

            //startButton = GameObject.Find("StartGame").GetComponent<Button>();
            UIElements.UI.startButton.onClick.AddListener(OnStartGameButtonClicked);

        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    //When MoveRight/MoveLeft is clicked, move "myAvatar" to the right/left
    public void OnRightButtonClicked()
    {
        Debug.Log("Moves right");
        myAvatar.transform.position += new Vector3(0.2f, 0, 0);  
    }
    public void OnLeftButtonClicked()
    {
        Debug.Log("Moves left");
        myAvatar.transform.position += new Vector3(-0.2f, 0, 0);
    }

    public void OnStartGameButtonClicked()
    {
        if (PV.IsMine)
        {
            PV.RPC("RPC_ActivateGameUI", RpcTarget.AllBuffered);
        }
    }

    //If tankSpawnButton is clicked then and RPC call is sent to master client
    //who instantitates an object at a certain position
    public void OnTankSpawnButtonClicked()
    {
        Debug.Log("Spawns Tank");
        PV.RPC("RPC_SpawnTank", RpcTarget.MasterClient);
    }

    [PunRPC]
    void RPC_SpawnTank() {
        PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefabs", "PlayerAvatar"),
                new Vector3(0, 0, 5), new Quaternion(0, 0, 0, 0), 0);
    }

    [PunRPC]
    void RPC_ActivateGameUI()
    {
        UIElements.UI.tankSpawnButton.onClick.AddListener(OnTankSpawnButtonClicked);
        UIElements.UI.rightButton.onClick.AddListener(OnRightButtonClicked);
        UIElements.UI.leftButton.onClick.AddListener(OnLeftButtonClicked);
    }
}
