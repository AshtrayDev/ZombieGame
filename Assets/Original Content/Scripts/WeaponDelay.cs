using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDelay : MonoBehaviour
{
    List<Weapon> weapons = new List<Weapon>();
    List<bool> weaponDelays = new List<bool>();

    WeaponSwitcher switcher;
    PlayerPerk perk;
    float delayMultiplier = 1f;

    private void Awake()
    {
        switcher = GetComponent<WeaponSwitcher>();
        perk = FindObjectOfType<PlayerPerk>();
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
        StartCoroutine(DelayWeapon(weaponIndex, weapon.GetShotDelay() * perk.delayMultiplier));
    }

    public void AddWeapon(Weapon weapon)
    {
        weapons.Add(weapon);
        weaponDelays.Add(false);
        switcher.AddWeapon(weapon);
    }

    public void RemoveWeapon(Weapon weapon)
    {
        weaponDelays.RemoveAt(weapons.IndexOf(weapon));
        weapons.Remove(weapon);
        switcher.RemoveWeapon(weapon);
    }
}
