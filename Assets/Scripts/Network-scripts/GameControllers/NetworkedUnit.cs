using Photon.Pun;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Positions Tanks as children of "Game"
//Also smoothly syncs their movement so no visible staggering will occur for player who aren't master
public class NetworkedUnit :  MonoBehaviour, IPunObservable
{
    Vector3 realPos;
    Vector3 lastPos;
    Quaternion realRot;
    Vector3 velocity;
    PhotonView PV;
    public float PredictionCoeffecient = 1.0f;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        GetComponent<Transform>().SetParent(GameSetup.GS.instanceOfMap.transform, false);
        GetComponent<Transform>().localEulerAngles = new Vector3(0, 0, 0);
        //this.GetComponent<NavTank>().SetDestination();
    }
    
    //Updates every NetworkUpdate that occurs ~10 times/second
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //Host sends data
        if(stream.IsWriting && PV.IsMine)
            {
                //Current Position
                stream.SendNext(transform.localPosition);
                stream.SendNext(transform.localRotation);
                stream.SendNext((realPos - lastPos)/Time.deltaTime);
            }
            //Players recieve data
            else
            {
                //Position to lerp to
                realPos = (Vector3)(stream.ReceiveNext());
                realRot = (Quaternion)(stream.ReceiveNext());
                velocity = (Vector3)(stream.ReceiveNext());
            }
    }

    //Lerps tank to a predicted position to create smooth movement
    void Update()
    {
        lastPos = realPos;
        if(!PV.IsMine){
            transform.localPosition = Vector3.Lerp(transform.localPosition, realPos+(PredictionCoeffecient*velocity*Time.deltaTime), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, realRot, Time.deltaTime);
        }
    }
}