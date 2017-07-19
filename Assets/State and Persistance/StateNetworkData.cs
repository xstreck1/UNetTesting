using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StateNetworkData : NetworkBehaviour {
    [SyncVar]
    public int sceneCount;

    public override void OnStartServer()
    {
        sceneCount = 0;
    }
}
