using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryAfterLifetime : MonoBehaviour
{
    public float lifetime;
    public PlatformUsed platform;
    private void Start()
    {
        Debug.Log(lifetime);
        Destroy(gameObject, lifetime);
    }
    private void OnDestroy()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Power Down");
            GameSetup.GS.player.UpdatePlatform(this.GetComponent<EnergyGeneration>().team, platform.name, false);
            PhotonNetwork.Destroy(gameObject);
        } 
    }

}
