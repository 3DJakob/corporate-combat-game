using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby lobby; //singleton = template of instances

    public GameObject battleButton; //match-making system sliterio, clash royale

    public int roomNumber;
    RoomInfo[] rooms;

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
        Debug.Log("Player has connected to Photon master server");
        battleButton.SetActive(true); // Player is now connected to servers, enables battlebutton to allow join a game
    }

    public void OnBattleButtonClicked()
    {
        PhotonNetwork.JoinRandomRoom(); //picks random room
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to join join a random game but failed. There must be no open games available");
        int randomRoomName = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 10 }; //Room specifications
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
    }

    void CreateRooms()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
