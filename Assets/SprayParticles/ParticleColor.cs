using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleColor : MonoBehaviour
{
    public ParticleSystem particles;
    public ColorManager cm;

    void changeColor()
    {
        particles.startColor = cm.color;
    }
}
