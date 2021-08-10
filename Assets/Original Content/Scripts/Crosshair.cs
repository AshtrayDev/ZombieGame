using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Crosshair : MonoBehaviour
{
    [SerializeField] GameObject top;
    [SerializeField] GameObject bottom;
    [SerializeField] GameObject right;
    [SerializeField] GameObject left;

    [SerializeField][Range(0, 3)] float size;
    [SerializeField] Color color;
    public float spread;
    [SerializeField] [Range(0, 100)] public float offset;
    [SerializeField] [Range(0, 100)] float spreadMultiplier = 50;
    [SerializeField][Range(0.01f, 0.05f)] float length;
    [SerializeField] [Range(0.001f, 0.01f)] float thickness;
    [SerializeField] float crosshairChangeSpeed = 10f;
    [SerializeField] float velocityThreshold = 0.5f;
    [SerializeField] GameObject testBullet;

    RigidbodyFirstPersonController fpsController;

    bool isADS = false;

    private void Awake()
    {
        fpsController = FindObjectOfType<RigidbodyFirstPersonController>();
    }
    private void Start()
    {
        
    }

    private void Update()
    {
        CalculateSpread();
        UpdateCrosshair();
    }

    private void CalculateSpread()
    {
        print(fpsController.GetActualMoveSpeed());
        if (!isADS && fpsController.GetActualMoveSpeed() == 1)
        {
            if (spread < 100)
            {
                spread = spread + crosshairChangeSpeed * Time.deltaTime;
            }

            UpdateCrosshair();
        }

        if (!isADS && fpsController.GetActualMoveSpeed() == 1)
        {
            if (spread > 100)
            {
                spread = spread - crosshairChangeSpeed * Time.deltaTime;
            }
        }

        if (!isADS && fpsController.GetActualMoveSpeed() == 2)
        {
            if (spread < 200)
            {
                spread = spread + crosshairChangeSpeed*2 * Time.deltaTime;
            }

            UpdateCrosshair();
        }

        if (!isADS && fpsController.GetActualMoveSpeed() == 0)
        {
            if (spread > 0)
            {
                spread = spread - crosshairChangeSpeed * Time.deltaTime;
            }
        }
    }

    private void OnValidate()
    {
        UpdateCrosshairSettings();
        UpdateCrosshair();
    }

    void UpdateCrosshair()
    {
        top.GetComponent<RectTransform>().localPosition = new Vector3(0, spread + offset, 0);
        bottom.GetComponent<RectTransform>().localPosition = new Vector3(0, -spread + -offset, 0);
        left.GetComponent<RectTransform>().localPosition = new Vector3(-spread + -offset, 0, 0);
        right.GetComponent<RectTransform>().localPosition = new Vector3(spread + offset, 0, 0);
    }

    void UpdateCrosshairSettings()
    {
        foreach (Transform transform in transform)
        {
            transform.GetComponent<Image>().color = color;
            transform.localScale = new Vector3(thickness, length, 0);
        }

        top.GetComponent<RectTransform>().localPosition = new Vector3(0, offset, 0);
        bottom.GetComponent<RectTransform>().localPosition = new Vector3(0, -offset, 0);
        left.GetComponent<RectTransform>().localPosition = new Vector3(-offset, 0, 0);
        right.GetComponent<RectTransform>().localPosition = new Vector3(offset, 0, 0);
    }


    public Vector3 GetRandomAngle()
    {
        Vector3 angle = new Vector3();
        angle = new Vector3(Random.Range(spread / 1000, -spread / 1000), Random.Range(spread / 1000, -spread / 1000), 0);
        return angle;
    }

    public void ADS()
    {
        isADS = true;
        spread = 0 + offset;
    }

    public void ReleaseADS()
    {
        isADS = false;
    }

}
