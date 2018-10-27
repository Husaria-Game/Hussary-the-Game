using System.Collections;
using System.Collections.Generic;
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

        //var sprite = Resources.Load<Sprite>("Sprites/sprite01");
        //Card pocztowy = Resources.Load<Card>("Pocztowy");
        deckCardList.Add(Resources.Load<Card>("Cards/Artyleria"));
        deckCardList.Add(Resources.Load<Card>("Cards/Janczar"));
        deckCardList.Add(Resources.Load<Card>("Cards/Kazasker"));
        deckCardList.Add(Resources.Load<Card>("Cards/Sulejman"));
        //Debug.Log("deckcardList lenght: " + this.deckCardList.Count);
        //GameObject instance = Instantiate(Resources.Load("enemy", typeof(GameObject))) as GameObject;

        foreach (Card item in deckCardList)
        {
            Debug.Log("DECK INIT: name -" + item.cardName.ToString() + ", cost - " + item.cardCost.ToString());
        }
        //Debug.Log(pocztowy.cardName.ToString());
        //Debug.Log(pocztowy.cardName.ToString());
        this.handCardList = new List<Card>();
        this.frontCardList = new List<Card>();
        this.graveyardCardList = new List<Card>();
        //handCardList.Add(moveCardFromDeckToHand());

    }

    public Card drawCardFromDeckList()
    {

        int r = UnityEngine.Random.Range(0, this.deckCardList.Count - 1);

        Card card = this.deckCardList[r];
        Debug.Log("random: " + r + " karta:  " + card.cardName.ToString());


        this.deckCardList.RemoveAt(r);

        foreach (Card item in deckCardList)
        {
            Debug.Log("DECK: name -" + item.cardName.ToString() + ", cost - " + item.cardCost.ToString());
        }
        return card;
        //this.handCardList.Add(card);
    }
    public Card moveCardFromDeckListToHandList()
    {
        Card card = drawCardFromDeckList();
        //this.deckCardList.RemoveAt(r);
        this.handCardList.Add(card);


        foreach (Card item in handCardList)
        {
            Debug.Log("HAND: name -" + item.cardName.ToString() + ", cost - "+ item.cardCost.ToString());
        }
        return card;
    }

    public void moveCardFromHandToFront()
    {

    }

    public void moveCardFromFrontToGraveyard()
    {

    }
}

