using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class WeaponZoom : MonoBehaviour
{


    [SerializeField] float originalFOV = 90;
    [SerializeField] float zoomFOV = 45;
    [SerializeField] float originalSens = 2;
    [SerializeField] float zoomSpeed = 1;
    [SerializeField] float zoomMovementSpeed = 2;
    [SerializeField] Vector3 zoomPos;
    [SerializeField] Vector3 zoomRotation;

    Vector3 originalPos;
    Vector3 originalRotation;
    Animator animator;
    float currentSens;
    float zoomSens = 1;
    float originalMovementSpeed;
    float currentMovementSpeed;

    UIHandler uiHandler;
    Weapon weapon;
    RigidbodyFirstPersonController fpsController;
    Camera playerCam;
    Crosshair crosshair;

    bool isShotgun = false;


    private void Awake()
    {
        if(GetComponentInParent<WeaponSwitcher>() == null)
        {
            enabled = false;
            return;
        }
        uiHandler = FindObjectOfType<UIHandler>();
        animator = GetComponent<Animator>();
        weapon = GetComponent<Weapon>();
        playerCam = transform.transform.GetComponentInParent<Camera>();
        fpsController = transform.transform.transform.GetComponentInParent<RigidbodyFirstPersonController>();
        crosshair = FindObjectOfType<Crosshair>();
        originalMovementSpeed = fpsController.currentMoveSpeed;
    }

    void Start()
    {
        if(weapon.GetWeaponType() is Weapon.WeaponType.shotgun)
        {
            isShotgun = true;
        }

        originalPos = transform.localPosition;
        originalRotation = transform.localEulerAngles;
        currentSens = originalSens;
        fpsController.sensitivityX = originalSens;
        fpsController.sensitivityY = originalSens;
        originalMovementSpeed = fpsController.originalSpeed;

        zoomSens = originalSens / (originalFOV / zoomFOV);
    }

    void Update()
    {
        if(crosshair == null)
        {
            FindObjectOfType<Crosshair>();
        }
        if (Input.GetButton("Fire2") && !weapon.IsReloading() && !weapon.isSprinting)
        {
            Zoom();
            crosshair.ADS(isShotgun);
            animator.SetBool("ADS", true);
        }

        else
        {
            StopZoom();
            crosshair.ReleaseADS();
            animator.SetBool("ADS", false);
        }

        if (Input.GetButtonUp("Fire2"))
        {
            //MovementSpeed
            fpsController.currentMoveSpeed = originalMovementSpeed;
        }
    }

    private void Zoom()
    {
        uiHandler.SetActiveReticleUI(false);

        if(transform.localPosition != zoomPos && transform.localRotation != Quaternion.Euler(zoomRotation))
        {

            //FOV
            float FOVdiff = zoomFOV - playerCam.fieldOfView;
            playerCam.fieldOfView = playerCam.fieldOfView + (FOVdiff * Time.deltaTime * zoomSpeed);

            //Sens
            float sensDiff = zoomSens - currentSens;
            currentSens = currentSens + (sensDiff * Time.deltaTime * zoomSpeed);
            fpsController.sensitivityX = currentSens;
            fpsController.sensitivityY = currentSens;

            //MovementSpeed
            fpsController.currentMoveSpeed = zoomMovementSpeed;
        }
    }

    private void StopZoom()
    {
        if (!weapon.isSprinting)
        {
            uiHandler.SetActiveReticleUI(true);
        }

        else
        {
            uiHandler.SetActiveReticleUI(false);
        }

        if (transform.localPosition != originalPos && transform.localRotation != Quaternion.Euler(originalRotation))
        {
            //FOV
            float FOVdiff = originalFOV - playerCam.fieldOfView;
            playerCam.fieldOfView = playerCam.fieldOfView + (FOVdiff * Time.deltaTime * zoomSpeed);

            //Sens
            float sensDiff = originalSens - currentSens;
            currentSens = currentSens + (sensDiff * Time.deltaTime * zoomSpeed);
            fpsController.sensitivityX = currentSens;
            fpsController.sensitivityY = currentSens;


        }
    }

    public void OnWeaponReset()
    {
        fpsController.currentMoveSpeed = originalMovementSpeed;
        playerCam.fieldOfView = originalFOV;
    }


}
