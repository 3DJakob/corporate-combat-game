using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TankNav : MonoBehaviour
{
    public string lineName;
    LineRenderer line;
    public float speed;
    float moveSpeed;
    float rotationSpeed;
    float stepMove;
    float stepRotate;
    bool rotating = false;
    bool tankInitiated = false;
    public int team;
    int i;
    private Vector3 prevPosition;
    private Vector3 nextPosition;


   public void InitiateTank(){

        line = GameSetup.GS.instanceOfMap.transform.Find(lineName).GetComponent<LineRenderer>();

        //Decide route depending on team
        if(team == 0){
            i = 0;
            nextPosition = line.GetPosition(i+1);
        }
        else if(team == 1)
        {
            i = line.positionCount-1;
            nextPosition = line.GetPosition(i-1);
        }

        prevPosition = line.GetPosition(i);
        this.transform.localPosition = prevPosition;
        tankInitiated = true;
        Debug.Log(line.positionCount);

        //StartCoroutine("GetNextPositionDelay", .2f);
    }

     void Update(){
        if (!tankInitiated) 
        {
            return;        
        }
        stepMove = Mathf.FloorToInt(moveSpeed);
        Debug.Log(nextPosition);

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
        if (!GetComponent<FOV>().found){
            moveSpeed += speed/Vector3.Distance(prevPosition, nextPosition);
            this.transform.localPosition = Vector3.Lerp(prevPosition, nextPosition, moveSpeed - stepMove); 
        }
        
        if(this.transform.localPosition == nextPosition)
        {
            getNextPosition();
            this.transform.LookAt(nextPosition);
            Debug.Log("Got a new position!");
        }
    }

    void getNextPosition(){
        if(team == 0 && i < line.positionCount)
            {
                i++;
                nextPosition = line.GetPosition(i+1);
            }
            else if(team == 1 && i > 0)
            {
                i--;
                nextPosition = line.GetPosition(i-1);
            }
        prevPosition = line.GetPosition(i);
        
    }
}