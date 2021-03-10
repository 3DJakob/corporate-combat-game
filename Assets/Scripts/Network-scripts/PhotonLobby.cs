using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby lobby; //singleton = template of instances

    public GameObject teamPicker;
    public GameObject battleButton; //match-making system sliterio, clash royale
    public GameObject cancelButton;

    //public int roomNumber;
    //RoomInfo[] rooms;

    private void Awake()
    {
        lobby = this; //Create the singleton, lives within the Main menu scene 
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); //Connects to Master photon server
        //tries to setup a connecting and gives feedback

    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true; //When masterclient loads scene, everyone connected to master loads scene
        Debug.Log("Player has connected to Photon master server");
        battleButton.SetActive(true); // Player is now connected to servers, enables Join/create to allow join a game
    }

    public void OnBattleButtonClicked()
    {
        Debug.Log("Battle button was clicked");
        battleButton.SetActive(false);
        teamPicker.SetActive(true);
        cancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom(); //picks random room
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to join a random game but failed. There must be no open games available");
        CreateRoom();
    }

    void CreateRoom()
    {
        Debug.Log("Trying to create a new room");
        int randomRoomName = Random.Range(0, 10);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiplayerSetting.multiplayerSetting.maxPlayers }; //Room specifications
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
        Debug.Log("Created room" + randomRoomName);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create a new room, there must already be room with the same name");
        CreateRoom();
    }

    public void OnCancelButtonClicked()
    {
        cancelButton.SetActive(false);
        teamPicker.SetActive(false);
        battleButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
        //PhotonNetwork.Disconnect(); //Vet ej om denna behövs
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
