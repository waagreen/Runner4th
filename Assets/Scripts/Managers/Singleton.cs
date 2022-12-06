using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    //private instance
    private static T _instance;
    private static object _lock = new object();
    private static bool applicationIsQuitting = false;

    //public property 
    public static T Instance 
    {
        get 
        {
            if(applicationIsQuitting)
            {
                return null;
            }
            
            lock (_lock)
            {
                if(_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if(_instance == null) 
                    {
                        GameObject newGo = new GameObject("DataManager");
                        _instance = newGo.AddComponent<T>();
                        DontDestroyOnLoad(newGo);
                    }
                }
            }
            
            return _instance;
        }
    }

    protected virtual void Awake() 
    {
        _instance = this as T;
        applicationIsQuitting = false;
    }

    private void OnDestroy()
    {
        applicationIsQuitting = true;
    }
}
