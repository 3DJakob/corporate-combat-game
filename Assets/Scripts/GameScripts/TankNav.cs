using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankNav : MonoBehaviour
{
public LineRenderer line;
public float speed;
float moveSpeed;
int i;
    void Start(){
        //line = GetComponent<LineRenderer>();
        i=line.positionCount-1;
    }
     void Update(){
        /*i = Mathf.FloorToInt(moveSpeed);

        moveSpeed += speed/Vector3.Distance(line.GetPosition(i), line.GetPosition(i+1));
        
        Debug.Log(i);
        this.transform.position = Vector3.Lerp(line.GetPosition(i), line.GetPosition(i+1), moveSpeed-i);
*/
        

        moveSpeed -= speed/Vector3.Distance(line.GetPosition(i), line.GetPosition(i-1));
        
        Debug.Log(i);
        this.transform.position = Vector3.Lerp(line.GetPosition(i), line.GetPosition(i-1), moveSpeed+i);
    
        if(this.transform.position == line.GetPosition(i-1) && i>0){
            i--;
        }
    }
}