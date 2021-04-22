using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float attackRange;

    NavMeshAgent navMeshAgent;
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

        if (currentDistanceFromTarget < attackRange)
        {
            navMeshAgent.SetDestination(target.position);
        }

        else
        {
            navMeshAgent.SetDestination(transform.position);
        }
    }
}
