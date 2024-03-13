using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class BtnOnClick : MonoBehaviour
{
    public static System.Action isPause;
    public static System.Action isResume;
    [SerializeField] GameObject Pause_Panel;
    public void PauseOnClick()
    {
        if (GameController.instance.CD_click >= 0)
            return;
        GameController.instance.CD_click = 0.1f;
        isPause.Invoke();
        Pause_Panel.SetActive(true);
        PlaySound.instance.PlayClickSound();
    }
    public void ResumeOnClick()
    {
        if (GameController.instance.CD_click >= 0)
            return;
        GameController.instance.CD_click = 0.1f;
        isResume.Invoke();
        Pause_Panel.SetActive(false);
        PlaySound.instance.PlayClickSound();
    }
}
