using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TankNav : MonoBehaviour
{
    public LineRenderer line;
    public float speed;
    float moveSpeed;
    float rotationSpeed;
    float step;
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
                
        step = Mathf.FloorToInt(moveSpeed);
         
        Debug.Log(rotating);

        
        //------Test with rotation ----
        // if(!FOV.found && !rotating){
        //     moveSpeed += speed/Vector3.Distance(prevPosition, nextPosition);
        //     this.transform.position = Vector3.Lerp(prevPosition, nextPosition, moveSpeed - step);
        // } 
        // if(this.transform.position == nextPosition)
        // {
        //     if(this.transform.rotation != Quaternion.LookRotation(nextPosition)){
        //         rotating = true;
        //         step = Mathf.FloorToInt(rotationSpeed);
        //         Debug.Log(step);
        //         rotationSpeed += speed/Quaternion.Angle(this.transform.rotation, Quaternion.LookRotation(nextPosition));
        //         this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(nextPosition), rotationSpeed - step);
        //     }
        //     else{
        //         getNextPosition();
        //         rotating = false;
        //     }

        // }
        

        //If no targets are in sight, move along the road        
        if(!FOV.found){
            moveSpeed += speed/Vector3.Distance(prevPosition, nextPosition);
            this.transform.position = Vector3.Lerp(prevPosition, nextPosition, moveSpeed - step);
        }
        //if(this.transform.rotation != Quaternion.LookRotation(nextPosition))
        //this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(nextPosition), moveSpeed - step);
        //Switch positions to interpolate between
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