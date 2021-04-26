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
            Debug.Log("WindPower down");
            platform.isUsed = false;
            PhotonNetwork.Destroy(gameObject);
        } 
    }

}
