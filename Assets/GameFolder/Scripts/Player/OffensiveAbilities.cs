using UnityEngine;
using System.Collections;
using Leap;

public class OffensiveAbilities : MonoBehaviour
{
	// PLAYER OBJECTS
	public PlayerLogic playerLogic;
	public GameObject thisCamera;
    //public HandController handController = null;'
    private RigidHand rightHand;
    private RigidHand leftHand;

	// GAME LOGIC
	private GameLogic game;
	// INTERNAL VARIABLES
    //private Controller controller;
	// Min value for the fist dot product
	private float minVal = 0.65f;
	// ATTACK SELECTION
	private bool selectingAttack = false;

	// ATTACK CALLBACKS
	public BasicFireballAttack rightHandFlipAttack;
	public AAttack rightHandFistAttack;

	public AAttack leftHandFlipAttack;
	public AAttack leftHandFistAttack;

	public AAttack clapAttack;
	public AAttack emptyAttack;

	// right hand
	public AAttack rightHandOffensiveFlip;
	public AAttack rightHandOffensiveFist;
	public AAttack rightHandDefensiveFlip;
	public AAttack rightHandDefensiveFist;

	// left hand
	public AAttack leftHandOffensiveFlip;
	public AAttack leftHandOffensiveFist;
	public AAttack leftHandDefensiveFlip;
	public AAttack leftHandDefensiveFist;

	// ATTACKS SET BY THE UI
	public AAttack uiHandFlipAttack;
	public AAttack uiHandFistAttack;


	// DEFENSIVE ABILITIES
	private DefensiveAbilities defense;
	private bool fireballCharged = false;
	private bool handWasFist = false;
	private bool makingAFist = false;
	private bool fireballChargedTwo = false;
	private bool handWasFistTwo = false;
	private bool makingAFistTwo = false;


	// FLAMETHROWER VARIABLES
	private bool flamethrowersActive = false;
	private bool firstFlameThrowerActive = false;
	private bool firstFlameThrowerActivated = false;
	private int flamethrowerChargeLevel = 0;
	public int numFireballsForFlamethrower = 4;
	public float flamethrowerTimeframe = 6.0f;
	public AudioClip clapToActivateFlameThrowerExplanation;
	public AudioClip faceHandsToEnemiesExplanation;
	public GameObject flameThrowerChargeCounters;

	// HANDS
    //private HandModel[] hands;
	private AudioSource source;
	public AudioClip headshotSound;

	// Use this for initialization
	void Start ()
	{
		game = (GameLogic)GetComponent (typeof(GameLogic));
        //controller = new Controller ();
        //controller.EnableGesture (Gesture.GestureType.TYPE_CIRCLE);
		defense = (DefensiveAbilities)GetComponent (typeof(DefensiveAbilities));
		source = gameObject.GetComponent<AudioSource> ();

        // Get the Leap hands
        HandPool pool = FindObjectOfType<HandPool>();
        rightHand = (RigidHand) pool.RightPhysicsModel;
        leftHand = (RigidHand) pool.LeftPhysicsModel;
	}

	// Check for input once a frame
	public void controlCheck ()
	{
        if (rightHand.gameObject.activeSelf)
        {
            //// Check to ignite hands if flamethrower is charged up	
            //if (playerLogic.getSpecialAttackPower() > 100 && !flamethrowersActive)
            //{
            //    // Play an explanation the first time the user charges up
            //    if (!firstFlameThrowerActive)
            //    {
            //        firstFlameThrowerActive = true;
            //        source.PlayOneShot(clapToActivateFlameThrowerExplanation);
            //    }

            //    for (int handIndex = 0; handIndex < hands.Length; handIndex++)
            //    {
            //        GameObject hand = hands[handIndex].gameObject;
            //        hand.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
            //    }
            //}
            //else
            //{
            //    for (int handIndex = 0; handIndex < hands.Length; handIndex++)
            //    {
            //        GameObject hand = hands[handIndex].gameObject;
            //        hand.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            //    }
            //}

            // Check for one handed attacks
            //Vector3 direction0 = (hands[0].GetPalmPosition() - handController.transform.position).normalized;
            Vector3 normal0 = rightHand.GetPalmNormal();

            //			checkHandFlip (normal0, ref fireballCharged, ref makingAFist);
            //			checkHandFist (hands [0].GetLeapHand (), ref handWasFist, ref makingAFist, ref makingAFistTwo);

            checkHandFlip(normal0, ref fireballCharged, ref makingAFist, 1);

            //if (hands[0].GetLeapHand().IsLeft)
            //{
            //    checkHandFlip(normal0, ref fireballCharged, ref makingAFist, 0);
            //    //checkHandFist(hands[0].GetLeapHand(), ref handWasFist, ref makingAFist, ref makingAFistTwo, 0);
            //}
            //else
            //{
            //    checkHandFlip(normal0, ref fireballChargedTwo, ref makingAFistTwo, 1);
            //    //checkHandFist(hands[0].GetLeapHand(), ref handWasFistTwo, ref makingAFistTwo, ref makingAFist, 1);
            //}


            //// Check for attacks with the second hand
            //if (hands.Length > 1)
            //{
            //    Vector3 direction1 = (hands[1].GetPalmPosition() - handController.transform.position).normalized;
            //    Vector3 normal1 = hands[1].GetPalmNormal().normalized;


            //    if (hands[1].GetLeapHand().IsLeft)
            //    {
            //        checkHandFlip(normal1, ref fireballCharged, ref makingAFist, 0);
            //        checkHandFist(hands[1].GetLeapHand(), ref handWasFist, ref makingAFist, ref makingAFistTwo, 0);
            //    }
            //    else
            //    {
            //        checkHandFlip(normal1, ref fireballChargedTwo, ref makingAFistTwo, 1);
            //        checkHandFist(hands[1].GetLeapHand(), ref handWasFistTwo, ref makingAFistTwo, ref makingAFist, 1);
            //    }
            //    //				checkHandFlip (normal1, ref fireballChargedTwo, ref makingAFistTwo);
            //    //				checkHandFist (hands [1].GetLeapHand (), ref handWasFistTwo, ref makingAFistTwo, ref makingAFist);

            //    checkClap(hands);

            //    if (flamethrowersActive)
            //    {
            //        clapAttack.holdGestureFunction(hands);
            //    }
            //}
        }
        else
        {
            //clapAttack.chargedFunction(hands);
            //			handFlipAttack.inactiveFunction ();
            //rightHandFistAttack.inactiveFunction();
            //leftHandFistAttack.inactiveFunction();

            fireballCharged = false;
            //handWasFist = false;
            //makingAFist = false;

            //fireballChargedTwo = false;
            //handWasFistTwo = false;
            //makingAFistTwo = false;
        }
	}

	public void headShotAchieved ()
	{
//		if (flamethrowerChargeLevel < flameThrowerChargeCounters.transform.childCount)
//		{
//			// Set the appropriate charge active
//			GameObject flameThrowerCharge = flameThrowerChargeCounters.transform.GetChild (flamethrowerChargeLevel).gameObject;
//			flameThrowerCharge.SetActive (true);
//		}

		// Update the special bar instead

		flamethrowerChargeLevel++;
		source.PlayOneShot (headshotSound);
	}

	IEnumerator flamethrowerCooldown ()
	{
		yield return new WaitForSeconds (flamethrowerTimeframe);
		deactivateFlameThrowers ();
	}

	public void deactivateFlameThrowers ()
	{
        //flamethrowersActive = false;
        //clapAttack.chargedFunction (hands);
        //flamethrowerChargeLevel = 0;
		
//		// Disable all flame thrower charges on the UI
//		for (int chargeIndex = 0; chargeIndex < flameThrowerChargeCounters.transform.childCount; chargeIndex++) {
//			GameObject flameThrowerCharge = flameThrowerChargeCounters.transform.GetChild (chargeIndex).gameObject;
//			flameThrowerCharge.SetActive (false);
//		}
	}

	IEnumerator attackSelectionCooldown ()
	{
		yield return new WaitForSeconds (1.0f);
		selectingAttack = false;
	}

	// Check if the player has clapped
	public void checkClap ()
	{
//        Vector3 distance = hands [0].GetPalmPosition () - hands [1].GetPalmPosition ();
//        if (distance.magnitude < .09 && playerLogic.getSpecialAttackPower() >= 100) 
//        {
//            if (!firstFlameThrowerActivated) 
//            {
//                firstFlameThrowerActivated = true;
////				source.PlayOneShot (faceHandsToEnemiesExplanation);
//            }
//            flamethrowersActive = true;
//            StartCoroutine (flamethrowerCooldown ());
//            clapAttack.releaseFunction (hands);
//            playerLogic.useSpecialAttack();
//        }
	}

	// Logic to check for fists
//    public void checkHandFist (Hand handInput, ref bool handWasFistInput, ref bool makingAFistInput, ref bool otherMakingAFist, int handLeftOrRight)
//    {
//        bool handIsFist = checkFistHelper (handInput);
//        if (handIsFist) {
//            if (!handWasFistInput) {
//                handWasFistInput = true;
//                // Check for left handed fist attack
//                if (handLeftOrRight == 0)
//                {
//                    leftHandFistAttack.chargedFunction (hands);
//                    leftHandFlipAttack.inactiveFunction();
//                }
//                // Right handed fist attack
//                else
//                {
//                    rightHandFistAttack.chargedFunction (hands);
//                    rightHandFlipAttack.inactiveFunction();
//                }
//                makingAFistInput = true;
//            }

//            if (handLeftOrRight == 0)
//            {
//                leftHandFistAttack.holdGestureFunction (hands);
//            }
//            // Right handed fist attack
//            else
//            {
//                rightHandFistAttack.holdGestureFunction (hands);
//            }
//        } else if (!handIsFist && handWasFistInput) {
//            handWasFistInput = false;
//            makingAFistInput = false;
//            if (handLeftOrRight == 0)
//            {
//                leftHandFistAttack.releaseFunction (hands);
//            }
//            // Right handed fist attack
//            else
//            {
//                rightHandFistAttack.releaseFunction (hands);
//            }
////			fistAttack.releaseFunction (hands);
//        } else if (!handIsFist && !handWasFistInput) {
//            if (!otherMakingAFist) {
//                if (handLeftOrRight == 0)
//                {
//                    leftHandFistAttack.inactiveFunction ();
//                }
//                // Right handed fist attack
//                else
//                {
//                    rightHandFistAttack.inactiveFunction ();
//                }
////				fistAttack.inactiveFunction ();	
//            }
//        }
//    }

    // Check if the inputted hand is performing a flip attack
    public void checkHandFlip(Vector3 handNormal, ref bool fireballCharge, ref bool makingAFistInput, int handLeftOrRight)
    {
		//print ("Hand normal: " + handNormal);
		//print ("Camera normal: " + thisCamera.transform.forward);
		//print ("Dot product: " + Vector3.Dot (handNormal, thisCamera.transform.forward));

        //  Charge a fist attack, -.6 or less means the palm is facing the camera
        if (Vector3.Dot(handNormal, thisCamera.transform.forward) < -.6)
        {
//            if (handLeftOrRight == 0)
//            {
//                leftHandFlipAttack.chargingFunction(hands);
//            }
            // Right handed fist attack
//            else
//            {
            
//            }
            //			handFlipAttack.chargingFunction (hands);

			rightHandFlipAttack.chargingFunction(rightHand);

            if (!makingAFist && !makingAFistTwo)
            {
                if (!fireballCharge)
                {
                    fireballCharge = true;
//
//                    if (handLeftOrRight == 0)
//                    {
//                        leftHandFlipAttack.chargedFunction(hands);
//                        leftHandFistAttack.inactiveFunction();
//                    }
//                    // Right handed fist attack
//                    else
//                    {
					rightHandFlipAttack.chargedFunction(rightHand);
//                    rightHandFistAttack.inactiveFunction();
//                    }
                    //					handFlipAttack.chargedFunction (hands);
                    //					fistAttack.inactiveFunction ();
                }
            }
            else
            {
//                if (handLeftOrRight == 0)
//                {
//                    leftHandFlipAttack.inactiveFunction();
//                }
//                // Right handed fist attack
//                else
//                {
                rightHandFlipAttack.inactiveFunction();
//                }

                //				handFlipAttack.inactiveFunction ();
            }
        }
        else
        {
//            if (handLeftOrRight == 0)
//            {
//                leftHandFlipAttack.inactiveFunction();
//            }
            // Right handed fist attack
//            else
//            {
            rightHandFlipAttack.inactiveFunction();
//            }
            //			handFlipAttack.inactiveFunction ();
        }

        // Fire a fireball, .6 or more means the palm is facing away from the camera
        if (Vector3.Dot(handNormal, thisCamera.transform.forward) > .6)
        {
            if (fireballCharge)
            {
                fireballCharge = false;
                // First check if the player has enough energy
                if (playerLogic.getEnergy() > 10)
                {
//                    if (handLeftOrRight == 0)
//                    {
//                        leftHandFlipAttack.releaseFunction(rightHand);
//                    }
                    // Right handed fist attack
//                    else
//                    {
//                    }
                    //					handFlipAttack.releaseFunction (hands);

					rightHandFlipAttack.releaseFunction(rightHand);

//                    if (flamethrowerChargeLevel == numFireballsForFlamethrower)
//                    {
//                        AudioSource source = (AudioSource)clapAttack.gameObject.GetComponent<AudioSource>();
//                        if (source != null)
//                            source.Play();
//                    }
                }
            }
			rightHandFlipAttack.holdGestureFunction(rightHand);
//            if (handLeftOrRight == 0)
//            {
//                leftHandFlipAttack.holdGestureFunction(hands);
//            }
            // Right handed fist attack
//            else
//            {
               
//            }
            //			handFlipAttack.holdGestureFunction (hands);
        }
    }

//    // Helper funtion to check if a hand is making a fist
//    private bool checkFistHelper (Hand hand)
//    {
//        float sum = 0;
//        for (int i = 0; i < hand.Fingers.Count; i++) {
//            Finger f = hand.Fingers [i];
//            if (f.Type () == Finger.FingerType.TYPE_THUMB)
//                continue;
//            Vector meta = f.Bone (Bone.BoneType.TYPE_METACARPAL).Direction;
//            Vector proxi = f.Bone (Bone.BoneType.TYPE_PROXIMAL).Direction;
//            Vector inter = f.Bone (Bone.BoneType.TYPE_INTERMEDIATE).Direction;
//            float dMetaProxi = meta.Dot (proxi);
//            float dProxiInter = proxi.Dot (inter);
//            sum += dMetaProxi;
//            sum += dProxiInter;
//        }
//        sum = sum / 8;
////		print("sum = " + sum);
//        if (sum <= minVal && getExtendedFingers (hand) == 0) {
//            return true;
//        } else {
//            return false;
//        }
//    }

//    private int getExtendedFingers (Hand h)
//    {
//        int extendedFingers = 0;
//        for (int i=0; i <h.Fingers.Count; i++) {
//            Finger finger = h.Fingers [i];
//            if (finger.Type () == Finger.FingerType.TYPE_THUMB)
//                continue;
//            if (finger.IsExtended)
//                extendedFingers++;
//        }
//        return extendedFingers;
//    }

}



// OLD DETECTION CODE
//			//////////////////////////////////////////////////////////////
//			//////////////////////  DETECT HAND SPIN  ////////////////////
//			//////////////////////////////////////////////////////////////
//			if (controller.IsConnected) 
//			{ //controller is a Controller object
//				Frame currentFrame = controller.Frame (); //The latest frame
//				Frame previousFrame = controller.Frame (1); //The previous frame
//				
//				GestureList gesturesInFrame = currentFrame.Gestures (previousFrame);
//	
////				currentFrame.Gestures
//				if (!currentFrame.Gestures (previousFrame).IsEmpty) {
//					for (int i = 0; i < currentFrame.Gestures(previousFrame).Count; i++) {
//						Gesture gesture = currentFrame.Gestures (previousFrame) [i];
//						
//						if (gesture.Type == Gesture.GestureType.TYPE_CIRCLE) {
//							CircleGesture circleGesture = new CircleGesture (gesture);
//							// Limit the radius to prevent accidental gesture recognition
//							if (!isCircle && circleGesture.Radius > 50) {
//								isCircle = true;
//
//								circularHandAttack.releaseFunction (hands);
////								Instantiate (psychicParticle, playerLogic.transform.position, playerLogic.transform.rotation);
//							}
//						}
//					}
//				} else {
//					isCircle = false;
//				}
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







