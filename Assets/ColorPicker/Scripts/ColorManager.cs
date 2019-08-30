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

    
    void OnColorChange(HSBColor color)
    {
        this.color = color.ToColor();
    }
}
