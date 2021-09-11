using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackAPunch : MonoBehaviour
{
    [SerializeField] Trigger trigger;
    [SerializeField] int cost;
    [SerializeField] int delay;
    [SerializeField] AudioClip song;
    [SerializeField] AudioClip ding;
    [SerializeField] AudioClip whirl;


    bool isTriggeredByPlayer;
    bool isWeaponReady;
    bool isDelayed;
    Weapon savedWeapon;

    UIHandler ui;
    WeaponSwitcher switcher;
    PlayerPoints points;
    Audio audio;

    // Start is called before the first frame update
    void Start()
    {
        ui = FindObjectOfType<UIHandler>();
        switcher = FindObjectOfType<WeaponSwitcher>();
        points = FindObjectOfType<PlayerPoints>();
        audio = FindObjectOfType<Audio>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isWeaponReady)
        {
            if (Input.GetButtonDown("Interact") && isTriggeredByPlayer && !isDelayed)
            {
                TakeWeapon();
            }
        }

        else
        {
            if (Input.GetButtonDown("Interact") && isTriggeredByPlayer && points.IsAbleToAfford(cost) && !isDelayed)
            {
                PackAPunchProcess();
            }
        }
    }

    public void TriggerEnter()
    {
        if (trigger.IsTriggeredByPlayer() && !isDelayed)
        {
            isTriggeredByPlayer = true;

            if (!isWeaponReady)
            {
                ui.SetTooltipBuy("Hold F to Upgrade Weapon", cost);
            }
            else 
            {
                ui.SetToolTipCustom("Hold F to Take Upgraded Weapon");
            }
        }
    }

    public void TriggerExit()
    {
        if (!trigger.IsTriggeredByPlayer())
        {
            isTriggeredByPlayer = false;
            ui.SetActiveTooltipUI(false);
        }
    }

    void PackAPunchProcess()
    {
        Weapon currentWeapon = switcher.GetCurrentWeapon();
        if (!CanUpgradeWeapon(currentWeapon)) { return; }
        else
        {
            savedWeapon = currentWeapon.GetUpgradedVersion();
            currentWeapon.DeleteWeapon();
        }
        points.RemovePoints(cost);
        trigger.SetTriggerState(false);
        ui.SetActiveTooltipUI(false);
        SoundFX();
    }

    bool CanUpgradeWeapon(Weapon weapon)
    {
        if(weapon.GetUpgradedVersion() == null)
        {
            return false;
        }

        else
        {
            return true;
        }
    }

    void SoundFX()
    {
        audio.PlaySound(song);
        audio.PlaySound(whirl);
        StartCoroutine(PlayDing());
    }

    IEnumerator PlayDing()
    {
        yield return new WaitForSeconds(whirl.length-0.5f);
        audio.PlaySound(ding);
        WeaponReady();
    }

    void WeaponReady()
    {
        trigger.SetTriggerState(true);
        isWeaponReady = true;
    }

    void TakeWeapon()
    {
        Instantiate(savedWeapon, points.transform.position, Quaternion.identity, switcher.transform);
        isWeaponReady = false;
        ui.SetActiveTooltipUI(false);
        StartCoroutine(StartDelay());
    }

    IEnumerator StartDelay()
    {
        isDelayed = true;
        yield return new WaitForSeconds(delay);
        isDelayed = false;
    }
}
