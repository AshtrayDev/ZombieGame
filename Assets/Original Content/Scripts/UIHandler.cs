using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHandler : MonoBehaviour
{
    [Header("UI Links")]
    public GameObject reticleUI;
    public GameObject tooltipUI;
    public GameObject pointsUI;
    public GameObject addedPointsUI;
    public GameObject roundUI;
    public GameObject ammoUI;
    public GameObject perkUI;
    public GameObject pickupUI;

    [Header("Perks")]
    [SerializeField] Sprite jugImage;
    [SerializeField] Sprite colaImage;
    [SerializeField] Sprite tapImage;
    [SerializeField] Sprite reviveImage;


    [Header("Points")]
    [SerializeField] GameObject addPointsTextPrefab;
    [SerializeField] GameObject lostPointsTextPrefab;
    public RectTransform pointsTextTransform;

    [Header("Pickups")]
    [SerializeField] Sprite instakillImage;
    [SerializeField] Sprite doublePointsImage;
    [SerializeField] GameObject pickupUIPrefab;
    [SerializeField] float pickupUIYPos;

    Image[] perkSprites;
    List<GameObject> pickupWindows = new List<GameObject>();
    List<Pickup.PickupType> types = new List<Pickup.PickupType>();

    private void Start()
    {
        perkSprites = perkUI.GetComponentsInChildren<Image>();
        foreach(Image sprite in perkSprites)
        {
            sprite.enabled = false;
        }
    }

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

    public void SetToolTipRepairBarrier()
    {
        tooltipUI.SetActive(true);
        tooltipUI.GetComponentInChildren<TMP_Text>().text = "Hold F to Repair Barrier";
    }

    public void SetToolTipTakeWeapon()
    {
        tooltipUI.SetActive(true);
        tooltipUI.GetComponentInChildren<TMP_Text>().text = "Hold F to Take Weapon";
    }

    //Points---------------------------------------------------------------------------
    public void SetPointsUIText(int points)
    {
        pointsUI.GetComponentInChildren<TMP_Text>().text = points.ToString();
    }
    public void AddPointsUI(int points, bool gainedPoints)
    {
        GameObject changedPoints;

        Vector2 pos = pointsTextTransform.anchoredPosition;

        if (gainedPoints)
        {
            changedPoints = Instantiate(addPointsTextPrefab, addedPointsUI.transform);
            changedPoints.GetComponent<RectTransform>().anchoredPosition = pos;
        }
        else
        {
            changedPoints = Instantiate(lostPointsTextPrefab, pos, Quaternion.identity, addedPointsUI.transform);
        }
        
        changedPoints.GetComponentInChildren<Animator>().Play(0);
        if (gainedPoints)
        {
            changedPoints.GetComponentInChildren<TMP_Text>().text = "+ " + points.ToString();
        }
        else
        {
            changedPoints.GetComponentInChildren<TMP_Text>().text = "- " + points.ToString();
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

    //Perks
    public void AddPerk(PerkList perk, int imageNum)
    {
        perkSprites[imageNum].sprite = FindPerkImage(perk);
        perkSprites[imageNum].enabled = true;
    }

    public void RemovePerk(int imageNum)
    {
        print(imageNum);
        perkSprites[imageNum].enabled = false;
    }

    Sprite FindPerkImage(PerkList perk)
    {
        switch (perk)
        {
            case PerkList.Juggernog:
                {
                    return jugImage;
                }
            case PerkList.SpeedCola:
                {
                    return colaImage;
                }
            case PerkList.DoubleTap:
                {
                    return tapImage;
                }
            case PerkList.QuickRevive:
                {
                    return reviveImage;
                }
            default:
                {
                    return null;
                }
        }
    }

    //Pickups
    public void AddPickup(Pickup.PickupType type)
    {
        if (pickupWindows.Count == 0)
        {
            Vector2 pos = new Vector2(0, pickupUIYPos);
            GameObject newUI = Instantiate(pickupUIPrefab, pickupUI.transform);
            newUI.GetComponent<RectTransform>().anchoredPosition = pos;
            newUI.GetComponent<Image>().sprite = GetSpriteFromPickupType(type);
            pickupWindows.Add(newUI);
            types.Add(type);
        }

        else
        {


            float size = pickupWindows[pickupWindows.Count - 1].GetComponent<RectTransform>().sizeDelta.x;

            foreach (GameObject window in pickupWindows)
            {
                Vector2 newPos = window.transform.position;
                newPos.x = newPos.x - size/2;
                window.transform.position = newPos;
            }

            float xPos = pickupWindows[pickupWindows.Count - 1].GetComponent<RectTransform>().anchoredPosition.x;


            Vector2 pos = new Vector2(xPos+size, pickupUIYPos);
            GameObject newUI = Instantiate(pickupUIPrefab, pickupUI.transform);
            newUI.GetComponent<RectTransform>().anchoredPosition = pos;
            newUI.GetComponent<Image>().sprite = GetSpriteFromPickupType(type);
            pickupWindows.Add(newUI);
            types.Add(type);
        }
    }

    public void RemovePickup(Pickup.PickupType type)
    {
        if(pickupWindows.Count == 0)
        {
            Debug.LogWarning("RemovePickup called when no pickup UIs active.");
        }

        if(pickupWindows.Count == 1)
        {
            GameObject window = FindUIWindow(type);
            pickupWindows.Remove(window);
            types.Remove(type);
            Destroy(window);
        }

        if(pickupWindows.Count > 1)
        {
            GameObject window = FindUIWindow(type);
            pickupWindows.Remove(window);
            Destroy(window);
            types.Remove(type);
            RestructureExistingWindows();
        }
    }

    GameObject FindUIWindow(Pickup.PickupType type)
    {
        foreach(Pickup.PickupType pickup in types)
        {
            if (pickup == type)
            {
                return pickupWindows[types.IndexOf(pickup)];
            }
        }

        Debug.LogWarning("No windows found in FindUIWindow function.");
        return null;
    }

    void RestructureExistingWindows()
    {
        float size = pickupWindows[0].GetComponent<RectTransform>().sizeDelta.x;

        foreach(GameObject window in pickupWindows)
        {
            RectTransform rect = window.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(-size/2  * (pickupWindows.Count-1), pickupUIYPos);

            Vector2 addedPos = new Vector2();
            addedPos.x = pickupWindows.IndexOf(window) * size;
            rect.anchoredPosition = rect.anchoredPosition + addedPos;
        }
    }

    Sprite GetSpriteFromPickupType(Pickup.PickupType type)
    {
        switch (type)
        {
            case Pickup.PickupType.instakill:
                {
                    return instakillImage;
                }
            case Pickup.PickupType.doublePoints:
                {
                    return doublePointsImage;
                }
            default:
                {
                    Debug.LogWarning("No image associated with pickupType. (Function: GetImageFromPickupType)");
                    return null;
                }
        }
    }
}
