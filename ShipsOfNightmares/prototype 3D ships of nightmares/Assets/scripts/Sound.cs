//Heavily based on https://www.youtube.com/watch?v=6OT43pvUyfY&t=652s&ab_channel=Brackeys

using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;

    public bool varyPitch;

    [Range(0.1f, 3f)]
    public float pitchMax;


    public bool loop;

    public string subtitle;

    [HideInInspector]
    public AudioSource source;
}
