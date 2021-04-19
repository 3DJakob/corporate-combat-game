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
    public GameObject winPanel;
    public GameObject winText;

    public Canvas canvasGame;
    public Canvas canvasAR;
    public GameObject CardController;

    public int spawnPicker;
    public static int playersReady;

    //---Event Codes ----
    private const int STARTGAME = 1;
    private const int ENDGAME = 2;
    private const int UPDATEENERGY = 3;

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
            winPanel = GameObject.Find("WinState");
            winText = GameObject.Find("Winner");

            startButton.onClick.AddListener(OnStartGameButtonClicked);
            readyButton.onClick.AddListener(OnReadyButtonClicked);
            startButton.gameObject.SetActive(false);
            winPanel.SetActive(false);

            canvasGame.enabled = false;
            canvasAR.enabled = true;
            Debug.Log("winPanel " + winPanel);
            Debug.Log("winText " + winText);
        }
    }

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
            
            if (eventCode == STARTGAME)
            {
                string[] selectedCards = { "FastTank", "FastTank", "FastTank", "FastTank", "FastTank" }; // TODO set from card rooster
                CardController.GetComponent<CardController>().initiate(GameSetup.GS.cardPoints[spawnPicker], selectedCards);

                Debug.Log("Enabling UI");
                canvasGame.enabled = true;
                canvasAR.enabled = false;

                GameSetup.GS.ARSetup = false;

                tankSpawnButton = GameObject.Find("Spawn cube").GetComponent<Button>();
                tankSpawnButton.onClick.AddListener(OnTankSpawnButtonClicked);

                GameSetup.GS.instanceOfMap.SetActive(true);

            }
            if (eventCode == ENDGAME)
            {
                Debug.Log("Event 2 is called");

                string data = (string)photonEvent.CustomData;

                winPanel.SetActive(true);
                if (data == "Player_1")
                    winText.GetComponent<Text>().text += "\n\n RED TEAM";
                else
                    winText.GetComponent<Text>().text += "\n\n BLUE TEAM";

            }
            if(eventCode == UPDATEENERGY)
            {
                Debug.Log("Event 3 is called");
                int[] data = (int[])photonEvent.CustomData; //data[0] is team, data[1] is the new energy

                if(data[0] == PlayerInfo.PI.mySelectedTeam)
                {
                    //UPDATE ENERGY
                    Debug.Log("A unit was bought!");
                    GameObject.Find("EnergyController").GetComponent<EnergyController>().updateEnergy(data[1]);
                }
                
            }
        }
    }

    //If tankSpawnButton is clicked then and RPC call is sent to all clients
    //who instantitates an object at a certain position depending on which team a player belongs to.
    //All clients have their own instance of the spawned tank
    public void OnTankSpawnButtonClicked()
    {
        Debug.Log("button clicked...");
        SpawnTank(2.0f, 10.0f, 0.3f, 45.0f, "Tank");
    }

    public void SpawnTank(float fireRate, float damage, float speed, float range, string nameOfObjectToSpawn) {
        Debug.Log("spawn tank...");
        if (PV.IsMine)
        {
            //GameObject tank = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"), GameSetup.GS.spawnPoints[PlayerInfo.PI.mySelectedTeam].position, localT.rotation, 0);
            Debug.Log("Spawns Tank");
            PV.RPC("RPC_SpawnTank", RpcTarget.MasterClient, PlayerInfo.PI.mySelectedTeam, "Highway", fireRate, damage, speed, range, nameOfObjectToSpawn);
        }
    }

    public void SpawnEnergySource(int team, string nameOfObjectToSpawn)
    {
        if (PV.IsMine)
        {
            Debug.Log("Spawns EnergySource");
            PV.RPC("RPC_SpawnEnergySource", RpcTarget.MasterClient, team, nameOfObjectToSpawn);
        }
    }

    //RPC Function that instatiates a tank in the multiplayer room. Use RpcTarget.MasterClient when calling.
    [PunRPC]
    void RPC_SpawnTank(int team, string lane, float fireRate, float damage, float speed, float range, string nameOfObjectToSpawn)
    {
        Debug.Log("HERE IS THE SPEED");
        Debug.Log(nameOfObjectToSpawn);
        Debug.Log(speed / 1000);

        GameObject Tank = PhotonNetwork.InstantiateRoomObject(Path.Combine("GamePrefabs", nameOfObjectToSpawn), GameSetup.GS.spawnPoints[team].localPosition, GameSetup.GS.spawnPoints[team].localRotation, 0);
        TankNav nav = Tank.GetComponent<TankNav>();

        float scale = GameSetup.GS.instanceOfMap.transform.localScale.x;

        //FOV
        FOV fov = Tank.GetComponent<FOV>();
        fov.damage = damage;
        fov.fireRate = fireRate;
        fov.viewRadius = range*scale*0.01f; //scale range after host scale;
        
        //TankNav
        nav.team = team;
        nav.lineName = "HighwayLine"; //Put line name here
        nav.speed = speed / 1000; // good speed!

        //Initialize TankNav
        nav.InitiateTank();
    }
    
    [PunRPC]
    void RPC_SpawnEnergySource(int team, string nameOfObjectToSpawn)
    {
        Transform temp = GameSetup.GS.windPointsT1[1].transform;
        GameObject ES = PhotonNetwork.InstantiateRoomObject(Path.Combine("GamePrefabs", nameOfObjectToSpawn), temp.localPosition, temp.localRotation, 0);
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
