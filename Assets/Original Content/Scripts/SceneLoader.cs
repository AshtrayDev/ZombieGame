﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public void PlayAgainButton()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }
}