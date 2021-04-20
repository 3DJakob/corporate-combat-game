using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyGeneration : MonoBehaviour
{
    private EnergyController myEC;
    public float rate;
    public int team;
    private float timeAlive;

    private void Awake()
    {
        timeAlive = 0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        myEC = GameObject.Find("EnergyController").GetComponent<EnergyController>();
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime;

        if(timeAlive > rate)
        {
            Debug.Log(timeAlive);
            //int[] content = {team, myEC.getEnergy() + 1};
            myEC.updateEnergy(team, myEC.energy + 1);
            timeAlive = timeAlive - rate;
        }
    }

}
