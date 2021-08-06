using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Camera playerCam;
    [SerializeField] Ammo ammoSlot;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject hitEffect;
    [SerializeField] float weaponRange = 100f;
    [SerializeField] float weaponDamage = 25f;
    [SerializeField] float headshotMultiplier = 2;
    [SerializeField] bool isAutomatic = false;
    [SerializeField] int ammoCost = 1;
    [SerializeField] float shotDelay = 0.5f;
    [SerializeField] AmmoType ammoType;

    WeaponDelay weaponDelay;
    PlayerPoints playerPoints;
    Animator animator;

    bool isReloading;

    private void Awake()
    {
        weaponDelay = GetComponentInParent<WeaponDelay>();
        playerPoints = FindObjectOfType<PlayerPoints>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isAutomatic)
        {
            if (Input.GetButton("Fire1"))
            {
                CanWeaponShoot();
            }
        }

        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                CanWeaponShoot();
            }
        }


        if (Input.GetButtonDown("Reload") && !isReloading)
        {
            Reload();
        }
    }

    void CanWeaponShoot()
    {
        if(ammoSlot.GetCurrentAmmo(ammoType) > 0 && weaponDelay.CanWeaponShoot(this) && !isReloading)
        {
            Shoot();
        }
    }

    void Reload()
    {
        animator.SetTrigger("Reload");
        isReloading = true;
    }

    void Shoot()
    {
        MuzzleFlash();
        animator.SetTrigger("Fire");
        ammoSlot.ReduceCurrentAmmo(ammoCost, ammoType);
        RaycastHit hit = ProcessRaycast();
        weaponDelay.StartDelay(this);

        if (hit.transform == null) { return; }

        InstantiateHitEffect(hit);
        ProcessDamage(hit);
    }

    RaycastHit ProcessRaycast()
    {
        RaycastHit hit;
        Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, weaponRange);

        return hit;
    }

    void ProcessDamage(RaycastHit hit)
    {
        if(hit.transform.gameObject.name == "HeadHitbox")
        {
            EnemyHealth enemyHealth = hit.transform.GetComponentInParent<EnemyHealth>();
            enemyHealth.TakeDamage(weaponDamage * headshotMultiplier, EnemyHealth.DamageType.headshot);
        }

        else if(hit.transform.gameObject.name == "BodyHitbox")
        {
            EnemyHealth enemyHealth = hit.transform.GetComponentInParent<EnemyHealth>();
            enemyHealth.TakeDamage(weaponDamage, EnemyHealth.DamageType.body);
        }
    }

    void MuzzleFlash()
    {
        muzzleFlash.Play();
    }

    void InstantiateHitEffect(RaycastHit hit)
    {
        GameObject impact = Instantiate(hitEffect, hit.point, Quaternion.identity);
        Destroy(impact, 1f);
    }

    public float GetShotDelay()
    {
        return shotDelay;
    }

    public bool IsReloading()
    {
        return isReloading;
    }

    public void FinishHolsterAnim()
    {
        SendMessageUpwards("SwitchWeapon");
    }

    public void FinishReloadAnim()
    {
        isReloading = false;
    }
}
