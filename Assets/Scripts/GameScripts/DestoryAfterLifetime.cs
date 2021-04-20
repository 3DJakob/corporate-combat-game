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
        Destroy(this.gameObject, lifetime);
    }

    private void Update()
    {

    }

    private void OnDestroy()
    {
        Debug.Log("WindPower down");
    }

}
