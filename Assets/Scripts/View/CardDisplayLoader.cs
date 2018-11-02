using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplayLoader : MonoBehaviour {

    public Card card;
    public CardDisplayLoader cardPreviewLoader;
    public UnitVisualManager cardUnitLoader;
    [Header("Text References")]
    public Text nameText;
    public Text cardCostText;
    public Text descriptionText;
    public Text healthText;
    public Text attackText;
    [Header("Image References")]
    public Image topRibbonImage;
    public Image lowRibbonImage;
    public Image profileImage;
    public Image bodyImage;
    public Image cardFaceFrameImage;
    public Image cardFaceGlowImage;
    public Image cardBackGlowImage;


    // Use this for initialization
    void Start () {
        if(card != null) loadCardAsset();
	}

    public void loadCardAsset()
    {
        nameText.text = card.cardName ;
        cardCostText.text = card.cardCost.ToString();
        descriptionText.text = card.description;
        profileImage.sprite = card.cardImage;
        //Debug.Log(card.name);
        if (card.maxHealth > 0)
        {
            healthText.text = card.maxHealth.ToString();
            attackText.text = card.attack.ToString();
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
