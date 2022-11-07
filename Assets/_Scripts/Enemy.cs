using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int health = 5;

    private GameObject player;

    public float minDistanceFromThePlayer = 3f;
    public float speed = 2f;
    public float rotationSpeed = 2f;

    public float attackSpeed = 1f;
    
    protected Animator animator;

    private bool canAttack = true;

    protected bool isDead = false;

    private NavMeshAgent agent;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("MainCamera");
        animator = GetComponent<Animator>();
        animator.SetFloat("AttackSpeed", 1/attackSpeed);
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

        

        if(!canAttack)
        {
            agent.SetDestination(targetPos);
        }

        //Go to the player
        if (distanceFromThePlayer > minDistanceFromThePlayer && !canAttack)
        {
            
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsAttacking", false);
        }
        else if (canAttack)
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
        Debug.Log("Attack");
        //player.GetComponent<Player>().TakeDamage(1);

        yield return new WaitForSeconds(attackSpeed);
        canAttack = true;
        
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        isDead = true;
    }
}
