using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class UIElements : MonoBehaviour
{
    public static UIElements UI;
    private PhotonView PV;

    public Button startButton;
    public Button readyButton;

    public Button tankSpawnButton;
    public Button rightButton;
    public Button leftButton;

    public Canvas canvasGame;
    public Canvas canvasAR;

    private void Awake()
    {
        if (UIElements.UI == null)
        {
            UIElements.UI = this;
        }
        
        DontDestroyOnLoad(this.gameObject);
        canvasGame.enabled = false;
        canvasAR.enabled = true;
    }

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        //Debug.Log(PV.IsMine);
        startButton = GameObject.Find("StartGame").GetComponent<Button>();
        
    }

    public void OnStartGameButtonClicked()
    {
        if(PhotonNetwork.IsMasterClient)
            PV.RPC("RPC_EnableUI", RpcTarget.All);

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

        var planeManager = GameObject.Find("AR Session Origin").GetComponent<ARPlaneManager>();
        Debug.Log(planeManager);
        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }


    }
}


