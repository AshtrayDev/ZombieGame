using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{ 
    [SerializeField] GameObject reticleUI;

    public void SetActiveReticleUI(bool state)
    {
        reticleUI.SetActive(state);
    }


}
