using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] float holsterSpeed = 1;
    [SerializeField] float knifeHolsterSpeed = 4;

    [SerializeField] int currentWeaponID = 0;
    [SerializeField] Weapon deathWeapon;

    bool isAllowedToSwitch = true;
    bool isKnifing = false;

    List<Weapon> weapons = new List<Weapon>();
    Weapon currentWeapon;
    Crosshair crosshair;

    void Start()
    {
        crosshair = FindObjectOfType<Crosshair>();
        SetWeaponActive();
    }

    public void SetWeaponActive()
    {
        isKnifing = false;

        if(currentWeapon != null)
        {
            currentWeapon.GetComponent<Animator>().speed = holsterSpeed;
        }

        int weaponIndex = 0;
        

        foreach(Weapon weapon in weapons)
        {

            //Resets every weapon
            weapon.SendMessage("OnWeaponReset", SendMessageOptions.DontRequireReceiver);

            //Activates and deactivates correct weapons
            if (weaponIndex == currentWeaponID)
            {
                weapon.gameObject.SetActive(true);
                currentWeapon = weapon;
                crosshair.ChangeCurrentWeapon(weapon);
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
        if (!currentWeapon.gameObject.activeSelf)
        {
            print("Bug fixed");
            SetWeaponActive();
        }
        if (isAllowedToSwitch)
        {
            if (Input.GetAxis("SwitchWeapon") < 0 && currentWeaponID < GetAmountOfWeapons() - 1)
            {
                if(currentWeapon != null)
                {
                    currentWeaponID++;
                    HolsterWeapon(currentWeapon);
                }

                else
                {
                    currentWeaponID++;
                    SetWeaponActive();
                }
            }

            if (Input.GetAxis("SwitchWeapon") > 0 && currentWeaponID > 0)
            {
                if (currentWeapon != null)
                {
                    currentWeaponID--;
                    HolsterWeapon(currentWeapon);
                }

                else
                {
                    currentWeaponID--;
                    SetWeaponActive();
                }
            }
        }
    }
    
    void HolsterWeapon(Weapon weapon)
    {
        weapon.GetComponent<Animator>().speed = holsterSpeed;

        if (weapon.gameObject.activeInHierarchy == true)
        {
            weapon.GetComponent<Animator>().SetTrigger("Holster");
        }
    }

    public void HolsterCurrentWeapon()
    {
        currentWeapon.GetComponent<Animator>().speed = knifeHolsterSpeed;
        currentWeapon.GetComponent<Animator>().SetTrigger("Holster");
        isKnifing = true;
    }

    public void HolsterFinish()
    {
        if (isKnifing)
        {
            FindObjectOfType<Melee>().KnifeAnimStart();
        }
        else
        {
            SetWeaponActive();
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
        return weapons.Count;
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public void EquipWeapon(Weapon equipWeapon)
    {
        foreach(Weapon weapon in weapons)
        {
            if(weapon == equipWeapon)
            {
                currentWeaponID = weapons.IndexOf(weapon);
                SetWeaponActive();
                return;
            }
        }

        Debug.LogWarning("EquipWeapon: equipWeapon not found.");
    }

    public void QuickReviveMode()
    {
        Instantiate(deathWeapon, transform.position, Quaternion.identity, transform);
        isAllowedToSwitch = false;
    }

    public void QuickReviveModeEnd()
    {
        currentWeapon.DeleteWeapon();
        currentWeaponID--;
        SetWeaponActive();
        isAllowedToSwitch = true;
    }
}
