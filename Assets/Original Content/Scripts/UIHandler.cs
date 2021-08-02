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

    [SerializeField] GameObject addPointsTextPrefab;
    [SerializeField] GameObject lostPointsTextPrefab;
  
    public void SetActiveReticleUI(bool state)
    {
        reticleUI.SetActive(state);
    }

    public void SetActiveTooltipUI(bool state)
    {
        tooltipUI.SetActive(state);
    }

    public void SetTooltipDoor(string text, int cost)
    {
        tooltipUI.SetActive(true);
        tooltipUI.GetComponentInChildren<TMP_Text>().text = text + "[Cost: " + cost + "]";
    }

    public void SetPointsUIText(int points)
    {
        pointsUI.GetComponentInChildren<TMP_Text>().text = points.ToString();
    }

    public void SetRoundUIText(int round)
    {
        roundUI.GetComponentInChildren<TMP_Text>().text = round.ToString();
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


}
