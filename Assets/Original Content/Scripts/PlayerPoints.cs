using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPoints : MonoBehaviour
{
    [SerializeField] int startingPoints;

    UIHandler uiHandler;

    int currentPoints;

    private void Awake()
    {
        uiHandler = FindObjectOfType<UIHandler>();
    }
    private void Start()
    {
        AddPoints(startingPoints);
    }

    public bool IsAbleToAfford(int cost)
    {
        if(currentPoints >= cost)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    public void AddPoints(int amount)
    {
        currentPoints = currentPoints + amount;
        uiHandler.SetPointsUIText(currentPoints);
    }

    public void RemovePoints(int amount)
    {
        currentPoints = currentPoints - amount;
        uiHandler.SetPointsUIText(currentPoints);
    }

}
