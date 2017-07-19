using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StateClientPanel : NetworkBehaviour
{
    public Text sceneNoText;
    
    public override void OnStartClient()
    {
        sceneNoText.text = "Scene Count: " + PersistentObject.Singleton.networkData.sceneCount;
    }

    public void StopClient()
    {
        if (!isServer)
        {
            StateNetworkManager.singleton.StopClient();
        }
    }
}
