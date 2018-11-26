using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CardEncyclopedia : MonoBehaviour {

    private Object[] cards;
    private List<Card> allCards;
    private List<Card> currentListToDisplay;
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

        //Loading all Card Templates from Unity
        cards = Resources.LoadAll("Cards", typeof(Card));

        

        //Changing loaded object from type Object[] to List<Card>
        foreach (Card card in cards)
        {
            if (card.cardType == CardType.UnitCard) {
                allCards.Add(card);
            }
        }

        currentListToDisplay = allCards.ToList();
        polandCards = allCards.Where(c => c.affiliation == Affiliation.Poland).ToList();
        ottomanCards = allCards.Where(c => c.affiliation == Affiliation.Ottoman).ToList();

        //Setting previousCardButton to off mode
        setButton(previousCardButton, false, "Brak");

        //Loading of the first Card Template on the page
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

    public void resetSet()
    {
        //Setting counter to zero (First card being loaded)
        cardNumber = 0;

        //Turning nextCardButton on and previousCardButton off
        setButton(nextCardButton, true, "Następna");
        setButton(previousCardButton, false, "Brak");

        //Loading new set of cards
        loadCard(cardNumber);
    }

    private void setButton(Button b, bool isEnabled, string buttonText)
    {
        b.enabled = isEnabled;
        b.GetComponentInChildren<Text>().text = buttonText;
    }

}
