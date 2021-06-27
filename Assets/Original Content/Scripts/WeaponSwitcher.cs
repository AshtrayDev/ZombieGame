using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] int currentWeapon = 0;

    void Start()
    {
        SetWeaponActive();
    }

    void SetWeaponActive()
    {

        int weaponIndex = 0;
        

        foreach(Transform weapon in transform)
        {
            //Resets every weapon that is active
            if(weapon.gameObject.activeInHierarchy)
            {
                weapon.SendMessage("OnWeaponReset");
            }

            //Activates and deactivates correct weapons
            if (weaponIndex == currentWeapon)
            {
                
                weapon.gameObject.SetActive(true);
            }

            else
            {

                weapon.gameObject.SetActive(false);
            }

            weaponIndex++;
        }
    }

    void Update()
    {

        if (Input.GetAxis("SwitchWeapon") < 0 && currentWeapon < CountWeaponAmount()-1)
        {
            currentWeapon++;
            SetWeaponActive();
        }

        if (Input.GetAxis("SwitchWeapon") > 0 && currentWeapon > 0)
        {
            currentWeapon--;
            SetWeaponActive();
        }
    }

    int CountWeaponAmount()
    {
        return transform.childCount;
    }
}
