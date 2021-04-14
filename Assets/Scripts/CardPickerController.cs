using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardPickerController : MonoBehaviour
{
    public GameObject teamText;
    public GameObject[] cards;
    private bool[] picked;

    private void OnEnable()
    {
        if(PlayerInfo.PI.mySelectedTeam == 0)
        {
            teamText.GetComponent<Text>().text = "RED";
            teamText.GetComponent<Text>().color = new Color(250, 13, 13, 255);
        }
        else
        {
            teamText.GetComponent<Text>().text = "BLUE";
            teamText.GetComponent<Text>().color = new Color(13, 54, 250, 255);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        picked = new bool[10];
    }

    //Recieves int clickedCard for the cards onClick in CardPickerScene
    //Sends an event with selected team and effected card
    public void CardPickedEvent(int clickedCard)
    {
        Debug.Log("Clicked card");
        byte eventId = 3;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        int[] content = {PlayerInfo.PI.mySelectedTeam, clickedCard};

        PhotonNetwork.RaiseEvent(eventId, content, raiseEventOptions, SendOptions.SendReliable);
    }

    //Run by event "SELECTEDCARD" (defined in PhotonPlayer)
    //Keeps track of the selected cards within the team
    public void CardPicked(int clickedCard)
    {
        picked[clickedCard] = !picked[clickedCard];
        HighLight(clickedCard);
    }

    void HighLight(int n)
    {
        if (picked[n])
        {
            cards[n].GetComponent<Image>().color = new Color(155, 253, 123, 255);
            Debug.Log("Selected Card " + n+1);
        }
        else
        {
            cards[n].GetComponent<Image>().color = new Color(255, 255, 255, 255);
            Debug.Log("Deelected Card" + n+1);
        }

    }
}
