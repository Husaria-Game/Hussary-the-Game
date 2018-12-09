using System;
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
        GameManager.Instance.endTurnButtonManager.ARSceneBecomesActive();
        SceneManager.LoadScene("ARScene", LoadSceneMode.Additive);
    }

    public void ARSceneResult(bool ARResult, int gameMode)
    {
        // resume game 
        //Time.timeScale = 1;
        if (ARResult == true && gameMode == 1)
        {
            BonusEffects.Instance.createMoneyGainEffect(ARControl.arPoints);
            ARControl.arPoints = 0;
        }

        if (ARResult == true && gameMode == 2)
        {
            //BonusEffects.Instance.createMoneyGainEffect(ARControl.arPoints);
            ARControl.arHits = 0;
        }
    }
}
