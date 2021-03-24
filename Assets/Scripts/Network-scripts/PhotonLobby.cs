using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby lobby; //singleton = template of instances

    //public GameObject teamPicker;

    //public GameObject createButton;
    //public GameObject joinButton;
    //public GameObject cancelButton;
    //public GameObject inputField;
    //public GameObject inputFieldNick;
    //public GameObject lobbyText;
    //public GameObject errorJoinRoom;
    //public GameObject playerNameText;
    //public GameObject players;

    //private bool inLobby;
    private static readonly string[] roomCodes = { "abc", "tank", "vip", "mvp", "ggwp" };

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
        //inLobby = true;
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true; //When masterclient loads scene, everyone connected to master loads scene
        Debug.Log("Player has connected to Photon master server");

        MenuScript.menu.ToggleButtons(1); // Player is now connected to servers, enables Join/create to allow join a game 
    }

    public void OnCreateButtonClicked()
    {
        Debug.Log("Creates new lobby");
        CreateRoom();
    }

    public void OnJoinButtonClicked()
    {
        Debug.Log("Trying to join a new lobby");
        MenuScript.menu.ToggleButtons(2);
    }

    public void OnRoomCodeEntered()
    {
        string input = MenuScript.menu.inputFieldRoom.GetComponent<InputField>().text;
        Debug.Log("Searching for game with name: " + input);
        PhotonNetwork.JoinRoom(input);
    }

    public void OnNicknameEntered()
    {
        string input = MenuScript.menu.inputFieldNick.GetComponent<InputField>().text;

        if (!string.IsNullOrEmpty(input))
        {
            PhotonNetwork.NickName = input;
            Debug.Log("Your name is " + PhotonNetwork.NickName);

            MenuScript.menu.joinButton.GetComponent<Button>().interactable = true;
            MenuScript.menu.createButton.GetComponent<Button>().interactable = true;
            MenuScript.menu.playerNameText.GetComponent<Text>().text = PhotonNetwork.NickName;
            MenuScript.menu.playerNameText.SetActive(true);
            MenuScript.menu.inputFieldNick.SetActive(false);
        } 
    }


    public void OnCancelButtonClicked()
    {
        if(PhotonNetwork.InRoom)
            PhotonNetwork.LeaveRoom();
        MenuScript.menu.ToggleButtons(1);
    }

    public override void OnJoinedRoom()
    {
        MenuScript.menu.ToggleButtons(3);
        MenuScript.menu.lobbyText.GetComponent<Text>().text = "Room Code: " + PhotonNetwork.CurrentRoom.Name;
    }

    void CreateRoom()
    {
        int randomInt = Random.Range(0, 5);
        RoomOptions roomOps = new RoomOptions() { IsVisible = false, IsOpen = true, MaxPlayers = (byte)MultiplayerSetting.multiplayerSetting.maxPlayers }; //Room specifications
        PhotonNetwork.CreateRoom(roomCodes[randomInt], roomOps);
        Debug.Log("Created Room: " + roomCodes[randomInt]);
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
