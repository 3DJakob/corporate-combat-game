using Photon.Pun;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NetworkedUnit :  MonoBehaviour, IPunObservable
{
    Vector3 realPos;
    Vector3 lastPos;
    Quaternion realRot;
    Vector3 velocity;
    PhotonView PV;
    public float PredictionCoeffecient = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        GetComponent<Transform>().SetParent(GameSetup.GS.instanceOfMap.transform, false);
        GetComponent<Transform>().localEulerAngles = new Vector3(0, 0, 0);
        //this.GetComponent<NavTank>().SetDestination();
    }
    // Update is called once per frame
    
    
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting && PV.IsMine)
            {
                stream.SendNext(transform.localPosition);
                stream.SendNext(transform.localRotation);
                stream.SendNext((realPos - lastPos)/Time.deltaTime);
            }
            else
            {
                realPos = (Vector3)(stream.ReceiveNext());
                realRot = (Quaternion)(stream.ReceiveNext());
                velocity = (Vector3)(stream.ReceiveNext());
            }
        
    }
    
    void Update()
    {
        lastPos = realPos;
        if(!PV.IsMine){
            transform.localPosition = Vector3.Lerp(transform.localPosition, realPos+(PredictionCoeffecient*velocity*Time.deltaTime), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, realRot, Time.deltaTime);
        }
    }

    





}