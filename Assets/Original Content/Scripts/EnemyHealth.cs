using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float health = 100f;
    [SerializeField] [Range(0f, 10f)] float destroyDelay = 1f;

    public enum DamageType {body, headshot, melee, explosive}

    bool isDead = false;

    ZombieSpawner zombieSpawner;
    PlayerPoints playerPoints;

    private void Awake()
    {
        zombieSpawner = FindObjectOfType<ZombieSpawner>();
        playerPoints = FindObjectOfType<PlayerPoints>();
    }

    public void TakeDamage(float amount, DamageType damageType)
    {
        health = health - amount;
        playerPoints.AddPoints(10);
        BroadcastMessage("OnDamageTaken");
        if (IsHealthZero())
        {
            if(damageType == DamageType.body)
            {
                playerPoints.AddPoints(60);
            }

            if (damageType == DamageType.headshot)
            {
                playerPoints.AddPoints(100);
            }
        }
    }

    public void SetHealth(float health)
    {
        this.health = health;
    }

    bool IsHealthZero()
    {
        if(health <= 0)
        {
            StartCoroutine(DeathSequence());
            return true;
        }
        return false;
    }

    IEnumerator DeathSequence()
    {
        if (isDead) yield break;

        foreach(Collider collider in GetComponentsInChildren<Collider>())
        collider.enabled = false;

        isDead = true;
        GetComponent<Animator>().SetTrigger("death");
        GetComponent<EnemyAI>().enabled = false;
        zombieSpawner.DestroyZombie();
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }

    public bool IsDead()
    {
        return isDead;
    }
}
