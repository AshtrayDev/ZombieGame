using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    Transform player;
    public Transform target;
    [SerializeField] float chaseRange = 5;
    [SerializeField] float attackRange = 2;
    [SerializeField] float turnSpeed = 2;
    [SerializeField] float walkSpeed = 2;
    [SerializeField] float runSpeed = 5;

    NavMeshAgent navMeshAgent;
    Animator animator;
    public bool isThroughBarrier = false;
    bool isClimbing = false;
    bool hasMoved = false;
    public bool isAttackingBarrier = false;
    float currentDistanceFromTarget = Mathf.Infinity;

    public bool isRunner = false;

    // Start is called before the first frame update
    void Start()
    {
        target = null;
        player = FindObjectOfType<PlayerHealth>().transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isClimbing && hasMoved) { return; }

        if(target != null)
        {
            currentDistanceFromTarget = Vector3.Distance(target.position, transform.position); 
        }

        if (isAttackingBarrier && !isThroughBarrier)
        {
            AttackTarget();
        }

        else if (!isThroughBarrier && !isAttackingBarrier)
        {
            EngageBarrier(FindClosestBarrier());
        }

        else if (currentDistanceFromTarget < chaseRange)
        {
            EngagePlayer();   
        }
    }

    void EngageBarrier(Barrier barrier)
    {
        target = barrier.transform;
        navMeshAgent.SetDestination(target.transform.position);
        FaceTarget();

        if (currentDistanceFromTarget == Mathf.Infinity || currentDistanceFromTarget == 0)
        {
            ChaseTarget();
        }

        else if (currentDistanceFromTarget > attackRange)
        {
            ChaseTarget();
        }

        else if (currentDistanceFromTarget <= attackRange && isAttackingBarrier)
        {
            AttackTarget();
        }
    }

    void EngagePlayer()
    {
        if (FindObjectOfType<DeathHandler>().isDying)
        {
            target = FindClosestBarrier().transform;
            navMeshAgent.SetDestination(target.position);
            FaceTarget();
        }

        else
        {
            target = FindObjectOfType<PlayerHealth>().transform;
            navMeshAgent.SetDestination(target.position);
            FaceTarget();
        }




        if (currentDistanceFromTarget == Mathf.Infinity)
        {
            ChaseTarget();
        }
        
        if (currentDistanceFromTarget > attackRange) 
        {
            ChaseTarget();
        }

        else if (currentDistanceFromTarget <= attackRange)
        {
            AttackTarget();
        }
    }

    void ChaseTarget()
    {
        navMeshAgent.SetDestination(target.position);
        animator.SetBool("attack", false);
        animator.SetBool("attackBarrier", false);

        if (isRunner)
        {
            animator.SetTrigger("run");
            hasMoved = true;
            navMeshAgent.speed = runSpeed;
        }

        else
        {
            animator.SetTrigger("walk");
            hasMoved = true;
            navMeshAgent.speed = walkSpeed;
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    void AttackTarget()
    {
        if (!isThroughBarrier)
        {
            if (target.GetComponent<Barrier>().IsBarrierClear())
            {
                navMeshAgent.SetDestination(player.position);
                navMeshAgent.speed = 1;
                animator.SetTrigger("climb");
                isClimbing = true;
            }
        }

        if (isThroughBarrier)
        {
            animator.SetBool("attack", true);
            navMeshAgent.speed = 0;
        }

        else if(!isClimbing)
        {
            animator.SetBool("attackBarrier", true);
            navMeshAgent.speed = 0;
        }

    }

    Barrier FindClosestBarrier()
    {
        Vector3 currentPos = transform.position;
        Barrier closestBarrier = null;

        foreach(Barrier barrier in FindObjectsOfType<Barrier>())
        {
            if(closestBarrier == null)
            {
                closestBarrier = barrier;
            }

            else if(Vector3.Distance(currentPos, closestBarrier.transform.position) > Vector3.Distance(currentPos, barrier.transform.position))
            {
                closestBarrier = barrier;
            }
        }
        return closestBarrier;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void ClimbFinishEvent()
    {
        isThroughBarrier = true;
        isClimbing = false;
    }


}
