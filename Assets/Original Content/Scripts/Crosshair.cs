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
    [SerializeField][Range(0, 400)] public float offset;
    [SerializeField] [Range(0, 100)] float offsetMultiplier = 50;
    [SerializeField][Range(0.01f, 0.05f)] float length;
    [SerializeField] [Range(0.001f, 0.01f)] float thickness;
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
        if (!isADS)
        {
            offset = fpsController.GetActualMoveSpeed() * offsetMultiplier;
            UpdateCrosshair();
        }
    }

    private void OnValidate()
    {
        UpdateCrosshair();
    }

    void UpdateCrosshair()
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
        angle = new Vector3(Random.Range(offset / 1000, -offset / 1000), Random.Range(offset / 1000, -offset / 1000), 0);
        return angle;
    }

    public void ADS()
    {
        isADS = true;
        offset = 0;
    }

    public void ReleaseADS()
    {
        isADS = false;
    }

}
