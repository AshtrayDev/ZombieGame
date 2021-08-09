using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIHandler : MonoBehaviour
{
    public GameObject reticleUI;
    public GameObject tooltipUI;
    public GameObject pointsUI;
    public GameObject addedPointsUI;
    public GameObject roundUI;
    public GameObject ammoUI;

    [SerializeField] GameObject addPointsTextPrefab;
    [SerializeField] GameObject lostPointsTextPrefab;
  
    //SetActives----------------------------------------------
    public void SetActiveReticleUI(bool state)
    {
        reticleUI.SetActive(state);
    }
    public void SetActiveTooltipUI(bool state)
    {
        tooltipUI.SetActive(state);
    }

    //Tooltip---------------------------------------------------------------------------
    public void SetTooltipBuy(string text, int cost)
    {
        tooltipUI.SetActive(true);
        tooltipUI.GetComponentInChildren<TMP_Text>().text = text + "[Cost: " + cost + "]";
    }

    //Points---------------------------------------------------------------------------
    public void SetPointsUIText(int points)
    {
        pointsUI.GetComponentInChildren<TMP_Text>().text = points.ToString();
    }
    public void AddPointsUI(int points, bool gainedPoints)
    {
        GameObject changedPoints;

        if (gainedPoints)
        {
            changedPoints = Instantiate(addPointsTextPrefab, pointsUI.transform.transform.position, Quaternion.identity, addedPointsUI.transform);
        }
        else
        {
            changedPoints = Instantiate(lostPointsTextPrefab, pointsUI.transform.transform.position, Quaternion.identity, addedPointsUI.transform);
        }
        
        changedPoints.GetComponent<Animator>().Play(0);
        if (gainedPoints)
        {
            changedPoints.GetComponent<TMP_Text>().text = "+ " + points.ToString();
        }
        else
        {
            changedPoints.GetComponent<TMP_Text>().text = "- " + points.ToString();
        }

        Destroy(changedPoints, 2f);
    }

    //Rounds-------------------------------------------------------
    public void SetRoundUIText(int round)
    {
        roundUI.GetComponentInChildren<TMP_Text>().text = round.ToString();
    }

    //Ammo
    public void SetAmmoText(int clip, int stored)
    {
        ammoUI.GetComponentInChildren<TMP_Text>().text = clip.ToString() + " / " + stored.ToString();
    }
}
