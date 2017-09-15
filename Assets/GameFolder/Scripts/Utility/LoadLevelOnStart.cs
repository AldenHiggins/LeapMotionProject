using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevelOnStart : MonoBehaviour
{
    [SerializeField]
    private string levelName;

	// Use this for initialization
	void Start ()
    {
        Application.LoadLevelAdditive(levelName);	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
