using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<T>();
                _instance.Initialize();
            }
            return _instance;
        }
    }
    protected static T _instance;

    // Start is called before the first frame update
    void Start()
    {
        if (_instance == null)
        {
            _instance = (T)this;
            Initialize();
        }
        else if (_instance != this)
        {
            Debug.LogError($"Cannot have multiple {this.GetType().Name} objects in one scene.");
            Destroy(this);
        }
    }

    protected virtual void Initialize()
    {

    }
}
