using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

namespace Matchmaker
{
    public class NewMatch : MonoBehaviour
    {
        public Text matchName;
        public Text playerCount;
        public Text domain;

        public void Click()
        {
            if (MyNetworkManager.matchNetID == 0)
            {
                NetworkManager.singleton.matchMaker.CreateMatch(matchName.text, UInt32.Parse(playerCount.text), true, "", "", "", 0, Int32.Parse(domain.text), OnInternetMatchCreate);
                Logger.Log("Creating the game " + matchName.text);
            }
        }

        //this method is called when your request for creating a match is returned
        private void OnInternetMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
        {
            if (success)
            {
                Logger.Log("Internet Match Created. " + matchInfo.ToString());
                MyNetworkManager.matchNetID = matchInfo.networkId;
                NetworkServer.Listen(matchInfo, 9000);
            }
            else
            {
                Logger.Log("Create match failed.\n" + extendedInfo);
            }
        }
    }
}
