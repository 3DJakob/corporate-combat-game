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
    public PhotonPlayer player;
    public GameObject gameMap;
    public GameObject instanceOfMap;
    public ARSessionOrigin ARSO;

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

    private void Start()
    {
        instanceOfMap = Instantiate(gameMap);
        instanceOfMap.SetActive(false);
        Debug.Log("Game map is" + instanceOfMap != null);
        //adjustedScene.transform.position = PlayerInfo.PI.positionOfTable;
        //adjustedScene.transform.eulerAngles = PlayerInfo.PI.rotationOfTable;
        //adjustedScene.transform.localScale = PlayerInfo.PI.scaleOfTable;

        //adjustedScene.transform.localPosition = PlayerInfo.PI.T.localPosition;
        //adjustedScene.transform.localRotation = PlayerInfo.PI.T.rotation;
        //adjustedScene.transform.localPosition = PlayerInfo.PI.T.localScale;

        //ARSO.MakeContentAppearAt(adjustedScene.transform, PlayerInfo.PI.T.position, PlayerInfo.PI.T.rotation);
    }

    private void Update()
    {
        if (PlayerInfo.PI.T != null && instanceOfMap != null) 
        {
            instanceOfMap.SetActive(true);
            instanceOfMap.transform.position = PlayerInfo.PI.T.position;
            instanceOfMap.transform.eulerAngles = PlayerInfo.PI.T.eulerAngles;
            instanceOfMap.transform.localScale = PlayerInfo.PI.T.localScale;
        }
    }

    //internal void setActive(bool v)
    //{
    //    throw new NotImplementedException();
    //}
}
