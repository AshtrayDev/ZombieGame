using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] int currentWeaponID = 0;

    List<Weapon> weapons = new List<Weapon>();
    Weapon currentWeapon;

    void Start()
    {
        SetWeaponActive();
    }

    public void SetWeaponActive()
    {

        int weaponIndex = 0;
        

        foreach(Weapon weapon in weapons)
        {

            //Resets every weapon that is active
            if(weapon.gameObject.activeInHierarchy)
            {
                weapon.SendMessage("OnWeaponReset", SendMessageOptions.DontRequireReceiver);
            }

            //Activates and deactivates correct weapons
            if (weaponIndex == currentWeaponID)
            {
                weapon.gameObject.SetActive(true);
                currentWeapon = weapon.GetComponent<Weapon>();
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

        if (Input.GetAxis("SwitchWeapon") < 0 && currentWeaponID < GetAmountOfWeapons()-1)
        {
            currentWeaponID++;
            HolsterWeapon(currentWeapon);
        }

        if (Input.GetAxis("SwitchWeapon") > 0 && currentWeaponID > 0)
        {
            currentWeaponID--;
            HolsterWeapon(currentWeapon);
        }
    }
    
    void HolsterWeapon(Weapon weapon)
    {
        if (weapon.gameObject.activeInHierarchy == true)
        {
            weapon.GetComponent<Animator>().SetTrigger("Holster");
        }

    }

    public void AddWeapon(Weapon weapon)
    {
        weapons.Add(weapon);
        currentWeaponID = weapons.IndexOf(weapon);
        SetWeaponActive();
    }

    public void RemoveWeapon(Weapon weapon)
    {
        weapons.Remove(weapon);
    }

    public int GetAmountOfWeapons()
    {
        return transform.childCount;
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }
}
