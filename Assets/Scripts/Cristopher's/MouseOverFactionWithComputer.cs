using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseOverFactionWithComputer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    public ChooseFactionScriptWithComputer CFS;
    public Image imageThis;
    public MouseOverFactionWithComputer imageOther;
    private bool isClicked = false;
    private bool isBlocked = false;


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isClicked)
        {
            imageThis.transform.localScale += new Vector3(0.25f, 0.25f, 0);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isClicked)
        {
            imageThis.transform.localScale -= new Vector3(0.25f, 0.25f, 0);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (!isClicked)
            {
                isClicked = true;
                CFS.isClicked = !CFS.isClicked;
                CFS.setSecondFaction(imageThis.GetComponentInChildren<Text>().text);
                UnclickOtherImage();
                isBlocked = false;
            }
            else if (isClicked && !isBlocked)
            {
                isClicked = false;
                CFS.isClicked = !CFS.isClicked;
                isBlocked = true;
            }
        }
    }

    private void UnclickOtherImage()
    {
        if (imageOther.isClicked)
        {
            imageOther.imageThis.transform.localScale -= new Vector3(0.25f, 0.25f, 0);
            CFS.isClicked = !CFS.isClicked;
        }
        imageOther.isClicked = false;
    }
}
