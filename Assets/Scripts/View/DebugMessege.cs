using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugMessege : MonoBehaviour {

    public Image incomingTextImageWarning;
    public Image incomingTextImagePositive;
    public Text incomingText;

    void Start() {
        incomingText.enabled = false;
        incomingTextImageWarning.enabled = false;
        incomingTextImagePositive.enabled = false;
    }


    public void ShowDebugText(string s, bool showPositiveMark)
    {
        StartCoroutine(ShowDebugTextWithDelay(s, showPositiveMark));
    }

    IEnumerator ShowDebugTextWithDelay(string s, bool positiveMark)
    {
        incomingTextImageWarning.enabled = !positiveMark;
        incomingTextImagePositive.enabled = positiveMark;
    
        incomingText.text = s;
        yield return new WaitForSeconds(1f);
        incomingText.enabled = true;
        yield return new WaitForSeconds(6f);

        incomingText.enabled = false;
        incomingTextImageWarning.enabled = false;
        incomingTextImagePositive.enabled = false;
    }
}
