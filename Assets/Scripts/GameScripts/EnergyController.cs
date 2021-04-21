using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyController : MonoBehaviour
{
    public static EnergyController EC;

    public PhotonPlayer PP;
    public Text textObject;
    float timeAlive = 0;
    public int rate = 1;
    public int energy = 200;

    // Start is called before the first frame update
    void Start()
    {
        textObject.text = "Energy: " + energy.ToString();
    }
    private void OnEnable()
    {
        if (EnergyController.EC == null)
        {
            EnergyController.EC = this;
        }
        else
        {
            if (EnergyController.EC != this) 
            {
              Destroy(EnergyController.EC.gameObject);
                EnergyController.EC = this;
            }
        }
    }

    public bool Buy(int cost) {
        if (energy >= cost) {
            
            int[] content = { PlayerInfo.PI.mySelectedTeam, energy - cost };
            byte eventId = 3;

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
    //For every spawned energyGeneration, update rate by 1 for your team
    //Called by energyGeneration
    public void updateRate(int value)
    {
        rate += value;
    }

    private void Update() {

        timeAlive += Time.deltaTime;
        if(timeAlive > 2f)
        {
            //Debug.Log(timeAlive);
            //Update the energy for all players
            energy += rate;
            textObject.text = "Energy: " + energy.ToString();
            timeAlive = timeAlive - 1;
        }
    }
}
