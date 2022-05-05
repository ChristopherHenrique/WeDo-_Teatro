using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseManager<T> : MonoBehaviour where T : MonoBehaviour
{

    protected static T _instance;

    public static T Instance
    {
        get { return _instance; }
        set
        {
            if (_instance == null)
            {
                _instance = value;
                DontDestroyOnLoad(_instance.gameObject);
            }
        }
    }

    protected virtual void Awake()
    {
        transform.SetParent(null);
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this as T;

    }
}
