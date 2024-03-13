using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    [SerializeField] GameObject Music_On;
    [SerializeField] GameObject Music_Off;
    [SerializeField] GameObject Sound_On;
    [SerializeField] GameObject Sound_Off;
    AudioSource MS;
    AudioSource Sound;
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        Sound = PlaySound.instance.audioSource;
        MS = PlaySound.instance.transform.GetChild(0).GetComponent<AudioSource>();
        if (!PlayerPrefs.HasKey("MUSIC"))
        {
            PlayerPrefs.SetInt("MUSIC", 1);
        }
        if (!PlayerPrefs.HasKey("SOUND"))
        {
            PlayerPrefs.SetInt("SOUND", 1);
        }
        Checking("SOUND",1);
        Checking("MUSIC",0.8f);
    }
    void Checking(string name, float volume)
    {
        AudioSource g = (name == "MUSIC") ? MS : Sound;
        GameObject on = (name == "MUSIC") ? Music_On : Sound_On;
        GameObject off = (name == "MUSIC") ? Music_Off : Sound_Off;

        on.SetActive((PlayerPrefs.GetInt(name) == 1) ? true : false);
        off.SetActive((PlayerPrefs.GetInt(name) == 1) ? false : true);
        g.volume = (PlayerPrefs.GetInt(name) == 1) ? volume : 0;
    }
    public void OnClickSound()
    {
        PlayerPrefs.SetInt("SOUND", (PlayerPrefs.GetInt("SOUND") == 1) ? 0 : 1);
        Checking("SOUND",1);
        PlaySound.instance.PlayClickSound();
    }
    public void OnClickMusic()
    {
        PlayerPrefs.SetInt("MUSIC", (PlayerPrefs.GetInt("MUSIC") == 1) ? 0 : 1);
        Checking("MUSIC",0.8f);
        PlaySound.instance.PlayClickSound();
    }
}
