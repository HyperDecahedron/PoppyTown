using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonUI : MonoBehaviour
{
    private static SingletonUI instance;

    private void Awake()
    {
        // Singleton check to prevent duplicates
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // An object already exists, destroy this one
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
