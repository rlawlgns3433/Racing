using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadingScreen : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
