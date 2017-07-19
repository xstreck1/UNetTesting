using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIElement : MonoBehaviour {

	public void SetIP(string ip)
    {
        Logger.Log("Set IP " + ip);
    }

    public void SetPort(string port)
    {
        Logger.Log("Set Port " + Int32.Parse(port));
    }
}
