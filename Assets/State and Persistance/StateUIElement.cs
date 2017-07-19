using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateUIElement : MonoBehaviour {

	public void HostGame()
    {
        StateNetworkManager.singleton.StartHost();
    }

    public void JoinGame()
    {
        StateNetworkManager.singleton.StartClient();
    }

    public void ChangeScene(Object scene)
    {
        StateNetworkManager.singleton.ServerChangeScene(scene.name);
    }
}
