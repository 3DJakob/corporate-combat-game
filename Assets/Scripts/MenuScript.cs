using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public static MenuScript menu;

    //Pages
    public GameObject menuPage;
    public GameObject joinPage;
    public GameObject lobbyPage;

    //UIElements
    public GameObject inputFieldNick;

    public GameObject createButton;
    public GameObject joinButton;
    public GameObject ARDebugButton;
    public GameObject gameDebugButton;
    public GameObject playerNameText;

    public GameObject inputFieldRoom;
    public GameObject cancelJoinButton;
    public GameObject errorJoinRoom;

    public GameObject lobbyText;
    public GameObject playersText;
    public GameObject exitRoomButton;
    public GameObject loadARSetupButton;

    public GameObject[] teamButtons;

    private const int MENU = 1;
    private const int JOIN = 2;
    private const int LOBBY = 3;

    private void Awake()
    {
        if (MenuScript.menu == null)
        {
            MenuScript.menu = this;
        }
        else
        {
            if (MenuScript.menu != this)
            {
                Destroy(MenuScript.menu.gameObject);
                MenuScript.menu = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {

    }

    public void ToggleButtons(int page)
    {
        if (page == MENU)
        {
            menuPage.SetActive(true);
            joinPage.SetActive(false);
            lobbyPage.SetActive(false);
        }
        else if(page == JOIN)
        {
            menuPage.SetActive(false);
            joinPage.SetActive(true);
            lobbyPage.SetActive(false);
        }
        else if(page == LOBBY)
        {
            menuPage.SetActive(false);
            joinPage.SetActive(false);
            lobbyPage.SetActive(true);
        }

    }
}
