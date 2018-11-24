using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[ExecuteInEditMode]
public class ResourcePool : MonoBehaviour {

    public Image ResourceImage;
    public Text ProgressText;
    public int ResourceLeft;
    public int ResourceMax;

    public void updateResourcesView(int resourcesLeft, int resourcesCurrent)
    {
        ProgressText.text = string.Format("{0}/{1}", resourcesLeft.ToString(), resourcesCurrent.ToString());
    }
    public void showMoneyGainAnimation()
    {
        Vector3 punch = new Vector3(2, 2, 0);
        ResourceImage.transform.DOPunchScale(punch, 4f, 2, 0);
        ProgressText.color = new Color32(255, 0, 0, 255);
    }
}