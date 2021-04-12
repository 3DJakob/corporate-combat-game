
using Photon.Pun;
using UnityEngine;

public class RPCHit : MonoBehaviour
{
    /*
    public HealthManager hs;
    PhotonView PV;
    public float Health;
    // Use this for initialization
    void Start()
    {
        hs = this.GetComponent<HealthManager>();
        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine)
        {
            hs.health = Health;
        }
        if (PV.IsMine)
        {
            Health = hs.health;
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(Health);
        }
        else if (stream.isReading)
        {
            Health = (float)stream.ReceiveNext();
        }
    }

    */
}