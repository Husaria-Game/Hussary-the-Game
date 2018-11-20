using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugMessege : MonoBehaviour {

    public Image incomingTextImage;
    public Text incomingText;

    void Start() {
        incomingText.enabled = false;
        incomingTextImage.enabled = false;
    }


    public void ShowDebugText(string s)
    {
        StartCoroutine(ShowDebugTextWithDelay(s));
    }

    IEnumerator ShowDebugTextWithDelay(string s)
    {       
        incomingTextImage.enabled = true;
        yield return new WaitForSeconds(2f);
    
        incomingText.text = s;
        incomingText.enabled = true;
        yield return new WaitForSeconds(1f);

        incomingTextImage.enabled = false;
        incomingText.enabled = true;
        yield return new WaitForSeconds(5f);

        incomingText.enabled = false;
    }
}
