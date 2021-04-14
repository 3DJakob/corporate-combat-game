using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo PI;
    public string nickname;
    public int mySelectedTeam;
    public string[] selectedCards;

    public Transform T;
    public int[] teams;

    private void OnEnable()
    {
        if (PlayerInfo.PI == null)
        {
            PlayerInfo.PI = this;
        }
        else
        {
            if (PlayerInfo.PI != this) 
            {
              Destroy(PlayerInfo.PI.gameObject);
                PlayerInfo.PI = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void updateOrigin(Transform transform)
    {
        T = transform;
    }


    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("MyTeam"))
        {
            mySelectedTeam = PlayerPrefs.GetInt("MyTeam");
        }
        else 
        {
            mySelectedTeam = 0;
            PlayerPrefs.SetInt("MyTeam", mySelectedTeam);
        }

    }

    private void Update()
    {
        if (T == null && GameSetup.GS != null) {
            T = GameSetup.GS.instanceOfMap.transform;
        }
    }
}
