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

    // Update is called once per frame
    void Update () {
        //============TEST of  messageManager============
        if (Input.GetKeyDown(KeyCode.A))
        {
            ShowMessage(playerSouthName + " - Twoja Tura", 2f);
        }
            

        if (Input.GetKeyDown(KeyCode.B))
            ShowMessage(playerNorthName + " - Twoja Tura", 2f);
        //===============================================
    }

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
