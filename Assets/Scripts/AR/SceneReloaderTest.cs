using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SceneReloaderTest : MonoBehaviour
{
    //Make sure to attach these Buttons in the Inspector
    public Button trueButton, falseButton;

    void Start()
    {
        //Calls the goBackToTableScene/TaskWithParameters/ButtonClicked method when you click the Button
        trueButton.onClick.AddListener(delegate { goBackToTableScene(true); });
        falseButton.onClick.AddListener(delegate { goBackToTableScene(false); });
    }

    void goBackToTableScene(bool isARBonusGranted)
    {
        //Output this to console when Button1 or Button3 is clicked
        ARManager.Instance.ARSceneResult(isARBonusGranted);
        SceneManager.UnloadSceneAsync("ARScene");
    }
}