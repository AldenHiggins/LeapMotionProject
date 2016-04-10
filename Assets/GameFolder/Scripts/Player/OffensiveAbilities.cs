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
    void Start()
    {
        game = (GameLogic)GetComponent(typeof(GameLogic));
        //controller = new Controller ();
        //controller.EnableGesture (Gesture.GestureType.TYPE_CIRCLE);
        defense = (DefensiveAbilities)GetComponent(typeof(DefensiveAbilities));
        source = gameObject.GetComponent<AudioSource>();

        // Get the Leap hands
        HandPool pool = FindObjectOfType<HandPool>();
        rightHand = (RigidHand)pool.RightPhysicsModel;
        leftHand = (RigidHand)pool.LeftPhysicsModel;
    }

    // Check for input once a frame
    public void controlCheck()
    {

    }

    public void headShotAchieved()
    {
        //		if (flamethrowerChargeLevel < flameThrowerChargeCounters.transform.childCount)
        //		{
        //			// Set the appropriate charge active
        //			GameObject flameThrowerCharge = flameThrowerChargeCounters.transform.GetChild (flamethrowerChargeLevel).gameObject;
        //			flameThrowerCharge.SetActive (true);
        //		}

        // Update the special bar instead

        flamethrowerChargeLevel++;
        source.PlayOneShot(headshotSound);
    }

}
