using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NetworkedTurret : MonoBehaviour, IPunObservable
{
    // Start is called before the first frame update
    Quaternion realRot;
    PhotonView PV;
    void Start()
    {
        PV = GetComponent<PhotonView>();
    }
    
     //Updates every NetworkUpdate that occurs ~10 times/second
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //Host sends data
        if(stream.IsWriting && PV.IsMine)
            {
                //Current Position
                stream.SendNext(transform.localRotation);
            }
            //Players recieve data
            else
            {
                //Position to lerp to
                realRot = (Quaternion)(stream.ReceiveNext());
            }
    }

    //Lerps tank to a predicted rotation to create smooth movement
    void Update()
    {
        if(!PV.IsMine){
            transform.localRotation = Quaternion.Lerp(transform.localRotation, realRot, Time.deltaTime);
        }
    }
}
