using UnityEngine;

public class ColorIndicator : MonoBehaviour {

	HSBColor color;
    public Renderer render;

	void Start() {
		color = HSBColor.FromColor(render.sharedMaterial.GetColor("_Color"));
		transform.parent.BroadcastMessage("SetColor", color);
        transform.parent.BroadcastMessage("OnColorChange", color, SendMessageOptions.DontRequireReceiver);
    }

	void ApplyColor ()
	{
        render.sharedMaterial.SetColor ("_Color", color.ToColor());
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
