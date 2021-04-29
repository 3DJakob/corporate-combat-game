using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformUsed : MonoBehaviour
{
    //PhotonPlayer PP;
    public bool isUsed = false;

    private void Start() {
        //PP = GameSetup.GS.player;
    }

    public void ToggleVisablity()
    {
        if (isUsed)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }
}
