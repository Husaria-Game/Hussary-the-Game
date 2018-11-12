using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ResourcePool : MonoBehaviour {

    public Text ProgressText;
    public int ResourceLeft;
    public int ResourceMax;

    // Use this for initialization
    //void Start()
    //{
    //    ProgressText.text = string.Format("{0}/{1}", ResourceLeft.ToString(), ResourceMax.ToString());
    //}

    public void updateResourcesView(int resourcesLeft, int resourcesCurrent)
    {
        ProgressText.text = string.Format("{0}/{1}", resourcesLeft.ToString(), resourcesCurrent.ToString());
    }

}