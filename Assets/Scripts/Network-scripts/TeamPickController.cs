using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamPickController : MonoBehaviour
{
    public void OnClickTeamPick(int clickedTeam) 
    {
        if (PlayerInfo.PI != null)
        {
            PlayerInfo.PI.mySelectedTeam = clickedTeam;
            PlayerPrefs.SetInt("MyTeam", clickedTeam);
            Debug.Log("Im in team" + clickedTeam);
        }
    }
}
