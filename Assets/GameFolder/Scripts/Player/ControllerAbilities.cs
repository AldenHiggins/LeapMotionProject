using System;
using UnityEngine;

public class ControllerAbilities : MonoBehaviour
{
	// MAIN GAME CAMERA
	public GameObject camera;
    public PlayerLogic player;
    public DefensiveAbilities defense;

	// GAME LOGIC
	private GameLogic game;

	// CURRENTLY ACTIVE ATTACKS
	public AControllerAttack rightTriggerAttack;
	public AControllerAttack leftTriggerAttack;

	// OFFENSIVE ATTACKS
	public AControllerAttack fireballAttack;
	public AControllerAttack iceballAttack;

	// DEFENSIVE ATTACKS
	public AControllerAttack placeDefense;
	public AControllerAttack switchDefense;

	// UTILITY ATTACKS
	public AControllerAttack emptyAttack;

	// PREVIOUS BUTTON PRESSES/AXIS VALUES
	float previousRightTrigger;
	float previousLeftTrigger;
	bool previousAPressed;
	bool previousBPressed;
	bool previousStartPressed;
    bool previousLeftStickPressed;

	void Start()
	{
		game = (GameLogic) gameObject.GetComponent (typeof(GameLogic));
	}

	void Update()
	{
		///////////////////
		//   Start Button    //
		///////////////////
		bool startPressed = OVRGamepadController.GPC_GetButton(OVRGamepadController.Button.Start);
		if (startPressed && !previousStartPressed)
		{
			game.startRound();
		}
		previousStartPressed = startPressed;

		///////////////////
		//   A Button    //
		///////////////////
		bool aPressed = OVRGamepadController.GPC_GetButton(OVRGamepadController.Button.A);
		if (aPressed && !previousAPressed)
		{
			Debug.Log ("A Pressed");
		}
		previousAPressed = aPressed;

		///////////////////
		//   B Button    //
		///////////////////
		bool bPressed = OVRGamepadController.GPC_GetButton(OVRGamepadController.Button.B);
		if (bPressed && !previousBPressed)
		{
			Debug.Log ("B Pressed");
		}
		previousBPressed = bPressed;

        ///////////////////
        //LeftStick Button/
        ///////////////////
        bool leftStickPressed = OVRGamepadController.GPC_GetButton(OVRGamepadController.Button.LStick);
        if (leftStickPressed && !previousLeftStickPressed)
        {
            if (player.gameObject.transform.localScale.x == 1.0f)
            {
                player.gameObject.transform.localScale = new Vector3(12.0f, 12.0f, 12.0f);
                player.gameObject.transform.position = new Vector3(0.0f, 18.59f, 0.0f);
            }
            else if (player.gameObject.transform.localScale.x == 12.0f)
            {
                RaycastHit hit = defense.getRayHit();
                player.gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                player.gameObject.transform.position = hit.point;
            }
        }
        previousLeftStickPressed = leftStickPressed;

		///////////////////
		// RIGHT TRIGGER //
		///////////////////
		float rightTriggerPressed = OVRGamepadController.GPC_GetAxis(OVRGamepadController.Axis.RightTrigger);
        if (rightTriggerPressed > .1f && previousRightTrigger > .1f)
        {
            rightTriggerAttack.holdFunction();
        }
        else if (rightTriggerPressed < .1f && previousRightTrigger >= .1f)
        {
            rightTriggerAttack.releaseFunction(camera);
        }
        else
        {
            rightTriggerAttack.inactiveFunction();
            rightTriggerAttack.chargingFunction();
        }
		previousRightTrigger = rightTriggerPressed;

		///////////////////
		// LEFT TRIGGER  //
		///////////////////
		float leftTriggerPressed = OVRGamepadController.GPC_GetAxis(OVRGamepadController.Axis.LeftTrigger);
		if (leftTriggerPressed > .1f && previousLeftTrigger <= .1f)
		{
			leftTriggerAttack.releaseFunction(camera);
		}
		previousLeftTrigger = leftTriggerPressed;
	}
}


