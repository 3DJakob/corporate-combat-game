using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    // Start is called before the first frame update
    
    public float Health = 50f;
    public bool TakeDamage(float amount)
    {
        Health -= amount;
        Debug.Log(Health);
        if (Health <= 0 && gameObject.tag != "Finish")
        {
            Die();
            return true;
        }
        else if(Health <= 0 && gameObject.tag == "Finish"){

            //Send event to photonPlayer and attach this.gameObject.layer
            if (PhotonNetwork.IsMasterClient)
            {
                byte eventId = 2;
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                PhotonNetwork.RaiseEvent(eventId, this.gameObject.layer, raiseEventOptions, SendOptions.SendReliable);
            }

            Die();
            return false;
        }
        else{
            return false;
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        //----TO BE REMOVED WHEN TANKS CAN SHOOT AT THE FACTORY ----
        if (Health <= 0 && gameObject.tag == "Finish")
        {

            //Send event to photonPlayer and attach this.gameObject.layer
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Calling event");
                Debug.Log("Layer is " + LayerMask.LayerToName(this.gameObject.layer));
                byte eventId = 2;
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                PhotonNetwork.RaiseEvent(eventId, LayerMask.LayerToName(this.gameObject.layer), raiseEventOptions, SendOptions.SendReliable);
            }
            Die();
        }
    }
}
