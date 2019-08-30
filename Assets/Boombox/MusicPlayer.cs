using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource player;
    public AudioClip firstClip;


    // Start is called before the first frame update
    void Start()
    {
        player.clip = firstClip;
        player.loop = false;

    }

    public void PlayPause()
    {
        if (player.isPlaying) {
            player.Stop();
        } else
        {
            player.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            PlayPause();
        }
    }
}
