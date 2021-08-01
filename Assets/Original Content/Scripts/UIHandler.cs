using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public GameObject reticleUI;

    public void SetActiveReticleUI(bool state)
    {
        reticleUI.SetActive(state);
    }


}
