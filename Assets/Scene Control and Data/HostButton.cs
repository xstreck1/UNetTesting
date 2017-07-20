using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkScene
{
    public class HostButton : MonoBehaviour
    {
        public void HostGame()
        {
            MyNetworkManager.singleton.StartHost();
        }
    }
}
