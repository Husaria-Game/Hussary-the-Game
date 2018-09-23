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

    public void moveCardFromDeckToHand()
    {
        System.Random rnd = new System.Random();
        int r = rnd.Next(this.deckCardList.Count);

        //TODO define indexer??
        Card card = this.deckCardList[r];
        this.deckCardList.RemoveAt(r);
        this.handCardList.Add(card);
    }

    public void moveCardFromHandToFront()
    {

    }

    public void moveCardFromFrontToGraveyard()
    {

    }
}
