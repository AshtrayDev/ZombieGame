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


    private void Awake()
    {
        uiHandler = FindObjectOfType<UIHandler>();
        animator = GetComponent<Animator>();
        weapon = GetComponent<Weapon>();
        playerCam = transform.transform.GetComponentInParent<Camera>();
        fpsController = transform.transform.transform.GetComponentInParent<RigidbodyFirstPersonController>();
        crosshair = FindObjectOfType<Crosshair>();
    }

    void Start()
    {
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
        if (Input.GetButton("Fire2") && !weapon.IsReloading())
        {
            Zoom();
            uiHandler.SetActiveReticleUI(false);
            crosshair.ADS();
            animator.SetBool("ADS", true);
        }

        else
        {
            StopZoom();
            uiHandler.SetActiveReticleUI(true);
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
            //Position
            //Vector3 posDiff = zoomPos - transform.localPosition;
            //transform.localPosition = transform.localPosition + (posDiff * Time.deltaTime * zoomSpeed);

            //Rotation
            //Vector3 rotation = transform.localEulerAngles;
            //Vector3 rotationDiff = zoomRotation - rotation;
            //transform.localRotation = Quaternion.Euler(rotation + (rotationDiff * Time.deltaTime * zoomSpeed));

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
        uiHandler.SetActiveReticleUI(true);

        if (transform.localPosition != originalPos && transform.localRotation != Quaternion.Euler(originalRotation))
        {
            //Position
            //Vector3 posDiff = originalPos - transform.localPosition;
            //transform.localPosition = transform.localPosition + (posDiff * Time.deltaTime * zoomSpeed);

            //Rotation
            //Vector3 rotation = transform.localEulerAngles;
            //Vector3 rotationDiff = originalRotation - rotation;
            //transform.localRotation = Quaternion.Euler(rotation + (rotationDiff * Time.deltaTime * zoomSpeed));

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
