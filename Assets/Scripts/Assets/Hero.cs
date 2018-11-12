using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hero", menuName = "Hero")]
public class Hero : ScriptableObject
{

    [Header("General Info")]
    public string heroName;
    [TextArea(2, 3)]
    public string description;
    public Sprite heroImage;
    public Affiliation affiliation;


    [Header("Hero Parameters Info")]
    //public int attack;
    public int maxHealth;   // if maxHealth == 0 then card is a tactics card
    public int skillCost;
    //public string UnitScriptName;

    [Header("Skill Info")]
    public string TacticsScriptName;
    public int effectAmount;
    public Sprite skillImage;

}
