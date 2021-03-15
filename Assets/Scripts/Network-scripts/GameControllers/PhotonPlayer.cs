using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;


public class PhotonPlayer : MonoBehaviour, IOnEventCallback
{
    private PhotonView PV;
    public GameObject myAvatar;
    //private UIElements myUI;

    public Button tankSpawnButton;
    public Button startButton;
    public Button rightButton;
    public Button leftButton;

    public Canvas canvasGame;
    public Canvas canvasAR;

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
            //myUI = new UIElements();
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"),
                GameSetup.GS.spawnPoints[spawnPicker].position, GameSetup.GS.spawnPoints[spawnPicker].rotation, 0);
            Debug.Log("Avatar spawned at spawnpoint" + spawnPicker);
            Debug.Log("Player created");

            canvasGame = GameObject.Find("InGameUI").GetComponent<Canvas>();
            canvasAR = GameObject.Find("ARSetup").GetComponent<Canvas>();
            startButton = GameObject.Find("StartGame").GetComponent<Button>();
            startButton.onClick.AddListener(OnStartGameButtonClicked);
            canvasGame.enabled = false;
            canvasAR.enabled = true;
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
        if (PhotonNetwork.IsMasterClient)
        {
            byte eventId = 1;

            bool startGame = true;
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(eventId, startGame, raiseEventOptions, SendOptions.SendReliable);

            //PV.RPC("RPC_EnableUI", RpcTarget.All);
            //PV.RPC("RPC_ActivateGameUI", RpcTarget.All);
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
            if (eventCode == 1)
            {
                Debug.Log("Enabling UI");
                canvasGame.enabled = true;
                canvasAR.enabled = false;

                tankSpawnButton = GameObject.Find("Spawn cube").GetComponent<Button>();
                rightButton = GameObject.Find("MoveRight").GetComponent<Button>();
                leftButton = GameObject.Find("MoveLeft").GetComponent<Button>();

                tankSpawnButton.onClick.AddListener(OnTankSpawnButtonClicked);
                rightButton.onClick.AddListener(OnRightButtonClicked);
                leftButton.onClick.AddListener(OnLeftButtonClicked);

                var planeManager = GameObject.Find("AR Session Origin").GetComponent<ARPlaneManager>();
                Debug.Log(planeManager);
                foreach (var plane in planeManager.trackables)
                {
                    plane.gameObject.SetActive(false);
                }
            }
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
        Debug.Log("Activating UI");
    }

    [PunRPC]
    void RPC_EnableUI()
    {
        Debug.Log("Enabling UI");
        canvasGame.enabled = true;
        canvasAR.enabled = false;

        tankSpawnButton = GameObject.Find("Spawn cube").GetComponent<Button>();
        rightButton = GameObject.Find("MoveRight").GetComponent<Button>();
        leftButton = GameObject.Find("MoveLeft").GetComponent<Button>();

        tankSpawnButton.onClick.AddListener(OnTankSpawnButtonClicked);
        rightButton.onClick.AddListener(OnRightButtonClicked);
        leftButton.onClick.AddListener(OnLeftButtonClicked);

        var planeManager = GameObject.Find("AR Session Origin").GetComponent<ARPlaneManager>();
        Debug.Log(planeManager);
        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }


    }
}
