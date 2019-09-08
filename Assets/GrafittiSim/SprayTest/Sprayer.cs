using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains the function used to control the spraying of color
/// </summary>
public class Sprayer : MonoBehaviour {

    //currently spraying
    private bool spraying = false;
    //current color of all sprayers
    private static Color sprayColor = Color.blue;

    //Multiplier for the spray radius
    private static float radiusMult = 1;
    //Multiplier for the spray distance
    private static float distanceMult = 1;
    //Multiplier for the radius of the particle effect
    private static float partWidth = 1;
    //Multiplier for the length of the particle effect
    private static float partLength = 1;

    //reference to the particle effect
    public ParticleSystem particles;

    //saves the original LocalScale values of the particle effect
    private Vector3 particleTransform;

    //spray sound
    public AudioSource player;
    public AudioClip firstClip;

    //if the sprayer can spray (set to false when opening the color picker)
    private bool canSpray = true;

    /// <summary>
    /// Sets all current sprayers to be able to spray or not
    /// </summary>
    /// <param name="open">if the color picker is openend or closed</param>
    public static void openColorPicker(bool open)
    {
        if (open)
        {
            foreach (Sprayer s in FindObjectsOfType<Sprayer>())
            {
                s.stopSpray();
                s.canSpray = false;
            }
        } else
        {
            foreach (Sprayer s in FindObjectsOfType<Sprayer>())
            {
                s.canSpray = true;
            }
        }
    }

    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        updateParticleColor();
        particleTransform = particles.transform.localScale;
        updateParticleTransform();

        player.clip = firstClip;

    }

    /// <summary>
    /// Updates particle effect color to the Sprayer color.
    /// Eraser is shown as white
    /// </summary>
    private void updateParticleColor()
    {
        ParticleSystem.MainModule ma = particles.main;
        if (sprayColor.a == 0)
        {
            ma.startColor = new Color(0.8f, 0.8f, 0.8f, 0.5f);
        }
        else
        {
            ma.startColor = sprayColor;
        }
    }

    /// <summary>
    /// Sets all multipliers for the sprayers and updates their particle effect scale
    /// </summary>
    /// <param name="distance">Distance multiplier</param>
    /// <param name="radius">Radius multiplier</param>
    /// <param name="particleLength">Particle effect length multiplier</param>
    /// <param name="particleWidth">Particle effect width multiplier</param>
    public static void setSprayCap(float distance, float radius, float particleLength, float particleWidth)
    {
        distanceMult = distance;
        radiusMult = radius;
        partLength = particleLength;
        partWidth = particleWidth;

        foreach (Sprayer s in FindObjectsOfType<Sprayer>())
        {
            s.updateParticleTransform();
        }
    }

    /// <summary>
    /// Changes the particle effect LocalScale depending on the static multipliers
    /// </summary>
    public void updateParticleTransform() {
        particles.transform.localScale = new Vector3(particleTransform.x * partWidth, particleTransform.y * partWidth, particleTransform.z * partLength);
    }

    /// <summary>
    /// Sets the spray color and updates the particle effect color for any sprayers
    /// </summary>
    /// <param name="c">spray color</param>
    public static void setSprayColor(Color c) {
        sprayColor = c;
        foreach (Sprayer s in FindObjectsOfType<Sprayer>())
        {
            s.updateParticleColor();
        }
    }

    /// <summary>
    /// Called by the spray can
    /// handles starting and stopping the spray depending on spray strength and calls sprayStep
    /// </summary>
    /// <param name="origin">Raycast Origin</param>
    /// <param name="direction">RayCast Direction</param>
    /// <param name="sprayStrength">strength of the spray</param>
    public void sprayerUpdate(Vector3 origin, Vector3 direction, float sprayStrength) {
        if (!spraying && canSpray && sprayStrength > 0) {
            startSpray();
        }

        if (spraying) {
            if (sprayStrength == 0)
            {
                stopSpray();
            }
            else
            {
                sprayStep(origin, direction, sprayStrength);
                player.volume = sprayStrength;
                ParticleSystem.MainModule ma = particles.main;
                if (sprayColor.a == 0)
                {
                    ma.startColor = new Color(0.8f, 0.8f, 0.8f, 0.5f * sprayStrength);
                }
                else
                {
                    ma.startColor = new Color(ma.startColor.color.r, ma.startColor.color.g, ma.startColor.color.b, sprayStrength);
                }
            }
        }
    }

    /// <summary>
    /// Uses a raycast to check for collision and initiates drawing process if a SprayTarget is hit
    /// </summary>
    /// <param name="origin">Raycast Origin</param>
    /// <param name="direction">RayCast Direction</param>
    /// <param name="strength">strength of the spray</param>
    public void sprayStep(Vector3 origin, Vector3 direction, float strength) {
        RaycastHit ray;
        if (Physics.Raycast(origin, direction, out ray)) {
            SprayTarget t = ray.collider.gameObject.GetComponent<SprayTarget>();
            if (t != null) {
                t.drawSpray(ray.textureCoord, sprayColor, ray.distance, strength, distanceMult, radiusMult);
            }

        }
    }

    /// <summary>
    /// Saves a step for all SprayTargets to undo
    /// activates particle effect and sound
    /// </summary>
	public void startSpray() {
        spraying = true;
        SprayTarget.saveStep();
        player.Play();
        particles.Play();
    }

    /// <summary>
    /// deactivates particle effect and sound
    /// </summary>
    public void stopSpray() {
        spraying = false;
        player.Stop();
        particles.Stop();
    }

}
