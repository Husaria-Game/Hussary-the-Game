using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;


public class SpeechRecognitionSystem : MonoBehaviour
{
    private string[] wordsToRecognize = new string[] { "pomoc", "szansa" };
    private ConfidenceLevel confidenceLevel = ConfidenceLevel.Low;
    private PhraseRecognizer recognizer;

    public Text resultOfVoiceCommand;
    public static string word;


    void Start()
    {
        if (wordsToRecognize != null)
        {
            recognizer = new KeywordRecognizer(wordsToRecognize, confidenceLevel);
            recognizer.Start();
            recognizer.OnPhraseRecognized += WhenPhraseRecognized;
        }
    }

    private void WhenPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        word = args.text;
        resultOfVoiceCommand.text = word;
    }

    private void OnApplicationQuit()
    {
        if (recognizer != null && recognizer.IsRunning)
        {
            recognizer.OnPhraseRecognized -= WhenPhraseRecognized;
            recognizer.Stop();
        }
    }
}
