using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby lobby; //singleton = template of instances

    public GameObject teamPicker;

    public GameObject createButton;
    public GameObject joinButton;
    public GameObject cancelButton;
    public GameObject inputField;
    public GameObject inputFieldNick;
    public GameObject lobbyText;
    public GameObject errorJoinRoom;
    public GameObject playerNameText;
    public GameObject players;

    private bool inLobby;

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
        inLobby = true;
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true; //When masterclient loads scene, everyone connected to master loads scene
        Debug.Log("Player has connected to Photon master server");

        joinButton.SetActive(true); // Player is now connected to servers, enables Join/create to allow join a game
        createButton.SetActive(true);
    }

    public void ToggleButtons()
    {
        //Debug.Log("ToggleButtons is " + inLobby);
        createButton.SetActive(!inLobby);
        joinButton.SetActive(!inLobby);
        playerNameText.SetActive(!inLobby);
        teamPicker.SetActive(inLobby);
        cancelButton.SetActive(inLobby);

        inLobby = !inLobby; //Toggles bool, works like a light switch
    }

    public void OnCreateButtonClicked()
    {
        Debug.Log("Creates new lobby");

        CreateRoom();
    }

    public void OnJoinButtonClicked()
    {
        Debug.Log("Trying to join a new lobby");
        joinButton.SetActive(false); 
        createButton.SetActive(false);
        inputField.SetActive(true);
        //submitButton.SetActive(true);
    }

    public void OnRoomCodeEntered()
    {
        string input = inputField.GetComponent<InputField>().text;
        Debug.Log("Searching for game with name: Room" + input);
        PhotonNetwork.JoinRoom("Room" + input);
    }

    public void OnNicknameEntered()
    {
        string input = inputFieldNick.GetComponent<InputField>().text;

        if (!string.IsNullOrEmpty(input))
        {
            PhotonNetwork.NickName = input;
            Debug.Log("Your name is " + PhotonNetwork.NickName);

            joinButton.GetComponent<Button>().interactable = true;
            createButton.GetComponent<Button>().interactable = true;
            playerNameText.GetComponent<Text>().text = PhotonNetwork.NickName;
            playerNameText.SetActive(true);
            inputFieldNick.SetActive(false);
        } 
    }


    public void OnCancelButtonClicked()
    {
        if(PhotonNetwork.InRoom)
            PhotonNetwork.LeaveRoom();
        inputField.SetActive(false);
        lobbyText.SetActive(false);
        ToggleButtons(); 
    }

    public override void OnJoinedRoom()
    {
        inputField.SetActive(false); 
        ToggleButtons();
        lobbyText.GetComponent<Text>().text = "You are in " + PhotonNetwork.CurrentRoom.Name;
        //PhotonNetwork.MasterClient.NickName
    }

    void CreateRoom()
    {
        Debug.Log("Trying to create a new room");
        int randomRoomName = Random.Range(0, 10);
        RoomOptions roomOps = new RoomOptions() { IsVisible = false, IsOpen = true, MaxPlayers = (byte)MultiplayerSetting.multiplayerSetting.maxPlayers }; //Room specifications
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
        Debug.Log("Created Room" + randomRoomName);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create a new room, there must already be room with the same name");
        CreateRoom();
    }  
    
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
