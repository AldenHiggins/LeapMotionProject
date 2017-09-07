using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ControllableUnit : MonoBehaviour, IUnit
{
    // CONTROLLER
    private CharacterController controller;
    // HEALTH
    [SerializeField]
    private int startingHealth = 100;
    private int currentHealth;
    // MOVEMENT
    [SerializeField]
    private float walkSpeed = .1f;
    // IS ALIVE
    private bool isDying;

    // Use this for initialization
    void Start()
    {
        currentHealth = startingHealth;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 leftInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        Vector3 movementVector = new Vector3(leftInput.x, 0.0f, leftInput.y);
        movementVector *= walkSpeed;
        controller.Move(movementVector);
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
