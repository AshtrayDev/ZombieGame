using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{

    [SerializeField] float meleeDamage;
    PlayerHealth playerHealth;
    BoxCollider box;
    WeaponSwitcher switcher;
    Weapon currentWeapon;

    private void Awake()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        box = GetComponent<BoxCollider>();
        switcher = FindObjectOfType<WeaponSwitcher>();
    }
    private void Update()
    {
        if (Input.GetButtonDown("Melee"))
        {
            switcher.HolsterCurrentWeapon();
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.GetComponentInParent<EnemyHealth>() != null)
        {
            other.GetComponentInParent<EnemyHealth>().TakeDamage(meleeDamage, EnemyHealth.DamageType.melee);
            box.enabled = false;
        }
    }

    public void KnifeAnimStart()
    {
        GetComponent<Animator>().SetTrigger("MeleeTrigger");
        box.enabled = true;
    }

    public void KnifeAnimEnd()
    {
        box.enabled = false;
        switcher.SetWeaponActive();
    }
}
