﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawnPoint : MonoBehaviour
{
    [SerializeField] bool isActive = true;

    public bool IsActive()
    {
        return isActive;
    }

    public void SetActive(bool state)
    {
        isActive = state;
    }
}
