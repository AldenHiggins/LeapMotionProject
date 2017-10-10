using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelOnStart : MonoBehaviour
{
    [SerializeField]
    private string levelName;

	// Use this for initialization
	void Start ()
    {
        // Only load the scene if it isn't already loaded, so far this is useful to keep the script enabled
        // during multi-scene editing and not have it doubly load scenes
        if (!SceneManager.GetSceneByName(levelName).isLoaded)
        {
            SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
        }
    }
}
