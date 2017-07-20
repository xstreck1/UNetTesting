using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkScene
{
    public class CameraColor : MonoBehaviour
    {
        public Color oddColor;
        public Color evenColor;

        public static CameraColor Singleton;

        void Start()
        {
            SetColor(PersistentObject.Singleton.networkData.sceneCount % 2 == 0);
        }

        void Update()
        {

        }

        public void SetColor(bool even)
        {
            Logger.Log("Color set to " + (even ? "even" : "odd"));
            GetComponent<Camera>().backgroundColor = even ? evenColor : oddColor;
        }
    }
}
