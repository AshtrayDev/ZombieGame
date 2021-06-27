using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDelay : MonoBehaviour
{
    List<Weapon> weapons = new List<Weapon>();
    List<bool> weaponDelays = new List<bool>();

    private void Start()
    {
        RefreshLists();
    }

    private void RefreshLists()
    {
        foreach(Transform weapon in transform)
        {
            weapons.Add(weapon.GetComponent<Weapon>());
            weaponDelays.Add(false);
        }
    }

    IEnumerator DelayWeapon(int weaponIndex, float shotDelay)
    {
        weaponDelays[weaponIndex] = true;
        yield return new WaitForSeconds(shotDelay);
        weaponDelays[weaponIndex] = false;
    }

    public bool CanWeaponShoot(Weapon weapon)
    {
        int weaponIndex = weapons.IndexOf(weapon);

        if (weaponDelays[weaponIndex] == true)
        {
            return false;
        }

        else
        {
            return true;
        }

    }

    public void StartDelay(Weapon weapon)
    {
        int weaponIndex = weapons.IndexOf(weapon);
        StartCoroutine(DelayWeapon(weaponIndex, weapon.GetShotDelay()));
    }
}
