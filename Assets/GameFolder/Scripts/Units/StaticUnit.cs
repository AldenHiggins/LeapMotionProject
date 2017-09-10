﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticUnit : MonoBehaviour, IUnit
{
    [SerializeField]
    private int startingHealth = 1000;

    private int currentHealth;

    private bool isDead = false;

    // DELEGATES
    private Action onDeath;
    private Action<int, Vector3> onDamageTaken;

    // Use this for initialization
    void Start()
    {
        currentHealth = startingHealth;
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
            isDead = true;
            if (onDeath != null)
            {
                onDeath();
            }
        }
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

    public bool isUnitDying()
    {
        return isDead;
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
