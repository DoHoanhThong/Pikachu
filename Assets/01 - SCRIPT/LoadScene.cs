using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScene : MonoBehaviour
{
    public void Load_Scene(int scenceId)
    {
        SceneManager.LoadScene(scenceId);
    }
}
