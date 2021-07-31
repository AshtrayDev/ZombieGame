using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class WeaponZoom : MonoBehaviour
{

    [SerializeField] RigidbodyFirstPersonController fpsController;
    [SerializeField] Camera playerCam;
    [SerializeField] float originalFOV = 90;
    [SerializeField] float zoomFOV = 45;
    [SerializeField] float originalSens = 2;
    [SerializeField] float zoomSpeed = 1;
    [SerializeField] float zoomMovementSpeed = 2;
    [SerializeField] Vector3 zoomPos;
    [SerializeField] Vector3 zoomRotation;

    Vector3 originalPos;
    Vector3 originalRotation;
    float currentSens;
    float zoomSens = 1;
    float originalMovementSpeed;
    float currentMovementSpeed;

    UIHandler uiHandler;


    private void Awake()
    {
        uiHandler = FindObjectOfType<UIHandler>();
    }

    void Start()
    {
        originalPos = transform.localPosition;
        originalRotation = transform.localEulerAngles;
        currentSens = originalSens;
        fpsController.mouseLook.XSensitivity = originalSens;
        fpsController.mouseLook.YSensitivity = originalSens;
        originalMovementSpeed = fpsController.movementSettings.ForwardSpeed;
        currentMovementSpeed = originalMovementSpeed;

        zoomSens = originalSens / (originalFOV / zoomFOV);
    }

    void Update()
    {
        if (Input.GetButton("Fire2"))
        {
            Zoom();
        }

        else
        {
            StopZoom();
        }
    }

    private void Zoom()
    {

        uiHandler.SetActiveReticleUI(false);

        if(transform.localPosition != zoomPos && transform.localRotation != Quaternion.Euler(zoomRotation))
        {
            //Position
            Vector3 posDiff = zoomPos - transform.localPosition;
            transform.localPosition = transform.localPosition + (posDiff * Time.deltaTime * zoomSpeed);

            //Rotation
            Vector3 rotation = transform.localEulerAngles;
            Vector3 rotationDiff = zoomRotation - rotation;
            transform.localRotation = Quaternion.Euler(rotation + (rotationDiff * Time.deltaTime * zoomSpeed));

            //FOV
            float FOVdiff = zoomFOV - playerCam.fieldOfView;
            playerCam.fieldOfView = playerCam.fieldOfView + (FOVdiff * Time.deltaTime * zoomSpeed);

            //Sens
            float sensDiff = zoomSens - currentSens;
            currentSens = currentSens + (sensDiff * Time.deltaTime * zoomSpeed);
            fpsController.mouseLook.XSensitivity = currentSens;
            fpsController.mouseLook.YSensitivity = currentSens;

            //MovementSpeed
            float movementSpeedDiff = zoomMovementSpeed - currentMovementSpeed;
            currentMovementSpeed = currentMovementSpeed + (movementSpeedDiff * Time.deltaTime * zoomSpeed);
            fpsController.movementSettings.ForwardSpeed = currentMovementSpeed;
        }
    }

    private void StopZoom()
    {
        uiHandler.SetActiveReticleUI(true);

        if (transform.localPosition != originalPos && transform.localRotation != Quaternion.Euler(originalRotation))
        {
            //Position
            Vector3 posDiff = originalPos - transform.localPosition;
            transform.localPosition = transform.localPosition + (posDiff * Time.deltaTime * zoomSpeed);

            //Rotation
            Vector3 rotation = transform.localEulerAngles;
            Vector3 rotationDiff = originalRotation - rotation;
            transform.localRotation = Quaternion.Euler(rotation + (rotationDiff * Time.deltaTime * zoomSpeed));

            //FOV
            float FOVdiff = originalFOV - playerCam.fieldOfView;
            playerCam.fieldOfView = playerCam.fieldOfView + (FOVdiff * Time.deltaTime * zoomSpeed);

            //Sens
            float sensDiff = originalSens - currentSens;
            currentSens = currentSens + (sensDiff * Time.deltaTime * zoomSpeed);
            fpsController.mouseLook.XSensitivity = currentSens;
            fpsController.mouseLook.YSensitivity = currentSens;

            //MovementSpeed
            float movementSpeedDiff = originalMovementSpeed - currentMovementSpeed;
            currentMovementSpeed = currentMovementSpeed + (movementSpeedDiff * Time.deltaTime * zoomSpeed);
            fpsController.movementSettings.ForwardSpeed = currentMovementSpeed;
        }
    }

    public void OnWeaponReset()
    {
        StopZoom();
    }


}
