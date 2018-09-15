using System;
using System.Collections;
using System.Collections.Generic;

public class CardContainer
{
    public List<Card> deckCardList { get; set; }
    public List<Card> handCardList { get; set; }
    public List<Card> frontCardList { get; set; }
    public List<Card> graveyardCardList { get; set; }

    public CardContainer()
    {
        this.deckCardList = new List<Card>();
        this.handCardList = new List<Card>();
        this.frontCardList = new List<Card>();
        this.graveyardCardList = new List<Card>();
    }

    public void moveCardFromDeckToHand()
    {
        Random rnd = new Random();
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
