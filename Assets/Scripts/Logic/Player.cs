using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour{
        public int playerID;
        public Text Name;
        public Faction faction;
        public Army army;

        //public Player(int playerID, Text name, Faction faction)
        //{
        //    this.playerID = playerID;
        //    this.Name = name;
        //    this.faction = faction;
        //    this.army = new Army(faction);
        //}

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

        public void generateHand()
        {
            for (int i = 0; i < 5; i++)
            {
                this.army.cardContainer.moveCardFromDeckToHand();
            }
        }

        public void drawACardFromDeck()
        {
           this.army.cardContainer.moveCardFromDeckToHand();
        }

}
