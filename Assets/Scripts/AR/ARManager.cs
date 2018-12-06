﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AugmentedRealitySystem : MonoBehaviour
{
    // SINGLETON
    public static AugmentedRealitySystem Instance;
    void Awake()
    {
        Instance = this;
    }

    public void GoToARScene()
    {
        //Output this to console when Button1 or Button3 is clicked
        //Time.timeScale = 0;
        //visuals.SetActive(false);
        GameManager.Instance.endTurnButtonManager.ARSceneBecomesActive();
        SceneManager.LoadScene("ARScene", LoadSceneMode.Additive);
    }

    public void ARSceneResult(bool ARResult)
    {
        // resume game 
        //Time.timeScale = 1;
        if (ARResult)
        {
            BonusEffects.Instance.createMoneyGainEffect(ARControl.arPoints);
            ARControl.arPoints = 0;
        }
    }
}
