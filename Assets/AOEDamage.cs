using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEDamage : MonoBehaviour
{

    public float weaponDamage;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other);
        other.SendMessage("OnExplosiveHit", weaponDamage, SendMessageOptions.DontRequireReceiver);
        Destroy(this);
    }
}
