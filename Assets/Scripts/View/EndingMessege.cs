using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingMessege : MonoBehaviour {

    public GameObject MessegeCanvasForQuitGame;
    public GameObject MessegeCanvasAfterGameIsDone;

    public Text afterGameText;
    private string winnerName = "Zwycięzca";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MessegeCanvasForQuitGame.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void StopTheGame()
    {
        SettsHolder.instance.UnsetIsPlayedAgain();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void ReturnToTheGame()
    {
        MessegeCanvasForQuitGame.SetActive(false);
        Time.timeScale = 1;
    }

    public void QuitTheApplication()
    {
        Application.Quit();
    }

    public void PlayAgain()
    {
        SettsHolder.instance.SetIsPlayedAgain();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    //Method to be used in GameManager to pass winning player name
    public IEnumerator DrawMessage(PlayerModel player)
    {
        afterGameText.text = "REMIS!\nWyczerpała się liczba dostępnych kart.";
        yield return new WaitForSeconds(1f);
        MessegeCanvasAfterGameIsDone.SetActive(true);
        Time.timeScale = 0;
    }

    //Method to be used in GameManager to pass winning player name
    public IEnumerator WhoWonMessege(PlayerModel player, bool reachedEndOfDeckCards)
    {
        winnerName = player.name;
        afterGameText.text = "Zwycięża:\t" + winnerName +"!";
        if (reachedEndOfDeckCards)
        {
            afterGameText.text += "\nWyczerpała  się  liczba  dostępnych  kart.";
        }
        yield return new WaitForSeconds(1f);
        MessegeCanvasAfterGameIsDone.SetActive(true);
        Time.timeScale = 0;
    }
}

