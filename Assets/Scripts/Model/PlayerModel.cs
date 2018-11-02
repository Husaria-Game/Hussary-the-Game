using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel {


    public int playerID;
    public string name;
    public Faction faction;
    public Position position;
    public ArmyModel armymodel;
    public int resourcesMaxThisTurn;
    public int resourcesCurrent;

    public PlayerModel(int playerID, string name, Faction faction, Position position)
    {
        this.playerID = playerID;
        this.name = name;
        this.faction = faction;
        this.position = position;
        if(position == Position.North)
        {
            this.resourcesMaxThisTurn = 1;
        }
        else
        {
            this.resourcesMaxThisTurn = 0;
        }
        this.resourcesCurrent = resourcesMaxThisTurn;
        this.armymodel = new ArmyModel(faction);
    }

    public void updateResourcesNewTurn()
    {
        if(this.resourcesMaxThisTurn < 10)
        {
            this.resourcesMaxThisTurn++;
        }
        this.resourcesCurrent = this.resourcesMaxThisTurn;
    }
}