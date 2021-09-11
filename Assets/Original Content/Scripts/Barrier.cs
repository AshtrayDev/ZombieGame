using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    List<Animator> activePlanks = new List<Animator>();
    List<Animator> inactivePlanks = new List<Animator>();
    bool isTriggered = false;
    bool isRepairDelayed = false;
    bool isBarrierClear = false;

    LevelSettings settings;

    // Start is called before the first frame update
    void Start()
    {
        settings = FindObjectOfType<LevelSettings>();
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
        if (Input.GetButton("Interact") && !isRepairDelayed && isTriggered)
        {
            RepairBarrier();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerHealth>())
        {
            FindObjectOfType<UIHandler>().SetToolTipCustom("Hold F to Repair Barrier");
            isTriggered = true;
        }

        else if (other.GetComponentInParent<EnemyAI>())
        {
            other.GetComponentInParent<EnemyAI>().isAttackingBarrier = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<PlayerHealth>())
        {
            FindObjectOfType<UIHandler>().SetActiveTooltipUI(false);
            isTriggered = false;
        }

        else if (other.GetComponentInParent<EnemyAI>())
        {
            other.GetComponentInParent<EnemyAI>().isAttackingBarrier = false;
        }
    }


    public void AttackBarrier()
    {
        if (activePlanks.Count != 0)
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

            isRepairDelayed = true;
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
        StartCoroutine(RepairDelay());
        isBarrierClear = false;
    }

    public bool IsBarrierClear()
    {
        return isBarrierClear;
    }

    public void Carpenter()
    {
        foreach (Animator plank in inactivePlanks)
        {
            plank.SetTrigger("Repair");
            activePlanks.Add(plank);
        }
        inactivePlanks.Clear();
    }

    IEnumerator RepairDelay()
    {
        isRepairDelayed = true;
        yield return new WaitForSeconds(settings.barrierRepairDelay);
        isRepairDelayed = false;
    }
}
