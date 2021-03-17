using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    //UI elements
    public Button loadARSetupButton;
    public GameSetup gameSetup;
    public Canvas gameCanvas;
    public Canvas menuCanvas;

    //Room info
    public static PhotonRoom room;
    private PhotonView PV;

    //public bool readyToStart = false;
    bool gameDebug = false;

    public int currentScene;

    //Player info
    Player[] photonPlayers;
    public int playersInRoom;
    public int myNumberInRoom;

    private void Awake()
    {
        if (PhotonRoom.room == null)
        {
            PhotonRoom.room = this;
        }
        else
        {
            if (PhotonRoom.room != this)
            {
                Destroy(PhotonRoom.room.gameObject);
                PhotonRoom.room = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(menuCanvas);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
        
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;

    }

    void Start()
    {
        PV = GetComponent<PhotonView>();

    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("We are now in " + PhotonNetwork.CurrentRoom.Name);

        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom++;

        if (gameDebug == true)
            PhotonNetwork.LoadLevel(MultiplayerSetting.multiplayerSetting.gameScene);
    }

    //When Scene is ready create local player
    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;

        if (currentScene == MultiplayerSetting.multiplayerSetting.gameScene)
        {
            menuCanvas.enabled = false;
            Debug.Log("GameScene loaded");
            CreatePlayer();
        }
        else if (currentScene == MultiplayerSetting.multiplayerSetting.menuScene)
        {
            menuCanvas.enabled = true;
            Debug.Log("MenuScene loaded");
        }


    }
    
    //Creates player network controller but not player character
    private void CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer")
            , transform.position, Quaternion.identity, 0);
        Debug.Log("NetworkPlayer created");
    }
    public void OnARSetupButtonClicked()
    {
        Debug.Log("AR setup");
        loadARSetupButton.gameObject.SetActive(false);

        if (!PhotonNetwork.IsMasterClient)
            return;
        PhotonNetwork.LoadLevel(MultiplayerSetting.multiplayerSetting.gameScene);

        PhotonNetwork.CurrentRoom.IsVisible = false;
    }

    public void OnQuitButtonClicked()
    {
        Debug.Log("Quits game");
        Application.Quit();
    }
    

    //If two players or more are in the room, set Startbutton as active
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log(newPlayer.ActorNumber + " has joined the game");
        playersInRoom++;

        if (PhotonNetwork.PlayerList.Length >= 2 && PhotonNetwork.IsMasterClient)
        {  
            loadARSetupButton.gameObject.SetActive(true);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        if (PhotonNetwork.PlayerList.Length < 2 && PhotonNetwork.IsMasterClient && currentScene == 0)
        {
            loadARSetupButton.gameObject.SetActive(false);
        }
        Debug.Log(otherPlayer.ActorNumber + " has left the game");
        playersInRoom--;
    }

    //------Debug starts here ----
    //Temporary function, puts us in ARsetup scene
    public void LoadARSetupDebug()
    {
        PhotonNetwork.LoadLevel(MultiplayerSetting.multiplayerSetting.gameScene);
    }
    //Temporary function, puts us in GameScene (no network)
    public void LoadGameDebug()
    {
        gameDebug = true;
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to join a random game but failed. There must be no open games available");
        int randomRoomName = Random.Range(10, 13);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiplayerSetting.multiplayerSetting.maxPlayers }; //Room specifications
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
    }

    //-----Debug ends here -----

    

    // Update is called once per frame
    void Update()
    {
       
    }
}
