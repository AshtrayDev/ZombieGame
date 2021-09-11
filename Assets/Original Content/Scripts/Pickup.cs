using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum PickupType
    {
        instakill, doublePoints, carpenter, nuke, maxAmmo
    }


    [SerializeField] PickupType pickupType;
    [SerializeField] int timer;

    [SerializeField] float rotateSpeed = 70f;
    [SerializeField] float hoverSpeed = 0.1f;
    [SerializeField] float hoverAmount = 0.1f;

    float startYPos;
    bool isMovingUp;

    PlayerPerk perk;
    LevelSettings settings;
    UIHandler ui;

    // Start is called before the first frame update
    void Start()
    {
        settings = FindObjectOfType<LevelSettings>();
        startYPos = transform.position.y;
        isMovingUp = true;
        perk = FindObjectOfType<PlayerPerk>();
        ui = FindObjectOfType<UIHandler>();
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

        if (isMovingUp == true)
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
        if (other.GetComponent<PlayerHealth>())
        {
            PickupEffect();
            Destroy(gameObject);
        }
    }

    void PickupEffect()
    {

        if (pickupType == PickupType.nuke)
        {
            foreach (EnemyHealth enemy in FindObjectsOfType<EnemyHealth>())
            {
                enemy.SetHealth(0);
            }
            FindObjectOfType<PlayerPoints>().AddPoints(settings.nukePoints);
            return;
        }

        if (pickupType == PickupType.carpenter)
        {
            foreach(Barrier barrier in FindObjectsOfType<Barrier>())
            {
                barrier.Carpenter();
            }
            FindObjectOfType<PlayerPoints>().AddPoints(settings.carpenterPoints);
            return;
        }

        if (pickupType == PickupType.maxAmmo)
        {
            foreach (Weapon weapon in FindObjectsOfType<Weapon>())
            {
                weapon.MaxAmmo();
            }
            return;
        }


        //Timer based pickups
        perk.AddPickup(pickupType, timer);

    }

}
