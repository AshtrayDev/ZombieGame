using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plank : MonoBehaviour
{

    public void PlankDestroyAnim()
    {
        transform.GetComponentInParent<Barrier>().DestroyBarrierAnim();
    }

    public void PlankRepairAnim()
    {
        transform.GetComponentInParent<Barrier>().RepairBarrierAnim();
    }
}
