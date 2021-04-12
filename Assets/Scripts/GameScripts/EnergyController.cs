using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyController : MonoBehaviour
{
    public Text textObject;
    private int energy = 200;

    // Start is called before the first frame update
    void Start()
    {

    }

    public bool Buy(int cost) {
        if (energy >= cost) {
            Debug.Log("A unit was bought!");
            energy = energy - cost;
            return true;
        } else {
            Debug.Log("You broke bastard!");
            return false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        textObject.text = "Energy: " + energy.ToString();
    }
}
