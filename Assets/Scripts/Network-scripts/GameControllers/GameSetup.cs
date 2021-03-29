using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class GameSetup : MonoBehaviour
{
    public static GameSetup GS;
    public Transform[] spawnPoints;
    public Transform[] cardPoints;
    public PhotonPlayer player;
    public GameObject gameMap;
    public GameObject tankToSpawn;

    public bool ARSetup = true;

    public GameObject instanceOfMap;

    //Create GameSetup OnEnable (When switching to the game scene)
    private void OnEnable()
    {
        if (GameSetup.GS == null)
        {
            GameSetup.GS = this;
        }
    }
    //If a player is disconnected load menu-scene (could be in UIElements)
    public void DisconnectPlayer()
    {
        StartCoroutine(DisconnectAndLoad());
    }

    IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
            yield return null;

        SceneManager.LoadScene(MultiplayerSetting.multiplayerSetting.menuScene);
    }

    public void OnQuitButtonClicked()
    {
        Debug.Log("Quits game");
        Application.Quit();
    }

    public void PositionMap(Transform T) {
        if (instanceOfMap != null)
        {
            //Calculate offset and Scale for gameMap
            float scale = T.localScale.x;
            instanceOfMap.SetActive(true);
            instanceOfMap.transform.localPosition = new Vector3(T.position.x, T.position.y, T.position.z);
            instanceOfMap.transform.localEulerAngles = new Vector3(0,T.eulerAngles.y, 0);
            instanceOfMap.transform.localScale = new Vector3(scale, scale, scale);
            //Debug.Log(GameObject.Find("SpawnPoint t1").GetComponent<Transform>());
        }
    }


    private void Start()
    {
        instanceOfMap = Instantiate(gameMap);
        spawnPoints = new Transform[2];

        Transform temp = instanceOfMap.transform;
        GameSetup.GS.spawnPoints[0] = temp.Find("SpawnPoint t1");
        GameSetup.GS.spawnPoints[1] = temp.Find("SpawnPoint t2");

        GameSetup.GS.cardPoints[0] = temp.Find("CardPoint t1");
        GameSetup.GS.cardPoints[1] = temp.Find("CardPoint t2");
        

    }

    private void Update()
    {
        /*
        if (PlayerInfo.PI.T != null && instanceOfMap != null)
        {
            //Calculate offset and Scale for gameMap
            float scale = PlayerInfo.PI.T.localScale.x;
            instanceOfMap.SetActive(true);
            instanceOfMap.transform.position = new Vector3(PlayerInfo.PI.T.position.x, PlayerInfo.PI.T.position.y, PlayerInfo.PI.T.position.z);
            instanceOfMap.transform.eulerAngles = PlayerInfo.PI.T.eulerAngles;
            instanceOfMap.transform.localScale = new Vector3(scale, scale, scale);
            //Debug.Log(GameObject.Find("SpawnPoint t1").GetComponent<Transform>());
        }
        */
    }

}
//spawnPoints[0] = GameObject.Find("SpawnPoint t1").GetComponent<Transform>();
//spawnPoints[1] = GameObject.Find("SpawnPoint t2").GetComponent<Transform>();