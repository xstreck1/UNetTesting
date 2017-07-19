using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateSceneNo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Text>().text = "Scene No " + PersistentObject.Singleton.SceneCount;
    }
}
