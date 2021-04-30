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
    private const int UPDATERATE = 4;

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
            

            EnergyController.EC.PP = this;

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
                //string[] selectedCards = { "FastTank", "FastTank", "FastTank", "FastTank", "FastTank" }; // TODO set from card rooster
                GameSetup.GS.player = this;
                CardController.GetComponent<CardController>().initiate(GameSetup.GS.cardPoints[PlayerInfo.PI.mySelectedTeam], PlayerInfo.PI.selectedCards);
                Debug.Log("Enabling UI");
                canvasGame.enabled = true;
                canvasAR.enabled = false;

                GameSetup.GS.ARSetup = false;

                tankSpawnButton = GameObject.Find("Spawn cube").GetComponent<Button>();
                tankSpawnButton.onClick.AddListener(OnTankSpawnButtonClicked);

                GameSetup.GS.instanceOfMap.SetActive(true);
                
                
                EnergyController.EC.energy = 200;
                EnergyController.EC.textObject.gameObject.SetActive(true);
                
                if(PhotonNetwork.IsMasterClient){
                    Debug.Log("im here?");
                    PhotonNetwork.InstantiateRoomObject(Path.Combine("GamePrefabs", "RedTarget"), new Vector3(0, 0, 0.001f), Quaternion.Euler(0, 0 ,0), 0);
                    PhotonNetwork.InstantiateRoomObject(Path.Combine("GamePrefabs", "BlueTarget"),new Vector3(0, 0, 0.001f), Quaternion.Euler(0, 0 ,0), 0);
                }
            }
            if (eventCode == ENDGAME)
            {
                Debug.Log("Event 2 is called");
                
                string data = (string)photonEvent.CustomData;
                GameObject.Find("WinSound").GetComponent<AudioSource>().Play();
                winPanel.SetActive(true);
                if (data == "Player_2")
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
                    //Debug.Log("A unit was bought!");
                    EnergyController.EC.updateEnergy(data[1]);
                }
                
            }
            if(eventCode == UPDATERATE){
                int[] data = (int[])photonEvent.CustomData; //data[0] is team, data[1] is new value

                if(data[0] == PlayerInfo.PI.mySelectedTeam)
                {
                    //UPDATE ENERGY
                    EnergyController.EC.updateRate(data[1]);
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
        SpawnTank(2.0f, 10.0f, 0.4f, 45.0f, "SlowTank", "Forest");
        SpawnTurret(2.0f, 10.0f, new Vector3(0,0,0) , 45.0f, "MediumTurret", "Platform1" );
    }

    public void SpawnTank(float fireRate, float damage, float speed, float range, string nameOfObjectToSpawn, string lane) {
        Debug.Log("spawn tank...");
        if (PV.IsMine)
        {
            //GameObject tank = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"), GameSetup.GS.spawnPoints[PlayerInfo.PI.mySelectedTeam].position, localT.rotation, 0);
            Debug.Log("Spawns Tank");
            PV.RPC("RPC_SpawnTank", RpcTarget.MasterClient, PlayerInfo.PI.mySelectedTeam, lane, fireRate, damage, speed, range, nameOfObjectToSpawn);
        }
    }
    public void SpawnEnergySource(int generationRate, float lifetime, string nameOfObjectToSpawn, Vector3 pos, string nameOfPlatform)
    {
        if (PV.IsMine)
        {
            Debug.Log("Spawns EnergySource");
            PV.RPC("RPC_SpawnEnergySource", RpcTarget.MasterClient, PlayerInfo.PI.mySelectedTeam, generationRate, lifetime, pos, nameOfObjectToSpawn, nameOfPlatform);
        }
    }
    public void SpawnTurret(float fireRate, float damage, Vector3 pos, float range, string nameOfObjectToSpawn, string nameOfPlatform){
        if (PV.IsMine)
        {
            Debug.Log("Spawns Turret");
            PV.RPC("RPC_SpawnTurret", RpcTarget.MasterClient, PlayerInfo.PI.mySelectedTeam, fireRate, damage, pos, range, nameOfObjectToSpawn, nameOfPlatform);
        }
    }

    public void UpdateEnergy(int team, int changeAmount)
    {
        if (PV.IsMine)
        {
            Debug.Log("Update Energy source");
            //PV.RPC("RPC_SpawnEnergySource", RpcTarget.MasterClient, team, generationRate, lifetime, nameOfObjectToSpawn);
        }
    }

    //RPC Function that instatiates a tank in the multiplayer room. Use RpcTarget.MasterClient when calling.
    [PunRPC]
    void RPC_SpawnTank(int team, string lane, float fireRate, float damage, float speed, float range, string nameOfObjectToSpawn)
    {
        //Debug.Log("HERE IS THE SPEED");
        //Debug.Log(nameOfObjectToSpawn);
        //Debug.Log(speed / 1000);

        GameObject Tank = PhotonNetwork.InstantiateRoomObject(Path.Combine("GamePrefabs", nameOfObjectToSpawn), GameSetup.GS.spawnPoints[team].localPosition, GameSetup.GS.spawnPoints[team].localRotation, 0);
        

        float scale = GameSetup.GS.instanceOfMap.transform.localScale.x;

        //FOV
        FOV fov = Tank.GetComponent<FOV>();
        fov.damage = damage;
        fov.fireRate = fireRate;
        fov.viewRadius = range*scale*0.01f; //scale range after host scale;
        
        Tank.GetComponent<TankHealth>().team = team;

        //TankNav
        TankNav nav = Tank.GetComponent<TankNav>();
        nav.team = team;
        nav.lineName = lane + "Line"; //Put line name here
        nav.speed = speed / 1000; // good speed!

        //Initialize TankNav
        nav.InitiateTank();
    }
    
    [PunRPC]
    void RPC_SpawnEnergySource(int team, int generationRate, float lifetime, Vector3 pos, string nameOfObjectToSpawn, string nameOfPlatform)
    {
        PlatformUsed platform = GameSetup.GS.instanceOfMap.transform.Find(team.ToString()).Find(nameOfPlatform).GetComponent<PlatformUsed>();
        if(platform.isUsed){
            Debug.Log("Spot already taken!");
            return;
        }
        PV.RPC("RPC_UpdatePlatform", RpcTarget.All, team, nameOfPlatform, true);
        
        GameObject ES = PhotonNetwork.InstantiateRoomObject(Path.Combine("GamePrefabs", nameOfObjectToSpawn), pos, new Quaternion(0,0,0,0), 0);
        
        EnergyGeneration EG = ES.GetComponent<EnergyGeneration>();
        EG.rate = generationRate;
        EG.team = team;
        
        DestoryAfterLifetime DAL = ES.GetComponent<DestoryAfterLifetime>();
        DAL.lifetime = lifetime;
        DAL.platform = platform;
        DAL.enabled = true;

    }

    public void UpdatePlatform(int team, string nameOfPlatform, bool isUsed){
        PV.RPC("RPC_UpdatePlatform", RpcTarget.All, team, nameOfPlatform, isUsed);
    }


    //RPC Function that instatiates a turret in the multiplayer room. Use RpcTarget.MasterClient when calling.
    [PunRPC]
    void RPC_SpawnTurret(int team,  float fireRate, float damage, Vector3 pos, float range, string nameOfObjectToSpawn, string nameOfPlatform)
    {
        Debug.Log("TurretSpawned");
        Debug.Log(nameOfObjectToSpawn);

        PlatformUsed platform = GameSetup.GS.instanceOfMap.transform.Find(team.ToString()).Find(nameOfPlatform).GetComponent<PlatformUsed>();
        if(platform.isUsed){
            Debug.Log("Spot already taken!");
            return;
        }
        PV.RPC("RPC_UpdatePlatform", RpcTarget.All, team, nameOfPlatform, true);

        GameObject Turret = PhotonNetwork.InstantiateRoomObject(Path.Combine("GamePrefabs", nameOfObjectToSpawn), pos, new Quaternion(0,0,0,0), 0);
        
        float scale = GameSetup.GS.instanceOfMap.transform.localScale.x;

        //FOV
        FOV fov = Turret.transform.Find("TurretTop").GetComponent<FOV>();
        fov.damage = damage;
        fov.fireRate = fireRate;
        fov.viewRadius = range*scale*0.01f; //scale range after host scale;
        Turret.GetComponent<TankHealth>().team = team;
    }

    [PunRPC]
    void RPC_UpdatePlatform(int team, string nameOfPlatform, bool isUsed){
        PlatformUsed platform = GameSetup.GS.instanceOfMap.transform.Find(team.ToString()).Find(nameOfPlatform).GetComponent<PlatformUsed>();
        platform.isUsed = isUsed;
        platform.ToggleVisablity();
    }

    [PunRPC]
    void RPC_updateEnergy(int team, int changeAmount){
        if(PlayerInfo.PI.mySelectedTeam == team){
            EnergyController.EC.updateEnergy(changeAmount);
        }
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
