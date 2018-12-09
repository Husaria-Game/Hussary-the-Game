using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour {

    public Text MessageText;
    public GameObject MessageCanvasGO;

    public String playerSouthName;
    public String playerNorthName;

    public void ShowMessage(string message, float showTime)
    {
        StartCoroutine(ShowMessageCoroutine(message, showTime));
    }

    IEnumerator ShowMessageCoroutine(string message, float showTime)
    {
        MessageText.text = message;
        MessageCanvasGO.SetActive(true);

        yield return new WaitForSeconds(showTime);

        MessageCanvasGO.SetActive(false);
    }
}
