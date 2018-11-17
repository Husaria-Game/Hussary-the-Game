using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Affiliation
{
    Poland,
    Ottoman
}

[CreateAssetMenu(fileName ="New Card", menuName = "Card")]
public class Card : ScriptableObject {

    [Header("General Info")]
    public string cardName;
    [TextArea(2,3)]
    public string description;
    public Sprite cardImage;
    public int cardCost;
    public Affiliation affiliation;
    public int cardID;
    public string historyDescription;
    public int maxAttacksPerTurn;
    public int currentAttacksPerTurn;


    [Header("Unit Card Info")]
    public int attack;
    public int maxHealth;   // if maxHealth == 0 then card is a tactics card
    public int currentHealth;
    public string UnitScriptName;

    [Header("Tactics Card Info")]
    public string TacticsScriptName;
    public int effectAmount;

}
