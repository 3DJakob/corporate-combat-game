using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TankNav : MonoBehaviour
{
    public LineRenderer line;
    public float speed;
    float moveSpeed;
    float rotationSpeed;
    float stepMove;
    float stepRotate;
    bool rotating = false;
    bool gettingNewPos = false;
    public int team = 1;
    int i;
    private Vector3 prevPosition;
    private Vector3 nextPosition;


    void Start(){

        //Decide route depending on team
        if(team == 0){
            i = 0;
            nextPosition = line.GetPosition(i+1);
        }
        else
        {
            i = line.positionCount-1;
            nextPosition = line.GetPosition(i-1);
        }
        prevPosition = line.GetPosition(i);

        //StartCoroutine("GetNextPositionDelay", .2f);
    }
     void Update(){
                
        stepMove = Mathf.FloorToInt(moveSpeed);
        stepRotate = Mathf.FloorToInt(rotationSpeed);
        Debug.Log(stepRotate);
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
        if(!GetComponent<FOV>().found){
            moveSpeed += speed/Vector3.Distance(prevPosition, nextPosition);
            this.transform.position = Vector3.Lerp(prevPosition, nextPosition, moveSpeed - stepMove);
        }
        
        if(this.transform.position == nextPosition )
        {
            getNextPosition();
            this.transform.LookAt(nextPosition);
        }
    }
    
    // IEnumerator GetNextPositionDelay(float delay)
    // {
    //     while (gettingNewPos)
    //     {
    //         yield return new WaitForSeconds(delay);
    //         getNextPosition();
    //     }
    // }

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
        gettingNewPos = false;
    }

}