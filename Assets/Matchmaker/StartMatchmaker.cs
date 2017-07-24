using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

namespace Matchmaker
{
    public class StartMatchmaker : MonoBehaviour
    {
        public void Click()
        {
            NetworkManager.singleton.StartMatchMaker();
            Logger.Log("Start Matchmaker");
        }
    }
}
