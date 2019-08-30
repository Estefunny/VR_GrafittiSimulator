using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprayer : MonoBehaviour {

    private bool spraying = false;
    private Color sprayColor = Color.blue;

    private float radiusMult = 1;
    private float distanceMult = 1;

    public ParticleSystem particles;

    private Vector3 particleTransform;

    private void Start()
    {
        updateParticleColor();
        particleTransform = particles.transform.localScale;
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

    public void setSprayColor(Color c) {
        sprayColor = c;
        updateParticleColor();
    }

    public void sprayerUpdate(Vector3 origin, Vector3 direction, float sprayStrength) {
        if (!spraying && sprayStrength > 0) {
            startSpray();
        }

        if (spraying) {
            if (sprayStrength == 0) {
                stopSpray();
                particles.Stop();
            } else {
                sprayStep(origin, direction, sprayStrength);
                particles.Play();
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
    }

    public void stopSpray() {
        spraying = false;
    }

}
