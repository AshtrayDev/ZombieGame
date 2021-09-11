using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Weapon : MonoBehaviour
{
    enum WeaponType {auto, semiauto, shotgun}

    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject hitEffect;
    [SerializeField] float weaponRange = 100f;
    [SerializeField] float weaponDamage = 25f;
    [SerializeField] float headshotMultiplier = 2;
    [SerializeField] WeaponType weaponType;
    [SerializeField] int ammoCost = 1;
    [SerializeField] int hitsPerShot = 1;
    [SerializeField] float shotDelay = 0.5f;
    [Range(0, 400)] public float crosshairOffset;
    [SerializeField] int maxAmmoInClip = 30;
    [SerializeField] int maxAmmo = 300;
    [SerializeField] Weapon upgradedVersion;

    [SerializeField] float boxScale = 1;
    [SerializeField] Vector3 boxRotation;


    int ammoInClip;
    int storedAmmo;
 

    WeaponDelay weaponDelay;
    PlayerPoints points;
    PlayerPerk perk;
    Animator animator;
    WeaponSwitcher switcher;
    Camera playerCam;
    UIHandler ui;
    Crosshair crosshair;
    RigidbodyFirstPersonController controller;
    

    public bool isReloading;
    public bool isSprinting = false;
    public bool isDrawing = false;

    private void Awake()
    {
        if (GetComponentInParent<WeaponSwitcher>() == null)
        {
            Destroy(GetComponent<Animator>());
            enabled = false;
            return;
        }
        playerCam = transform.transform.GetComponentInParent<Camera>();
        weaponDelay = GetComponentInParent<WeaponDelay>();
        points = FindObjectOfType<PlayerPoints>();
        perk = FindObjectOfType<PlayerPerk>();
        animator = GetComponent<Animator>();
        switcher = GetComponentInParent<WeaponSwitcher>();
        ui = FindObjectOfType<UIHandler>();
        crosshair = FindObjectOfType<Crosshair>();
        controller = FindObjectOfType<RigidbodyFirstPersonController>();
    }

    private void Start()
    {
        weaponDelay.AddWeapon(this);
        ammoInClip = maxAmmoInClip;
        storedAmmo = maxAmmo;
        RefreshAmmoUI();
        isDrawing = true;
    }

    private void OnEnable()
    {
        RefreshAmmoUI();
        isDrawing = true;
    }


    void Update()
    {
        if (weaponType == WeaponType.auto)
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

        if (controller.dash)
        {
            animator.SetBool("Sprint", true);
            isSprinting = true;
        }
        else
        {
            animator.SetBool("Sprint", false);
            isSprinting = false;
        }
    }

    void CanWeaponShoot()
    {
        if(ammoInClip > 0 && weaponDelay.CanWeaponShoot(this) && !isReloading && !isSprinting && !isDrawing)
        {
            Shoot();
        }
    }

    void Reload()
    {
        if(ammoInClip < maxAmmoInClip && storedAmmo > 0)
        {
            animator.speed = perk.reloadSpeed;
            animator.SetTrigger("Reload");
            isReloading = true;
        }
    }

    void Shoot()
    {
        MuzzleFlash();
        animator.SetTrigger("Fire");
        ammoInClip = ammoInClip - ammoCost;
        RefreshAmmoUI();

        for (int i = 0; i < hitsPerShot; i++)
        {
            RaycastHit hit = ProcessRaycast();
            weaponDelay.StartDelay(this);

            if (hit.transform == null) { return; }

            InstantiateHitEffect(hit);
            ProcessDamage(hit);
        }
    }

    RaycastHit ProcessRaycast()
    {
        RaycastHit hit;
        Physics.Raycast(playerCam.transform.position, playerCam.transform.forward + crosshair.GetRandomAngle(), out hit, weaponRange);
        return hit;
    }

    void ProcessDamage(RaycastHit hit)
    {
        if(hit.transform.GetComponentInParent<EnemyHealth>() == null) { return; }

        if (perk.HasGotPickup(Pickup.PickupType.instakill))
        {
            EnemyHealth enemyHealth = hit.transform.GetComponentInParent<EnemyHealth>();
            enemyHealth.TakeDamage(999999999999999999, EnemyHealth.DamageType.body);
        }

        else if(hit.transform.gameObject.name == "HeadHitbox")
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
    
    public void RefreshAmmoUI()
    {
        ui.SetAmmoText(ammoInClip, storedAmmo);
    }

    public void DeleteWeapon()
    {
        weaponDelay.RemoveWeapon(this);
    }

    public float GetShotDelay()
    {
        return shotDelay;
    }

    public Weapon GetUpgradedVersion()
    {
        return upgradedVersion;
    }

    public bool IsReloading()
    {
        return isReloading;
    }

    public (float, Vector3) GetBoxDetails()
    {
        return (boxScale, boxRotation);
    }

    public void FinishHolsterAnim()
    {
        switcher.HolsterFinish();
        gameObject.SetActive(false);
    }

    public void FinishDrawAnim()
    {
        isDrawing = false;
    }

    public void FinishReloadAnim()
    {
        if(maxAmmoInClip > storedAmmo)
        {
            ammoInClip = ammoInClip + storedAmmo;
            storedAmmo = 0;
        }
        else
        {
            int ammoToLoad = maxAmmoInClip - ammoInClip;
            storedAmmo = storedAmmo - ammoToLoad;
            ammoInClip = maxAmmoInClip;
        }

        RefreshAmmoUI();
        animator.speed = 1;
        isReloading = false;
    }

    public void OnWeaponReset()
    {
        isReloading = false;
        animator.speed = 1;
    }

    public void MaxAmmo()
    {
        ammoInClip = maxAmmoInClip;
        storedAmmo = maxAmmo;
        RefreshAmmoUI();
    }
}
