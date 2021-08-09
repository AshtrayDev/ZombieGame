using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuy : MonoBehaviour
{
    [SerializeField] Vector3 triggerBoxSize;
    [SerializeField] Weapon weaponForSale;
    [SerializeField] int weaponCost;

    UIHandler ui;
    BoxCollider trigger;
    GameObject player;
    WeaponSwitcher switcher;

    bool isTriggered;

    private void Awake()
    {
        ui = FindObjectOfType<UIHandler>();
    }
    // Start is called before the first frame update
    void Start()
    {
        trigger = gameObject.AddComponent<BoxCollider>();
        trigger.isTrigger = true;
        trigger.size = triggerBoxSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTriggered)
        {
            if (Input.GetButtonDown("Interact"))
            {
                BuyWeapon();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isTriggered = true;
        player = other.gameObject;
        ui.SetTooltipBuy("Press F to buy " + weaponForSale.name + " ", weaponCost);
    }

    private void OnTriggerExit(Collider other)
    {
        isTriggered = false;
        player = null;
        ui.SetActiveTooltipUI(false);
    }

    void BuyWeapon()
    {
        switcher = player.GetComponentInChildren<WeaponSwitcher>();
        PlayerPoints points = player.GetComponent<PlayerPoints>();
        if (points.IsAbleToAfford(weaponCost))
        {
            points.RemovePoints(weaponCost);
            ui.SetActiveTooltipUI(false);
            if (HasTwoWeapons())
            {
                switcher.GetCurrentWeapon().DeleteWeapon();
            }

            Instantiate(weaponForSale, player.transform.position, Quaternion.identity, switcher.transform);
            
        }
    }

    bool HasTwoWeapons()
    {
        switcher = player.GetComponentInChildren<WeaponSwitcher>();
        if(switcher.GetAmountOfWeapons() >= 2)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

}
