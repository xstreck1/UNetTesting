using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentObject : MonoBehaviour {
    public static PersistentObject Singleton { get; private set; }
    public int SceneCount { get; private set; }

    private void Awake()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Singleton = this;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }


    // Use this for initialization
    void Start ()
    {
        SceneCount = 0;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneCount += 1;
    }

}
