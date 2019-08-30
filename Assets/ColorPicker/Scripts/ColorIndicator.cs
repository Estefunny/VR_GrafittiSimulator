using System.Collections.Generic;
using UnityEngine;

public class ColorIndicator : MonoBehaviour {

	HSBColor color;
    public Renderer render;

    private int savedColorMax = 8;
    private LinkedList<Color> savedColorList = new LinkedList<Color>();
    public Renderer[] savedColorRenderers;

    public void saveColor(Color c)
    {
        savedColorList.AddFirst(c);
        if (savedColorList.Count > savedColorMax)
        {
            savedColorList.RemoveLast();
        }
        updateSavedColors();
    }

    private void updateSavedColors()
    {
        int i = 0;
        foreach (Color c in savedColorList)
        {
            //savedColorRenderers[i]
            i++;
        }
    }

	void Start() {
		color = HSBColor.FromColor(render.sharedMaterial.GetColor("_Color"));
		transform.parent.BroadcastMessage("SetColor", color);
        transform.parent.BroadcastMessage("OnColorChange", color, SendMessageOptions.DontRequireReceiver);

        savedColorList.AddLast(Color.red);
        savedColorList.AddLast(Color.green);
        savedColorList.AddLast(Color.blue);
        savedColorList.AddLast(Color.yellow);
        updateSavedColors();
    }

	void ApplyColor ()
	{
        render.sharedMaterial.SetColor("_Color", color.ToColor());
		transform.parent.BroadcastMessage("OnColorChange", color, SendMessageOptions.DontRequireReceiver);
	}

	void SetHue(float hue)
	{
		color.h = hue;
		ApplyColor();
    }	

	void SetSaturationBrightness(Vector2 sb) {
        color.s = sb.x;
		color.b = sb.y;
		ApplyColor();
	}
}
