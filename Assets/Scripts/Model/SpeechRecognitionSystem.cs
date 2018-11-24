using System.Collections;
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
    private string debugText;

    //Other elements
    public System.Random random = new System.Random();
    private const int SPEECH_EFFECT_CHANCE = 30;

    //Variables needed to achieve s
    private bool isEffectgoingToTakePlace = false; //variable to later achieve effect in unit card
    Coroutine co;

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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            heardWord = "moc";
            resultOfVoiceCommand.text = heardWord;
            debugText = "Twoja losowa jednostka otrzymuje wsparcie zaopatrzeniowe + 1 Siła.";
            Defendable randomCard = GameManager.Instance.pickRandomDropZoneUnitCard(GameManager.Instance.currentPlayer);
            if (randomCard != null)
            {
                Transform cardUnit = randomCard.GetComponent<CardDisplayLoader>().Unit.transform;
                GameManager.Instance.createFriendlyBonusEffect(randomCard, cardUnit, CardVisualStateEnum.TacticsStrengthOne, 1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            heardWord = "obrona";
            resultOfVoiceCommand.text = heardWord;
            debugText = "Twoja losowa jednostka otrzymuje wsparcie zaopatrzeniowe + 1 Pancerz.";
            Defendable randomCard = GameManager.Instance.pickRandomDropZoneUnitCard(GameManager.Instance.otherPlayer);
            if (randomCard != null)
            {
                Transform cardUnit = randomCard.GetComponent<CardDisplayLoader>().Unit.transform;
                GameManager.Instance.createHostileBonusEffect(randomCard, cardUnit, CardVisualStateEnum.TacticsWithAim, 1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            heardWord = "pomór";
            resultOfVoiceCommand.text = heardWord;
            debugText = "Wrogie jednostki trawione chorobą tracą -1 Pancerza.";
            Defendable randomCard = GameManager.Instance.pickRandomDropZoneUnitCard(GameManager.Instance.currentPlayer);
            if (randomCard != null)
            {
                Transform cardUnit = randomCard.GetComponent<CardDisplayLoader>().Unit.transform;
                GameManager.Instance.createFriendlyBonusEffect(randomCard, cardUnit, CardVisualStateEnum.TacticsHealOne, 2);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            heardWord = "zbrodnia";
            resultOfVoiceCommand.text = heardWord;
            debugText = "Wrogi bohater otruty przez szpiegów traci - 1 Pancerza.";

            GameObject hero = null;
            DropZone initialDropZone = null;
            if (GameManager.Instance.currentPlayer == GameManager.Instance.playerSouth)
            {
                hero = GameManager.Instance.heroNorth;
                initialDropZone = GameManager.Instance.dropZoneSouth;
            }
            else if (GameManager.Instance.currentPlayer == GameManager.Instance.playerNorth)
            {
                hero = GameManager.Instance.heroSouth;
                initialDropZone = GameManager.Instance.dropZoneNorth;

            }
            GameManager.Instance.createHostileEffectHero(hero, 1, initialDropZone);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            heardWord = "fortuna";
            resultOfVoiceCommand.text = heardWord;
            debugText = "Jednostki wsparcia przybywają - losujesz dodakową kartę.";
            GameManager.Instance.drawNewCard(GameManager.Instance.currentPlayer, false);
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            GameManager.Instance.createMoneyGainEffect(2);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            CheckWhetherToShowSpeechSign();
        }
    }

    public void CheckWhetherToShowSpeechSign()
    {
        co = null; //reset coroutine of ASR so that pushing turnButton wont work when dont needed
        int number = random.Next(0, 101);
        if(number < SPEECH_EFFECT_CHANCE)
        {
            WhatSpeechSignToShow();
            Debug.Log("ASR Effect Possible");
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
        co = StartCoroutine(ShowSpeechSignCoroutine(signImage, signMark));
    }

    IEnumerator ShowSpeechSignCoroutine(Sprite signImage, SpeechSign signMark)
    {
        int number = random.Next(5, 20);

        yield return new WaitForSeconds(number);  //Random second in which system starts to show signImage
        //recognizer.Start();
        speechSign.sprite = signImage;
        speechSign.enabled = true;
        currentSpeechSign = signMark;

        yield return new WaitForSeconds(5f);      //How long signImage is going to last
        CompareShownSignAndSpeech();
        speechSign.enabled = false;
        currentSpeechSign = SpeechSign.nic;
        heardWord = "";
        resultOfVoiceCommand.text = heardWord;
        //recognizer.Stop();
    }

    private void CompareShownSignAndSpeech()
    {     
        string currentSpeechSignString = currentSpeechSign.ToString();

        if (currentSpeechSignString.Equals(heardWord))
        {
            //Play sound effect and put text in debugMessegeBox
            GameManager.Instance.debugMessageBox.ShowDebugText("Rozpoznano komendę głosową:   " + currentSpeechSignString + ". " + debugText, true);
            GameManager.Instance.audioGenerator.PlayClip(GameManager.Instance.audioGenerator.effectAudio);
            isEffectgoingToTakePlace = true;
            Debug.Log("Effect of Speech");
        }
        else
        {
            //Play sound effect and put text in debugMessegeBox
            GameManager.Instance.debugMessageBox.ShowDebugText("Nie rozpoznano komendy głosowej - brak efektu", false);
            GameManager.Instance.audioGenerator.PlayClip(GameManager.Instance.audioGenerator.noEffectAudio);
            Debug.Log("No Effect of Speech");
        }
    }

    public void StopCoroutineIfTurnButtonClicked()
    {
        if(co != null)
        {
            StopCoroutine(co);
            //Those are in case turnButton clicked when speech sign is already shown and recognizer listen
            speechSign.enabled = false;
            currentSpeechSign = SpeechSign.nic;
            heardWord = "";
            resultOfVoiceCommand.text = heardWord;
            //recognizer.Stop();
        }

    }
}
