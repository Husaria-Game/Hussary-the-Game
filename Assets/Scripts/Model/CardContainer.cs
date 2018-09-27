using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardContainer : MonoBehaviour
{
    public List<Card> deckCardList = new List<Card>();
    public List<Card> handCardList = new List<Card>();
    public List<Card> frontCardList { get; set; }
    public List<Card> graveyardCardList { get; set; }

    //public CardContainer()
    //{
    //    this.deckCardList = new List<Card>();
    //    this.handCardList = new List<Card>();
    //    this.frontCardList = new List<Card>();
    //    this.graveyardCardList = new List<Card>();
    //}

    public Card moveCardFromDeckToHand()
    {

        int r = UnityEngine.Random.Range(0, this.deckCardList.Count-1);

        Card card = this.deckCardList[r];
        Debug.Log("random: " + r + " karta:  " + card.cardName.ToString());

        return card;
        //this.deckCardList.RemoveAt(r);
        //this.handCardList.Add(card);
    }

    public void moveCardFromHandToFront()
    {

    }

    public void moveCardFromFrontToGraveyard()
    {

    }
}
