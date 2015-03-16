using UnityEngine;
using System.Collections;
using Leap;

public class OffensiveAbilities : MonoBehaviour
{
	// PLAYER OBJECTS
	public PlayerLogic playerLogic;
	public GameObject thisCamera;
	public HandController handController = null;

	// PARTICLES
	public GameObject psychicParticle;
	// GAME LOGIC
	private GameLogic game;
	// INTERNAL VARIABLES
	private Controller controller;
	// SWIPE DETECTION
	private float previousAmountHandIsOnRightSideOfScreen = 0;
	// Min value for the fist dot product
	private float minVal = 0.5f;
	// ATTACK SELECTION
	public AttackSelection attackSelector;		
	private bool selectingAttack = false;

	// ATTACK CALLBACKS
	public AAttack handFlipAttack;
	public AAttack fistAttack;
	public AAttack circularHandAttack;
	public AAttack clapAttack;
	// DEFENSIVE ABILITIES
	private DefensiveAbilities defense;

	private bool fireballCharged = false;
	private bool handWasFist = false;
	private bool isChargingAttack = false;
	private bool isCircle = false;

	// HANDS
	private HandModel[] hands; 

	// Use this for initialization
	void Start ()
	{
		game = (GameLogic)GetComponent (typeof(GameLogic));
		controller = new Controller();
		controller.EnableGesture(Gesture.GestureType.TYPE_CIRCLE);
		defense = (DefensiveAbilities)GetComponent (typeof(DefensiveAbilities));
	}


	// Check for input once a frame
	public void controlCheck ()
	{
		hands = handController.GetAllGraphicsHands ();
		if (hands.Length == 1) {
			Vector3 direction0 = (hands [0].GetPalmPosition () - handController.transform.position).normalized;
			Vector3 normal0 = hands [0].GetPalmNormal ().normalized;

			//////////////////////////////////////////////////////////////
			//////////////////////  DETECT HAND SPIN  ////////////////////
			//////////////////////////////////////////////////////////////
			if(controller.IsConnected) //controller is a Controller object
			{
				Frame currentFrame = controller.Frame(); //The latest frame
				Frame previousFrame = controller.Frame(1); //The previous frame
				
				GestureList gesturesInFrame = currentFrame.Gestures(previousFrame);
	
//				currentFrame.Gestures
				if (!currentFrame.Gestures(previousFrame).IsEmpty) 
				{
					for(int i = 0; i < currentFrame.Gestures(previousFrame).Count; i++) 
					{
						Gesture gesture = currentFrame.Gestures(previousFrame)[i];
						
						if(gesture.Type == Gesture.GestureType.TYPE_CIRCLE) 
						{
							CircleGesture circleGesture = new CircleGesture(gesture);
							// Limit the radius to prevent accidental gesture recognition
							if (!isCircle && circleGesture.Radius > 50)
							{
								isCircle = true;

								circularHandAttack.releaseFunction(hands);
//								Instantiate (psychicParticle, playerLogic.transform.position, playerLogic.transform.rotation);
							}
						}
					}
				}
				else
				{
					isCircle = false;
				}
			}
		
			//////////////////////////////////////////////////////////////
			//////////////////////  DETECT HAND FLIP  ////////////////////
			//////////////////////////////////////////////////////////////

			//  Charge a fireball, -.6 or less means the palm is facing the camera
			if (Vector3.Dot (normal0, thisCamera.transform.forward) < -.6) 
			{
				handFlipAttack.chargingFunction(hands);
				if (!fireballCharged)
				{
					fireballCharged = true;
					handFlipAttack.chargedFunction(hands);
				}
			}
			else
			{
//				handFlipNotChargingFunction();
			}
		

			// Fire a fireball, .6 or more means the palm is facing away from the camera
			if (Vector3.Dot (normal0, thisCamera.transform.forward) > .6) {
				if (fireballCharged) {
					fireballCharged = false;
					// First check if the player has enough energy
					if (playerLogic.getEnergy () > 10)
					{
						handFlipAttack.releaseFunction(hands);
					}
				}
				handFlipAttack.holdGestureFunction(hands);
			}

			//////////////////////////////////////////////////////////////
			//////////////////////  DETECT A FIST  ///////////////////////
			//////////////////////////////////////////////////////////////
			bool handIsFist = checkFist (hands[0].GetLeapHand());
			if (handIsFist)
			{
				if (!handWasFist)
				{
					handWasFist = true;
					fistAttack.chargedFunction(hands);
//					game.fistProjectile();
				}
				fistAttack.holdGestureFunction(hands);
			}
			else if (!handIsFist && handWasFist)
			{
				handWasFist = false;
				fistAttack.releaseFunction(hands);
				fistAttack.inactiveFunction(hands);
			}
//			if (handIsFist && !handWasFist)
//			{
//				fistAttack.releaseFunction();
//				game.fistProjectile();
//				handWasFist = true;
//			}
//			else if(!handIsFist && handWasFist){
//				handWasFist = false;
//				fistAttack.inactiveFunction();
//			}


//			//////////////////////////////////////////////////////////////
//			//////////////////////  DETECT A SWIPE  //////////////////////
//			//////////////////////////////////////////////////////////////
//			Vector3 relativeHandPosition = hands[0].GetPalmPosition() - thisCamera.transform.position;
//			Vector3 playerForward = thisCamera.transform.forward;
//			Vector3 perpendicularVector = new Vector3(playerForward.z, playerForward.y, -1 * playerForward.x);
//			// Take the dot product of the hand's position and the right vector (the vector pointing
//			// to the right side of the screen space) in order to get an amount that describes how far to the
//			// right the hand is.  The normal range is around .35 (furthest right) to -.2 (furthest left)
//			float amountHandIsOnRightSideOfScreen = Vector3.Dot(relativeHandPosition, perpendicularVector);
//			// Only accept swipes if the player's hand is low enough on the screen
//			if  ((amountHandIsOnRightSideOfScreen - previousAmountHandIsOnRightSideOfScreen < -.02) && relativeHandPosition.y < -.1)
//			{
//				if (!selectingAttack)
//				{
//					selectingAttack = true;
//					int attackSelection = attackSelector.selectNextAttack();
//
//					// For now hardcode all the options
//					// 0 is the regular fireball
//					if (attackSelection == 0)
//					{
//						handFlipChargeFunction = testFunction;
//						handFlipNotChargingFunction = testFunction;
//						handFlipReleaseFunction = fireballFunction;
//						defense.hideTurretPositions();
//					}
//					// 1 highlights and places a turret
//					else if (attackSelection == 1)
//					{
//						handFlipChargeFunction = defense.highlightClosestTurretPlacementPosition;
//						handFlipNotChargingFunction = defense.hideTurretPositions;
//						handFlipReleaseFunction = defense.placeClosestTurret;
//					}
//					// 2 placeholder
//					else if (attackSelection == 2)
//					{
//						handFlipChargeFunction = testFunction;
//						handFlipNotChargingFunction = testFunction;
//						handFlipReleaseFunction = fireballFunction;
//					}
//					// 3 placeholder
//					else
//					{
//						handFlipChargeFunction = testFunction;
//						handFlipNotChargingFunction = testFunction;
//						handFlipReleaseFunction = fireballFunction;
//						defense.hideTurretPositions();
//					}
//
//					// Switch the attacks that the player uses
//					StartCoroutine(attackSelectionCooldown());
//				}
//				print ("Swiping Left");
//			}
//
//			previousAmountHandIsOnRightSideOfScreen = amountHandIsOnRightSideOfScreen;

		} 
		else if (hands.Length > 1) 
		{
			Vector3 direction0 = (hands [0].GetPalmPosition () - handController.transform.position).normalized;
			Vector3 normal0 = hands [0].GetPalmNormal ().normalized;
	
			Vector3 direction1 = (hands [1].GetPalmPosition () - handController.transform.position).normalized;
			Vector3 normal1 = hands [1].GetPalmNormal ().normalized;
			isChargingAttack = true;

			//////////////////////////////////////////////////////////////
			//////////////////////  DETECT A CLAP  ///////////////////////
			//////////////////////////////////////////////////////////////
			if (Vector3.Dot (normal0, normal1) < -.6) 
			{
				Vector3 distance = hands [0].GetPalmPosition () - hands [1].GetPalmPosition ();
				if (distance.magnitude < .09) 
				{
//					game.clapAttack (playerLogic.transform.position + new Vector3 (0.0f, 0.7f, 0.0f));
					clapAttack.releaseFunction(hands);
				}
			}

			//////////////////////////////////////////////////////////////
			//////////////////////  DETECT A PUSH AWAY  //////////////////
			//////////////////////////////////////////////////////////////
			if (Mathf.Abs (Vector3.Dot (normal0, normal1)) > .8) 
			{
				// Do push attack

			}
			else
			{
				isChargingAttack = false;
			}
		}
	}


	IEnumerator attackSelectionCooldown()
	{
		yield return new WaitForSeconds (1.0f);
		selectingAttack = false;
	}

	/// <summary>
	/// Checks the fist.
	/// </summary>
	/// <returns><c>true</c>, if fist was checked, <c>false</c> otherwise.</returns>
	/// <param name="hand">Hand.</param>
	private bool checkFist(Hand hand)
	{
		float sum = 0;
		for (int i = 0; i < hand.Fingers.Count; i++) 
		{
			Finger f = hand.Fingers[i];
			Vector meta = f.Bone(Bone.BoneType.TYPE_METACARPAL).Direction;
			Vector proxi = f.Bone(Bone.BoneType.TYPE_PROXIMAL).Direction;
			Vector inter = f.Bone(Bone.BoneType.TYPE_INTERMEDIATE).Direction;
			float dMetaProxi = meta.Dot(proxi);
			float dProxiInter = proxi.Dot(inter);
			sum += dMetaProxi;
			sum += dProxiInter;
		}
		sum = sum/10;
//		print("sum = " + sum);
		if(sum <= minVal && getExtendedFingers(hand)== 0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	private int getExtendedFingers(Hand h)
	{
		int extendedFingers = 0;
		for(int i=0;i <h.Fingers.Count;i++)
		{
			Finger finger = h.Fingers[i];
			if(finger.IsExtended) extendedFingers++;
		}
		return extendedFingers;
	}

}
