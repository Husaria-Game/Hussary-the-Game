using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class SpeechRecognitionSystem : MonoBehaviour
{
    //Elements needed to create listening system
    private string[] wordsToRecognize = new string[] {"atak", "obrona", "zbrodnia", "pomór", "fortuna" };
    private ConfidenceLevel confidenceLevel = ConfidenceLevel.Low;
    private PhraseRecognizer recognizer;

    //SpeechSign Images to load
    public Sprite power;
    public Sprite defence;
    public Sprite blight;
    public Sprite crime;
    public Sprite fortune;
    public Image speechSign;

    //Elements connected with specific voice effect
    private SpeechSign currentSpeechSign;
    public Text resultOfVoiceCommand;
    private string heardWord = "";
    private string debugText;
    private int effectPower;

    //Other elements
    public System.Random random = new System.Random();
    Coroutine coroutineToStop;

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

    void Start()
    {
        if (wordsToRecognize != null)
        {
            if(recognizer == null)
            {
                recognizer = new KeywordRecognizer(wordsToRecognize, confidenceLevel);
            }
            recognizer.OnPhraseRecognized += WhenPhraseRecognized;
            speechSign.enabled = false;
            resultOfVoiceCommand.text = heardWord;
        }
    }

    public void WhatSpeechSignToShow()
    {
        coroutineToStop = null; //reset coroutineToStop of ASR so that pushing turnButton wont work when dont needed
        int number = random.Next(0, 101);
        if(number < 20)
        {
            ShowSpeechSign(power, SpeechSign.atak);
            debugText = "Twoja losowa jednostka otrzymuje wsparcie zaopatrzeniowe + 1 Siła.";
            effectPower = 1;
        }
        else if(number >= 20 && number < 40)
        {
            ShowSpeechSign(defence, SpeechSign.obrona);
            debugText = "Twoja losowa jednostka otrzymuje wsparcie zaopatrzeniowe + 1 Pancerz.";
            effectPower = 1;
        }
        else if (number >= 40 && number < 60)
        {
            ShowSpeechSign(blight, SpeechSign.pomór);
            debugText = "Wrogie jednostki trawione chorobą tracą -1 Pancerza.";
            effectPower = 1;
        }
        else if (number >= 60 && number < 80)
        {
            ShowSpeechSign(crime, SpeechSign.zbrodnia);
            debugText = "Wrogi bohater otruty przez szpiegów traci - 1 Pancerza.";
            effectPower = 1;
        }
        else if (number >= 80 && number < 100)
        {
            ShowSpeechSign(fortune, SpeechSign.fortuna);
            debugText = "Jednostki wsparcia przybywają - losujesz dodakową kartę.";
            effectPower = 1;
        }
    }

    public void ShowSpeechSign(Sprite signImage, SpeechSign signMark)
    {
        coroutineToStop = StartCoroutine(ShowSpeechSignCoroutine(signImage, signMark));
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
            WhatSpeechBonusToGive(currentSpeechSignString, effectPower);
            currentSpeechSignString = currentSpeechSignString.ToUpper();
            //Play sound effect and put text in debugMessegeBox
            GameManager.Instance.debugMessageBox.ShowDebugText("Rozpoznano komendę głosową:   " + currentSpeechSignString + ".\n " + debugText, true);
            GameManager.Instance.audioGenerator.PlayClip(GameManager.Instance.audioGenerator.effectAudio);
        }
        else
        {
            //Play sound effect and put text in debugMessegeBox
            GameManager.Instance.debugMessageBox.ShowDebugText("Nie rozpoznano komendy głosowej - brak efektu", false);
            GameManager.Instance.audioGenerator.PlayClip(GameManager.Instance.audioGenerator.noEffectAudio);
        }
    }

    private void WhatSpeechBonusToGive(string effectToDo, int effectPower)
    {
        Defendable randomCard;
        if (effectToDo == "pomór")
        {
            randomCard = BonusEffects.Instance.pickRandomDropZoneUnitCard(GameManager.Instance.otherPlayer);
        }
        else
        {
            randomCard = BonusEffects.Instance.pickRandomDropZoneUnitCard(GameManager.Instance.currentPlayer);
        }
        if (randomCard != null)
        {
            Transform cardUnit = randomCard.GetComponent<CardDisplayLoader>().Unit.transform;
            if (effectToDo == "atak")
            {
                BonusEffects.Instance.createFriendlyBonusEffect(randomCard, cardUnit, CardVisualStateEnum.TacticsStrengthOne, effectPower);
            }
            else if (effectToDo == "obrona")
            {
                BonusEffects.Instance.createFriendlyBonusEffect(randomCard, cardUnit, CardVisualStateEnum.TacticsHealOne, effectPower);
            }
            else if (effectToDo == "pomór")
            {
                if (cardUnit != null)
                    BonusEffects.Instance.createHostileBonusEffect(randomCard, cardUnit, CardVisualStateEnum.TacticsAttackOne, effectPower);
            }
        }

        if (effectToDo == "zbrodnia")
        {
            BonusEffects.Instance.createHostileEffectHero(GameManager.Instance.otherPlayer.heroVisual.gameObject, 
                GameManager.Instance.currentPlayer.dropZoneVisual, effectPower);
        }
        else if (effectToDo == "fortuna")
        {
            BonusEffects.Instance.drawNewCard(GameManager.Instance.currentPlayer, false);
        }    
    }

    public void StopCoroutineIfTurnButtonClicked()
    {
        if(coroutineToStop != null)
        {
            StopCoroutine(coroutineToStop);
            speechSign.enabled = false;
            currentSpeechSign = SpeechSign.nic;
            heardWord = "";
            resultOfVoiceCommand.text = heardWord;
            //recognizer.Stop();
        }
    }
}
