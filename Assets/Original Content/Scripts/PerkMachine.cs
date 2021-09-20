using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkMachine : MonoBehaviour
{

    [SerializeField] PerkList perk;
    [SerializeField] int cost;
    [SerializeField] bool needsPower = true;
    [SerializeField] float musicVolume = 1f;


    GameObject player;

    bool isTriggered;
    bool isPowered;

    UIHandler ui;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<BoxCollider>();
        ui = FindObjectOfType<UIHandler>();
        if (!needsPower)
        {
            PowerOn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact") && isTriggered)
        {
            if (needsPower && !isPowered)  {return;}

            if (player.GetComponent<PlayerPoints>().IsAbleToAfford(cost))
            {
                BuyPerk();
                FindObjectOfType<UIHandler>().SetActiveTooltipUI(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerHealth>())
        {
            if (!other.GetComponent<PlayerPerk>().HasPerk(perk))
            {
                if (isPowered)
                {
                    FindObjectOfType<UIHandler>().SetTooltipBuy("Hold F to buy " + perk.ToString(), cost);
                }

                else
                {
                    FindObjectOfType<UIHandler>().SetToolTipCustom("Needs Power!");

                }
                player = other.gameObject;
                isTriggered = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerHealth>())
        {
            player = null;
            FindObjectOfType<UIHandler>().SetActiveTooltipUI(false);
            isTriggered = false;
        }
    }

    void BuyPerk()
    {
        player.GetComponent<PlayerPerk>().AddPerk(perk);
        player.GetComponent<PlayerPoints>().RemovePoints(cost);
    }

    public void PowerOn()
    {
        isPowered = true;
        GetComponent<AudioSource>().volume = musicVolume;
        Material material = GetComponent<MeshRenderer>().material;
        material.EnableKeyword("_EMISSION");
    }
}
