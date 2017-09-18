using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;

namespace Matchmaker
{
    public class MyNetworkManager : NetworkManager
    {
        public static NetworkID matchNetID = (ulong) 0;
        const int clientReq = 2;
        private int clientCount = 0;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void OnServerConnect(NetworkConnection conn)
        {
            Logger.Log("Client connected.");
            if (++clientCount >= clientReq)
            {
                StartCoroutine(ChangeToGame());
            }
        }

        public IEnumerator ChangeToGame()
        {
            Logger.Log("Starting in 3.");
            yield return new WaitForSeconds(1f);
            Logger.Log("Starting in 2.");
            yield return new WaitForSeconds(1f);
            Logger.Log("Starting in 1.");
            yield return new WaitForSeconds(1f);
            singleton.ServerChangeScene("QuickMatchOnline");
            yield return null;
        }
    }
}
