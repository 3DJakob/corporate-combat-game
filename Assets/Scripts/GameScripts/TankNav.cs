using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


public class TankNav : MonoBehaviour
{

    PhotonView PV;
    public string lineName;
    LineRenderer line;
    public float speed;
    float moveSpeed;
    float rotationSpeed;
    float stepMove;
    float stepRotate;
    //bool rotating = false;
    bool tankInitiated = false;
    public int team;
    int i;
    private Vector3 prevPosition;
    private Vector3 nextPosition;
    Vector3 lineOffset = new Vector3(0, 0.15f, 0);

    public void InitiateTank()
    {
        line = GameSetup.GS.instanceOfMap.transform.Find(lineName).GetComponent<LineRenderer>();
        PV = GetComponent<PhotonView>();

        //Decide route depending on team
        if (team == 0)
        {
            i = 0;
            nextPosition = line.GetPosition(i + 1) + lineOffset;
        }
        else if (team == 1)
        {
            i = line.positionCount - 1;
            nextPosition = line.GetPosition(i - 1) + lineOffset;
        }
        prevPosition = line.GetPosition(i) + lineOffset;
        this.transform.localPosition = prevPosition;
        tankInitiated = true;
        Debug.Log(line.positionCount);

        //StartCoroutine("GetNextPositionDelay", .2f);
    }


    public void SetRotation(){
        this.transform.LookAt(GameSetup.GS.instanceOfMap.transform.TransformPoint(nextPosition));
    }

    public void SetRotation(Transform T){
        this.transform.LookAt(T);
    }

    void Update()
    {
        if (!tankInitiated) return;
        stepMove = Mathf.FloorToInt(moveSpeed);

        if (PV.IsMine)
        {
            if (nextPosition == null)
            {
                PhotonNetwork.Destroy(gameObject);
            }

            if (!GetComponent<FOV>().found)
            {
                
                moveSpeed += speed / Vector3.Distance(prevPosition, nextPosition);
                this.transform.localPosition = Vector3.Lerp(prevPosition, nextPosition, moveSpeed - stepMove);
                
                this.transform.localRotation = Quaternion.RotateTowards (this.transform.localRotation, Quaternion.LookRotation(nextPosition - this.transform.localPosition), Time.deltaTime * 40 );


            }
            if (this.transform.localPosition == nextPosition)
            {
                getNextPosition();
                //SetRotation();
            }
        }
        else
        {
            this.transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, 1.0f);
        }
    }
    void getNextPosition()
    {
        if (team == 0 && i < line.positionCount)
        {
            i++;
            nextPosition = line.GetPosition(i + 1) + lineOffset;
        }
        else if (team == 1 && i > 0)
        {
            i--;
            nextPosition = line.GetPosition(i - 1) + lineOffset;
        }
        prevPosition = line.GetPosition(i) + lineOffset;

    }
}



//Debug.Log(nextPosition);

//stepRotate = Mathf.FloorToInt(rotationSpeed);
//Debug.Log(stepRotate);
//Debug.Log(rotating);


//------Test with rotation ----
// if(!GetComponent<FOV>().found && !rotating){
//     moveSpeed += speed/Vector3.Distance(prevPosition, nextPosition);
//     Debug.Log("MoveSpeed " + moveSpeed);
//     this.transform.position = Vector3.Lerp(prevPosition, nextPosition, moveSpeed - stepMove);
// } 
// if(this.transform.position == nextPosition)
// {
//     if(this.transform.rotation != Quaternion.LookRotation(nextPosition)){
//         rotating = true;

//         Debug.Log("Quaternion " +Quaternion.Angle(this.transform.rotation, Quaternion.LookRotation(nextPosition)));
//         if(Quaternion.Angle(this.transform.rotation, Quaternion.LookRotation(nextPosition)) != 0)
//             rotationSpeed += speed/(100*Quaternion.Angle(this.transform.rotation, Quaternion.LookRotation(nextPosition)));
//         Debug.Log("RotationSpeed " + rotationSpeed);
//         Debug.Log("NextRotation " + Quaternion.LookRotation(nextPosition));

//         Debug.Log("Current rotation " + this.transform.rotation);
//         this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(nextPosition), rotationSpeed - stepRotate);
//     }
//     else{
//         getNextPosition();
//         rotating = false;
//     }

// }

//If no targets are in sight, move along the road   