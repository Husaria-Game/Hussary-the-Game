using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ResourcePool : MonoBehaviour {

    public Text ProgressText;
    public int TestResourceLeft;
    public int TestResourceMax;

    private int moneyTotal;
    public int MoneyTotal
    {
        get { return moneyTotal; }

        set
        {
            //Debug.Log("Changed total resources to: " + value);

            if (value > 10)
                moneyTotal = 10;
            else if (value < 0)
                moneyTotal = 0;
            else
                moneyTotal = value;

            // update the text
            ProgressText.text = string.Format("{0}/{1}", moneyAvailable.ToString(), moneyTotal.ToString());
        }
    }

    private int moneyAvailable;
    public int MoneyAvailable
    {
        get { return moneyAvailable; }

        set
        {
            //Debug.Log("Changed resources this turn to: " + value);

            if (value > moneyTotal)
                moneyAvailable = moneyTotal;
            else if (value < 0)
                moneyAvailable = 0;
            else
                moneyAvailable = value;

            // update the text
            ProgressText.text = string.Format("{0}/{1}", moneyAvailable.ToString(), moneyTotal.ToString());

        }
    }

    void Update()
    {
        if(Application.isEditor && !Application.isPlaying)
        {
            MoneyTotal = TestResourceMax;
            MoneyAvailable = TestResourceLeft;
        }
    }

}