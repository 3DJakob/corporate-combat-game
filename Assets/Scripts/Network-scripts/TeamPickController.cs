using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamPickController : MonoBehaviourPunCallbacks, IInRoomCallbacks
{

    private void Awake()
    {
        //TPC = this;
        chosenField = -1;
    }

    private PhotonView PV;
    private int chosenField;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    public void OnClickTeamPick(int clickedTeam) 
    {
        if (PlayerInfo.PI != null)
        {
            PlayerInfo.PI.mySelectedTeam = clickedTeam;
            PlayerPrefs.SetInt("MyTeam", clickedTeam);
            Debug.Log("Im in team" + clickedTeam);

            //teamButtons[0].image.fillCenter = false;
            //teamButtons[1].image.fillCenter = false;

            //teamButtons[clickedTeam].image.fillCenter = true;
            
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        for(int i = 0; i < MenuScript.menu.teamButtons.Length; ++i)
        {
            if (MenuScript.menu.teamButtons[i].GetComponentInChildren<Text>().text.Equals(otherPlayer.NickName))
            {
                PV.RPC("RPC_SetName", RpcTarget.AllBuffered, "empty", i, -1);
                MenuScript.menu.teamButtons[i].GetComponent<Button>().interactable = true;
                break;
            }
        }

    }

    public void TeamPicked(int clickedButton)
    {
        
        PV.RPC("RPC_SetName", RpcTarget.AllBuffered, PhotonNetwork.NickName, clickedButton, chosenField);
        chosenField = clickedButton;
        
        Debug.Log(clickedButton); 
    }

    [PunRPC]
    void RPC_SetName(string name, int newField, int oldField)
    {
        Debug.Log("Changing team-buttons");
        //Debug.Log(MenuScript.menu.teamButtons[newField]);

        //Debug.Log(MenuScript.menu.teamButtonRed1.GetComponent<Text>().text);
        //Debug.Log(MenuScript.menu.teamButtons[newField].GetComponentInChildren<Text>().text);
        MenuScript.menu.teamButtons[newField].GetComponentInChildren<Text>().text = name;
        MenuScript.menu.teamButtons[newField].GetComponent<Button>().interactable = false;
        if(oldField != -1)
        {
            MenuScript.menu.teamButtons[oldField].GetComponent<Button>().interactable = true;
            MenuScript.menu.teamButtons[oldField].GetComponentInChildren<Text>().text = "empty";
        }
        
    }

}
