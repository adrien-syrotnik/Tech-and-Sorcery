using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, ICanTakeDamage
{
    public float health = 1f;
    public float damage = 1;

    public float damageDelay = 0.5f;

    private bool canTakeDamage = true;

    private GameObject player;

    public float minDistanceFromThePlayer = 3f;
    public float speed = 2f;
    public float rotationSpeed = 2f;

    public float attackSpeed = 1f;
    
    protected Animator animator;

    private bool canAttack = true;
    private bool isAttacking = false;

    public bool isDead = false;

    private NavMeshAgent agent;

    public LayerMask whatIsGround;

    protected virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("MainCamera");
        animator = GetComponent<Animator>();
        animator.SetFloat("AttackSpeed", 1/(attackSpeed));
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
            agent = gameObject.AddComponent<NavMeshAgent>();

        agent.speed = speed;
        agent.angularSpeed = rotationSpeed;
        agent.stoppingDistance = minDistanceFromThePlayer;
    }

    protected virtual void Update()
    {
        if (isDead)
            return;
        
        
        Vector3 targetPos = player.transform.position;
        targetPos.y = 0;

        float distanceFromThePlayer = Vector3.Distance(transform.position, targetPos);

        

        if(!isAttacking && canTakeDamage && agent.enabled)
        {
            agent.SetDestination(targetPos);
        }

        if (agent.enabled == false && canTakeDamage)
        {
            //Check if y velocity is 0
            if (GetComponent<Rigidbody>() == null || GetComponent<Rigidbody>().velocity.y == 0)
            {
                agent.enabled = true;
            }
        }

        //Go to the player
        if (agent.velocity.magnitude > 0.1f && !isAttacking)
        {
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsAttacking", false);
        }
        else if (canAttack && distanceFromThePlayer <= minDistanceFromThePlayer)
        {
            animator.SetBool("IsAttacking", true);
            animator.SetBool("IsWalking", false);
            //Attack the player
            StartCoroutine(Attack());
        }

        
    }

    

    protected virtual IEnumerator Attack()
    {
        canAttack = false;
        isAttacking = true;

        yield return new WaitForSeconds(attackSpeed);

        //If hit the player give damage to him
        Player player = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Player>();
        if (player != null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= minDistanceFromThePlayer)
                player.TakeDamage(damage);
        }


        canAttack = true;
        isAttacking = false;

    }

    protected virtual void Die()
    {
        isDead = true;
        agent.enabled = false;

        
    }

    public virtual void TakeDamage(float damage)
    {
        if (canTakeDamage)
        {
            health -= damage;
            if (health <= 0)
            {
                Die();
            }
            StartCoroutine(DamageDelay());
        }
    }

    protected virtual IEnumerator DamageDelay()
    {
        canTakeDamage = false;
        agent.enabled = false;
        yield return new WaitForSeconds(damageDelay);
        canTakeDamage = true;
    }
}
