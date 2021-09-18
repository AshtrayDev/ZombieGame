using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float health = 100f;
    [SerializeField] [Range(0f, 10f)] float destroyDelay = 1f;

    public enum DamageType {body, headshot, melee, explosive}

    bool isDead = false;

    ZombieSpawner zombieSpawner;
    PlayerPoints playerPoints;
    LevelSettings settings;

    private void Awake()
    {
        zombieSpawner = FindObjectOfType<ZombieSpawner>();
        playerPoints = FindObjectOfType<PlayerPoints>();
        settings = FindObjectOfType<LevelSettings>();
    }

    public void TakeDamage(float amount, DamageType damageType)
    {
        health = health - amount;
        playerPoints.AddPoints(10);
        BroadcastMessage("OnDamageTaken", SendMessageOptions.DontRequireReceiver);
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

            if (damageType == DamageType.melee)
            {
                playerPoints.AddPoints(130);
            }
        }
    }

    public void SetHealth(float health)
    {
        this.health = health;
        IsHealthZero();
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
        ChanceToSpawnPickup();
        GetComponent<Animator>().SetTrigger("death");
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<EnemyAI>().enabled = false;
        zombieSpawner.DestroyZombie();
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }

    void ChanceToSpawnPickup()
    { 
        if (Random.Range(0, 100) <= settings.chanceOfSpawning) //Chance that spawn is happening
        {
            Vector3 spawnPos = new Vector3(0, settings.pickupYOffset, 0);
            spawnPos = spawnPos + transform.position; //Setting spawn location

            int random = Random.Range(0, settings.pickupsList.Length - 1); //Choosing random pickup

            Instantiate(settings.pickupsList[random], spawnPos, Quaternion.identity, settings.pickupsTransform); //Actual spawning
        }
    }

    public void OnExplosiveHit(float damage)
    {
        TakeDamage(damage, DamageType.body);
    }

    public bool IsDead()
    {
        return isDead;
    }
}
