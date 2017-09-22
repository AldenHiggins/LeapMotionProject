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
        SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
	}
}
