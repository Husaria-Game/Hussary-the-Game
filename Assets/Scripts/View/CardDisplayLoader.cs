using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplayLoader : MonoBehaviour {

    public Card card;
    public CardDisplayLoader cardPreviewLoader;
    public UnitVisualManager cardUnitLoader;
    public GameObject Unit;
    [Header("Text References")]
    public Text cardNameText;
    public Text cardMoneyText;
    public Text armorText;
    public Text attackText;
    public Text descriptionText;
    [Header("Image References")]
    public Image topRibbonImage;
    public Image profileImage;
    public Image OutsideBGColor;
    public Image cardFaceGlowImage;
    public Image cardBackGlowImage;
    public CardType cardType;

    public enum CardType
    {
        UnitCard, TacticsCard
    }


    // Use this for initialization
    void Start () {
        if(card != null) loadCardAsset();
	}

    public void loadCardAsset()
    {
        cardNameText.text = card.cardName ;
        cardMoneyText.text = card.cardCost.ToString();
        descriptionText.text = card.description;
        profileImage.sprite = card.cardImage;
        if (card.maxHealth > 0)
        {
            cardType = CardType.UnitCard;
            attackText.text = card.attack.ToString();
            armorText.text = card.maxHealth.ToString();
        }
        else
        {
            cardType = CardType.TacticsCard;
        }

        // load card color based on affiliation
        if(card.affiliation == Affiliation.Poland)
        {
            //topRibbonImage.color = card.;
            //lowRibbonImage;
            //profileImage;
            //bodyImage.color = ;
        }
        else if(card.affiliation == Affiliation.Ottoman)
        {

        }

        if (cardPreviewLoader != null)
        {
            cardPreviewLoader.card = card;
            cardPreviewLoader.loadCardAsset();
        }

        if (cardUnitLoader != null)
        {
            cardUnitLoader.card = card;
            cardUnitLoader.loadUnitAsset();
        }
    }
}
