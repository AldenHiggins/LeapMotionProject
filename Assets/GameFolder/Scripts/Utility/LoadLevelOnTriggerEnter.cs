using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelOnTriggerEnter : MonoBehaviour
{
    [SerializeField]
    private string levelName;

    [SerializeField]
    private string unloadLevel;

    void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(unloadLevel);
    }
}
