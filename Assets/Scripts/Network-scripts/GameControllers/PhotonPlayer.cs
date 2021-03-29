using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;


public class PhotonPlayer : MonoBehaviour, IOnEventCallback
{
    private PhotonView PV;
    public GameObject myAvatar;

    public Button startButton;
    public Button readyButton;
    public Button tankSpawnButton;
    
    //public Button rightButton;
    //public Button leftButton;

    public Canvas canvasGame;
    public Canvas canvasAR;
    public GameObject CardController;

    public int spawnPicker;
    public static int playersReady;

    // public UnityEvent onGameStart;

    // Start is called before the first frame update
    private void Start()
    {
        PV = GetComponent<PhotonView>();

        playersReady = 0;

        //Spawn set, depending on player who owns the current instance 
        //---Selected based on teams for now-----
        spawnPicker = PlayerInfo.PI.mySelectedTeam;
        Debug.Log("Spawn is at " + spawnPicker);
        CardController = GameObject.Find("CardController");
        // Debug.Log(CardController);

        //If PV is of the current instance, instantiate a player avatar and add onClick-events to the UI-buttons
        if (PV.IsMine)
        {
            //myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"),
            //    GameSetup.GS.spawnPoints[spawnPicker].position, GameSetup.GS.spawnPoints[spawnPicker].rotation, 0);
            Debug.Log("Player created");

            canvasGame = GameObject.Find("InGameUI").GetComponent<Canvas>();
            canvasAR = GameObject.Find("ARSetup").GetComponent<Canvas>();
            startButton = GameObject.Find("StartGame").GetComponent<Button>();
            readyButton = GameObject.Find("Ready").GetComponent<Button>();

            startButton.onClick.AddListener(OnStartGameButtonClicked);
            readyButton.onClick.AddListener(OnReadyButtonClicked);
            startButton.gameObject.SetActive(false);

            canvasGame.enabled = false;
            canvasAR.enabled = true;

            //if (!PhotonNetwork.IsMasterClient)
            //    startButton.gameObject.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
       
    }
    //When MoveRight/MoveLeft is clicked, move "myAvatar" to the right/left
    //public void OnRightButtonClicked()
    //{
        
    //    Debug.Log("Moves right");
    //    myAvatar.transform.position += new Vector3(0.2f, 0, 0);
        
    //}
    //public void OnLeftButtonClicked()
    //{
    //    Debug.Log("Moves left");
    //    myAvatar.transform.position += new Vector3(-0.2f, 0, 0);
    //}
    public void OnReadyButtonClicked()
    {
        GameSetup.GS.ARSetup = false;
        if (PV.IsMine)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                startButton.gameObject.SetActive(true);
                startButton.interactable = false;
            }

            PV.RPC("RPC_UpdateReady", RpcTarget.MasterClient);
        }     
    }

    public void OnStartGameButtonClicked()
    {
        
        if (PhotonNetwork.IsMasterClient)
        {
            byte eventId = 1;
            bool startGame = true;
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(eventId, startGame, raiseEventOptions, SendOptions.SendReliable);
        }
    }
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        if (PV != null && PV.IsMine) { 
            byte eventCode = photonEvent.Code;
            
            //Start Game Event
            if (eventCode == 1)
            {
                string[] selectedCards = { "FastTank", "FastTank", "FastTank", "FastTank", "FastTank" }; // TODO set from card rooster
                CardController.GetComponent<CardController>().initiate(GameSetup.GS.cardPoints[spawnPicker], selectedCards);

                Debug.Log("Enabling UI");
                canvasGame.enabled = true;
                canvasAR.enabled = false;

                GameSetup.GS.ARSetup = false;

                tankSpawnButton = GameObject.Find("Spawn cube").GetComponent<Button>();
                tankSpawnButton.onClick.AddListener(OnTankSpawnButtonClicked);

                //rightButton = GameObject.Find("MoveRight").GetComponent<Button>();
                //leftButton = GameObject.Find("MoveLeft").GetComponent<Button>();
                //rightButton.onClick.AddListener(OnRightButtonClicked);
                //leftButton.onClick.AddListener(OnLeftButtonClicked);

                GameSetup.GS.instanceOfMap.SetActive(true);
                GameSetup.GS.instanceOfMap.transform.Find("Spelplan 1").GetComponent<NavMeshBaker>().Bake();

            }
        }
    }

    //If tankSpawnButton is clicked then and RPC call is sent to all clients
    //who instantitates an object at a certain position depending on which team a player belongs to.
    //All clients have their own instance of the spawned tank
    public void OnTankSpawnButtonClicked()
    {
        if (PV.IsMine)
        {
            //GameObject tank = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"), GameSetup.GS.spawnPoints[PlayerInfo.PI.mySelectedTeam].position, localT.rotation, 0);
            Debug.Log("Spawns Tank");
            PV.RPC("RPC_SpawnTank", RpcTarget.MasterClient, PlayerInfo.PI.mySelectedTeam);
        }
        
    }

    [PunRPC]
    void RPC_SpawnTank(int team)
    {
        GameObject Tank = PhotonNetwork.InstantiateRoomObject(Path.Combine("GamePrefabs", "Tank"), GameSetup.GS.spawnPoints[team].localPosition, new Quaternion(0,0,0,0), 0);
        NavTank tankAgent = Tank.GetComponent<NavTank>();

        tankAgent.team = team;
        //tankAgent.WarpToPosition(GameSetup.GS.spawnPoints[team].localPosition);
        tankAgent.SetDestination();

        Tank = null;
        tankAgent = null;

    }

    [PunRPC]
    void RPC_UpdateReady()
    {
        playersReady++;
        Debug.Log(PhotonNetwork.IsMasterClient + "Players ready = " + playersReady);
        if (playersReady == PhotonNetwork.PlayerList.Length)
        {
            startButton = GameObject.Find("StartGame").GetComponent<Button>();
            startButton.interactable = true;
        }
            
    }
 

}
