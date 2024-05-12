using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public bool isDontDestroy;
    private static T _instance;
    public static T instance => _instance;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this.GetComponent<T>();
            if (isDontDestroy)
            {
                DontDestroyOnLoad(this);
            }
            
            return;
        }
        if (_instance.gameObject.GetInstanceID() != this.gameObject.GetInstanceID())
        {
            Destroy(this.gameObject);
        }
    }
}
