using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float health = 100f;
    [SerializeField] [Range(0f, 10f)] float destroyDelay = 1f;

    bool isDead = false;



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
            StartCoroutine(DeathSequence());
        }
    }

    IEnumerator DeathSequence()
    {
        if (isDead) yield break;

        GetComponent<CapsuleCollider>().enabled = false;

        isDead = true;
        GetComponent<Animator>().SetTrigger("death");
        GetComponent<EnemyAI>().enabled = false;
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }

    public bool IsDead()
    {
        return isDead;
    }
}
