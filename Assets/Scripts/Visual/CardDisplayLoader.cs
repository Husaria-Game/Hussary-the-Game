using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplayLoader : MonoBehaviour {

    public Card card;
    public CardDisplayLoader cardPreviewLoader;
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

    private void loadCardAsset()
    {
        nameText.text = card.name;
        cardCostText.text = card.cardCost.ToString();
        descriptionText.text = card.description;
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

    }
}
