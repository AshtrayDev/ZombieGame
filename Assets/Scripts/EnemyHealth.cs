using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float health = 100f;

    public void ChangeHealth(bool state, float amount)
    {
        if (state)
        {
            health = health + amount;
        }

        else
        {
            health = health - amount;
        }

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
