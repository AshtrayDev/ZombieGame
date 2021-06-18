using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class WeaponZoom : MonoBehaviour
{

    [SerializeField] RigidbodyFirstPersonController fpsController;
    [SerializeField] Camera playerCam;
    [SerializeField] float FOV = 90;
    [SerializeField] float zoomFOV = 45;
    [SerializeField] float sens = 2;
    [SerializeField] float zoomSens = 1;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetButton("Fire2"))
        {
            playerCam.fieldOfView = zoomFOV;
            fpsController.mouseLook.XSensitivity = zoomSens;
            fpsController.mouseLook.YSensitivity = zoomSens;
        }

        else
        {
            playerCam.fieldOfView = FOV;
            fpsController.mouseLook.XSensitivity = sens;
            fpsController.mouseLook.YSensitivity = sens;
        }
    }


}
