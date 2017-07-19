using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Logger : MonoBehaviour {

    public static Logger Singleton;
    Text text;
    

    private void Awake()
    {
        Singleton = this;
        text = GetComponent<Text>();
    }

    static public void Log(string text)
    {
        Singleton.text.text += DateTime.UtcNow.ToString("HH:mm:ss.ffff") + " " + text + "\n";
    }
}
