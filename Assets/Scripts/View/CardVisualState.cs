using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardVisualState : MonoBehaviour {

    public CardVisualStateEnum cardVisualStateEnum;
    public GameObject cardPanel;
    public GameObject unitGameObject;

    public void changeStateToUnit()
    {
        cardPanel.SetActive(false);
        unitGameObject.SetActive(true);
        cardVisualStateEnum = CardVisualStateEnum.Unit;
    }
}

public enum CardVisualStateEnum
{
    Card, Unit
}