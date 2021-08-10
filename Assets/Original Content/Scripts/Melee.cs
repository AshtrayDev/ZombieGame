using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{

    [SerializeField] float meleeDamage;
    PlayerHealth playerHealth;
    BoxCollider collider;

    private void Awake()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        collider = GetComponent<BoxCollider>();
        
    }
    private void Update()
    {
        if (Input.GetButtonDown("Melee"))
        {
            GetComponent<Animator>().SetTrigger("MeleeTrigger");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Hit");

        if (other.GetComponentInParent<EnemyHealth>() != null)
        {
            other.GetComponentInParent<EnemyHealth>().TakeDamage(meleeDamage, EnemyHealth.DamageType.melee);
        }
        
        collider.enabled = false;
    }

    public void KnifeAnimStart()
    {
        collider.enabled = true;
    }
}
