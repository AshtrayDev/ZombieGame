using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float health = 100f;

    // Start is called before the first frame update
    void Start()
    {

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
            print("You have died!");
        }
    }

}
