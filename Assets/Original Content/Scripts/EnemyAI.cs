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

    NavMeshAgent navMeshAgent;
    bool isProvoked = false;
    float currentDistanceFromTarget = Mathf.Infinity;

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
        Debug.Log(target.position);
        Debug.Log(navMeshAgent.remainingDistance);
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
        GetComponent<Animator>().SetTrigger("move");
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
    }

    void OnDamageTaken()
    {
        isProvoked = true;
        Debug.Log("Test");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
