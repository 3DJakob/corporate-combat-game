using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    // Start is called before the first frame update
    
    public float Health = 50f;
    public bool TakeDamage(float amount)
    {
        Health -= amount;
        Debug.Log(Health);
        if (Health <= 0 && gameObject.tag != "Finish")
        {
            Die();
            return true;
        }
        else if(Health <= 0 && gameObject.tag == "Finish"){
            Debug.Log("You WIN!!!");
            Die();
            return false;
        }
        else{
            return false;
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
