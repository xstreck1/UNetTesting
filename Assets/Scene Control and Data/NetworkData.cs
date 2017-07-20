using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace NetworkScene
{
    public class NetworkData : NetworkBehaviour
    {
        [SyncVar(hook = "ChangeColor")]
        public int sceneCount;

        public override void OnStartServer()
        {
            sceneCount = 0;
        }

        void ChangeColor(int newSceneCount)
        {
            Camera.main.GetComponent<CameraColor>().SetColor(newSceneCount % 2 == 0);
        }
    }
}