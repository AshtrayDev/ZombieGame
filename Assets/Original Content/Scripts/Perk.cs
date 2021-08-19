using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk : MonoBehaviour
{

    PerkList perk;
    PlayerHealth player;



    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Perk(PerkList perk)
    {
        this.perk = perk;
    }

    public void SetPerk(PerkList perk)
    {
        this.perk = perk;
    }



    public PerkList GetPerkName()
    {
        return perk;
    }

}
