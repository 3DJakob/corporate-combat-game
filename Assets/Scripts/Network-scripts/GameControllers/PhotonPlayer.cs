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

    public Button tankSpawnButton;
    public Button rightButton;
    public Button leftButton;

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

            rightButton = GameObject.Find("Spawn cube").GetComponent<Button>();
            rightButton = GameObject.Find("MoveRight").GetComponent<Button>();
            rightButton.onClick.AddListener(OnRightButtonClicked);
            leftButton = GameObject.Find("MoveLeft").GetComponent<Button>();
            leftButton.onClick.AddListener(OnLeftButtonClicked);
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

    public void OnTankSpawnButtonClicked()
    {
        Debug.Log("Spawns Tank");
        PV.RPC("RPC_SpawnTank", RpcTarget.MasterClient);
    }

    [PunRPC]
    void RPC_SpawnTank() {
        PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefabs", "PlayerAvatar"),
                new Vector3(0, 0, -5), new Quaternion(0, 0, 0, 0), 0);
    }
}
