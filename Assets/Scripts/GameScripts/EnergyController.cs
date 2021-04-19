using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyController : MonoBehaviour
{
    public Text textObject;
    private int energy = 200;

    // Start is called before the first frame update
    void Start()
    {
        textObject.text = "Energy: " + energy.ToString();
    }

    public bool Buy(int cost) {
        if (energy >= cost) {
            //energy = energy - cost;

            byte eventId = 3;
            int[] content = { PlayerInfo.PI.mySelectedTeam, energy - cost };

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(eventId, content, raiseEventOptions, SendOptions.SendReliable);

            return true;
        } else {
            Debug.Log("You broke bastard!");
            return false;
        }
    }

    public void updateEnergy(int newEnergy)
    {
        energy = newEnergy;
        textObject.text = "Energy: " + energy.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
