using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{

    [SerializeField][Range(0,1)] float masterVolume = 0.5f;
    public void PlaySound(AudioClip clip, float volume)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.volume = masterVolume + volume;
        source.clip = clip;
        source.Play();
        Destroy(source, source.clip.length + 1);
    }
}
