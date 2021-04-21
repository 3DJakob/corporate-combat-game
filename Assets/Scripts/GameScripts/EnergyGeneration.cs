using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class EnergyGeneration : MonoBehaviour
{
    public int rate;
    public int team;
    public Transform myTransform;

    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(GameSetup.GS.instanceOfMap.transform, false);
        transform.localPosition += new Vector3(0, 0.1f, 0);
        //Update energyBySecond for your team
        int[] content = {team, rate};
        byte eventId = 4;

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(eventId, content, raiseEventOptions, SendOptions.SendReliable);
    }

    // Update is called once per frame

    private void OnDestroy() {
        int[] content = {team, -rate};
        byte eventId = 4;

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(eventId, content, raiseEventOptions, SendOptions.SendReliable);
    }

}
