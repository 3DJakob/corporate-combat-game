using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryAfterLifetime : MonoBehaviour
{
    public float lifetime;
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
            PhotonNetwork.Destroy(gameObject);
        } 
    }

}
