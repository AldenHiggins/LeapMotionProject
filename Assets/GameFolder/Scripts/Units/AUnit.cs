using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AUnit : MonoBehaviour, IUnit
{
    // UNIT CHARACTERISTICS
    [SerializeField]
    protected int startingHealth = 100;
    [SerializeField]
    protected float attackRadius = 2.0f;
    [SerializeField]
    protected int attackDamage = 1;
    [SerializeField]
    protected float bodyDespawnTime = 10.0f;
    // ALLY/ENEMY
    protected bool isAlly = false;
    // HEADSHOTS
    [SerializeField]
    protected float headshotHeight = 1.5f;
    // ANIMATOR
    protected Animator anim;
    protected int health;
    protected bool attacking = false;
    // MOVEMENT
    protected UnityEngine.AI.NavMeshAgent agent;
    protected IUnit target;
    // ENEMY SEARCH
    protected int enemySearchLayer = (1 << 16) | (1 << 11);
    [SerializeField]
    protected float enemySearchRadius = 10.0f;
    // AUDIO
    protected AudioSource source;
    [SerializeField]
    protected AudioClip woundSound;
    [SerializeField]
    protected AudioClip killSound;
    // RAGDOLL/DEATH
    protected bool isDying = false;
    // DELEGATES
    private Action onDeath;
    private Action<int> onDamageTaken;

    // Use this for initialization
    void Start()
    {
        // Register the unit's components
        anim = transform.GetChild(0).gameObject.GetComponent<Animator>();
        agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        source = GetComponent<AudioSource>();

        health = startingHealth;
        agent.updateRotation = true;

        initializeUnit();
    }

    abstract protected void initializeUnit();

    // Update is called once per frame
    void Update()
    {
        // If the unit is dying don't do anything
        if (health <= 0)
        {
            return;
        }

        // Search for a new target
        findTarget();

        // If the unit has no target idle
        if (target == null)
        {
            // Stop running
            anim.SetBool("Running", false);
            anim.SetBool("Attacking", false);
            agent.isStopped = true;
            return;
        }

        // Determine if the unit should start attacking its target
        Vector3 targetVector = target.getGameObject().transform.position - transform.position;
        targetVector.y = 0.0f;
        float distanceToTarget = targetVector.magnitude;

        // If the enemy is outside attack range keep coming forward
        if (distanceToTarget > attackRadius)
        {
            moveToTarget();
        }
        // If the target is in range attack it
        else
        {
            attackTarget();
        }
    }

    protected virtual void moveToTarget()
    {
        // If we're still attacking wait to finish
        if (attacking == true)
        {
            return;
        }

        // Animate the unit to start running
        anim.SetBool("Running", true);
        anim.SetBool("Attacking", false);

        // Activate our nav mesh agent and start moving to our destination
        if (!agent.isActiveAndEnabled)
        {
            agent.enabled = true;
        }
        agent.isStopped = false;
        agent.SetDestination(target.getGameObject().transform.position);
    }

    protected virtual void attackTarget()
    {
        // Stop running
        anim.SetBool("Running", false);
        agent.isStopped = true;

        // Face the target
        transform.rotation = Quaternion.LookRotation(target.getGameObject().transform.position - transform.position);
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

        // If the unit isn't attacking yet start the attack function
        if (attacking == false)
        {
            attacking = true;
            StartCoroutine(attack());
        }
    }

    protected virtual void findTarget()
    {
        IUnit newTarget = findClosestUnit();

        if (newTarget != null)
        {
            target = newTarget;
        }
    }

    protected IUnit findClosestUnit()
    {
        IUnit foundTarget = null;

        // If we have a target and we are attacking it don't switch
        if (target != null)
        {
            // If our target is dying null it out
            if (target.isUnitDying())
            {
                anim.SetBool("Attacking", false);
                attacking = false;
                target = null;
            }
            // If the target is still alive and we are attacking don't switch targets
            else if (attacking)
            {
                return foundTarget;
            }
        }

        // Sphere cast around the unit for a target
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, enemySearchRadius, enemySearchLayer);
        float minimumDistance = float.MaxValue;
        // Find the closest enemy
        for (int hitIndex = 0; hitIndex < hitColliders.Length; hitIndex++)
        {
            IUnit enemyUnit = (IUnit) hitColliders[hitIndex].gameObject.GetComponent(typeof(IUnit));
            if (enemyUnit != null && !enemyUnit.isUnitDying())
            {
                float enemyDistance = Vector3.Distance(enemyUnit.getGameObject().transform.position, gameObject.transform.position);
                if (enemyDistance < minimumDistance)
                {
                    foundTarget = enemyUnit;
                    minimumDistance = enemyDistance;
                }
            }
        }

        return foundTarget;
    }

    IEnumerator attack()
    {
        anim.SetBool("Attacking", true);
        // Let the animation play for half a second before dealing damage to the target
        yield return new WaitForSeconds(.5f);

        // Check if our target is within the attack radius and isn't already dying
        Vector3 distance = target.getGameObject().transform.position - transform.position;
        distance.y = 0.0f;
        if (distance.magnitude <= attackRadius)
        {
            // Check to see if our target has died
            if (target.isUnitDying())
            {
                attacking = false;
                anim.SetBool("Attacking", false);
                yield break;
            }

            target.dealDamage(attackDamage);
        }

        // Wait another second before attacking again
        yield return new WaitForSeconds(2.0f);
        attacking = false;
    }

    public void dealDamage(int damage)
    {
        health -= damage;

        if (onDamageTaken != null)
        {
            onDamageTaken(damage);
        }

        if (health > 0)
        {
            anim.SetBool("Wounded", true);
            StopCoroutine(endWound());
            StartCoroutine(endWound());
            source.PlayOneShot(woundSound);
        }
        else
        {
            source.PlayOneShot(killSound);
            kill();
        }
    }

    IEnumerator endWound()
    {
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("Wounded", false);
    }

    public void applyForce(Vector3 force)
    {
        GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        GetComponent<Rigidbody>().isKinematic = false;
        StartCoroutine(restartAgent());
    }

    public void applyExplosiveForce(float force, Vector3 position, float radius)
    {
        GetComponent<Rigidbody>().AddExplosionForce(force, position, radius, 20.0f, ForceMode.Impulse);
        GetComponent<Rigidbody>().isKinematic = false;
        StartCoroutine(restartAgent());
    }

    IEnumerator restartAgent()
    {
        while (GetComponent<Rigidbody>().velocity.magnitude > 0.1)
        {
            yield return new WaitForSeconds(.1f);
        }
        agent.enabled = true;
    }

    void kill()
    {
        isDying = true;
        if (agent.enabled)
        {
            agent.enabled = false;
        }
        // Call our on death callbacks if necessary
        if (onDeath != null)
        {
            onDeath();
        }
        BoxCollider collider = gameObject.GetComponent<BoxCollider>();
        collider.enabled = false;
        StopCoroutine(attack());
        anim.SetBool("Attacking", false);
        anim.SetBool("Dead", true);
        StartCoroutine(despawnBody());
    }

    IEnumerator despawnBody()
    {
        yield return new WaitForSeconds(bodyDespawnTime);
        Destroy(gameObject);
    }

    public void installDeathListener(Action onDeathCallback)
    {
        onDeath += onDeathCallback;
    }

    public void installDamageListener(Action<int> onDamageCallback)
    {
        onDamageTaken += onDamageCallback;
    }

    public int getCurrentHealth()
    {
        return health;
    }

    public int getMaxHealth()
    {
        return startingHealth;
    }

    public void slowDown()
    {
        agent.speed = 1;
    }

    public bool isUnitDying()
    {
        return isDying;
    }

    public virtual bool isUnitAlly()
    {
        return isAlly;
    }

    public GameObject getGameObject()
    {
        return gameObject;
    }
}
