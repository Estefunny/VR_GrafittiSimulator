using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprayer : MonoBehaviour {

    private bool spraying = false;
    private Color sprayColor = Color.blue;

    private float radiusMult = 1;
    private float distanceMult = 1;
    public AudioSource player;
    public AudioClip firstClip;

    public ParticleSystem particles;


    // Start is called before the first frame update
    void Start()
    {
        player.clip = firstClip;
        player.loop = true;

    }

    public void setMultipliers(float distance, float radius) {
        distanceMult = distance;
        radiusMult = radius;
    }

    public void setSprayColor(Color c) {
        sprayColor = c;
    }

    public void sprayerUpdate(Vector3 origin, Vector3 direction, float sprayStrength) {
        if (!spraying && sprayStrength > 0) {
            startSpray();
        }

        if (spraying) {
            if (sprayStrength == 0) {
                stopSpray();
                particles.Stop();
                player.Stop();
            } else {
                sprayStep(origin, direction, sprayStrength);
                particles.Play();
                player.Play();
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
