using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CardEncyclopedia : MonoBehaviour {

    private Object[] cards;
    private List<Card> cardsToDisplay;
    private List<Card> filteredCards;
    public Text cardName;
    public Image cardImage;
    public Text cardHistoryDescription;
    public Dropdown factionMenu;

	// Use this for initialization
	void Start () {

        cardsToDisplay = new List<Card>();
        filteredCards = new List<Card>();

        cards = Resources.LoadAll("Cards", typeof(Card));

        foreach (Card card in cards)
        {
            cardsToDisplay.Add(card);     
        }

        cardName.text = cardsToDisplay[0].cardName;
        cardImage.sprite = cardsToDisplay[0].cardImage;
        cardHistoryDescription.text = cardsToDisplay[0].historyDescription;

    }
	
	void Update () {

		if(factionMenu.captionText.Equals("Wszystkie"))
        {
            filteredCards = cardsToDisplay;
        }
        else if(factionMenu.captionText.Equals("Rzeczpospolita Polska"))
        {
            filteredCards = cardsToDisplay.Select(n=>n).Where(n.affiliation.equals)
        }
        else if (factionMenu.captionText.Equals("Imperium Osmańskie"))
        {

        }


    }

}
