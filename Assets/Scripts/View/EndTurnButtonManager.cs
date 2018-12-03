using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndTurnButtonManager : MonoBehaviour
{

    public Button endTurnButton;
    public GameManager gameManager;
    public Text timerText;
    public Text buttonText;

    private bool isCounting = false;
    private float timerCountdown;
    private float timeLeft;
    private const float TIME = 30;  //Time per round in seconds;

    void Update()
    {
        if (isCounting)
        {
            timeLeft -= Time.deltaTime;
            timerText.text = ToString();

            if (timeLeft <= 0)
            {
                TimerStop();
            }
        }
    }

    public void TimerStart()
    {
        timeLeft = TIME;
        InitialButtonBlock(5f);
        StartCoroutine(TimerStartWithDelay());
    }

    IEnumerator TimerStartWithDelay()
    {
        yield return new WaitForSeconds(2f);
        isCounting = true;
    }

    public void TimerStop()
    {
        isCounting = false;
        gameManager.speechRecognition.StopCoroutineIfTurnButtonClicked();
        gameManager.nextTurn();
    }

    public override string ToString()
    {
        int seconds = Mathf.RoundToInt(timeLeft);
        string secondsText = (seconds).ToString();
        if (secondsText.Length == 1)
            secondsText = "0" + secondsText;

        return string.Format("{0}", secondsText + "s.");
    }

    public void InitialButtonBlock(float blockTime)
    {
        StartCoroutine(InitialButtonBlockWithCoroutine(blockTime));
    }

    IEnumerator InitialButtonBlockWithCoroutine(float blockTime)
    {
        endTurnButton.enabled = false;
        buttonText.text = "Czekaj...";
        yield return new WaitForSeconds(blockTime);
        endTurnButton.enabled = true;
        buttonText.text = "Koniec Tury";
    }


}

