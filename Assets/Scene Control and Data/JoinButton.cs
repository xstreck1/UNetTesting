using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkScene
{
    public class JoinButton : MonoBehaviour
    {
        public void JoinGame()
        {
            MyNetworkManager.singleton.StartClient();
        }
    }
}