using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSetup : MonoBehaviour
{
    public static GameSetup GS;
    public Transform[] spawnPoints;
    public PhotonPlayer player;
    public GameObject adjustedScene;

    //Create GameSetup OnEnable (When switching to the game scene)
    private void OnEnable()
    {
        if (GameSetup.GS == null)
        {
            GameSetup.GS = this;
        }
    }
    
    //If a player is disconnected load menu-scene
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
        adjustedScene.transform.position = PlayerInfo.PI.positionOfTable;
        adjustedScene.transform.eulerAngles = PlayerInfo.PI.rotationOfTable;
        adjustedScene.transform.localScale = PlayerInfo.PI.scaleOfTable;
    }

    //internal void setActive(bool v)
    //{
    //    throw new NotImplementedException();
    //}
}
