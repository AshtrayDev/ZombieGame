using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] int maxAmountOfZombies = 24;
    [SerializeField] GameObject zombiePrefab;
    [SerializeField] Transform spawnPoint;

    int roundNum;
    int zombieHP;
    int zombiesLeftToSpawn;
    int zombiesActive;

    bool roundActive;

    UIHandler uiHandler;
    
    
    RoundSystem roundSystem;

    private void Awake()
    {
        uiHandler = FindObjectOfType<UIHandler>();
        roundSystem = GetComponent<RoundSystem>();
    }

    private void Start()
    {
        StartRound();
    }

    private void Update()
    {
        if (zombiesActive < maxAmountOfZombies && zombiesLeftToSpawn > 0)
        {
            SpawnZombie();
        }

        else if (zombiesActive == 0 && zombiesLeftToSpawn == 0)
        {
            EndRound();
        }
    }

    void StartRound()
    {
        roundActive = true;
        (roundNum, zombieHP, zombiesLeftToSpawn) = roundSystem.GetNewRoundStats();
    }

    void EndRound()
    {
        roundActive = false;
        roundSystem.RoundEnd();
        StartRound();
    }

    void SpawnZombie()
    {
        GameObject enemy = Instantiate(zombiePrefab, RandomSpawnPoint().transform.position, Quaternion.identity);
        enemy.GetComponent<EnemyHealth>().SetHealth(zombieHP);
        zombiesActive++;
        zombiesLeftToSpawn--;
    }

    public void DestroyZombie()
    {
        zombiesActive--;
    }

    public ZombieSpawnPoint RandomSpawnPoint()
    {
        ZombieSpawnPoint[] spawnPoints = FindObjectsOfType<ZombieSpawnPoint>();

        return spawnPoints[Random.Range(0, spawnPoints.Length - 1)];
    }
}
