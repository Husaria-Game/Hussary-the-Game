using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroVisualManager : MonoBehaviour
{

    public Hero hero;
    [Header("Text References")]
    public Text nameText;
    public Text healthText;
    public Text skillCostText;
    [Header("Image References")]
    public Image profileImage;
    public Image skillImage;


    // Use this for initialization
    void Start()
    {
        if (hero != null) loadHeroAsset();
    }

    // Method for loading hero parameters from coresponding hero
    void loadHeroAsset()
    {
        nameText.text = hero.heroName;
        profileImage.sprite = hero.heroImage;
        healthText.text = hero.maxHealth.ToString();

        // load card color based on affiliation
        if (hero.affiliation == Affiliation.Poland)
        {
            //topRibbonImage.color = card.;
            //lowRibbonImage;
            //profileImage;
            //bodyImage.color = ;
        }
        else if (hero.affiliation == Affiliation.Ottoman)
        {

        }
    }
}