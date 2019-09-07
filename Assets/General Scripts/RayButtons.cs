using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Valve.VR;

public class RayButtons : MonoBehaviour
{


    public SteamVR_Input_Sources controller = SteamVR_Input_Sources.RightHand;
    public SteamVR_Behaviour_Pose controllerPose;
    public Collider buttonCollider;
    public TextMeshPro text;

    public enum buttonType
    {
        COLOR, UNDO, ERASE, FAT, MED, SKINNY, LOAD_WALL, LOAD_TRAIN, EXIT_GAME
    }

    public ColorSelect colorSelect;
    public LevelLoader levelLoader;
    public buttonType type;

    // Update is called once per frame
    void Update()
    {
        

        Ray ray = new Ray(controllerPose.transform.position, controllerPose.transform.forward);

        RaycastHit hit;
        if (buttonCollider.Raycast(ray, out hit, 1))
        {
            text.color = Color.yellow;

            if (SteamVR_Actions.default_Squeeze.GetAxis(SteamVR_Input_Sources.RightHand) >= 0.5f &&
            SteamVR_Actions.default_Squeeze.GetLastAxis(SteamVR_Input_Sources.RightHand) < 0.5f)
            {

                switch (type)
                {
                    case buttonType.COLOR:
                        colorSelect.setPickedColor();
                        break;
                    case buttonType.UNDO:
                        SprayTarget.undoStep();
                        break;
                    case buttonType.ERASE:
                        Sprayer.setSprayColor(Color.clear);
                        break;
                    case buttonType.FAT:
                        Sprayer.setSprayCap(0.7f, 1.5f, 0.7f, 1.5f);
                        break;
                    case buttonType.MED:
                        Sprayer.setSprayCap(1, 1, 1, 1);
                        break;
                    case buttonType.SKINNY:
                        Sprayer.setSprayCap(1.2f, 0.7f, 1.15f, 0.7f);
                        break;
                    case buttonType.LOAD_WALL:
                        levelLoader.LoadLevel("Scene_Wall");
                        break;
                    case buttonType.LOAD_TRAIN:
                        levelLoader.LoadLevel("Scene_Train");
                        break;
                    case buttonType.EXIT_GAME:
                        levelLoader.ExitGame();
                        break;
                }
            }
        } else
        {
            text.color = Color.white;
        }
            
    }
}
