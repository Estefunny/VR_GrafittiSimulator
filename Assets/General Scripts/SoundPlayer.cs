using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SoundPlayer : MonoBehaviour
{
    public AudioSource player;
    public AudioClip firstClip;
    public SteamVR_Input_Sources controller = SteamVR_Input_Sources.RightHand;
    public SteamVR_Action_Boolean selectAction;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Input_Sources handType;

    // Start is called before the first frame update
    void Start()
    {
        player.clip = firstClip;
        player.loop = true;

    }

    public void PlayPause()
    {
        if (player.isPlaying)
        {
            player.Stop();
        }
        else
        {
            player.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SteamVR_Actions.default_Squeeze.GetAxis(SteamVR_Input_Sources.RightHand) > 0)
        {
            player.Play();
        }
        player.Stop();
    }
}
