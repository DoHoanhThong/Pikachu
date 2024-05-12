using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
public class BtnOnClick : MonoBehaviour
{
    public static System.Action isPause;
    public static System.Action isResume;
    [SerializeField] GameObject Pause_Panel;

    [SerializeField] Button _change;
    [SerializeField] Button _hint;

    private void Awake()
    {
        //if (_change != null)
        //{
        //    _change.onClick.AddListener(() =>
        //    {
        //        if (PlayerPrefs.GetInt("Changes") == 0)
        //        {
        //            _change.interactable = false;
        //            GameManager.instance.isIAP = true;
        //        }


        //    });

        //}

        //if (_hint != null)
        //{
        //    _hint.onClick.AddListener(() =>
        //    {
        //        if (PlayerPrefs.GetInt("Hint") == 0)
        //        {
        //            _hint.interactable = false;
        //            GameManager.instance.isIAP = true;
        //        }

        //    });

        //}


    }
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
