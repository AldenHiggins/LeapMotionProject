using UnityEngine;
using System.Collections;

public class TurretPlacementSpot : MonoBehaviour 
{
	public Material highlightMaterial;
	public Material normalMaterial;

	public void select()
	{
		transform.GetChild (0).gameObject.renderer.material = highlightMaterial;
		transform.GetChild (1).gameObject.renderer.material = highlightMaterial;
	}

	public void deSelect()
	{
		transform.GetChild (0).gameObject.renderer.material = normalMaterial;
		transform.GetChild (1).gameObject.renderer.material = normalMaterial;
	}
}
