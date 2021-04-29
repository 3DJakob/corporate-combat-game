using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    void Awake()
    {
        GameObject[] musicPlay = GameObject.FindGameObjectsWithTag("MusicPlaying");
        GameObject music = GameObject.FindGameObjectWithTag("Music");

        if(musicPlay.Length > 1 || music != null)
            Destroy(this.gameObject);

            DontDestroyOnLoad(this.gameObject);
    }
}
