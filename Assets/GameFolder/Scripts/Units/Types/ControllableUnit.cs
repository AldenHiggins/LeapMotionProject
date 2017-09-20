using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ControllableUnit : MonoBehaviour, IUnit
{
    // CONTROLLER
    private CharacterController controller;
    // ANIMATOR
    private Animator anim;
    // AUDIO SOURCE
    private AudioSource source;
    // HEALTH
    [SerializeField]
    private int startingHealth = 100;
    private int currentHealth;
    // MOVEMENT
    [SerializeField]
    private float walkSpeed = .1f;
    [SerializeField]
    private float attackWalkSpeed = .1f;
    private float moveSpeed;
    [SerializeField]
    private float turnSpeed = .1f;
    [SerializeField]
    private float movementAngle = 10.0f;
    // ATTACKS
    [SerializeField]
    private GameObject fireBall;
    [SerializeField]
    private GameObject fireBallTransform;
    private bool attacking;
    private float attackCooldown = .5f;
    // ENEMIES
    private int enemySearchLayer = 1 << 8;
    // ALLIES
    [SerializeField]
    private GameObject skeletonAlly;
    // MELEE
    [SerializeField]
    private GameObject meleeHitbox;
    [SerializeField]
    private int meleeDamage = 10;
    [SerializeField]
    private GameObject meleeParticle;
    [SerializeField]
    private AudioClip meleeNoise;
    // PLAYER CAMERA
    private GameObject playerCamera;
    // IS ALIVE
    private bool isDying;
    // PLAYER CONTROL
    private bool isControlled;
    // DELEGATES
    private Action onDeath;
    private Action<int, Vector3> onDamageTaken;

    // Use this for initialization
    void Start()
    {
        currentHealth = startingHealth;
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        playerCamera = GetObjects.getCamera();
        moveSpeed = walkSpeed;
        isControlled = true;
        // Don't accept player input during the defensive phase
        EventManager.StartListening(GameEvents.DefensivePhaseStart, delegate { isControlled = false; });
        EventManager.StartListening(GameEvents.OffensivePhaseStart, delegate { isControlled = true; });
    }

    // Update is called once per frame
    void Update()
    {
        // If this isn't currently being controlled or is dead ignore input
        if (!isControlled || isDying)
        {
            // Stop movement
            anim.SetBool("Moving", false);
            return;
        }

        handleMovement();

        handleAttacks();
    }

    void handleMovement()
    {
        // Take in input from the player
        Vector2 leftInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        Vector2 rightInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        // Find the look vector of the unit
        Vector3 lookVector = new Vector3(rightInput.x, 0.0f, rightInput.y);
        // Find the movement vector of the unit
        Vector3 movementVector = new Vector3(leftInput.x, 0.0f, leftInput.y);

        // Figure out where the unit should look
        if (lookVector == Vector3.zero)
        {
            anim.SetBool("Strafing", false);
            if (movementVector == Vector3.zero)
            {
                lookVector = transform.forward;
            }
            else
            {
                lookVector = playerCamera.transform.rotation * movementVector;
            }
        }
        else
        {
            anim.SetBool("Strafing", true);
            lookVector = playerCamera.transform.rotation * lookVector;
        }

        lookVector.y = 0.0f;

        // Set the player's move speed
        float movementMagnitude = movementVector.magnitude;
        anim.SetFloat("MoveSpeed", movementMagnitude);
        movementVector = playerCamera.transform.rotation * movementVector;
        movementVector.y = 0.0f;
        movementVector = Quaternion.Inverse(transform.rotation) * movementVector;
        anim.SetFloat("MoveX", movementVector.x);
        anim.SetFloat("MoveY", movementVector.z);
        // Don't move if the player isn't using the left stick
        if (movementVector == Vector3.zero)
        {
            anim.SetBool("Moving", false);
        }
        else
        {
            anim.SetBool("Moving", true);
        }

        // Rotate the unit
        Quaternion targetRot = Quaternion.LookRotation(lookVector);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * turnSpeed);

        // Move the unit if it's sufficiently pointed in the direction of its intended movement
        float rotationAngle = Quaternion.Angle(targetRot, transform.rotation);
        if (rotationAngle > movementAngle)
        {
            anim.SetBool("Moving", false);
        }
    }

    void handleAttacks()
    {
        // Accept player input to initiate attacks
        if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger))
        {
            if (!attacking)
            {
                StopCoroutine(attack("Attack1Trigger"));
                StartCoroutine(attack("Attack1Trigger"));
            }            
        }
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
        {
            if (!attacking)
            {
                StopCoroutine(attack("Attack2Trigger"));
                StartCoroutine(attack("Attack2Trigger"));
            }
        }
    }

    IEnumerator attack(string attackName)
    {
        attacking = true;
        anim.SetTrigger(attackName);
        moveSpeed = attackWalkSpeed;

        // Wait for a cooldown before attacking again
        yield return new WaitForSeconds(attackCooldown);
        moveSpeed = walkSpeed;
        attacking = false;
    }

    public void fireFireball()
    {
        GameObject newFireball = Instantiate(fireBall, fireBallTransform.transform.position, fireBallTransform.transform.rotation);
        newFireball.transform.parent = GetObjects.getAttackParticleContainer();
    }

    public void dealMeleeDamage()
    {
        // Fire the melee particle
        meleeParticle.SetActive(false);
        meleeParticle.SetActive(true);

        // Play the melee sound
        source.PlayOneShot(meleeNoise);

        RaycastHit[] hits = Physics.BoxCastAll(meleeHitbox.transform.position, meleeHitbox.transform.localScale, 
            meleeHitbox.transform.forward, meleeHitbox.transform.rotation, meleeHitbox.transform.localScale.z,
            enemySearchLayer);

        for (int hitIndex = 0; hitIndex < hits.Length; hitIndex++)
        {
            IUnit hitUnit = hits[hitIndex].collider.gameObject.GetComponent<IUnit>();
            if (hitUnit != null)
            {
                Vector3 damageVector = hitUnit.getGameObject().transform.position - transform.position;
                hitUnit.dealDamage(meleeDamage, damageVector.normalized);

                // If we have killed a unit spawn a skeleton ally on their corpse
                if (hitUnit.getCurrentHealth() <= 0)
                {
                    StartCoroutine(spawnSkeleton(hitUnit.getGameObject().transform.position, hitUnit.getGameObject().transform.rotation));
                }
            }
        }
    }

    IEnumerator spawnSkeleton(Vector3 position, Quaternion rotation)
    {
        yield return new WaitForSeconds(3.0f);

        GameObject newSkeleton = Instantiate(skeletonAlly, position, rotation, transform.parent);
        newSkeleton.GetComponent<FollowLeader>().leader = gameObject;
    }

    public void installDeathListener(Action onDeathCallback)
    {
        onDeath += onDeathCallback;
    }

    public void installDamageListener(Action<int, Vector3> onDamageCallback)
    {
        onDamageTaken += onDamageCallback;
    }

    public int getMaxHealth()
    {
        return startingHealth;
    }

    public int getCurrentHealth()
    {
        return currentHealth;
    }

    public void dealDamage(int damageToDeal, Vector3 damageDirection)
    {
        currentHealth -= damageToDeal;

        if (onDamageTaken != null)
        {
            onDamageTaken(damageToDeal, damageDirection);
        }

        if (currentHealth <= 0)
        {
            isDying = true;
            if (onDeath != null)
            {
                onDeath();
            }
            anim.SetTrigger("DeathTrigger");
        }
    }

    public bool isUnitDying()
    {
        return isDying;
    }

    public bool isUnitAlly()
    {
        return true;
    }

    public void slowDown() { }

    public GameObject getGameObject()
    {
        return gameObject;
    }

    public void FootR() { }
    public void FootL() { }
}
