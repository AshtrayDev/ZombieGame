using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{

    UIHandler ui;
    Trigger trigger;
    [SerializeField] Material powerOn;

    public GameObject[] connectedObjects;

    bool isPowerOn;
    bool isTriggered;

    // Start is called before the first frame update
    void Start()
    {
        ui = FindObjectOfType<UIHandler>();
        trigger = GetComponentInChildren<Trigger>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact") && isTriggered)
        {
            PowerOn();
        }
    }

    public void TriggerEnter()
    {
        if (trigger.IsTriggeredByPlayer() && !isPowerOn)
        {
            ui.SetToolTipCustom("Hold F to Turn On Power");
            isTriggered = true;
        }

    }

    public void TriggerExit()
    {
        if (!trigger.IsTriggeredByPlayer())
        {
            ui.SetActiveTooltipUI(false);
            isTriggered = false;
        }
    }

    void PowerOn()
    {
        isPowerOn = true;
        GetComponent<MeshRenderer>().material = powerOn;

        foreach(GameObject item in connectedObjects)
        {
            item.SendMessage("PowerOn", SendMessageOptions.DontRequireReceiver);
        }
    }
}
