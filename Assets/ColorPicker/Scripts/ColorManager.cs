using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{

    public Color color;

    private void Start()
    {
        color = Color.blue;
    }

    /// <summary>
    /// converts a given HSBcolor to a "regular" RGB color
    /// </summary>
    /// <param name="color"></param> the HSBColor
    void OnColorChange(HSBColor color)
    {
        this.color = color.ToColor();
    }
}
