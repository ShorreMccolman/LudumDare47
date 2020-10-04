using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    public static DontDestroyOnLoad Instance;
    void Awake()
    {
        if (Instance != null)
            Destroy(this.gameObject);
        else
        {
            DontDestroyOnLoad(this);
        }
    }
}
