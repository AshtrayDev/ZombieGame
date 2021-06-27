using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDelay : MonoBehaviour
{
    List<Weapon> weapons = new List<Weapon>();

    private void Start()
    {
        RefreshWeaponsList();
    }

    private void RefreshWeaponsList()
    {
        foreach(Transform weapon in transform)
        {
            weapons.Add(weapon.GetComponent<Weapon>());
        }
    }

    IEnumerator StartDelay(int weaponIndex)
    {
        yield return new WaitForSeconds(1f);
    }
}
