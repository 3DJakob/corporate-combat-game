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
    public Button loadARSetupButton;
    public GameSetup gameSetup;
    public Canvas gameCanvas;
    public Canvas menuCanvas;

    //Room info
    public static PhotonRoom room;
    private PhotonView PV;
    public bool readyToStart = false;

    //public bool isGameLoaded;
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

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("We are now in a room");

        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom++;
        
        //StartGame()
    }

    //When Scene is ready create local player
    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;

        if (currentScene == MultiplayerSetting.multiplayerSetting.gameScene)
        {
            //UIElements.UI.startButton.onClick.AddListener(OnStartGameButtonClicked);
            menuCanvas.enabled = false;
            Debug.Log("GameScene loaded");
            CreatePlayer();
        }
        else if (currentScene == MultiplayerSetting.multiplayerSetting.gameScene)
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

        LoadARSetup();
        PhotonNetwork.CurrentRoom.IsVisible = false;
    }

    public void OnStartGameButtonClicked()
    {
        Debug.Log("Start game");
        StartGame();

    }

    public void OnQuitButtonClicked()
    {
        Debug.Log("Quits game");
        Application.Quit();
    }


    //If Current player is Master, Load game scene
    void StartGame() 
    {
        
        //setActive UI

    }

    void LoadARSetup()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        PhotonNetwork.LoadLevel(MultiplayerSetting.multiplayerSetting.gameScene);
    }

    //Temporary function, puts us in ARsetup scene
    public void LoadARSetupDebug()
    {
        PhotonNetwork.LoadLevel(MultiplayerSetting.multiplayerSetting.gameScene);
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
            //loadARSetupButton.gameObject.SetActive(false);
        }
        Debug.Log(otherPlayer.ActorNumber + " has left the game");
        playersInRoom--;
    }

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(SceneManager.activeSceneChanged)
        //if (SceneManager.GetActiveScene().buildIndex == MultiplayerSetting.multiplayerSetting.menuScene)
        //    menuCanvas.enabled = true;
        //else if (SceneManager.GetActiveScene().buildIndex == MultiplayerSetting.multiplayerSetting.gameScene)
        //    menuCanvas.enabled = false;
    }
}
