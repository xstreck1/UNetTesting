using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateJoinButton : MonoBehaviour
{
    public void JoinGame()
    {
        StateNetworkManager.singleton.StartClient();
    }
}
