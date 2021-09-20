using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{

    [SerializeField][Range(0,1)] float masterVolume = 0.5f;

    AudioSource source;
    AudioSource papSource;

    private void Awake()
    {
        source = gameObject.AddComponent<AudioSource>();
        papSource = gameObject.AddComponent<AudioSource>();
    }
    public void PlaySound(AudioClip clip, float addedVolume)
    {
        source.pitch = 1;
        source.PlayOneShot(clip, masterVolume + addedVolume);
    }

    public void PlayPAPSound(AudioClip clip, float addedVolume, float pitch)
    {
        papSource.pitch = pitch;
        papSource.PlayOneShot(clip, masterVolume + addedVolume);
    }
}
