using UnityEngine;


public class TouchToActivateRound : MonoBehaviour
{
	public GameLogic game;
	public int index;
	public Material unSelected;
	public Material selected;

	void OnTriggerEnter(Collider other)
	{
		Renderer thisRenderer = gameObject.GetComponent<Renderer> ();
		thisRenderer.material = selected;
		print ("Entered the triggers");
		if (index == 1)
		{
			game.startRound1 = true;
		}
		else
		{
			game.startRound2 = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		Renderer thisRenderer = gameObject.GetComponent<Renderer> ();
		thisRenderer.material = unSelected;
		print ("Exited the triggers");
		if (index == 1)
		{
			game.startRound1 = false;
		}
		else
		{
			game.startRound2 = false;
		}
	}
}


