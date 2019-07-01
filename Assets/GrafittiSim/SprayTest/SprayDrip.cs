using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayDrip : MonoBehaviour {

    private SprayTarget target;
    private Vector2 dripPos;
    private float dripTime = 1;
    private Color dripColor;

    public void initialize(Vector2 origin, SprayTarget t, Color c) {
        dripPos = origin;
        target = t;
        dripColor = c;
        dripTime = Random.Range(0.5f, 1);
    }

    private void FixedUpdate() {

        target.drawSpray(dripPos, dripColor, 0, Mathf.Clamp01(dripTime), 1, 0.3f);
        dripPos -= target.dripDirection * 2 * target.radiusScale / target.getTex().height;

        dripTime -= Time.fixedDeltaTime;
        if (dripTime <= 0) {
            target.removeDrip();
            Destroy(this);
        }
    }





}
