using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesManager : MonoBehaviour
{
    static ParticlesManager instance;
    ParticleSystem partS;
    void Awake()
    {
        instance = this;
        partS = GetComponent<ParticleSystem>();
    }

    public static void Explode(Vector3 position)
    {
        instance.transform.position = position;
        instance.partS.Clear();
        instance.partS.Play();
    }
}
