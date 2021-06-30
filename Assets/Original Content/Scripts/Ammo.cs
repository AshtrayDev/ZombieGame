using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField] AmmoSlot[] ammoSlots;
    [System.Serializable]
    private class AmmoSlot
    {
        public AmmoType ammoType;
        public int ammoAmount;
    }

    public int GetCurrentAmmo(AmmoType ammoType)
    {
        if(ammoType == null) { return 0; }
        return GetAmmoSlot(ammoType).ammoAmount;
    }

    public void ReduceCurrentAmmo(int amount, AmmoType ammoType)
    {
        GetAmmoSlot(ammoType).ammoAmount = GetAmmoSlot(ammoType).ammoAmount - amount; 
    }

    public void AddCurrentAmmo(int amount, AmmoType ammoType)
    {
        GetAmmoSlot(ammoType).ammoAmount = GetAmmoSlot(ammoType).ammoAmount + amount;
    }

    private AmmoSlot GetAmmoSlot(AmmoType ammoType)
    {
        foreach (AmmoSlot ammoSlot in ammoSlots)
        {
            if(ammoSlot.ammoType == ammoType)
            {
                return ammoSlot;
            }
        }

        return null;
    }
}
