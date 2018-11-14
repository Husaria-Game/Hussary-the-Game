using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CardEncyclopedia : MonoBehaviour {

    private Object[] cards;
    private List<Card> allCards;
    private List<Card> ottomanCards;
    private List<Card> polandCards;

    public Text cardName;
    public Image cardImage;
    public Text cardHistoryDescription;
    public Dropdown factionMenu;

    public Button nextCardButton;
    public Button previousCardButton;

    private int cardNumber = 0;

	// Use this for initialization
	void Start () {

        allCards = new List<Card>();
        cards = Resources.LoadAll("Cards", typeof(Card));

        foreach (Card card in cards)
        {
            allCards.Add(card);
            Debug.Log(card.cardName);
        }
        /*
        polandCards = allCards.Select(c => c).Where(c => c.affiliation == 
        Affiliation.Poland);

        ottomanCards = allCards.Select(c => c).Where(c => c.affiliation == 
        Affiliation.Ottoman);
        */

        //Ustawienie Guzika do tyłu w stan wyłączenia
        previousCardButton.enabled = false;
        previousCardButton.GetComponentInChildren<Text>().text = "Brak";

        //Załadowanie pierwszej karty
        cardName.text = allCards[cardNumber].cardName;
        cardImage.sprite = allCards[cardNumber].cardImage;
        cardHistoryDescription.text = allCards[cardNumber].historyDescription;
        
    }

    public void showCards()
    {
        if (factionMenu.captionText.text == "Wszystkie")
        {
            Debug.Log("All");

            //showCard(allCards);
        }
        else if (factionMenu.captionText.text == "Rzeczpospolita Polska")
        {
            Debug.Log("Poland");
            //showCard(polandCards);
        }
        else if (factionMenu.captionText.text == "Imperium Osmańskie")
        {
            Debug.Log("Ottoman");
            //showCard(ottomanCards);
        }
    }








    public void loadCard(int cardNumber)
    {
        cardName.text = allCards[cardNumber].cardName;
        cardImage.sprite = allCards[cardNumber].cardImage;
        cardHistoryDescription.text = allCards[cardNumber].historyDescription;
    }

    public void nextCard()
    {
        if (cardNumber == allCards.Count - 2)
        {
            nextCardButton.enabled = false;
            nextCardButton.GetComponentInChildren<Text>().text = "Brak"; 
        }

            cardNumber++;
            loadCard(cardNumber);
            if(previousCardButton.enabled == false)
            {
                previousCardButton.enabled = true;
                previousCardButton.GetComponentInChildren<Text>().text = "Poprzednia";
            }
    }

    public void previousCard()
    {
        if(cardNumber == 1)
        {
            previousCardButton.enabled = false;
            previousCardButton.GetComponentInChildren<Text>().text = "Brak";
        }

            cardNumber--;
            loadCard(cardNumber);
            if (nextCardButton.enabled == false)
            {
                nextCardButton.enabled = true;
                nextCardButton.GetComponentInChildren<Text>().text = "Następna";
            }
    }

}
