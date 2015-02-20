using UnityEngine;
using System.Collections;

public class HideOnGameStart : MonoBehaviour
{
	// Use this for initialization
	void Start () 
	{
		this.gameObject.SetActive (false);
	}
}
