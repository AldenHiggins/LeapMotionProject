using System;
using UnityEngine;
using UnityEngine.UI;

class PlaceDefenseControllerAttack : AControllerAttack
{
    public DefensiveAbilities defense;
    public PlayerLogic player;
    public int defenseCost;
    public GameObject defensiveObject;
    public GameObject defensiveObjectPending;
    public AudioClip placeObjectSound;
    private bool isInstantiated = false;
    private GameObject createdDefensiveObject;
    private AudioSource source;
    public bool rotateWhilePlacing;
    public GameObject goldSpentUI;
    public bool isAlly;

    private int currentRotation;
    public int rotationRate;

    void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
    }

    public override void chargingFunction() { }

    public override void chargedFunction() { }

    public override void releaseFunction(GameObject camera)
    {
        if (player.getCurrencyValue() >= defenseCost)
        {
            // Generate a popup to show how much gold was spent
            if (goldSpentUI != null)
            {
                GameObject thisDamage = (GameObject) Instantiate(goldSpentUI, defense.getRayHit().point, Quaternion.identity);
                thisDamage.SetActive(true);

                // Get the text field of the damage popup
                Text textFieldAmountOfDamage = thisDamage.transform.GetChild(1).GetChild(0).GetComponent<Text>();
                textFieldAmountOfDamage.text = "-" + defenseCost;
            }

            GameObject ballistaFinal = (GameObject) Instantiate(defensiveObject, defense.getRayHit().point, Quaternion.Euler(0.0f, currentRotation, 0.0f));
            ballistaFinal.SetActive(true);
            if (isAlly)
            {
                BasicEnemyController enemy = (BasicEnemyController) ballistaFinal.GetComponent(typeof(BasicEnemyController));
                NavMeshAgent agent = (NavMeshAgent) ballistaFinal.GetComponent(typeof(NavMeshAgent));
                agent.enabled = true;
                enemy.enabled = true;
            }

            if (source != null)
            { 
                source.PlayOneShot(placeObjectSound);
            }
            Destroy(createdDefensiveObject);
            isInstantiated = false;
            player.changeCurrency(-1 * defenseCost);
        }
    }

    public override void holdFunction() 
    {
        // Check for left or right axis input
        float axisRotationValue = OVRGamepadController.GPC_GetAxis(OVRGamepadController.Axis.RightXAxis);

        if (axisRotationValue > .1)
        {
            currentRotation += rotationRate;
        }
        else if (axisRotationValue < -.1)
        {
            currentRotation -= rotationRate;
        }
        Debug.Log(axisRotationValue);


        // Display prospective ballista spots
        // defense.showHideballistaPositions (true);
        //defense.highlightClosestballistaPlacementPosition();
        //print ("Place ballista is charging!");
        if (player.getCurrencyValue() >= defenseCost)
        {
            if (!isInstantiated)
            {
                createdDefensiveObject = (GameObject)Instantiate(defensiveObjectPending);
                createdDefensiveObject.SetActive(true);
                isInstantiated = true;
            }
            createdDefensiveObject.transform.position = defense.getRayHit().point;
            createdDefensiveObject.transform.rotation = Quaternion.Euler(0.0f, currentRotation, 0.0f);
        }
    }

    public override void inactiveFunction()
    {
        if (isInstantiated)
        {
            Destroy(createdDefensiveObject);
            isInstantiated = false;
        }
    }
}

