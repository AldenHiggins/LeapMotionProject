using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ControllableUnit : MonoBehaviour, IUnit
{
    // ANIMATOR
    private Animator anim;
    // AUDIO SOURCE
    private AudioSource source;
    // HEALTH
    [SerializeField]
    private int startingHealth = 100;
    private int currentHealth;
    // MOVEMENT
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
    private float attackCooldown = .3f;
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
    private int meleeAttackNumber;
    // IS ALIVE
    private bool isDying;
    // DELEGATES
    private Action onDeath;
    private Action<int, Vector3> onDamageTaken;

    // Use this for initialization
    void Start()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    public void movementUpdate(Vector3 moveVector, Vector3 lookVector, bool isStrafing)
    {
        // Don't take movement if the unit is dead
        if (isDying) return;

        // Set strafing to true if the look vector is zero
        anim.SetBool("Strafing", isStrafing);

        // Set moving to true if the move vector is non zero
        anim.SetBool("Moving", moveVector != Vector3.zero);

        // Set the player's move speed
        float movementMagnitude = moveVector.magnitude;
        anim.SetFloat("MoveSpeed", movementMagnitude);
        // Set the player's move direction
        anim.SetFloat("MoveX", moveVector.x);
        anim.SetFloat("MoveY", moveVector.z);

        // Rotate the unit towards lookVector
        Quaternion targetRot = Quaternion.LookRotation(lookVector);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * turnSpeed);

        // Only move the unit if it's sufficiently pointed in the direction of its intended movement
        float rotationAngle = Quaternion.Angle(targetRot, transform.rotation);
        if (rotationAngle > movementAngle)
        {
            anim.SetBool("Moving", false);
        }
    }

    public void doMeleeAttack()
    {
        if (!attacking && !isDying)
        {
            StopCoroutine(attack("Attack2Trigger"));
            StartCoroutine(attack("Attack2Trigger"));
        }
    }

    public void doShadowBallAttack()
    {
        if (!attacking && !isDying)
        {
            StopCoroutine(attack("Attack1Trigger"));
            StartCoroutine(attack("Attack1Trigger"));
        }
    }

    IEnumerator attack(string attackName)
    {
        attacking = true;
        anim.SetTrigger(attackName);

        // Wait for a cooldown before attacking again
        yield return new WaitForSeconds(attackCooldown);
        attacking = false;
    }

    public void fireFireball()
    {
        Instantiate(fireBall, fireBallTransform.transform.position,
            fireBallTransform.transform.rotation, GetObjects.instance.getAttackParticleContainer());
    }

    public void dealMeleeDamage()
    {
        // Increment to the next melee attack
        meleeAttackNumber++;
        if (meleeAttackNumber > 2)
        {
            meleeAttackNumber = 0;
        }
        anim.SetInteger("MeleeAttackNumber", meleeAttackNumber);

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
