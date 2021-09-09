using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    List<Collider> colliders = new List<Collider>();
    Collider trigger;
    bool isTriggeredByPlayer = false;

    private void Start()
    {
        trigger = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        colliders.Add(other);
        if (other.GetComponentInParent<PlayerHealth>())
        {
            isTriggeredByPlayer = true;
        }
        SendMessageUpwards("TriggerEnter", SendMessageOptions.DontRequireReceiver);
    }

    private void OnTriggerExit(Collider other)
    {
        colliders.Remove(other);

        if (other.GetComponentInParent<PlayerHealth>())
        {
            isTriggeredByPlayer = false;
        }

        SendMessageUpwards("TriggerExit", SendMessageOptions.DontRequireReceiver);
    }

    public List<Collider> GetColliders()
    {
        return colliders;
    }

    public bool IsTriggeredByPlayer()
    {
        return isTriggeredByPlayer;
    }

    public void SetTriggerState(bool state)
    {
        trigger.enabled = state;
        if(state == false)
        {
            isTriggeredByPlayer = false;
        }
    }
}
