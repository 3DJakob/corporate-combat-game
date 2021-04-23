using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARSelect : MonoBehaviour
{
    [SerializeField] private string selectableTag = "Selectable";
    public GameObject arCamera;
    // Start is called before the first frame update

    private GameObject selectedCard;
    private GameObject lastHovered;
    void Start()
    {
        
    }

    private void ColorObject(GameObject toColor, Color color) {
        if (toColor) {
            var cardRenderer = toColor.GetComponent<Renderer>();
            cardRenderer.material.SetColor("_Color", color);  
        }
    }

    // Update is called once per frame
    void Update() {
        RaycastHit hit;
        if (Physics.Raycast(arCamera.transform.position, arCamera.transform.forward, out hit)) {
            if (hit.transform.CompareTag(selectableTag)) {
                ColorObject(lastHovered, Color.white);

                lastHovered = hit.transform.gameObject;

                //Check if card type is the same as layer of the gameobject or if the hovered gameobject is of type card
                if((Card)lastHovered.GetComponent(typeof(Card)) || selectedCard.layer == lastHovered.layer && (LayerMask.LayerToName(lastHovered.layer) == "Road" || lastHovered.transform.parent.name == PlayerInfo.PI.mySelectedTeam.ToString()))
                {
                    ColorObject(lastHovered, Color.cyan);
                    ColorObject(selectedCard, Color.green);
                     
                }
            }
        } else {
            if (lastHovered) {
                if (!(selectedCard && lastHovered.name == selectedCard.name)) {
                    ColorObject(lastHovered, Color.white);
                }
            }
        }
    }

    public void Select() {
        RaycastHit hit;

        if (Physics.Raycast(arCamera.transform.position, arCamera.transform.forward, out hit)) {
            GameObject theObj = hit.transform.gameObject;

            
            Card card = (Card) theObj.GetComponent(typeof(Card));
            if (card) {
                ColorObject(selectedCard, Color.white);
                selectedCard = theObj;

                ColorObject(selectedCard, Color.green);
            }

            bool isRoad = (theObj.name == "ForestRoad" || theObj.name == "HighwayRoad" || theObj.name == "MountainRoad");
            bool isPlatform = (theObj.name == "WindPlatform" || theObj.name == "SunPlatform" || theObj.name == "OilPlatform");

            if(selectedCard)
            {
                Card cardScript = (Card)selectedCard.GetComponent(typeof(Card));

                if (isRoad && cardScript.type == "Tank")
                {
                    string lane = theObj.name.Split(new string[] { "Road" }, System.StringSplitOptions.None)[0];
                    cardScript.Spawn(lane);
                }
                else if(isPlatform && cardScript.type == "EnergySource" && theObj.transform.parent.name == PlayerInfo.PI.mySelectedTeam.ToString() && selectedCard.layer == theObj.layer)
                {
                    Transform pos = theObj.transform;
                    cardScript.Spawn(pos);
                }
                
            }
            


        }
    }
}
