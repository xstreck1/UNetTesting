using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class StateServerPanel : NetworkBehaviour {

    public override void OnStartClient()
    {
        if (!isServer)
        {
            gameObject.SetActive(false);
        }
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void ChangeScene()
    {
        PersistentObject.Singleton.networkData.sceneCount += 1;
        StateNetworkManager.singleton.ServerChangeScene(SceneManager.GetActiveScene().name);
    }

    public void StopHost()
    {
        StateNetworkManager.singleton.StopHost();
    }
}
