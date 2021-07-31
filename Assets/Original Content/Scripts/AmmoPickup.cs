using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] AmmoType ammoType;
    [SerializeField] int ammoAmount = 1;
    [SerializeField] float rotateSpeed = 70f;
    [SerializeField] float hoverSpeed = 0.1f;
    [SerializeField] float hoverAmount = 0.1f;

    float startYPos;
    bool isMovingUp;

    // Start is called before the first frame update
    void Start()
    {
        startYPos = transform.position.y;
        isMovingUp = true;
    }

    // Update is called once per frame
    void Update()
    {
        RotatePickup();
        HoverPickup();
    }

    void RotatePickup()
    {
        Vector3 rotate = new Vector3(0, Time.deltaTime * rotateSpeed, 0);
        gameObject.transform.Rotate(rotate);
    }

    void HoverPickup()
    {
        float yMin = startYPos - hoverAmount;
        float yMax = startYPos + hoverAmount;

        Vector3 currentPos = transform.position;
        Vector3 newPos = new Vector3(0, Time.deltaTime * hoverSpeed, 0);

        if (transform.position.y <= yMin)
        {
            isMovingUp = true;
        }

        if (transform.position.y >= yMax)
        {
            isMovingUp = false;
        }

        if(isMovingUp == true)
        {
            transform.position = currentPos + newPos;
        }

        else
        {
            transform.position = currentPos - newPos;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Ammo>().AddCurrentAmmo(ammoAmount, ammoType);
        Destroy(gameObject);
    }
}
