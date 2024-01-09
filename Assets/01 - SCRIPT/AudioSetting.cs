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
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.DeleteAll();
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
        Music_On.SetActive( (PlayerPrefs.GetInt("MUSIC")==1) ? true : false);
        Music_Off.SetActive( (PlayerPrefs.GetInt("MUSIC") == 1) ? false : true);

        Sound_On.SetActive( (PlayerPrefs.GetInt("MUSIC") == 1) ? true : false);
        Sound_Off.SetActive((PlayerPrefs.GetInt("MUSIC") == 1) ? false : true);
    }
    public void OnClickSound()
    {
        PlayerPrefs.SetInt("SOUND", (PlayerPrefs.GetInt("SOUND") == 1) ? 0 : 1);
        Sound_On.SetActive((PlayerPrefs.GetInt("SOUND") == 1) ? true : false);
        Sound_Off.SetActive((PlayerPrefs.GetInt("SOUND") == 1) ? false : true);
        Sound.volume = (PlayerPrefs.GetInt("SOUND") == 1) ? 1 : 0;
        PlaySound.instance.PlayClickSound();
    }
    public void OnClickMusic()
    {
        PlayerPrefs.SetInt("MUSIC", (PlayerPrefs.GetInt("MUSIC") == 1) ? 0 : 1);
        Music_On.SetActive((PlayerPrefs.GetInt("MUSIC") == 1) ? true : false);
        Music_Off.SetActive((PlayerPrefs.GetInt("MUSIC") == 1) ? false : true);
        MS.volume = (PlayerPrefs.GetInt("MUSIC") == 1) ? 1 : 0;
        PlaySound.instance.PlayClickSound();
    }
}
