using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float health = 100f;

    public void TakeDamage(float amount)
    {
        health = health - amount;

        BroadcastMessage("OnDamageTaken");
        CheckHealthAmount();
    }

    void CheckHealthAmount()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
