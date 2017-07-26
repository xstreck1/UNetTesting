using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

namespace Matchmaker
{
    public class StopMatchmaker : MonoBehaviour
    {
        public void Click()
        {
            NetworkManager.singleton.StopMatchMaker();
            Logger.Log("Stop Matchmaker");
        }
    }
}
