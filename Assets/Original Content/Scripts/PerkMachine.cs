using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkMachine : MonoBehaviour
{

    [SerializeField] PerkList perk;
    [SerializeField] int cost;
    [SerializeField] bool needsPower = true;


    GameObject player;

    bool isTriggered;
    bool isPowered;

    UIHandler ui;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<BoxCollider>();
        ui = FindObjectOfType<UIHandler>();
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
                player = other.gameObject;
                FindObjectOfType<UIHandler>().SetTooltipBuy("Hold F to buy " + perk.ToString(), cost);
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
        Material material = GetComponent<MeshRenderer>().material;
        material.EnableKeyword("_EMISSION");
    }
}
