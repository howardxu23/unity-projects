//Heavily based on https://www.youtube.com/watch?v=6OT43pvUyfY&t=652s&ab_channel=Brackeys

using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public bool subtitles;
    public Sound[] sounds;

    public static AudioManager instance;
    private Text subtitleUI;

    void Awake()
    {
        // Ensure only one audiomanager ever exists while game is running. ~ SK
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);// Protect AudioManager object from being reset on loading a new scene. ~ SK

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

            s.source.loop = s.loop;
        }

        subtitleUI = GameObject.Find("SubtitleText").GetComponent<Text>();
        subtitleUI.text = "";// Clear subtitle text field. ~ SK
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);// Find sound based on name in AudioManager editor. ~ SK

        // If there is no sound logged with that name reference, don't play anything. ~ SK
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found! Can't play.");
            return;
        }

        if (s.varyPitch && s.pitchMax >= 0.1f)// If varyPitch is true AND pitchMax has even been set (0f if not set). ~ SK
        {
            s.source.pitch = Random.Range(s.pitch, s.pitchMax);
        }
        s.source.Play();// Play the found sound. ~ SK

        if (subtitles)
        {
            if (s.subtitle == "") Debug.LogWarning(name + " subtitle missing!");
            else if (s.subtitle != "null") subtitleUI.text += "\n" + s.subtitle;// Display the subtitle of the sound being played. ~ SK
        }
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, item => item.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found! Can't stop.");
            return;
        }

        //s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        //s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        s.source.Stop();
    }

    public void Mute(bool mute)
    {
        Component[] audioSources = gameObject.GetComponents(typeof(AudioSource));

        if (mute)
        {
            foreach (AudioSource a in audioSources)
            {
                a.volume = 0f;
            }
        }
        else
        {
            int soundNum = 0;
            foreach (AudioSource a in audioSources)
            {
                a.volume = sounds[soundNum].volume;
                soundNum++;
            }
        }
    }
}
