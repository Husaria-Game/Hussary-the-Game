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
    public bool isARSceneActive = false;
    
    void Awake()
    {
        Instance = this;
    }

    public void GoToARScene()
    {
        isARSceneActive = true;
        
        // Hide and lock cursor 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        SceneManager.LoadScene("ARScene", LoadSceneMode.Additive);
    }

    public void ARSceneResult(bool ARResult, int gameMode)
    {
        // resume game 
        //Time.timeScale = 1;
        
        // Unhide and unlock cursor 
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        if (ARResult == true && gameMode == 1)
        {
            BonusEffects.Instance.createMoneyGainEffect(ARControl.arPoints);
            ARControl.arPoints = 0;
        }

        if (ARResult == true && gameMode == 2)
        {
            BonusEffects.Instance.createHostileEffectHero(GameManager.Instance.currentPlayer.heroVisual.gameObject, GameManager.Instance.currentPlayer.dropZoneVisual , ARControl.arHits);
            ARControl.arHits = 0;
        }
        isARSceneActive = false;
    }


}
