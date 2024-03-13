using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScene : MonoBehaviour
{
    public void Load_Scene(int scenceId)
    {
        PlaySound.instance.PlayClickSound();
        SceneManager.LoadScene(scenceId);
    }
    public void Load_SceneWithContinue(int scenceId)
    {
        GameManager.instance.isContinue = true;
        Load_Scene(scenceId);
    }
    public void Load_SceneToMenu(int scenceId)
    {
        GameController.onHomeClick?.Invoke();
        GameManager.instance.isExistData = true;
        Load_Scene(scenceId);
    }
}
