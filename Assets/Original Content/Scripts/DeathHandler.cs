using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class DeathHandler : MonoBehaviour
{
    [SerializeField] Canvas gameOverCanvas;

    PlayerPerk perk;
    PlayerHealth health;
    Rigidbody rb;
    RigidbodyFirstPersonController fps;
    WeaponSwitcher switcher;

    public bool isDying;
    [SerializeField] float quickReviveTime = 5f;

    private void Start()
    {
        health = FindObjectOfType<PlayerHealth>();
        perk = FindObjectOfType<PlayerPerk>();
        fps = FindObjectOfType<RigidbodyFirstPersonController>();
        rb = GetComponent<Rigidbody>();
        switcher = FindObjectOfType<WeaponSwitcher>();

        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameOverCanvas.enabled = false;
    }

    public void DeathSequence()
    {
        if (isDying) { return; }
        isDying = true;
        if (!GetComponent<PlayerPerk>().quickReviveOn)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            gameOverCanvas.enabled = true;
        }

        else
        {
            isDying = true;
            health.ChangeMaxHealth(1000000);
            health.AddHealth(10000000);
            fps.currentMoveSpeed = 0f;
            Vector3 newPos = transform.position;
            newPos.y = 0f;
            transform.position = newPos;
            rb.isKinematic = true;
            switcher.QuickReviveMode();
            StartCoroutine(QuickReviveTimer());
            perk.RemoveAllPerks();
        }
    }

    IEnumerator QuickReviveTimer()
    {
        yield return new WaitForSeconds(quickReviveTime);

        health.ChangeMaxHealth(100);
        health.SetHealth(100);
        fps.currentMoveSpeed = 1f;
        Vector3 newPos = transform.position;
        newPos.y = 1f;
        transform.position = newPos;
        rb.isKinematic = false;
        switcher.QuickReviveModeEnd();
        isDying = false;
    }
}
