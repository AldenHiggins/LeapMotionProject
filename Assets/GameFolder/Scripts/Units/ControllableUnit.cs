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
    // IS ALIVE
    private bool isDying;

    // Use this for initialization
    void Start()
    {
        currentHealth = startingHealth;
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        moveSpeed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        handleMovement();

        handleAttacks();
    }

    void handleMovement()
    {
        // Take in input from the player
        Vector2 leftInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        // Find the movement vector of the unit
        Vector3 movementVector = new Vector3(leftInput.x, 0.0f, leftInput.y);
        // Don't do anything if we haven't received any input
        if (movementVector == Vector3.zero)
        {
            anim.SetBool("Running", false);
            return;
        }
        // Rotate the unit
        Quaternion targetRot = Quaternion.LookRotation(movementVector);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * turnSpeed);

        // Move the unit if it's sufficiently pointed in the direction of its intended movement
        float rotationAngle = Quaternion.Angle(targetRot, transform.rotation);
        if (rotationAngle <= movementAngle)
        {
            anim.SetBool("Running", true);
            //controller.Move(movementVector * moveSpeed * Time.deltaTime);
        }
        else
        {
            anim.SetBool("Running", false);
        }
    }

    void handleAttacks()
    {
        // Accept player input to initiate attacks
        if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger))
        {
            if (!attacking)
            {
                StopCoroutine(attack());
                StartCoroutine(attack());
            }            
        }
    }

    IEnumerator attack()
    {
        attacking = true;
        anim.SetBool("Attacking", true);
        anim.Play("attack0");
        moveSpeed = attackWalkSpeed;
        // Let the animation play for half a second before dealing damage to the target
        //yield return new WaitForSeconds(.2f);

        //// Check if our target is within the attack radius and isn't already dying
        //Vector3 distance = target.getGameObject().transform.position - transform.position;
        //distance.y = 0.0f;
        //if (distance.magnitude <= attackRadius)
        //{
        //    // Check to see if our target has died
        //    if (target.isUnitDying())
        //    {
        //        attacking = false;
        //        anim.SetBool("Attacking", false);
        //        yield break;
        //    }

        //    target.dealDamage(attackDamage);
        //}

        // Wait another second before attacking again
        yield return new WaitForSeconds(0.5f);
        moveSpeed = walkSpeed;
        anim.SetBool("Attacking", false);
        attacking = false;
    }

    public void FireFireball()
    {
        GameObject newFireball = Instantiate(fireBall, fireBallTransform.transform.position, fireBallTransform.transform.rotation);
        newFireball.transform.parent = GetObjects.getAttackParticleContainer();
    }

    public void installDeathListener(Action onDeathCallback) { }

    public void installDamageListener(Action<int> onDamageCallback) { }

    public int getMaxHealth()
    {
        return startingHealth;
    }

    public int getCurrentHealth()
    {
        return currentHealth;
    }

    public void dealDamage(int damageToDeal)
    {
        currentHealth -= damageToDeal;

        if (currentHealth <= 0)
        {
            isDying = true;
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
}
