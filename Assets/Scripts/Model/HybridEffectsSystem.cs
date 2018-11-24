using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HybridEffectsSystem : MonoBehaviour {

    public SpeechRecognitionSystem SRS;
    //public AugmentedRealitySystem ARS;

    private System.Random random = new System.Random();
    private const int HYBRID_EFFECT_CHANCE = 30;

    public void CheckWhetherToUseHybridEffect()
    {
        int number = random.Next(0, 101);
        if (number < HYBRID_EFFECT_CHANCE)
        {
            Debug.Log("Hybrid Effect possible");
            number = random.Next(0, 101);
            if(number <= 50)
            {
                Debug.Log("ASR System runs...");
                SRS.WhatSpeechSignToShow();
            }
            else
            {
                Debug.Log("AR System runs...");
                //ARS.RunARScene();
            }
        }
    }
}
