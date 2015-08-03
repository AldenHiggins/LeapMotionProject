using UnityEngine;
using System.Collections;

public class LoadLevelOnButtonPress : MonoBehaviour
{

	public int levelToLoad;
	public ButtonDemoGraphics button;
	

	// Update is called once per frame
	void Update () 
	{
		if (button.gameObject.GetComponentInChildren<Renderer>().enabled)
			Application.LoadLevel (levelToLoad);
	}
}
