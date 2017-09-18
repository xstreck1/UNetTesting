using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

namespace Matchmaker
{
    public class ListMatches : MonoBehaviour
    {
        public void FindInternetMatch()
        {
            NetworkManager.singleton.matchMaker.ListMatches(0, 10, "", false, 0, 0, OnInternetMatchList);
        }
        
        private void OnInternetMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
        {
            if (success)
            {
                if (matches.Count != 0)
                {
                    for (int i = 0; i < matches.Count; i++)
                    {
                        string info = "Name: " + matches[i].name + ", networkID: " + matches[i].networkId;
                        Logger.Log("Match " + i + ": " + info);

                    }      
                }
                else
                {
                    Logger.Log("No matches found.");
                }
            }
            else
            {
                Logger.Log("Failed to list matches. " + extendedInfo);
            }
        }
    }
}
