using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CardEncyclopedia : MonoBehaviour {

    private Object[] cards;
    private List<Card> currentListToDisplay;
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


	void Start () {

        allCards = new List<Card>();
        currentListToDisplay = new List<Card>();

        //Zaciągnięcie wszystkich kart
        cards = Resources.LoadAll("Cards", typeof(Card));

        //Zamiana zaciągniętych obiektów z typu Object[] w typ List<Card>
        foreach (Card card in cards)
        {
            allCards.Add(card);
        }

        currentListToDisplay = allCards.ToList();
        polandCards = allCards.Where(c => c.affiliation == Affiliation.Poland).ToList();
        ottomanCards = allCards.Where(c => c.affiliation == Affiliation.Ottoman).ToList();

        //Ustawienie Guzika do tyłu w stan wyłączenia
        setButton(previousCardButton, false, "Brak");

        //Załadowanie pierwszej karty
        loadCard(cardNumber);      
    }

    public void chooseFaction()
    {
        if (factionMenu.captionText.text == "Wszystkie")
        {
            currentListToDisplay = allCards;
            resetSet();
        }
        else if (factionMenu.captionText.text == "Rzeczpospolita Polska")
        {
            currentListToDisplay = polandCards;
            resetSet();
        }
        else if (factionMenu.captionText.text == "Imperium Osmańskie")
        {
            currentListToDisplay = ottomanCards;
            resetSet();
        }
    }

    public void loadCard(int cardNumber)
    {
        cardName.text = currentListToDisplay[cardNumber].cardName;
        cardImage.sprite = currentListToDisplay[cardNumber].cardImage;
        cardHistoryDescription.text = currentListToDisplay[cardNumber].historyDescription;
    }

    public void nextCard()
    {
        if (cardNumber == currentListToDisplay.Count - 2)
        {
            setButton(nextCardButton, false, "Brak");
        }

            cardNumber++;
            loadCard(cardNumber);
            if(previousCardButton.enabled == false)
            {
                setButton(previousCardButton, true, "Poprzednia");
            }
    }

    public void previousCard()
    {
        if(cardNumber == 1)
        {
            setButton(previousCardButton, false, "Brak");
        }

            cardNumber--;
            loadCard(cardNumber);
            if (nextCardButton.enabled == false)
            {
                setButton(nextCardButton, true, "Następna");
            }
    }

    private void resetSet()
    {
        //Ustawienie licznika na zero (pierwsza karta ładowana)
        cardNumber = 0;

        //Włączenie guzika do przodu i wyłączenie do tyłu
        setButton(nextCardButton, true, "Następna");
        setButton(previousCardButton, false, "Brak");

        //Załadowanie nowego seta kart
        loadCard(cardNumber);
    }

    private void setButton(Button b, bool isEnabled, string buttonText)
    {
        b.enabled = isEnabled;
        b.GetComponentInChildren<Text>().text = buttonText;
    }

}
