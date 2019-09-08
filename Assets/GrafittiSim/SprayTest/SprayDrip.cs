using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used to draw drips on SprayTargets
/// </summary>
public class SprayDrip : MonoBehaviour {

    //The SprayTargetto paint on
    private SprayTarget target;

    //Position on the texture
    private Vector2 dripPos;

    //Delete Object after this timer
    private float dripTime = 1;

    //Color of the drip
    private Color dripColor;

    /// <summary>
    /// Sets all the necessary values of the drip. used after creation.
    /// </summary>
    /// <param name="origin">the position on the texture where the drip starts</param>
    /// <param name="t">the SprayTarget</param>
    /// <param name="c">the Color of the Drip</param>
    public void initialize(Vector2 origin, SprayTarget t, Color c) {
        dripPos = origin;
        target = t;
        dripColor = c;
        dripTime = Random.Range(0.5f, 1);
    }

    private void FixedUpdate() {

        //Draw on the target and move the drip position down
        target.drawSpray(dripPos, dripColor, 0, Mathf.Clamp01(dripTime), 1, 0.3f);
        dripPos -= target.dripDirection * 2 * target.radiusScale / target.getTex().height;

        //delete drip after certain time
        dripTime -= Time.fixedDeltaTime;
        if (dripTime <= 0) {
            target.removeDrip();
            Destroy(this);
        }
    }

}
