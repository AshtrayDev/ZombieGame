using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] int pointCost;
    [SerializeField] Vector3 size = new Vector3 (1,1,1);

    bool isTriggered;
    [SerializeField] List<ZombieSpawnPoint> unlockedSpawnPoints = new List<ZombieSpawnPoint>();
    GameObject player;

    BoxCollider boxCollider;
    UIHandler uiHandler;

    private void Awake()
    {
        uiHandler = FindObjectOfType<UIHandler>();
    }
    private void Start()
    {
        boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.size = size;
        boxCollider.isTrigger = true;
    }

    private void Update()
    {
        if (isTriggered)
        {
            if (Input.GetButton("Interact"))
            {
                OpenDoor();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isTriggered = true;
        player = other.gameObject;
        uiHandler.SetTooltipDoor("Press & Hold F to Open Door", pointCost);
    }

    private void OnTriggerExit(Collider other)
    {
        isTriggered = false;
        player = null;
        uiHandler.SetActiveTooltipUI(false);
    }

    void OpenDoor()
    {
        PlayerPoints points = player.GetComponent<PlayerPoints>();
        if (points.IsAbleToAfford(pointCost))
        {
            points.RemovePoints(pointCost);
            uiHandler.SetActiveTooltipUI(false);
            foreach(ZombieSpawnPoint spawnPoint in unlockedSpawnPoints)
            {
                spawnPoint.SetActive(true);
            }
            Destroy(gameObject);
        }

    }
}
