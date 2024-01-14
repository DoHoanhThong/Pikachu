using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnOnClick : MonoBehaviour
{
    public static System.Action isPause;
    [SerializeField] GameObject Pause_Panel;
    private void Start()
    {
        isPause += PauseOnClick;
    }
    public void PauseOnClick()
    {
        Pause_Panel.SetActive(true);
        PlaySound.instance.PlayClickSound();
    }
    public void ResumeOnClick()
    {
        Pause_Panel.SetActive(false);
        PlaySound.instance.PlayClickSound();
    }
}
