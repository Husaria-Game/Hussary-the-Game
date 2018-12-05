using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardVisualState : MonoBehaviour {

    public CardVisualStateEnum cardVisualStateEnum;
    public GameObject cardPanel;
    public GameObject unitGameObject;

    public void changeStateToUnit()
    {
        cardPanel.SetActive(false);
        unitGameObject.SetActive(true);
        unitGameObject.GetComponentInChildren<GraphicRaycaster>().enabled =
            !GameManager.Instance.isItAITurn;
        cardVisualStateEnum = CardVisualStateEnum.Unit;
        transform.GetComponent<CardDisplayLoader>().cardDetailedType = CardVisualStateEnum.Unit;
    }
}