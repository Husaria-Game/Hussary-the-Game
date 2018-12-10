using UnityEngine;

public class HybridEffectsSystem : MonoBehaviour
{
    public SpeechRecognitionSystem SRS;
    public AugmentedRealitySystem ARS;

    private System.Random random = new System.Random();
    private const int HYBRID_EFFECT_CHANCE = 30;

    public void CheckWhetherToUseHybridEffect()
    {
        int number = random.Next(0, 101);
        if (number < HYBRID_EFFECT_CHANCE)
        {
            //Case When both hybrid modules are available
            if (GameManager.Instance.isASRAvailable && GameManager.Instance.isARAvailable)
            {
                number = random.Next(0, 101);
                if (number <= 50)
                {
                    SRS.WhatSpeechSignToShow();
                }
                else if (number > 50)
                {
                    ARS.GoToARScene();
                }
            }
            //Cases whne only one of hybrid system is available
            else if (GameManager.Instance.isASRAvailable) SRS.WhatSpeechSignToShow();
            else if (GameManager.Instance.isARAvailable) ARS.GoToARScene();
        }
    }
}

