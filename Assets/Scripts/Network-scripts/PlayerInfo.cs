using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo PI;
    public int mySelectedTeam;
    public Vector3 positionOfTable;
    public Quaternion rotationOfTable;
    public Vector3 scaleOfTable;

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

    public void updateOrigin(Vector3 pos, Quaternion rot, Vector3 scale)
    {
        positionOfTable = pos;
        rotationOfTable = rot;
        scaleOfTable = scale;
        Debug.Log("X POS");
        Debug.Log(pos.x);
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
        if(SceneManager.GetActiveScene().buildIndex == MultiplayerSetting.multiplayerSetting.ARScene)
        {
            //-----This might work, check with Jakob----
            positionOfTable = GameObject.Find("spawnedObject").GetComponent<GameObject>().transform.position;
            rotationOfTable = GameObject.Find("spawnedObject").GetComponent<GameObject>().transform.rotation;
            //Debug.Log(positionOfTable);
        }
              
    }
}
