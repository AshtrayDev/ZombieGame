using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPerk : MonoBehaviour
{
    List<PerkList> perks = new List<PerkList>();
    List<Pickup.PickupType> pickups = new List<Pickup.PickupType>();
    List<int> timers = new List<int>();

    public float reloadSpeed = 1f;
    public float delayMultiplier = 1f;
    public bool quickReviveOn = false;

    UIHandler ui;

    // Start is called before the first frame update
    void Start()
    {
        ui = FindObjectOfType<UIHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPerk(PerkList perk)
    {
        perks.Add(perk);
        PerkEffect(true, perk);
        ui.AddPerk(perk, perks.Count - 1);
    }

    public void RemovePerk(PerkList perk)
    {
        PerkEffect(false, perk);
        ui.RemovePerk(perks.IndexOf(perk));
        perks.Remove(perk);
    }

    public void PerkEffect(bool state, PerkList perkName)
    {
        if(perkName == PerkList.Juggernog)
        {
            Juggernog(state);
        }

        if (perkName == PerkList.SpeedCola)
        {
            SpeedCola(state);
        }

        if (perkName == PerkList.DoubleTap)
        {
            DoubleTap(state);
        }

        if (perkName == PerkList.QuickRevive)
        {
            QuickRevive(state);
        }
    }

    void Juggernog(bool state)
    {
        PlayerHealth pHealth = GetComponent<PlayerHealth>();

        if (state)
        {
            pHealth.ChangeMaxHealth(pHealth.GetMaxHealth() * 2);
        }

        else
        {
            pHealth.ChangeMaxHealth(pHealth.GetMaxHealth() / 2);
        }
    }

    void SpeedCola(bool state)
    {
        if (state)
        {
            reloadSpeed = 2f;
        }

        else
        {
            reloadSpeed = 1f;
        }
    }

    void DoubleTap(bool state)
    {
        if (state)
        {
            delayMultiplier = 0.5f;
        }

        else
        {
            delayMultiplier = 1f;
        }
    }

    void QuickRevive(bool state)
    {
        if (state)
        {
            quickReviveOn = true;
        }

        else
        {
            quickReviveOn = false;
        }
    }

    public bool HasPerk(PerkList checkedPerk)
    {
        foreach(PerkList perk in perks)
        {
            if(perk == checkedPerk)
            {
                return true;
            }
        }

        return false;
    }

    public void RemoveAllPerks()
    {
        foreach(PerkList perk in perks)
        {
            PerkEffect(false, perk);
            ui.RemovePerk(perks.IndexOf(perk));
        }

        perks.Clear();
    }

    //==========================================================================================================================


    public void AddPickup(Pickup.PickupType type, int timer)
    {
        if(pickups.IndexOf(type) != -1)
        {
            Debug.LogWarning("Already have pickup, will add to timer.");
            timers[pickups.IndexOf(type)] += timer;
        }
        else
        {
            pickups.Add(type);
            ui.AddPickup(type);
            StartCoroutine(AddTimer(timer, type));
        }
    }

    IEnumerator AddTimer(int timer, Pickup.PickupType type)
    {
        timers.Add(timer);
        while(timers[pickups.IndexOf(type)] > 0)
        {
            yield return new WaitForSeconds(1f);
            timers[pickups.IndexOf(type)]--;
        }

        timers.RemoveAt(pickups.IndexOf(type));
        pickups.Remove(type);
        ui.RemovePickup(type);
    }

    public bool HasGotPickup(Pickup.PickupType type)
    {
        foreach(Pickup.PickupType pickup in pickups)
        {
            if(type == pickup)
            {
                return true;
            }
        }

        return false;
    }
}
