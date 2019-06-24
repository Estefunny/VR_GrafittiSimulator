using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayTarget : MonoBehaviour {

    public static List<SprayTarget> sceneTargets = new List<SprayTarget>();
    public Material material;
    public MeshRenderer meshRenderer;

    private Texture2D tex;
    private Texture2D originalTex;

    private int undoStepCount = 0;
    private LinkedList<Texture2D> undoSteps = new LinkedList<Texture2D>();

    private float maximumDistance = 4;

    private float maximumRadius = 75;
    private float maximumCenterRadius = 75 * 0.5f;
    private float maximumAlpha = 0;

    private float minimumRadius = 25;
    private float minimumCenterRadius = 25 * 0.5f;
    private float minimumAlpha = 0.5f;

    public float radiusScale = 1;

    public static void saveStep() {
        foreach (SprayTarget t in sceneTargets) {
            t.saveTextureToList();
        }
    }

    private void saveTextureToList() {
        if (undoStepCount > 0) {
            undoSteps.AddFirst(copyTexture(tex));
            if (undoSteps.Count > undoStepCount) {
                undoSteps.RemoveLast();
            }
        }
    }

    public static void undoStep() {
        foreach (SprayTarget t in sceneTargets) {
            t.loadTextureToList();
        }
    }

    private void loadTextureToList() {
        if (undoSteps.Count > 0) {
            tex = undoSteps.First.Value;
            undoSteps.RemoveFirst();

            meshRenderer.material.SetTexture("_MainTex", tex);
        }
    }

    private Texture2D copyTexture(Texture2D t) {
        Texture2D copy = new Texture2D(t.width, t.height);

        copy.SetPixels(t.GetPixels());
        copy.Apply();

        return copy;
    }

    public void drawSpray(Vector2 center, Color c, float distance, float strength) {
        if (distance <= maximumDistance) {
            float distanceValue = distance / maximumDistance;
            drawCircleOnTexture(tex, center,
                (int)Mathf.Lerp(minimumRadius, maximumRadius, distanceValue),
                (int)Mathf.Lerp(minimumCenterRadius, maximumCenterRadius, distanceValue),
                c,
                Mathf.Lerp(minimumAlpha, maximumAlpha, distanceValue) * strength);
        }
    }

    private Color getPixelColor(int x, int y, Color c) {
        if (c.a < 1) {
            return originalTex.GetPixel(x, y);
        }

        return c;
    }

    private void drawCircleOnTexture(Texture2D t, Vector2 center, int radius, int centerRadius, Color c, float alpha) {
        int x = (int)(t.width * center.x);
        int y = (int)(t.height * center.y);

        if (x - radius >= 0 && y - radius >= 0 && t.width - x >= radius && t.height - y >= radius) {
            x -= radius;
            y -= radius;
            int diameter = 2 * radius;
            Color[] colorArray = t.GetPixels(x, y, diameter, diameter);

            for (int i = 0; i < diameter; i++) {
                for (int j = 0; j < diameter; j++) {
                    float colorValue = Mathf.Clamp01((new Vector2(i - radius, j - radius).magnitude - centerRadius) / (radius - centerRadius));
                    colorValue = (1 - colorValue) * alpha;
                    colorArray[j * diameter + i] = Color.Lerp(colorArray[j * diameter + i], getPixelColor(i + x, j + y, c), colorValue);
                }
            }

            t.SetPixels(x, y, diameter, diameter, colorArray);

            t.Apply();
        }

    }

    private void Start() {
        sceneTargets.Add(this);

        Material m = new Material(material);
        tex = copyTexture((Texture2D)m.GetTexture("_MainTex"));
        originalTex = copyTexture(tex);

        m.SetTexture("_MainTex", tex);

        meshRenderer.material = m;

        maximumRadius *= radiusScale;
        maximumCenterRadius *= radiusScale;
        minimumRadius *= radiusScale;
        minimumCenterRadius *= radiusScale;
        maximumDistance *= radiusScale;
    }

}
