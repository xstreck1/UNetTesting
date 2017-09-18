using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;

public class QuickMatch : MonoBehaviour
{
    static NetworkClient myClient;
    static MatchInfo quickMatchInfo;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartQuickMatch()
    {
        NetworkManager.singleton.matchMaker.ListMatches(0, 10, "", false, 0, 0, OnInternetMatchList);
        Logger.Log("Searching for existing matches.");
    }

    public void StopQuickMatch()
    {

    }

    private void OnInternetMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
        if (success)
        {
            if (matches.Count != 0)
            {
                Logger.Log("Match found, connecting.");
                NetworkManager.singleton.matchMaker.JoinMatch((NetworkID)matches[0].networkId, "", "", "", 0, 0, OnJoinInternetMatch);
            }
            else
            {
                Logger.Log("No matches found, creating.");
                NetworkManager.singleton.matchMaker.CreateMatch("quickMatch", 2, true, "", "", "", 0, 0, OnInternetMatchCreate);
            }
        }
        else
        {
            Logger.Log("Failed to list matches on Quick Join. " + extendedInfo);
        }
    }

    //this method is called when your request to join a match is returned
    public void OnJoinInternetMatch(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            quickMatchInfo = matchInfo;
            myClient = NetworkManager.singleton.StartClient(matchInfo);
            Logger.Log("Match joined  " + quickMatchInfo.port + ", networkID: " + quickMatchInfo.networkId);
        }
        else
        {
            Logger.Log("Join match failed.\n" + extendedInfo);
        }
    }

    //this method is called when your request for creating a match is returned
    public void OnInternetMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        Logger.Log("Internet Match Created");
        if (success)
        {
            quickMatchInfo = matchInfo;
            NetworkManager.singleton.StartHost(matchInfo);
            Logger.Log("Listening on the port " + quickMatchInfo.port + ", networkID: " + quickMatchInfo.networkId);
            SceneManager.LoadScene("QuickMatchWait");
        }
        else
        {
            Logger.Log("Create match failed. " + extendedInfo);
        }
    }
}
