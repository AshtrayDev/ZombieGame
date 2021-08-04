using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float chaseRange = 5;
    [SerializeField] float attackRange = 2;
    [SerializeField] float turnSpeed = 2;
    [SerializeField] float walkSpeed = 2;
    [SerializeField] float runSpeed = 5;

    NavMeshAgent navMeshAgent;
    bool isProvoked = false;
    float currentDistanceFromTarget = Mathf.Infinity;

    public bool isRunner = false;

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<PlayerHealth>().transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        currentDistanceFromTarget = Vector3.Distance(target.position, transform.position);

        if (isProvoked)
        {
            EngageTarget();
        }

        else if (currentDistanceFromTarget < chaseRange)
        {
            isProvoked = true;
            EngageTarget();   
        }
    }

    void EngageTarget()
    {
        navMeshAgent.SetDestination(target.position);
        FaceTarget();



        if (navMeshAgent.remainingDistance == Mathf.Infinity)
        {
            ChaseTarget();
        }
        
        if (navMeshAgent.remainingDistance > attackRange) 
        {
            ChaseTarget();
        }

        else if (navMeshAgent.remainingDistance <= attackRange)
        {
            AttackTarget();
        }
    }

    void ChaseTarget()
    {
        navMeshAgent.SetDestination(target.position);
        GetComponent<Animator>().SetBool("attack", false);

        if (isRunner)
        {
            GetComponent<Animator>().SetTrigger("run");
            navMeshAgent.speed = runSpeed;
        }

        else
        {
            GetComponent<Animator>().SetTrigger("walk");
            navMeshAgent.speed = walkSpeed;
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    void AttackTarget()
    {
        GetComponent<Animator>().SetBool("attack", true);
        navMeshAgent.speed = 0;
    }

    void OnDamageTaken()
    {
        isProvoked = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
