using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPerk : MonoBehaviour
{
    List<PerkList> perks = new List<PerkList>();

    public float reloadSpeed = 1f;
    public float delayMultiplier = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPerk(PerkList perk)
    {
        perks.Add(perk);
        PerkEffect(true, perk);
    }

    public void RemovePerk(PerkList perk)
    {
        perks.Remove(perk);
        PerkEffect(false, perk);
    }

    public void PerkEffect(bool state, PerkList perkName)
    {
        if(perkName == PerkList.juggernog)
        {
            Juggernog(state);
        }

        if (perkName == PerkList.speedCola)
        {
            SpeedCola(state);
        }

        if (perkName == PerkList.doubleTap)
        {
            DoubleTap(state);
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
}
