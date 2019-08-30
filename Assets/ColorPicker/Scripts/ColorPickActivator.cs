using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ColorPickActivator : MonoBehaviour
{

    public GameObject colorPicker;

    public SteamVR_Input_Sources controller = SteamVR_Input_Sources.LeftHand;
    public SteamVR_Action_Boolean selectAction;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Input_Sources handType;

    // Update is called once per frame
    void Update()
    {
        if (SteamVR_Actions.default_GrabGrip.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            colorPicker.SetActive(!colorPicker.activeSelf);
            Sprayer.openColorPicker(colorPicker.activeSelf);
        }
    }
}
