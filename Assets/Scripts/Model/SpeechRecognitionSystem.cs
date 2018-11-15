using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using System;


public class SpeechRecognitionSystem : MonoBehaviour
{
    private string[] wordsToRecognize = new string[] { /*"pomoc", "szansa"*/ "sakwa", "miecz", "tarcza", "trucizna", "ręka" };
    private ConfidenceLevel confidenceLevel = ConfidenceLevel.Low;
    private PhraseRecognizer recognizer;

    public Text resultOfVoiceCommand;
    public string heardWord = "";

    //For testing puropose only
    public enum SpeechSign
    {
        nic, miecz, tarcza, trucizna, sakwa, ręka
    }

    public SpeechSign currentSpeechSign;

    public System.Random random = new System.Random();

    private const int SPEECH_EFFECT_CHANCE = 30;

    private IEnumerator toStop = null;

    //Images to load
    public Sprite sword;
    public Sprite shield;
    public Sprite pouch;
    public Sprite poison;
    public Sprite hand;
    
    public Image speechSign;
    private bool isEffectgoingToTakePlace = false;
    ////////////////////////////////////////////

    void Start()
    {
        if (wordsToRecognize != null)
        {
            recognizer = new KeywordRecognizer(wordsToRecognize, confidenceLevel);
            recognizer.Start();
            recognizer.OnPhraseRecognized += WhenPhraseRecognized;

            //For testing puropose only
            speechSign.enabled = false;
            resultOfVoiceCommand.text = heardWord;
        }
    }

    private void WhenPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        heardWord = args.text;
        resultOfVoiceCommand.text = heardWord;
    }

    private void OnApplicationQuit()
    {
        if (recognizer != null && recognizer.IsRunning)
        {
            recognizer.OnPhraseRecognized -= WhenPhraseRecognized;
            recognizer.Stop();
        }
    }

    //For testing puropose only
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            heardWord = "sword";
            resultOfVoiceCommand.text = heardWord;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            heardWord = "shield";
            resultOfVoiceCommand.text = heardWord;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            heardWord = "pouch";
            resultOfVoiceCommand.text = heardWord;
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            heardWord = "poison";
            resultOfVoiceCommand.text = heardWord;
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            CheckWhetherToShowSpeechSign();
        }
    }

    public void CheckWhetherToShowSpeechSign()
    {
        int number = random.Next(0, 101);
        if(number < SPEECH_EFFECT_CHANCE)
        {
            WhatSpeechSignToShow();
            Debug.Log("What effect");
        }

    }

    public void WhatSpeechSignToShow()
    {
        int number = random.Next(0, 101);
        if(number < 20)
        {
            ShowSpeechSign(sword, SpeechSign.miecz);
        }
        else if(number >= 20 && number < 40)
        {
            ShowSpeechSign(shield, SpeechSign.tarcza);
        }
        else if (number >= 40 && number < 60)
        {
            ShowSpeechSign(pouch, SpeechSign.sakwa);
        }
        else if (number >= 60 && number < 80)
        {
            ShowSpeechSign(poison, SpeechSign.trucizna);
        }
        else if (number >= 80 && number < 100)
        {
            ShowSpeechSign(hand, SpeechSign.ręka);
        }
    }

    public void ShowSpeechSign(Sprite signImage, SpeechSign signMark)
    {
        StartCoroutine(ShowSpeechSignCoroutine(signImage, signMark));
    }

    IEnumerator ShowSpeechSignCoroutine(Sprite signImage, SpeechSign signMark)
    {
        //toStop = ShowSpeechSignCoroutine(signImage, signMark);
        int number = random.Next(5, 20);

        yield return new WaitForSeconds(number);

        speechSign.sprite = signImage;
        speechSign.enabled = true;
        currentSpeechSign = signMark;

        yield return new WaitForSeconds(5f);

        CompareShownSignAndSpeech();
        speechSign.enabled = false;
        currentSpeechSign = SpeechSign.nic;
        heardWord = "";
        toStop = null;
    }

    private void CompareShownSignAndSpeech()
    {     
        string currentSpeechSignString = currentSpeechSign.ToString();

        if (currentSpeechSignString.Equals(heardWord))
        {
            isEffectgoingToTakePlace = true;
            Debug.Log("EffectOfSpeech");
        }
        else
        {
            Debug.Log("No Effect");
        }
    }
    //not working
    /*
    public void StopCoroutineIfTurnButtonClicked()
    {
        if(toStop != null)
        {
            StopCoroutine(toStop);
            Debug.Log("Coroutine Stopped");
        }
    }
    */

}
