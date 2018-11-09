using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArmyCardsModel : MonoBehaviour
{
    //public List<Card> deckCardList = new List<Card>();
    public List<Card> deckCardList { get; set; }
    public List<Card> handCardList { get; set; }
    public List<Card> frontCardList { get; set; }
    public List<Card> graveyardCardList { get; set; }

    public ArmyCardsModel()
    {
        this.deckCardList = new List<Card>();

        // Generate deck for an army
        deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Artyleria")));
        deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Taktyczny Odwrót")));
        //deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Janczar")));
        //deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Kazasker")));
        //deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Sulejman")));

        deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Artyleria")));
        deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Taktyczny Odwrót")));
        //deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Janczar")));
        //deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Kazasker")));
        //deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Sulejman")));


        // assign unique id for each card in deck
        foreach (Card item in deckCardList)
        {
            item.cardID = IDFactory.GetUniqueID();
        }

        this.handCardList = new List<Card>();
        this.frontCardList = new List<Card>();
        this.graveyardCardList = new List<Card>();

    }

    public Card drawCardFromDeckList()
    {

        int r = UnityEngine.Random.Range(0, this.deckCardList.Count - 1);

        Card card = this.deckCardList[r];
        //Debug.Log("random: " + r + " karta:  " + card.cardName.ToString() + ", id: " + card.cardID);


        this.deckCardList.RemoveAt(r);

        foreach (Card item in deckCardList)
        {
            //Debug.Log("DECK: name -" + item.cardName.ToString() + ", cost - " + item.cardCost.ToString() + ", id: " + card.cardID.ToString());
        }
        return card;
    }
    public Card moveCardFromDeckListToHandList()
    {
        Card card = drawCardFromDeckList();
        this.handCardList.Add(card);
        //foreach (Card item in handCardList)
        //{
        //    Debug.Log("HAND: name -" + item.cardName.ToString() + ", cost - "+ item.cardCost.ToString() + ", id: " + item.cardID.ToString());
        //}
        return card;
    }

    public void moveCardFromHandToFront(int id)
    {
        Card cardToMove = handCardList.Single(r => r.cardID == id);
        handCardList.Remove(cardToMove);
        frontCardList.Add(cardToMove);
    }

    public void moveCardFromFrontToGraveyard()
    {

    }
}

