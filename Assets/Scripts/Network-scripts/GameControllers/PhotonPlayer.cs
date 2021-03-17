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
            //myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"),
            //    GameSetup.GS.spawnPoints[spawnPicker].position, GameSetup.GS.spawnPoints[spawnPicker].rotation, 0);
            //Debug.Log("Avatar spawned at spawnpoint" + spawnPicker);
            Debug.Log("Player created");

            canvasGame = GameObject.Find("InGameUI").GetComponent<Canvas>();
            canvasAR = GameObject.Find("ARSetup").GetComponent<Canvas>();
            startButton = GameObject.Find("StartGame").GetComponent<Button>();
            startButton.onClick.AddListener(OnStartGameButtonClicked);
            canvasGame.enabled = false;
            canvasAR.enabled = true;

            if (!PhotonNetwork.IsMasterClient)
                startButton.gameObject.SetActive(false);
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
                
      
                GameSetup.GS.instanceOfMap.SetActive(true);

                var planeManager = GameObject.Find("AR Session Origin").GetComponent<ARPlaneManager>();
                Debug.Log(planeManager);
                foreach (var plane in planeManager.trackables)
                {
                    plane.gameObject.SetActive(false);
                }
            }
        }
    }

    //If tankSpawnButton is clicked then and RPC call is sent to all clients
    //who instantitates an object at a certain position depending on which team a player belongs to.
    //All clients have their own instance of the spawned tank
    public void OnTankSpawnButtonClicked()
    {

        //GameSetup.GS.spawnPoints[0] = GameObject.Find("SpawnPoint t1").GetComponent<Transform>();
        //GameSetup.GS.spawnPoints[1] = GameObject.Find("SpawnPoint t2").GetComponent<Transform>();
        Transform localT = PlayerInfo.PI.T;
        
        if (PV.IsMine)
        {
            //GameObject tank = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"), GameSetup.GS.spawnPoints[PlayerInfo.PI.mySelectedTeam].position, localT.rotation, 0);
            Debug.Log("Spawns Tank");
            PV.RPC("RPC_SpawnTank", RpcTarget.All, PlayerInfo.PI.mySelectedTeam);
        }
        
    }

    [PunRPC]
    void RPC_SpawnTank(int team)
    {
        Transform localT = PlayerInfo.PI.T;

        GameObject tank = (GameObject)Instantiate(GameSetup.GS.tankToSpawn, GameSetup.GS.spawnPoints[team].position, localT.rotation);
        tank.transform.parent = localT;

        //GameObject tank = PhotonNetwork.Instantiate(Path.Combine("GamePrefabs", "Tank"), GameSetup.GS.spawnPoints[team].position, localT.rotation, 0);
        

        //if (team == 0)
        //    tank.GetComponent<NavTank>().GetComponent<NavMeshAgent>().SetDestination(localT.Find("Spelplan 1").Find("Factory 1").position);
        //else
        //    tank.GetComponent<NavTank>().GetComponent<NavMeshAgent>().SetDestination(localT.Find("Spelplan 1").Find("Factory 2").position);

    }

    //[PunRPC]
    //void RPC_LocalizeTank(GameObject gObject)
    //{
    //    Transform temp = PlayerInfo.PI.T;

    //    GameObject myTank = GameObject.Find("PlayerAvatar(Clone)");
    //    Debug.Log(myTank);

    //    myTank.transform.parent = PlayerInfo.PI.T;
    //}

}
