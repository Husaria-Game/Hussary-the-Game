using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel {


    public int playerID;
    public string name;
    public Faction faction;
    public ArmyModel armymodel;

    public PlayerModel(int playerID, string name, Faction faction)
    {
        this.playerID = playerID;
        this.name = name;
        this.faction = faction;
        this.armymodel = new ArmyModel(faction);
    }

    public void showDeck()
    {
        //Console.WriteLine("Liczebność Armii: " + this.army.cardContainer.deckCardList.Count);
        //Console.WriteLine("KARTY:");
        //foreach (Card card in this.army.cardContainer.deckCardList)
        //{
        //    if (card is UnitCard)
        //    {
        //        UnitCard unitCard = (UnitCard)card;
        //        Console.WriteLine(card.name.ToString() + " faction: " + unitCard.faction + " health: " + unitCard.health);
        //    }
        //    else
        //    {
        //        TacticsCard tacticsCard = (TacticsCard)card;
        //        Console.WriteLine(card.name.ToString() + " faction: " + tacticsCard.faction);
        //    }
        //}
        //Console.WriteLine();
    }

    public void showHand()
    {
        //Console.WriteLine("Liczebność Ręki: " + this.army.cardContainer.handCardList.Count);
        //Console.WriteLine("KARTY:");
        //foreach (Card card in this.army.cardContainer.handCardList)
        //{
        //    if (card is UnitCard)
        //    {
        //        UnitCard unitCard = (UnitCard)card;
        //        Console.WriteLine(card.name.ToString() + " faction: " + unitCard.faction + " health: " + unitCard.health);
        //    }
        //    else
        //    {
        //        TacticsCard tacticsCard = (TacticsCard)card;
        //        Console.WriteLine(card.name.ToString() + " faction: " + tacticsCard.faction);
        //    }
        //}
        //Console.WriteLine();
    }

    //public void generateHand()
    //{
    //    for (int i = 0; i < 5; i++)
    //    {
    //        Card card = this.army.cardContainer.moveCardFromDeckToHand();
    //    }
    //}

    //public void drawACardFromDeck()
    //{
    //    this.army.cardContainer.moveCardFromDeckToHand();
    //}

}