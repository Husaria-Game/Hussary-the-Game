﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using System;

public class SpeechRecognitionSystem : MonoBehaviour
{
    //Elements needed to creat listening system
    private string[] wordsToRecognize = new string[] {"moc", "obrona", "zbrodnia", "pomór", "fortuna" };
    private ConfidenceLevel confidenceLevel = ConfidenceLevel.Low;
    private PhraseRecognizer recognizer;

    public enum SpeechSign
    {
        nic, moc, obrona, zbrodnia, pomór, fortuna
    }
    public SpeechSign currentSpeechSign;

    //SpeechSign Images to load
    public Sprite power;
    public Sprite defence;
    public Sprite blight;
    public Sprite crime;
    public Sprite fortune;

    public Image speechSign;

    //Elements showing what word was heard
    public Text resultOfVoiceCommand;
    public string heardWord = "";

    //Other elements
    public System.Random random = new System.Random();
    private const int SPEECH_EFFECT_CHANCE = 30;

    private bool isEffectgoingToTakePlace = false; //variable to later achieve effect in unit card
    private IEnumerator toStop = null;

    void Start()
    {
        if (wordsToRecognize != null)
        {
            recognizer = new KeywordRecognizer(wordsToRecognize, confidenceLevel);
            recognizer.OnPhraseRecognized += WhenPhraseRecognized;
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
            heardWord = "moc";
            resultOfVoiceCommand.text = heardWord;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            heardWord = "obrona";
            resultOfVoiceCommand.text = heardWord;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            heardWord = "pomór";
            resultOfVoiceCommand.text = heardWord;
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            heardWord = "zbrodnia";
            resultOfVoiceCommand.text = heardWord;
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            CheckWhetherToShowSpeechSign();
        }
    }
    ///////////////////////////// 

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
            ShowSpeechSign(power, SpeechSign.moc);
        }
        else if(number >= 20 && number < 40)
        {
            ShowSpeechSign(defence, SpeechSign.obrona);
        }
        else if (number >= 40 && number < 60)
        {
            ShowSpeechSign(blight, SpeechSign.pomór);
        }
        else if (number >= 60 && number < 80)
        {
            ShowSpeechSign(crime, SpeechSign.zbrodnia);
        }
        else if (number >= 80 && number < 100)
        {
            ShowSpeechSign(fortune, SpeechSign.fortuna);
        }
    }

    public void ShowSpeechSign(Sprite signImage, SpeechSign signMark)
    {
        StartCoroutine(ShowSpeechSignCoroutine(signImage, signMark));
    }

    IEnumerator ShowSpeechSignCoroutine(Sprite signImage, SpeechSign signMark)
    {
        toStop = ShowSpeechSignCoroutine(signImage, signMark);
        int number = random.Next(5, 20);

        yield return new WaitForSeconds(number);  //Random second in which system starts to show signImage
        recognizer.Start();
        speechSign.sprite = signImage;
        speechSign.enabled = true;
        currentSpeechSign = signMark;

        yield return new WaitForSeconds(5f);      //How long signImage is going to last
        CompareShownSignAndSpeech();
        speechSign.enabled = false;
        currentSpeechSign = SpeechSign.nic;
        heardWord = "";
        resultOfVoiceCommand.text = heardWord;
        recognizer.Stop();
    }

    private void CompareShownSignAndSpeech()
    {     
        string currentSpeechSignString = currentSpeechSign.ToString();

        if (currentSpeechSignString.Equals(heardWord))
        {
            GetComponent<AudioSource>().Play();
            isEffectgoingToTakePlace = true;
            Debug.Log("EffectOfSpeech");
        }
        else
        {
            Debug.Log("No Effect");
        }
    }

    //not working
    public void StopCoroutineIfTurnButtonClicked()
    {
        
    }
}
