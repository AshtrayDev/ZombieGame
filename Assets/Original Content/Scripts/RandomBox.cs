using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBox : MonoBehaviour
{

    public List<GameObject> weapons = new List<GameObject>();
    [SerializeField] Trigger trigger;
    [SerializeField] AudioClip boxJingle;
    [SerializeField] Transform startGunPos;
    [SerializeField] Transform endGunPos;
    [SerializeField] int costToOpen = 950;
    [SerializeField] float weaponChangeDelay = 0.2f;
    [SerializeField] float animationStopOffset = -1f;
    [SerializeField] float timeBoxOpen = 10f;


    bool isTriggered;
    bool isOpen;
    bool isAnimating;
    bool isDelayed;
    Coroutine boxTimer;

    int indexOfWeaponShown;



    float animationTime;
    GameObject weaponShown;

    PlayerPoints points;
    UIHandler ui;
    AudioSource audioSource;
    WeaponSwitcher switcher;


    private void Start()
    {
        switcher = FindObjectOfType<WeaponSwitcher>();
        audioSource = FindObjectOfType<AudioSource>();
        ui = FindObjectOfType<UIHandler>();
        points = FindObjectOfType<PlayerPoints>();
        animationTime = audioSource.clip.length + animationStopOffset;
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact") && isOpen && trigger.IsTriggeredByPlayer() && !isAnimating && !isDelayed)
        {
            TakeWeapon();
        }

        else if (Input.GetButtonDown("Interact") && !isOpen && !isDelayed && trigger.IsTriggeredByPlayer() && points.IsAbleToAfford(costToOpen))
        {
            OpenBox();
        }
    }

    public void TriggerEnter()
    {
        if (trigger.IsTriggeredByPlayer() && isOpen &&!isAnimating)
        {
            ui.SetToolTipTakeWeapon();
        }

        if (trigger.IsTriggeredByPlayer() && !isOpen)
        {
            ui.SetTooltipBuy("Hold F for a Random Weapon ", costToOpen);
        }

    }

    public void TriggerExit()
    {
        if (!trigger.IsTriggeredByPlayer())
        {
            ui.SetActiveTooltipUI(false);
        }
    }

    void OpenBox()
    {
        isOpen = true;
        print("Opening box");
        StopPlayerInteract();
        BoxOpenAnimation();
        CreateGun();
        PlayMusic();
        ChangeAndMoveGunUp();
        ChangeGun();
        AllowToTakeGun();
    }

    void StopPlayerInteract()
    {
        trigger.SetTriggerState(false);
        ui.SetActiveTooltipUI(false);
    }

    void BoxOpenAnimation()
    {
        GetComponent<Animator>().SetBool("BoxOpen", true);
    }

    void CreateGun()
    {
        weaponShown = Instantiate(weapons[Random.Range(0, weapons.Count - 1)], startGunPos.position, Quaternion.Euler(0, -90, 0), gameObject.transform);
        (float scale, Vector3 rotation) = weaponShown.GetComponent<Weapon>().GetBoxDetails();
        weaponShown.transform.localScale = new Vector3(scale, scale, scale);
        weaponShown.transform.rotation = Quaternion.Euler(rotation);
    }

    void PlayMusic()
    {
        audioSource.Play();
    }

    void ChangeAndMoveGunUp()
    {
        StartCoroutine(MoveGunUpAnimation());
        StartCoroutine(ChangeGun());
    }

    IEnumerator MoveGunUpAnimation()
    {
        isAnimating = true;
        float yDistanceRemaining = weaponShown.transform.position.y - endGunPos.position.y;
        float yPosPerSecond = yDistanceRemaining / animationTime;
        float yPosPerFrame = yPosPerSecond * Time.deltaTime;

        float elapsedTime = 0;
        Vector3 startingPos = weaponShown.transform.position;
        while (elapsedTime < animationTime)
        {
            weaponShown.transform.position = Vector3.Lerp(startingPos, endGunPos.transform.position, (elapsedTime / animationTime));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        weaponShown.transform.position = endGunPos.transform.position;
        isAnimating = false;
        TriggerEnter();
    }

    IEnumerator ChangeGun()
    {

        while (isAnimating)
        {

            GameObject oldWeapon = weaponShown;
            indexOfWeaponShown = Random.Range(0, weapons.Count - 1);
            GameObject newWeapon = Instantiate(weapons[indexOfWeaponShown], weaponShown.transform.position, Quaternion.identity, gameObject.transform);
            (float scale, Vector3 rotation) = newWeapon.GetComponent<Weapon>().GetBoxDetails();
            newWeapon.transform.localScale = new Vector3(scale, scale, scale);
            newWeapon.transform.localRotation = Quaternion.Euler(rotation);
            weaponShown = newWeapon;
            Destroy(oldWeapon);
            yield return new WaitForSeconds(weaponChangeDelay);
        }
        boxTimer = StartCoroutine(BoxCloseTimer());
        AllowToTakeGun();
    }

    void AllowToTakeGun()
    {
        trigger.SetTriggerState(true);
    }

    IEnumerator BoxCloseTimer()
    {
        yield return new WaitForSeconds(timeBoxOpen);
        StartCoroutine(BoxCloseAnimation());
    }

    void TakeWeapon()
    {
        isDelayed = true;
        StopCoroutine(boxTimer);
        ui.SetActiveTooltipUI(false);


        if (switcher.GetAmountOfWeapons() == 2)
        {
            switcher.GetCurrentWeapon().DeleteWeapon();
        }

        GameObject boughtWeapon = Instantiate(weapons[indexOfWeaponShown], FindObjectOfType<PlayerHealth>().transform.position, Quaternion.identity, switcher.transform);
        boughtWeapon.SetActive(true);
        StartCoroutine(BoxCloseAnimation());
    }

    IEnumerator BoxCloseAnimation()
    {
        GetComponent<Animator>().SetBool("BoxOpen", false);
        Destroy(weaponShown);
        yield return new WaitForSeconds(2f);
        isOpen = false;
        isDelayed = false;
        TriggerEnter();
    }
}
