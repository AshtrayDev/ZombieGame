using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    GameObject player;
    [SerializeField] float damage = 25;
    EnemyAI ai;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerHealth>().gameObject;
        ai = GetComponent<EnemyAI>();
    }

    public void AttackHitEvent()
    {
        if(player == null) { return; }

        player.GetComponent<PlayerHealth>().AddHealth(-damage);
        Debug.Log("Enemy dealt " + damage + " damage!");
    }

    public void AttackBarrierEvent()
    {
        if(ai.target.GetComponent<Barrier>() != null)
        {
            ai.target.GetComponent<Barrier>().AttackBarrier();
        }
    }
}
