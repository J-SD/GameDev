using UnityEngine;
using System.Collections;

public class music_play : MonoBehaviour {
    static bool AudioBegin = false;
    void Awake()
    {  
        if (!AudioBegin)
        {
            DontDestroyOnLoad(this.gameObject);
            AudioBegin = true;
        }
    }
}
