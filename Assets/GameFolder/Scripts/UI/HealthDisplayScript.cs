using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthDisplayScript : MonoBehaviour 
{
	public PlayerLogic player;
	
	private Text txt;
	// Use this for initialization
	void Start () 
	{
		txt = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		txt.text = "Health: " + player.getHealth ();
	}
}
