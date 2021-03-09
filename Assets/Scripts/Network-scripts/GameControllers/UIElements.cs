using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIElements : MonoBehaviour
{
    public static UIElements UI;

    public Button startButton;
    public Button readyButton;

    public Button tankSpawnButton;
    public Button rightButton;
    public Button leftButton;

    public GameObject canvas;

    private void Awake()
    {
        if (UIElements.UI == null)
        {
            UIElements.UI = this;
        }
        else
        {
            if (UIElements.UI != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        startButton = GameObject.Find("StartGame").GetComponent<Button>();
        
    }

    public void OnStartGameButtonClicked()
    {
        tankSpawnButton = GameObject.Find("Spawn cube").GetComponent<Button>();
        rightButton = GameObject.Find("MoveRight").GetComponent<Button>();
        leftButton = GameObject.Find("MoveLeft").GetComponent<Button>();

        //canvas.
    }
}


