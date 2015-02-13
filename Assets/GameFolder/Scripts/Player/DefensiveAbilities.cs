using UnityEngine;
using System.Collections;

public class DefensiveAbilities : MonoBehaviour
{
	// ATTACKS
	public GameObject clapProjectile;
	// TURRETS
	public GameObject cTurret;
	public GameObject turretPositions;
	// OIL MATERIALS
	public GameObject oilSlick;
	// GAME CONTROLLER VARIABLES
	private bool previousXDown = false;
	private bool previousTurretButtonPressed = false;
	private bool previousAoEButtonPressed = false;
	// GAME LOGIC
	private GameLogic game;

	// Use this for initialization
	void Start () 
	{
		game = (GameLogic) gameObject.GetComponent (typeof(GameLogic));
	}
	
	// Update is called once per frame
	public void controlCheck ()
	{
		//<--------------------- CONTROLLER X TO DROP OIL SLICK ----------------------->
		//OilSlick Projectile
		bool xPressed = OVRGamepadController.GPC_GetButton (OVRGamepadController.Button.X);
		
		// Create projectile
		if (xPressed && !previousXDown) 
		{
			oilSlick = (GameObject)Instantiate (oilSlick);
		}
		// Animate projectile in front of player
		else if (xPressed) 
		{
			RaycastHit hit = game.getRayHit ();
			oilSlick.transform.position = hit.point;
			//oilSlick.transform.rotation Quaternion.identity);
		}
		// Fire projectile
		else if (!xPressed && previousXDown) 
		{
			RaycastHit hit = game.getRayHit ();
			oilSlick.transform.position = hit.point;
			OilSlick oilSlickScript = (OilSlick)oilSlick.GetComponent (typeof(OilSlick));
			oilSlickScript.enabled = true;
			print ("placed it at " + hit.point.y);
		}
		
		previousXDown = xPressed;
		
		
		//<--------------------- B to create explosion ------------------------------->	
		if (OVRGamepadController.GPC_GetButton (OVRGamepadController.Button.B) && !previousAoEButtonPressed) 
		{
			RaycastHit hit = game.getRayHit();
			Instantiate (clapProjectile, hit.point + new Vector3 (0.0f, 2.0f, 0.0f), Quaternion.identity);
		}
		
		//<--------------------- Y to place turret ------------------------------->
		if (OVRGamepadController.GPC_GetButton (OVRGamepadController.Button.Y)) 
		{
			// Display prospective turret spots
			showHideTurretPositions (true);
		} 
		else if (!OVRGamepadController.GPC_GetButton (OVRGamepadController.Button.Y) && previousTurretButtonPressed) 
		{
			showHideTurretPositions (false);

			RaycastHit hit = game.getRayHit();
			float minDistance = float.MaxValue;
			GameObject closestTurret = null;
			for (int i = 0; i < turretPositions.transform.childCount; i++) 
			{
				GameObject turretPos = turretPositions.transform.GetChild (i).gameObject;
				float distance = Vector3.Distance (turretPos.transform.position, hit.point);
				if (distance < minDistance) 
				{
					minDistance = distance;
					closestTurret = turretPos;
				}
			}
			
			if (closestTurret != null) 
			{
				Instantiate (cTurret, closestTurret.transform.position, closestTurret.transform.rotation);
				Destroy (closestTurret);
			}
			else 
			{
				print ("Could not place a turret, no more locations available!");
			}
		}
		previousAoEButtonPressed = OVRGamepadController.GPC_GetButton (OVRGamepadController.Button.B);
		previousTurretButtonPressed = OVRGamepadController.GPC_GetButton (OVRGamepadController.Button.Y);
	}



	public void showHideTurretPositions(bool showOrHide)
	{
		// Don't display any of the turret positions
		for (int i = 0; i < turretPositions.transform.childCount; i++)
		{
			GameObject turretPos = turretPositions.transform.GetChild (i).gameObject;
			turretPos.transform.GetChild (0).gameObject.renderer.enabled = showOrHide;
			turretPos.transform.GetChild (1).gameObject.renderer.enabled = showOrHide;
		}
	}
}



