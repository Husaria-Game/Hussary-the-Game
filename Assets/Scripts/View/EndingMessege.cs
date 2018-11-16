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
        else if (Input.GetKeyDown(KeyCode.E))
        {
            MessegeCanvasAfterGameIsDone.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void StopTheGame()
    {
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

    }

    public void WhoWonMessege(string player)
    {
        winnerName = player;
    }
}

