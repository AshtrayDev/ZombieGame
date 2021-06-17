using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float health = 100f;

    DeathHandler deathHandler;

    // Start is called before the first frame update
    void Start()
    {
        deathHandler = GetComponent<DeathHandler>();
    }

    public void ChangeHealth(float addedHealth)
    {
        health = health + addedHealth;
        CheckHealth();
    }

    void CheckHealth()
    {
        if(health <= 0)
        {
            deathHandler.DeathSequence();
        }
    }

}
