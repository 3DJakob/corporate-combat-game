using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public GameObject startButton;

    //Room info
    public static PhotonRoom room;
    private PhotonView PV;
    public bool readyToStart = false;

    //public bool isGameLoaded;
    public int currentScene;
    public int multiplayerScene = 1;

    Player info;
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

        
        //StartGame()
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode) 
    {
        currentScene = scene.buildIndex;
        if (currentScene == 1) 
        {
            CreatePlayer();
        }
    }

    private void CreatePlayer()
    {
        //Creates player network controller but not player character
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0);
    }

    public void OnStartGameButtonClicked()
    {
        Debug.Log("This should start the game, but it does not yet. Call 911-420-1337 for more info");
        StartGame();
    }

    void StartGame() 
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        PhotonNetwork.LoadLevel(multiplayerScene);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("Someone joined!");
        if (PhotonNetwork.PlayerList.Length >= 2 && PhotonNetwork.IsMasterClient)
        {
            Debug.Log("I made It!!!!!");
            startButton.SetActive(true);
        }
    }
// Start is called before the first frame update
void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
