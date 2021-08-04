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
        uiHandler.SetRoundUIText(roundNum);
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
        enemy.GetComponent<EnemyAI>().isRunner = TrySpawnRunner();
        zombiesActive++;
        zombiesLeftToSpawn--;
    }

    public void DestroyZombie()
    {
        zombiesActive--;
    }

    public ZombieSpawnPoint RandomSpawnPoint()
    {
        List<ZombieSpawnPoint> spawnPoints = new List<ZombieSpawnPoint>();

        foreach (ZombieSpawnPoint point in FindObjectsOfType<ZombieSpawnPoint>())
        {
            if (point.IsActive())
            {
                spawnPoints.Add(point);
            }
        }

        return spawnPoints[Random.Range(0, spawnPoints.Count - 1)];
    }

    bool TrySpawnRunner()
    {
        if(roundNum > 10)
        {
            return true;
        }

        else if(roundNum < 5)
        {
            return false;
        }

        else
        {
            if(Random.Range(0, 11 - roundNum) == 1)
            {
                return true;
            }

            else
            {
                return false;
            }
        }
    }
}
