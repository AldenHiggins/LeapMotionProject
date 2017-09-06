using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticUnit : MonoBehaviour, IUnit
{
    [SerializeField]
    private int startingHealth = 1000;

    private int currentHealth;

    private bool isDead = false;
    
    // Use this for initialization
    void Start()
    {
        currentHealth = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void dealDamage(int damageToDeal)
    {
        currentHealth -= damageToDeal;

        if (currentHealth <= 0)
        {
            isDead = true;
            EventManager.TriggerEvent(GameEvents.GameOver);
        }
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
