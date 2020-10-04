using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip hit;
    AudioSource aud;

    [SerializeField]
    protected float lowPitchRange = 1.02F;
    [SerializeField]
    protected float highPitchRange = 0.98F;
    [SerializeField]
    protected float lowVolRange = .8F;
    [SerializeField]
    protected float highVolRange = 1F;

    public static SoundManager instance;
    void Awake()
    {
        instance = this;
        aud = GetComponent<AudioSource>();
    }

    public static void PlayHit()
    {
        instance.PlaySound(instance.hit);
    }

    public void PlaySound(AudioClip sound)
    {
        aud.pitch = Random.Range(lowPitchRange, highPitchRange);
        if (sound != null)
            aud.PlayOneShot(sound, Random.Range(lowVolRange, highVolRange));
    }

    void Update()
    {
        
    }
}
