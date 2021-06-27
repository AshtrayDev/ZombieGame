using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Camera playerCam;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject hitEffect;
    [SerializeField] float weaponRange = 100f;
    [SerializeField] float weaponDamage = 25f;
    [SerializeField] Ammo ammoSlot;
    [SerializeField] int ammoCost = 1;
    [SerializeField] float shotDelay = 0.5f;
    [SerializeField] bool isWeaponDelayed;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            CanWeaponShoot();
        }
    }

    void CanWeaponShoot()
    {
        if(ammoSlot.GetCurrentAmmo() > 0 && !isWeaponDelayed)
        {
            Shoot();
        }
    }



    void Shoot()
    {
        MuzzleFlash();
        ammoSlot.ReduceCurrentAmmo(ammoCost);
        RaycastHit hit = ProcessRaycast();
        StartCoroutine(ShootDelay());

        if (hit.transform == null) { return; }

        InstantiateHitEffect(hit);
        ProcessDamage(hit);
    }

    IEnumerator ShootDelay()
    {
        isWeaponDelayed = true;
        yield return new WaitForSeconds(shotDelay);
        isWeaponDelayed = false;
    }

    RaycastHit ProcessRaycast()
    {
        RaycastHit hit;
        Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, weaponRange);

        return hit;
    }

    void ProcessDamage(RaycastHit hit)
    {
        if (hit.transform.GetComponent<EnemyHealth>())
        {
            EnemyHealth enemyHealth = hit.transform.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(weaponDamage);
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

    public void OnWeaponReset()
    {
        isWeaponDelayed = false;
    }
}
