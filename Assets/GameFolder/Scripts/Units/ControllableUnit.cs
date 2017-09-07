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
    private float turnSpeed = .1f;
    [SerializeField]
    private float movementAngle = 10.0f;
    // IS ALIVE
    private bool isDying;

    // Use this for initialization
    void Start()
    {
        currentHealth = startingHealth;
        controller = GetComponent<CharacterController>();
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
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
            controller.Move(movementVector * walkSpeed * Time.deltaTime);
        }
        else
        {
            anim.SetBool("Running", false);
        }
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
