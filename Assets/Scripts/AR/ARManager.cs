using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ARManager : MonoBehaviour
{
    // SINGLETON
    public static ARManager Instance;
    void Awake()
    {
        Instance = this;
    }

    public void goToARScene()
    {
        //Output this to console when Button1 or Button3 is clicked
        //Time.timeScale = 0;
        //visuals.SetActive(false);
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
