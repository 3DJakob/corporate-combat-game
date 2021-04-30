using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardPickerController : MonoBehaviour, IOnEventCallback
{
    //---- UIElements -----
    public GameObject teamText;
    public GameObject numberOfCards;
    public GameObject doneButton;
    public GameObject allDoneButton;
    public GameObject infoText;
    public GameObject[] cards;

    public string[] namesOfCards;
    private bool[] picked;
    private int[] cardsLeft;
    private bool[] teamsDone;
    public const int cardLimit = 4;

    private const int SELECTEDCARD = 3;
    private const int TEAMDONE = 4;

    private void OnEnable()
    {
        if (PlayerInfo.PI.mySelectedTeam == 0)
        {
            teamText.GetComponent<Text>().text = "RED";
            teamText.GetComponent<Text>().color = new Color32(250, 13, 13, 255);
        }
        else
        {
            teamText.GetComponent<Text>().text = "BLUE";
            teamText.GetComponent<Text>().color = new Color32(13, 54, 250, 255);
        }
        PhotonNetwork.AddCallbackTarget(this);
        doneButton.SetActive(false);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        picked = new bool[10];
        cardsLeft = new int[2]{cardLimit, cardLimit};
        teamsDone = new bool[2];
        //namesOfCards = new string[10] { "FastTank", "FastTank", "FastTank", "FastTank", "FastTank", "FastTank", "FastTank", "FastTank", "FastTank", "WindPower" };
    }

    public void DoneButtonClicked()
    {
        byte eventId = TEAMDONE;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        int content = PlayerInfo.PI.mySelectedTeam;
        PhotonNetwork.RaiseEvent(eventId, content, raiseEventOptions, SendOptions.SendReliable);
    }

    //Recieves int clickedCard for the cards onClick in CardPickerScene
    //Sends an event with selected team and effected card
    public void CardPickedEvent(int clickedCard)
    {
        byte eventId = SELECTEDCARD;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        int[] content = { PlayerInfo.PI.mySelectedTeam, clickedCard };
        PhotonNetwork.RaiseEvent(eventId, content, raiseEventOptions, SendOptions.SendReliable);
    }

    //Run by event "SELECTEDCARD" (defined in this script)
    //Keeps track of the selected cards within the team
    public void CardPicked(int team, int clickedCard)
    {
        picked[clickedCard] = !picked[clickedCard];

        if (picked[clickedCard] && cardsLeft[team] > 0)
        {
            cards[clickedCard].GetComponent<Image>().color = new Color32(155, 253, 123, 255);
            cardsLeft[team]--;
        }
        else if(!picked[clickedCard] && cardsLeft[team] < cardLimit)
        {
            cards[clickedCard].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            cardsLeft[team]++;
        }
        else
        {
            picked[clickedCard] = !picked[clickedCard];
        }
        numberOfCards.GetComponent<Text>().text = "" + cardsLeft[team];

        if (cardsLeft[team] == 0)
            doneButton.SetActive(true);
        else
            doneButton.SetActive(false);
    }

    //The events for cardScene are handled here
    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == SELECTEDCARD)
        {
            
            int[] data = (int[])photonEvent.CustomData; //data[0] is the team that sent the event //data[1] is which card is effected

            if (data[0] == PlayerInfo.PI.mySelectedTeam && !teamsDone[data[0]])
            {
                GameObject.Find("CardPickerController").GetComponent<CardPickerController>().CardPicked(data[0], data[1]);
            }
                
        }
        if(eventCode == TEAMDONE)
        {
            
            int data = (int)photonEvent.CustomData; //data is the team that sent the event 
            teamsDone[data] = true;

            if (data == PlayerInfo.PI.mySelectedTeam)
            {
                PlayerInfo.PI.selectedCards = new string[cardLimit];
                doneButton.SetActive(false);
                infoText.SetActive(true);
                int counter = 0;

                for(int i = 0; i < 10; ++i) //Loop through all chosen cards and store the data in PlayerInfo
                {
                    if (picked[i])
                    {
                        PlayerInfo.PI.selectedCards[counter] = namesOfCards[i];
                        Debug.Log(namesOfCards[i]);
                        ++counter;
                    }
                }
                
            }   
            
            if (PhotonNetwork.IsMasterClient && teamsDone[0] && teamsDone[1])
            {
                allDoneButton.SetActive(true);
                infoText.SetActive(false);
            }
                
        } 
        
    }
}
