using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameSync : MonoBehaviour
{
    //-----This class is not currently used-----

    private PhotonView PV;
    public static GameSync GSync;
    public int syncVariable;

    private void OnEnable()
    {
        if (GameSync.GSync == null)
        {
            GameSync.GSync = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            PV.RPC("RPC_Function", RpcTarget.AllBuffered, syncVariable);
        }
    }

    public void UpdateVariable()
    {
        
             //syncVariable += 1;
             //PV.RPC("RPC_Function", RpcTarget.AllBuffered, syncVariable);
        
    }

    [PunRPC]
    void RPC_Function(int syncIn)
    {
        syncVariable = syncIn;
        //Debug.Log("Updated variable with 1 at " + PV.ViewID);
        Debug.Log(syncVariable);
    }
}
