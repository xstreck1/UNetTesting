using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCameraColor : MonoBehaviour {
    public Color oddColor;
    public Color evenColor;

	// Use this for initialization
	void Start ()
    {
        GetComponent<Camera>().backgroundColor = PersistentObject.Singleton.networkData.sceneCount % 2 == 1 ? oddColor : evenColor;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
