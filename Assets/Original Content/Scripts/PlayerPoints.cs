using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPoints : MonoBehaviour
{
    [SerializeField] int startingPoints;

    UIHandler uiHandler;
    PlayerPerk perk;

    int currentPoints;

    private void Awake()
    {
        uiHandler = FindObjectOfType<UIHandler>();
        perk = FindObjectOfType<PlayerPerk>();
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
        if (perk.HasGotPickup(Pickup.PickupType.doublePoints))
        {
            currentPoints = currentPoints + amount * 2;
            uiHandler.SetPointsUIText(currentPoints);
            uiHandler.AddPointsUI(amount, true);
        }

        currentPoints = currentPoints + amount;
        uiHandler.SetPointsUIText(currentPoints);
        uiHandler.AddPointsUI(amount, true);
    }

    public void RemovePoints(int amount)
    {
        currentPoints = currentPoints - amount;
        uiHandler.SetPointsUIText(currentPoints);
        uiHandler.AddPointsUI(amount, false);
    }

}
