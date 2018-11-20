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
        deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Spahis")));
        deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Akindżi")));
        deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Azab")));
        deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Janczar")));
        deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Kazasker")));
        deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Derwisz")));
        deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Mameluk")));
        deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Sarydża")));
        deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Sekban")));
        deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Śpiew muezzina")));
        deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Śpiew muezzina")));
        deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Śpiew muezzina")));
        deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Śpiew muezzina")));
        deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Śpiew muezzina")));
        deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Śpiew muezzina")));
        deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Śpiew muezzina")));
        deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Śpiew muezzina")));
        deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Śpiew muezzina")));
        deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Śpiew muezzina")));

        //deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Ciura obozowy")));
        //deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Czeladnik")));
        //deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Chorąży")));
        //deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Namiestnik")));
        //deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Pocztowy")));
        //deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Porucznik")));
        //deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Rotmistrz")));
        //deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Skrzydłowy")));
        //deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Towarzysz husarski")));
        //deckCardList.Add(Instantiate(Resources.Load<Card>("Cards/Towarzysz pancerny")));

        // assign unique id for each card in deck
        foreach (Card item in deckCardList)
        {
            item.cardID = IDFactory.GetUniqueID();
            item.maxAttacksPerTurn = 1;
            item.currentAttacksPerTurn = item.maxAttacksPerTurn;
        }

        this.handCardList = new List<Card>();
        this.frontCardList = new List<Card>();
        this.graveyardCardList = new List<Card>();
    }

    public Card drawCardFromDeckList()
    {

        int r = UnityEngine.Random.Range(0, this.deckCardList.Count - 1);

        Card card = this.deckCardList[r];

        this.deckCardList.RemoveAt(r);
        return card;
    }
    public Card moveCardFromDeckListToHandList()
    {
        Card card = drawCardFromDeckList();
        this.handCardList.Add(card);
        return card;
    }

    public void moveCardFromHandToFront(int id)
    {
        Card cardToMove = handCardList.Single(r => r.cardID == id);
        handCardList.Remove(cardToMove);
        frontCardList.Add(cardToMove);
    }

    public void moveCardFromFrontToGraveyard(int id)
    {
        Card cardToMove = frontCardList.Single(r => r.cardID == id);
        frontCardList.Remove(cardToMove);
        graveyardCardList.Add(cardToMove);
    }

    public void moveCardFromHandToGraveyard(int id)
    {
        Card cardToMove = handCardList.Single(r => r.cardID == id);
        handCardList.Remove(cardToMove);
        graveyardCardList.Add(cardToMove);
    }

    public void updateArmorAfterDamageTaken(int cardID, int newArmorValue)
    {

        findCardInFrontByID(cardID).currentHealth = newArmorValue;
    }

    public Card findCardInHandByID(int id)
    {
        return handCardList.Find(x => x.cardID == id);
    }

    public Card findCardInFrontByID(int id)
    {
        return frontCardList.Find(x => x.cardID == id);
    }

    public void restoreCardAttacksPerRound()
    {
        foreach (Card item in frontCardList)
        {
            item.currentAttacksPerTurn = item.maxAttacksPerTurn;
        }
    }
}

