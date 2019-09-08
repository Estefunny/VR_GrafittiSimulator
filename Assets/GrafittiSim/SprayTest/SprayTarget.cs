using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains the function used to apply color to textures
/// makes an Object sprayable
/// </summary>
public class SprayTarget : MonoBehaviour {

    //current list of sprayTargets in the scene
    public static List<SprayTarget> sceneTargets = new List<SprayTarget>();

    //Object Material
    public Material material;
    //Object Mesh Renderer
    public MeshRenderer meshRenderer;

    //current texture (copy)
    private Texture2D tex;
    //original texture
    private Texture2D originalTex;

    //the maximum amount of undos
    private int undoStepCount = 5;
    //list of textures for the undos
    private LinkedList<Texture2D> undoSteps = new LinkedList<Texture2D>();

    //Spraying will not be performed if the raycast distance is above this value
    private float maximumDistance = 4;

    //values at maximum distance
    private float maximumRadius = 75;
    private float maximumCenterRadius = 75 * 0.5f;
    private float maximumAlpha = 0;

    //values at minimum distance
    private float minimumRadius = 25;
    private float minimumCenterRadius = 25 * 0.5f;
    private float minimumAlpha = 0.5f;

    //values to adjust individually for each SprayTarget depending on texture size
    public float radiusScale = 1;
    public float distanceScale = 1;
    public Vector2 dripDirection;

    //current amount of drips
    private int drips = 0;
    //how many drips you can have at a time
    private int maxDrips = 1;

    /// <summary>
    /// Called by drip object when tehy are destroyed
    /// </summary>
    public void removeDrip() {
        drips -= 1;
    }

    /// <summary>
    /// creates a drip object
    /// </summary>
    /// <param name="pos">texture position of the drip</param>
    /// <param name="c">drip color</param>
    public void createDrip(Vector2 pos, Color c) {
        drips++;
        gameObject.AddComponent<SprayDrip>().initialize(pos, this, c);
    }

    /// <summary>
    /// Returns the current texure
    /// </summary>
    /// <returns>current texure</returns>
    public Texture2D getTex() {
        return tex;
    }

    /// <summary>
    /// calls clearTexture for all current SprayTargets
    /// </summary>
    public static void clear() {
        foreach (SprayTarget t in sceneTargets) {
            t.clearTexture();
        }
    }

    /// <summary>
    /// restores the copied texture back to the original one
    /// </summary>
    private void clearTexture() {
        Graphics.CopyTexture(originalTex, tex);
    }

    /// <summary>
    /// calls saveTextureToList for all current SprayTargets
    /// </summary>
    public static void saveStep() {
        foreach (SprayTarget t in sceneTargets) {
            t.saveTextureToList();
        }
    }

    /// <summary>
    /// Copies the current texture into the undo list
    /// </summary>
    private void saveTextureToList() {
        if (undoStepCount > 0) {
            undoSteps.AddFirst(copyTexture(tex));
            if (undoSteps.Count > undoStepCount) {
                undoSteps.RemoveLast();
            }
        }
    }

    /// <summary>
    /// calls loadTextureToList for all current SprayTargets
    /// </summary>
    public static void undoStep() {
        foreach (SprayTarget t in sceneTargets) {
            t.loadTextureToList();
        }
    }

    /// <summary>
    /// if there are any textures in the undo list, replaces the current texture with the first one and removes it from the list
    /// </summary>
    private void loadTextureToList() {
        if (undoSteps.Count > 0) {
            tex = undoSteps.First.Value;
            undoSteps.RemoveFirst();

            meshRenderer.material.SetTexture("_MainTex", tex);
        }
    }

    /// <summary>
    /// Creates a copy of a texture
    /// </summary>
    /// <param name="t">the texture to copy</param>
    /// <returns>the copy of the texture</returns>
    private Texture2D copyTexture(Texture2D t) {
        Texture2D copy = new Texture2D(t.width, t.height);

        Graphics.CopyTexture(t, copy);

        return copy;
    }

    /// <summary>
    /// Creates a copy of a texture
    /// called once at the start
    /// </summary>
    /// <param name="t">the texture to copy</param>
    /// <returns>the copy of the texture</returns>
    private Texture2D copyTextureStart(Texture2D t)
    {
        Texture2D copy = new Texture2D(t.width, t.height);

        copy.SetPixels(t.GetPixels());
        copy.Apply();

        return copy;
    }

    /// <summary>
    /// Compares two colors to see if they are similar enough
    /// </summary>
    /// <param name="c1">Color 1</param>
    /// <param name="c2">Color 2</param>
    /// <returns>if the colors are very similar</returns>
    private bool colorEqual(Color c1, Color c2) {
        return Mathf.Abs(c1.r - c2.r) + Mathf.Abs(c1.g - c2.g) + Mathf.Abs(c1.b - c2.b) < 0.06f;
    }

    public void drawSpray(Vector2 center, Color c, float distance, float strength, float distanceMult, float radiusMult) {
        if (distance <= maximumDistance) {
            float distanceValue = distance / (maximumDistance * distanceMult);
            drawCircleOnTexture(tex, center,
                (int)(Mathf.Lerp(minimumRadius, maximumRadius, distanceValue) * radiusMult),
                (int)(Mathf.Lerp(minimumCenterRadius, maximumCenterRadius, distanceValue) * radiusMult),
                c,
                Mathf.Lerp(minimumAlpha, maximumAlpha, distanceValue) * strength);
            if (drips < maxDrips && Random.value > 0.9f) {
                int x = (int)(tex.width * center.x);
                int y = (int)(tex.height * center.y);

                if (colorEqual(tex.GetPixel(x, y), c)) {
                    createDrip(center + new Vector2(Random.Range(-1.0f, 1.0f), 1).normalized * Mathf.Lerp(minimumRadius, maximumRadius, distanceValue) * radiusMult * 0.8f / tex.height, c);
                }
            }
        }
    }

    /// <summary>
    /// Returns the current drawing Color
    /// if the Eraser is used, return pixel color from the original texture
    /// </summary>
    /// <param name="x">texture x</param>
    /// <param name="y">texture y</param>
    /// <param name="c">drawing color</param>
    /// <returns>color to draw with</returns>
    private Color getPixelColor(int x, int y, Color c) {
        if (c.a < 1) {
            return originalTex.GetPixel(x, y);
        }

        return c;
    }

    /// <summary>
    /// Draws a cirle on a texture
    /// </summary>
    /// <param name="t">the texture to draw on</param>
    /// <param name="center">texture position of the circle</param>
    /// <param name="radius">radius of the circle</param>
    /// <param name="centerRadius">radius from the center</param>
    /// <param name="c">drawing color</param>
    /// <param name="alpha">strength of the color</param>
    private void drawCircleOnTexture(Texture2D t, Vector2 center, int radius, int centerRadius, Color c, float alpha) {
        int x = (int)(t.width * center.x);
        int y = (int)(t.height * center.y);

        //check if circle is entirely within the texture
        if (x - radius >= 0 && y - radius >= 0 && t.width - x >= radius && t.height - y >= radius) {
            x -= radius;
            y -= radius;
            int diameter = 2 * radius;

            //get pixels
            Color[] colorArray = t.GetPixels(x, y, diameter, diameter);

            //apply new colors
            for (int i = 0; i < diameter; i++) {
                for (int j = 0; j < diameter; j++) {
                    float colorValue = Mathf.Clamp01((new Vector2(i - radius, j - radius).magnitude - centerRadius) / (radius - centerRadius));
                    colorValue = (1 - colorValue) * alpha;
                    colorArray[j * diameter + i] = Color.Lerp(colorArray[j * diameter + i], getPixelColor(i + x, j + y, c), colorValue);
                }
            }

            //set pixels
            t.SetPixels(x, y, diameter, diameter, colorArray);

            t.Apply();
        }

    }

    private void Start() {
        sceneTargets.Add(this);

        Material m = new Material(material);
        tex = copyTextureStart((Texture2D)m.GetTexture("_MainTex"));
        originalTex = copyTextureStart(tex);

        m.SetTexture("_MainTex", tex);

        meshRenderer.material = m;

        maximumRadius *= radiusScale;
        maximumCenterRadius *= radiusScale;
        minimumRadius *= radiusScale;
        minimumCenterRadius *= radiusScale;
        maximumDistance *= radiusScale * distanceScale;
    }

}
