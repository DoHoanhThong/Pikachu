using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlaySound : Singleton<PlaySound>
{
    public Sound[] sounds;
    public AudioSource audioSource;
    public void PlaySFX(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            audioSource.PlayOneShot(s.clip, s.volume);
        }
    }
    public void PlayClickSound()
    {
        PlaySFX("CLICK");
    }
    public void PlayFailSound()
    {
        PlaySFX("BAD");
    }
    public void PlayRightSound()
    {
        PlaySFX("GOOD");
    }
    public void PlayGOverSound()
    {
        PlaySFX("OVER");
    }
    public void PlayRandomSound()
    {
        PlaySFX("RANDOM");
    }

}
