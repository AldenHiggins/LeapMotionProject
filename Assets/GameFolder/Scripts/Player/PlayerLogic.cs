﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class PlayerLogic : MonoBehaviour, IUnit
{
    [SerializeField]
    private int startingHealth = 100;

    [SerializeField]
    private int gold = 500;

    private int health;

    private bool isAlive = true;

    [HideInInspector]
    public bool isSmall = false;

    [SerializeField]
    private GameObject selectionPointer;
    private SelectionPointer rightPointer;

    // Use this for initialization
    void Start()
    {
        health = startingHealth;
        // Instantiate our selection pointers
        rightPointer = Instantiate(selectionPointer, transform.GetChild(0).GetChild(5)).GetComponent<SelectionPointer>();
        hidePointers();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void respawn()
    {
        isAlive = false;
        // Trigger the game over event
        EventManager.TriggerEvent(GameEvents.GameOver);
    }

    public void dealDamage(int damageToDeal, Vector3 damageDirection)
    {
        health -= damageToDeal;

        // Player is dead
        if (health < 0)
        {
            respawn();
        }
    }

    public void installDeathListener(Action onDeathCallback)
    { }

    public void installDamageListener(Action<int, Vector3> onDamageCallback)
    { }

    public int getMaxHealth()
    {
        return startingHealth;
    }

    public int getCurrentHealth()
    {
        return health;
    }

    public void resetHealth()
    {
        health = 100;
    }

    public GameObject getGameObject()
    {
        return gameObject;
    }

    public bool isUnitDying()
    {
        return !isAlive;
    }

    public bool isUnitAlly()
    {
        return true;
    }

    public void slowDown() { }

    public int getGold()
    {
        return gold;
    }

    public bool spendGold(int goldSpent)
    {
        if (goldSpent > gold)
        {
            return false;
        }

        gold -= goldSpent;
        return true;
    }

    public void showPointer(bool leftHand)
    {
        rightPointer.enablePointer();
    }

    public void hidePointers()
    {
        rightPointer.disablePointer();
    }
}

