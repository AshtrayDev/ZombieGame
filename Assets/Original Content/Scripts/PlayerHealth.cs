using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float currentHealth = 100f;
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float regenDelay = 2f;
    [SerializeField] float regenSpeed = 5f;

    bool regen;

    DeathHandler deathHandler;
    UIHandler ui;

    // Start is called before the first frame update
    void Start()
    {
        ui = FindObjectOfType<UIHandler>();
        deathHandler = GetComponent<DeathHandler>();
    }

    private void Update()
    {
        if (regen)
        {
            Regen();
        }
    }

    public void AddHealth(float addedHealth)
    {
        if(addedHealth < 0)
        {
            ResetRegenDelay();
        }

        currentHealth = currentHealth + addedHealth;
        ui.UpdateTakeDamagePanel(currentHealth, maxHealth);
        CheckHealth();
    }

    public void SetHealth(float newHealth)
    {
        currentHealth = newHealth;
    }

    public void ChangeMaxHealth(float newHealth)
    {
        maxHealth = newHealth;
    }

    void CheckHealth()
    {
        if(currentHealth <= 0)
        {
            deathHandler.DeathSequence();
        }
    }

    void ResetRegenDelay()
    {
        regen = false;
        StopCoroutine(RegenTimer());
        StartCoroutine(RegenTimer());
    }

    void Regen()
    {
        if(currentHealth < maxHealth)
        {
            AddHealth(regenSpeed * Time.deltaTime);
        }

        else if(currentHealth > maxHealth)
        {
            SetHealth(maxHealth);
        }
    }

    IEnumerator RegenTimer()
    {
        yield return new WaitForSeconds(regenDelay);
        regen = true;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void OnExplosiveHit()
    {
        AddHealth(-50);
    }

}
