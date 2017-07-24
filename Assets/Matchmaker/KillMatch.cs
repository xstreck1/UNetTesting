using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

namespace Matchmaker
{

    public class KillMatch : MonoBehaviour
    {
        public Text domain;

        public void Click()
        {
            NetworkManager.singleton.matchMaker.DestroyMatch(MyNetworkManager.matchNetID, Int32.Parse(domain.text), OnMatchDestroy);
            Logger.Log("Kill Match");
        }

        private void OnMatchDestroy(bool success, string extendedInfo)
        {
            if (success)
            {
                Logger.Log("Match destroyed.");
                MyNetworkManager.matchNetID = (ulong) 0;
            }
            else
            {
                Logger.Log("Match destroy failed.\n" + extendedInfo);
            }
        }
    }
}
