using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateHostButton : MonoBehaviour
{
    public void HostGame()
    {
        StateNetworkManager.singleton.StartHost();
    }
 }
