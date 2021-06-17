using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponZoom : MonoBehaviour
{
    [SerializeField] Camera playerCam;
    [SerializeField] float FOV = 90;
    [SerializeField] float zoomFOV = 45;


    void Update()
    {
        if (Input.GetButton("Fire2"))
        {
            playerCam.fieldOfView = zoomFOV;
        }

        else
        {
            playerCam.fieldOfView = FOV;
        }
    }
}
