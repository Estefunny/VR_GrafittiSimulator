﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{

    public Color color = Color.blue;

    // Update is called once per frame
    void onColorChange(HSBColor color)
    {
        this.color = color.ToColor();
    }
}
