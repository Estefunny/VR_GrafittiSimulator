using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprayer : MonoBehaviour {

    private bool spraying = false;
    private static Color sprayColor = Color.blue;

    private float radiusMult = 1;
    private float distanceMult = 1;

    public ParticleSystem particles;

    private Vector3 particleTransform;

    public AudioSource player;
    public AudioClip firstClip;

    private bool canSpray = true;

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

    private void Start()
    {
        updateParticleColor();
        particleTransform = particles.transform.localScale;

        player.clip = firstClip;

    }

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

    public void setMultipliers(float distance, float radius) {
        distanceMult = distance;
        radiusMult = radius;

        particles.transform.localScale = new Vector3(particleTransform.x * radiusMult, particleTransform.y * radiusMult, particleTransform.z * distanceMult);
    }

    public static void setSprayColor(Color c) {
        sprayColor = c;
        foreach (Sprayer s in FindObjectsOfType<Sprayer>())
        {
            s.updateParticleColor();
        }
    }

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

    public void sprayStep(Vector3 origin, Vector3 direction, float strength) {
        RaycastHit ray;
        if (Physics.Raycast(origin, direction, out ray)) {
            SprayTarget t = ray.collider.gameObject.GetComponent<SprayTarget>();
            if (t != null) {
                t.drawSpray(ray.textureCoord, sprayColor, ray.distance, strength, distanceMult, radiusMult);
            }

        }
    }

	public void startSpray() {
        spraying = true;
        SprayTarget.saveStep();
        player.Play();
        particles.Play();
    }

    public void stopSpray() {
        spraying = false;
        player.Stop();
        particles.Stop();
    }

}
