using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundSystem : MonoBehaviour
{
    int roundNum;
    int zombieHP;
    int zombieAmount;


    bool nextRound;

    [SerializeField] AudioClip nextRoundSound;
    // Start is called before the first frame update
    void Awake()
    {
        roundNum = 1;
        zombieHP = 150;
        zombieAmount = Mathf.RoundToInt(roundNum * 0.15f * 25);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CalculateZombieStats()
    {
        if(roundNum <= 10)
        {
            zombieAmount = Mathf.RoundToInt(roundNum * 0.125f * 24); //Actual original zombies health formula
            zombieHP = zombieHP + 100;
        }

        else
        {
            zombieHP = Mathf.RoundToInt(zombieHP + (zombieHP * 0.10f));  //Actual original zombies amount formula depending on round
            zombieAmount = Mathf.RoundToInt(0.000058f * roundNum * 
                roundNum * roundNum + 0.074032f * roundNum * roundNum + 
                0.718119f * roundNum + 14.7386999f);
        }

        
    }

    public (int roundNum, int zombieHP, int zombieAmount) GetNewRoundStats()
    {
        return (roundNum,zombieHP,zombieAmount);
    }

    public void RoundEnd()
    {
        StartCoroutine(NextRoundDelay());
    }

    IEnumerator NextRoundDelay()
    {
        FindObjectOfType<Audio>().PlaySound(nextRoundSound, 0);
        float delay = nextRoundSound.length;
        yield return new WaitForSeconds(delay-8);
        roundNum++;
        CalculateZombieStats();
        GetComponent<ZombieSpawner>().StartRound();
    }


}
