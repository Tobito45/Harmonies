using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceDontDestroy : MonoBehaviour
{
    private static AudioSourceDontDestroy _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
