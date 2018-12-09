using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndTurnButtonManager : MonoBehaviour
{
    public SpeechRecognitionSystem SRS;
    public Button endTurnButton;
    public GameManager gameManager;
    public Text timerText;
    public Text buttonText;

    private bool isARSceneActive = false;
    private float timerCountdown;
    private float timeLeft;

    public void TimerStart()
    {
        if(gameManager.typeOfEnemy == GameMode.Computer && gameManager.currentPlayer == gameManager.playerNorth)
        {
            ButtonDisable();
        }
        else
        {
            InitialButtonBlock(5f);
        }
    }

    public void TimerStop()
    {
        SRS.StopCoroutineIfTurnButtonClicked();
        gameManager.nextTurn();
    }

    private void ButtonDisable()
    {
        endTurnButton.enabled = false;
        buttonText.text = "Czekaj...";
    }

    private void ButtonEnable()
    {
        endTurnButton.enabled = true;
        buttonText.text = "Koniec Tury";
    }

    public void InitialButtonBlock(float blockTime)
    {
        StartCoroutine(InitialButtonBlockWithCoroutine(blockTime));
    }

    IEnumerator InitialButtonBlockWithCoroutine(float blockTime)
    {
        ButtonDisable();
        yield return new WaitForSeconds(blockTime);
        ButtonEnable();
    }

    public void ARSceneBecomesActive()
    {
        isARSceneActive = true;
    }
}

