using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSelect : MonoBehaviour
{
    public ColorManager cm;
    public void setColor(int c)
    {
        Color set = Color.blue;
        switch(c)
        {
            case 1: set = Color.red;
                break;
            case 2:
                set = Color.green;
                break;
            case 3:
                set = Color.yellow;
                break;
            case 4:
                set = Color.white;
                break;
            case 5:
                set = Color.black;
                break;
            case 6:
                set = Color.clear;
                break;
        }
        set = cm.color;
        set.a = 1;
        foreach (Sprayer s in FindObjectsOfType<Sprayer>())
        {
            s.setSprayColor(set);
        }
    }

    public void setPickedColor()
    {
        foreach (Sprayer s in FindObjectsOfType<Sprayer>())
        {
            print(cm.color);
            s.setSprayColor(new Color(cm.color.r, cm.color.g, cm.color.b, 1));
        }
    }
}
