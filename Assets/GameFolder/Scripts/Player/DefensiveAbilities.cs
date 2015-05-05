using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DefensiveAbilities : MonoBehaviour
{
	// PLAYER OBJECT
	public PlayerLogic player;
	public GameObject thisCamera;
	// ATTACKS
	public GameObject clapProjectile;
	public AAttack placeOilSlickAttack;
	public AAttack placeTurretAttack;
	// TURRETS
	public GameObject cTurret;
	public GameObject turretPlacementPositions;
	public GameObject placedTurrets;
	public GameObject turretTypes;
	// OIL MATERIALS
	public GameObject oilSlick;
	// TURRET COST
	public int turretCost;
	// OILSLICK COST
	public int oilSlickCost;
	// BALLISTA COST
	public int ballistaCost;
	// GAME CONTROLLER VARIABLES
	private bool previousOilButtonPressed = false;
	private bool previousTurretButtonPressed = false;
	private bool previousTurretSelectButtonPressed = false;
	private bool previousAoEButtonPressed = false;
	private bool previousNextTurretButtonPressed = false;
	private bool previousPreviousTurretButtonPressed = false;
	// GAME LOGIC
	private GameLogic game;
	// CURRENTLY SELECTED TURRET
	private EmitterBehavior selectedTurret = null;
	private int turretTypeIndex = 0;

	// DEFENSIVE STAGE
	public Text oilSlickCostText;
	public Text turretCostText;


	// Use this for initialization
	void Start () 
	{
		game = (GameLogic) gameObject.GetComponent (typeof(GameLogic));
		oilSlickCostText.text = "" + oilSlickCost;
		turretCostText.text = "" + turretCost;
	}
	
	// Check for input once a frame
	public void controlCheck ()
	{
		oilSlickCheck ();
		aoEAttackCheck ();
		placeTurretCheck ();
		selectTurretCheck ();
	
		// Enable modification of selected turret
		if (selectedTurret != null)
		{
			modifySelectedTurretCheck ();
		}
	}

	EmitterBehavior selectClosestTurret()
	{
		RaycastHit hit = game.getRayHit();
		float minDistance = float.MaxValue;
		GameObject closestTurret = null;
		for (int i = 0; i < placedTurrets.transform.childCount; i++) 
		{
			GameObject turretPos = placedTurrets.transform.GetChild (i).gameObject;
			float distance = Vector3.Distance (turretPos.transform.position, hit.point);
			if (distance < minDistance) 
			{
				minDistance = distance;
				closestTurret = turretPos;
			}
		}

		EmitterBehavior emitter = null;
		if (closestTurret != null)
		{
			emitter = (EmitterBehavior) closestTurret.GetComponent(typeof(EmitterBehavior));
		}

		return emitter;
	}

	void oilSlickCheck()
	{
		//<--------------------- CONTROLLER X TO DROP OIL SLICK ----------------------->
		bool oilButtonPressed = OVRGamepadController.GPC_GetButton (OVRGamepadController.Button.X);
		
		// Create projectile
		if (oilButtonPressed && !previousOilButtonPressed) 
		{
			oilSlick = (GameObject)Instantiate (oilSlick);
		}
		// Animate projectile in front of player
		else if (oilButtonPressed) 
		{
			RaycastHit hit = game.getRayHit ();
			oilSlick.transform.position = hit.point;
			//oilSlick.transform.rotation Quaternion.identity);
		}
		// Fire projectile
		else if (!oilButtonPressed && previousOilButtonPressed) 
		{
			RaycastHit hit = game.getRayHit ();
			oilSlick.transform.position = hit.point;
			OilSlick oilSlickScript = (OilSlick)oilSlick.GetComponent (typeof(OilSlick));
			oilSlickScript.enabled = true;
			print ("placed it at " + hit.point.y);
		}
		
		previousOilButtonPressed = oilButtonPressed;
	}

	void aoEAttackCheck()
	{
		//<--------------------- B to create explosion ------------------------------->	
		bool explosionButtonPressed = OVRGamepadController.GPC_GetButton (OVRGamepadController.Button.B);
		
		if (explosionButtonPressed && !previousAoEButtonPressed) 
		{
			RaycastHit hit = game.getRayHit();
			Instantiate (clapProjectile, hit.point + new Vector3 (0.0f, 2.0f, 0.0f), Quaternion.identity);
		}
		
		previousAoEButtonPressed = explosionButtonPressed;
	}


	private GameObject highlightTurret;
	void placeTurretCheck()
	{
		//<--------------------- Y to place turret ------------------------------->
		bool turretPlaceButtonPressed = OVRGamepadController.GPC_GetButton (OVRGamepadController.Button.Y);
		
		if (turretPlaceButtonPressed) 
		{
			// Display prospective turret spots
			showHideTurretPositions (true);

			highlightClosestTurretPlacementPosition();

//			if (highlightTurret != null) 
//			{
//				TurretPlacementSpot placement = (TurretPlacementSpot) highlightTurret.GetComponent(typeof(TurretPlacementSpot));
//				placement.deSelect();
//			}
//
//			highlightTurret = getClosestTurret();
//			
//			if (highlightTurret != null) 
//			{
//				TurretPlacementSpot placement = (TurretPlacementSpot) highlightTurret.GetComponent(typeof(TurretPlacementSpot));
//				placement.select();
//			}
		} 
		else if (!turretPlaceButtonPressed && previousTurretButtonPressed) 
		{
			placeClosestTurret();
//			showHideTurretPositions (false);
//
//			if (game.getCurrencyValue() < turretCost)
//				return;
//			game.changeCurrency(-turretCost);
//			
//			GameObject closestTurret = getClosestTurret();
//			
//			if (closestTurret != null) 
//			{
//				GameObject newTurret = (GameObject) Instantiate (cTurret, closestTurret.transform.position, closestTurret.transform.rotation);
//				// Add this turret to the list of all turrets
//				newTurret.transform.parent = placedTurrets.transform;
//				// Enable this turret's script
//				((EmitterBehavior) newTurret.GetComponent(typeof(EmitterBehavior))).enabled = true;
//				Destroy (closestTurret);
//			}
//			else 
//			{
//				print ("Could not place a turret, no more locations available!");
//			}
		}
		
		previousTurretButtonPressed = turretPlaceButtonPressed;
	}

	void selectTurretCheck()
	{
		//<--------------------- A to select turret ------------------------------->
		bool turretSelectButtonPressed = OVRGamepadController.GPC_GetButton (OVRGamepadController.Button.A);
		
		if (turretSelectButtonPressed) 
		{
			unHighlightAllTurrets();
			EmitterBehavior closestTurret = selectClosestTurret();
			if (closestTurret != null)
			{
				closestTurret.highlight(true);
				selectedTurret = closestTurret;
			}
			else
			{
				selectedTurret = null;
			}
		} 
		else if (!turretSelectButtonPressed && previousTurretSelectButtonPressed) 
		{
			EmitterBehavior closestTurret = selectClosestTurret();
			if (closestTurret != null)
			{
				print ("Deselecting should happen!");
				closestTurret.highlight(false);
			}
		}
		
		previousTurretSelectButtonPressed = turretSelectButtonPressed;
	}

	void modifySelectedTurretCheck()
	{
		//<--------------------- DPAD right for next turret type ------------------------------->
		bool nextTurret = OVRGamepadController.GPC_GetButton (OVRGamepadController.Button.Right);

		if (!nextTurret && previousNextTurretButtonPressed)
		{
			turretTypeIndex++;
			if (turretTypeIndex > turretTypes.transform.childCount - 1)
				turretTypeIndex = 0;

			GameObject newTurret = (GameObject) Instantiate (turretTypes.transform.GetChild(turretTypeIndex).gameObject,
                 selectedTurret.transform.position, selectedTurret.transform.rotation);
			newTurret.transform.parent = placedTurrets.transform;
			EmitterBehavior emitter = (EmitterBehavior) newTurret.GetComponent((typeof(EmitterBehavior)));
			emitter.enabled = true;
			Destroy (selectedTurret.gameObject);
			selectedTurret = emitter;
		}

		previousNextTurretButtonPressed = nextTurret;

		//<--------------------- DPAD left for next turret type ------------------------------->
		bool previousTurret = OVRGamepadController.GPC_GetButton (OVRGamepadController.Button.Left);
		
		if (!previousTurret && previousPreviousTurretButtonPressed)
		{
			turretTypeIndex--;
			if (turretTypeIndex < 0)
				turretTypeIndex = turretTypes.transform.childCount - 1;
			
			GameObject newTurret = (GameObject) Instantiate (turretTypes.transform.GetChild(turretTypeIndex).gameObject,
			                                                 selectedTurret.transform.position, selectedTurret.transform.rotation);
			newTurret.transform.parent = placedTurrets.transform;
			EmitterBehavior emitter = (EmitterBehavior) newTurret.GetComponent((typeof(EmitterBehavior)));
			emitter.enabled = true;
			Destroy (selectedTurret.gameObject);
			selectedTurret = emitter;
		}
		
		previousPreviousTurretButtonPressed = previousTurret;
	}

	void unHighlightAllTurrets()
	{
		for (int i = 0; i < placedTurrets.transform.childCount; i++) 
		{
			EmitterBehavior turret = (EmitterBehavior) placedTurrets.transform.GetChild (i).gameObject.GetComponent(typeof(EmitterBehavior));
			turret.highlight(false);
		}
	}

	public void placeClosestTurret()
	{
		showHideTurretPositions (false);
		
		if (player.getCurrencyValue() < turretCost)
			return;
		player.changeCurrency(-turretCost);
		updateDefencesCostText ();
		GameObject closestTurret = getClosestTurret();
		
		if (closestTurret != null) 
		{
			GameObject newTurret = (GameObject) Instantiate (cTurret, closestTurret.transform.position, closestTurret.transform.rotation);
			// Add this turret to the list of all turrets
			newTurret.transform.parent = placedTurrets.transform;
			// Enable this turret's script
			((EmitterBehavior) newTurret.GetComponent(typeof(EmitterBehavior))).enabled = true;
			Destroy (closestTurret);
		}
		else 
		{
			print ("Could not place a turret, no more locations available!");
		}
		updateDefencesCostText ();
	}

	public void tutorialPlaceClosestTurret()
	{
		showHideTurretPositions (false);
		
		GameObject closestTurret = getClosestTurret();
		
		if (closestTurret != null) 
		{
			GameObject newTurret = (GameObject) Instantiate (cTurret, closestTurret.transform.position, closestTurret.transform.rotation);
			// Add this turret to the list of all turrets
			newTurret.transform.parent = placedTurrets.transform;
			// Enable this turret's script
			((EmitterBehavior) newTurret.GetComponent(typeof(EmitterBehavior))).enabled = true;
			Destroy (closestTurret);
		}
		else 
		{
			print ("Could not place a turret, no more locations available!");
		}
	}

	public void highlightClosestTurretPlacementPosition()
	{
		//showHideTurretPositions (true);
		if (highlightTurret != null) 
		{
			TurretPlacementSpot placement = (TurretPlacementSpot) highlightTurret.GetComponent(typeof(TurretPlacementSpot));
			placement.deSelect();
		}
		
		highlightTurret = getClosestTurret();
		
		if (highlightTurret != null) 
		{
			TurretPlacementSpot placement = (TurretPlacementSpot) highlightTurret.GetComponent(typeof(TurretPlacementSpot));
			placement.select();
		}
	}

	public void showTurretPositions()
	{
		showHideTurretPositions (true);
	}

	public void hideTurretPositions()
	{
		showHideTurretPositions (false);
	}

	public void showHideTurretPositions(bool showOrHide)
	{
		// Don't display any of the turret positions
		for (int i = 0; i < turretPlacementPositions.transform.childCount; i++)
		{
			GameObject turretPos = turretPlacementPositions.transform.GetChild (i).gameObject;
			turretPos.transform.GetChild (0).gameObject.GetComponent<Renderer>().enabled = showOrHide;
			turretPos.transform.GetChild (1).gameObject.GetComponent<Renderer>().enabled = showOrHide;
		}
	}

	public GameObject getClosestTurret()
	{
		RaycastHit hit = game.getRayHit();
		float minDistance = float.MaxValue;
		GameObject closestTurret = null;
		for (int i = 0; i < turretPlacementPositions.transform.childCount; i++) 
		{
			GameObject turretPos = turretPlacementPositions.transform.GetChild (i).gameObject;
			float distance = Vector3.Distance (turretPos.transform.position, hit.point);
			if (distance < minDistance) 
			{
				minDistance = distance;
				closestTurret = turretPos;
			}
		}

		return closestTurret;
	}

	public int getNumTurretsLeft(){
		return (int) Mathf.Floor(player.getCurrencyValue() / turretCost);
	}

	public int getNumSlicksLeft()
	{
		return (int) Mathf.Floor(player.getCurrencyValue() / oilSlickCost);
	}

	public int getNumBallistasLeft()
	{
		return (int) Mathf.Floor(player.getCurrencyValue() / ballistaCost);
	}

//	public void slickUsed()
//	{
//		player.changeCurrency (-1 * oilSlickCost);
//		updateDefencesCostText ();
//	}
//
//	public void turretUsed()
//	{
//		player.changeCurrency (-1 * turretCost);
//		updateDefencesCostText ();
//	}
//
//	public void ballistaUsed()
//	{
//		player.changeCurrency (-1 * ballistaCost);
//		updateDefencesCostText ();
//	}


	public void updateDefencesCostText ()
	{
		if (player.getCurrencyValue () < oilSlickCost) 
		{
			oilSlickCostText.color = Color.red;		
		} 
		else
		{
			oilSlickCostText.color = Color.green;	
		}

		if (player.getCurrencyValue () < turretCost)
		{
			turretCostText.color = Color.red;		
		}
		else
		{
			turretCostText.color = Color.green;	
		}
	}

	public RaycastHit getRayHit()
	{
		int maskOne = 1 << 10;
//		int maskTwo = 1 << 11;
		int mask = maskOne;
		Ray ray = new Ray (thisCamera.transform.position, thisCamera.transform.forward);
		RaycastHit hit;
		Physics.Raycast (ray, out hit, 100f, mask);
		return hit;
	}

}




