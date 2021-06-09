using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    GameObject target;
    [SerializeField] float damage = 25;


    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<PlayerHealth>().gameObject;
    }

    public void AttackHitEvent()
    {
        if(target == null) { return; }

        target.GetComponent<PlayerHealth>().ChangeHealth(-damage);
        Debug.Log("Enemy dealt " + damage + " damage!");
    }
}
