using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    List<Animator> planks = new List<Animator>();
    List<Animator> activePlanks = new List<Animator>();
    [SerializeField] AnimationClip destroyAnim;
    int plankCount;
    int maxPlanks;
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
                planks.Add(transform.GetComponent<Animator>());
            }
        }
        plankCount = planks.Count;
        maxPlanks = plankCount;
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
            DestroyBarrier(planks[plankCount - 1]);
        }
    }

    public void RepairBarrier()
    {
        if (plankCount < maxPlanks)
        {
            planks[plankCount].SetTrigger("Repair");
            repairDelay = true;
        }
    }

    void DestroyBarrier(Animator plank)
    {
        plank.SetTrigger("Destroy");
    }

    public void DestroyBarrierAnim()
    {
        if(plankCount == 1)
        {
            plankCount--;
            isBarrierClear = true;
        }
        else
        {
            plankCount--;
        }

        repairDelay = false;
    }

    public void RepairBarrierAnim()
    {
        repairDelay = false;
        plankCount++;
        isBarrierClear = false;
    }

    public bool IsBarrierClear()
    {
        return isBarrierClear;
    }

}
