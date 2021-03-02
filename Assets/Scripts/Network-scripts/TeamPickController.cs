using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamPickController : MonoBehaviour
{
    public Button[] teamButtons;

    public void OnClickTeamPick(int clickedTeam) 
    {
        if (PlayerInfo.PI != null)
        {
            PlayerInfo.PI.mySelectedTeam = clickedTeam;
            PlayerPrefs.SetInt("MyTeam", clickedTeam);
            Debug.Log("Im in team" + clickedTeam);

            teamButtons[0].image.fillCenter = false;
            teamButtons[1].image.fillCenter = false;

            teamButtons[clickedTeam].image.fillCenter = true;
            
        }
    }
}
