using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    List<Animator> activePlanks = new List<Animator>();
    List<Animator> inactivePlanks = new List<Animator>();
    bool isTriggered = false;
    bool repairDelay = false;
    bool isBarrierClear = false;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform transform in transform)
        {
            if (transform.GetComponent<Animator>())
            {
                activePlanks.Add(transform.GetComponent<Animator>());
            }
        }
    }

    private void Update()
    {
        if (Input.GetButton("Interact") && !repairDelay && isTriggered)
        {
            RepairBarrier();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerHealth>())
        {
            FindObjectOfType<UIHandler>().SetToolTipRepairBarrier();
            isTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerHealth>())
        {
            FindObjectOfType<UIHandler>().SetActiveTooltipUI(false);
            isTriggered = false;
        }
    }


    public void AttackBarrier()
    {
        if (!isBarrierClear)
        {
            DestroyBarrier(activePlanks[activePlanks.Count-1]);
        }
    }

    public void RepairBarrier()
    {
        if (inactivePlanks.Count > 0)
        {
            inactivePlanks[inactivePlanks.Count-1].SetTrigger("Repair");
            activePlanks.Add(inactivePlanks[inactivePlanks.Count - 1]);
            inactivePlanks.Remove(inactivePlanks[inactivePlanks.Count - 1]);

            repairDelay = true;
        }
    }

    void DestroyBarrier(Animator plank)
    {
        plank.SetTrigger("Destroy");
        inactivePlanks.Add(plank);
        activePlanks.Remove(plank);
    }

    public void DestroyBarrierAnim()
    {
        if(activePlanks.Count == 0)
        {
            isBarrierClear = true;
        }
        else
        {
            
        }
    }

    public void RepairBarrierAnim()
    {
        repairDelay = false;
        isBarrierClear = false;
    }

    public bool IsBarrierClear()
    {
        return isBarrierClear;
    }

}
