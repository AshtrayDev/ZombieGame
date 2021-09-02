using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSettings : MonoBehaviour
{
    [Header("Pickups")]
    public Transform pickupsTransform;
    public GameObject[] pickupsList;
    public float pickupYOffset = 2;
    [Range(0, 100)] public float chanceOfSpawning;

    [Header("Points")]
    public int startPoints = 500;
    public int pointsPerHit = 10;
    public int bodyshotKillPoints = 60;
    public int headshotKillPoints = 100;
    public int knifeKillPoints = 130;
    public int carpenterPoints = 400;
    public int nukePoints = 400;

    private void Start()
    {
        if(pickupsTransform == null)
        {
            Debug.LogError("pickupsTransform is null! Please fix.");
        }

        if(pickupsList.Length == 0)
        {
            Debug.LogWarning("Pickups in Level Settings is empty.");
        }

        if(chanceOfSpawning == 0)
        {
            Debug.LogWarning("chanceOfSpawning is 0. Pickups will not spawn.");
        }
    }
}
