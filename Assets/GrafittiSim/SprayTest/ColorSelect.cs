using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSelect : MonoBehaviour
{
    public ColorManager cm;
    public ColorIndicator ci;

    public void setPickedColor()
    {

        Sprayer.setSprayColor(new Color(cm.color.r, cm.color.g, cm.color.b, 1));
        
    }
}
