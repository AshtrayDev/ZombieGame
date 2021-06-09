using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float chaseRange;
    [SerializeField] float attackRange;

    NavMeshAgent navMeshAgent;
    bool isProvoked = false;
    float currentDistanceFromTarget = Mathf.Infinity;

    // Start is called before the first frame update
    void Start()
    {
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
        GetComponent<Animator>().SetTrigger("move");
    }

    void AttackTarget()
    {
        print("Attacking!");
        GetComponent<Animator>().SetBool("attack", true);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
