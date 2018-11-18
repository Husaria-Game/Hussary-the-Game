﻿using System;
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
    public GameObject explosionEffect;
    public CanvasGroup canvasGroup;
    public Text damageNumberText;
    public Position ownerPosition;
    [Header("Image References")]
    public Image profileImage;

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
        skillCostText.text = hero.skillCost.ToString();

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
    public void createDamageVisual(int damage)
    {
        StartCoroutine(ReceiveDamage(damage));
    }

    public IEnumerator ReceiveDamage(int damage)
    {
        damageNumberText.text = damage.ToString();
        explosionEffect.SetActive(true);
        canvasGroup.alpha = 0.1f;
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
        // after the effect is shown it gets destroyed.
        //Destroy(this.gameObject);
    }
}