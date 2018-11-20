using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitVisualManager : MonoBehaviour
{

    public Card card;
    public CardDisplayLoader cardPreviewLoader;
    public GameObject explosionEffect;
    public GameObject healEffect;
    public GameObject unitParentCard;
    public CanvasGroup explosionCanvasGroup;
    public CanvasGroup healCanvasGroup;
    [Header("Text References")]
    public Text nameText;
    public Text armorText;
    public Text attackText;
    public Text damageNumberText;
    public Text healNumberText;
    [Header("Image References")]
    public Image profileImage;
    public Image unitGlowImage;
    public Image unitPointerGlowImage;


    void Start()
    {
        if (card != null) loadUnitAsset();
    }
    void Update()
    {

    }

    // Method for loading unit parameters from coresponding card
    public void loadUnitAsset()
    {
        nameText.text = card.cardName;
        profileImage.sprite = card.cardImage;
        if (card.maxHealth > 0)
        {
            attackText.text = card.attack.ToString();
            armorText.text = card.maxHealth.ToString();

        }

        // load card color based on affiliation
        if (card.affiliation == Affiliation.Poland)
        {
            //topRibbonImage.color = card.;
            //lowRibbonImage;
            //profileImage;
            //bodyImage.color = ;
        }
        else if (card.affiliation == Affiliation.Ottoman)
        {

        }

        if (cardPreviewLoader != null)
        {
            cardPreviewLoader.card = card;
            cardPreviewLoader.loadCardAsset();
        }
    }

    public void createDamageVisual(int damage)
    {
        StartCoroutine(ReceiveDamage(damage));
    }

    public void createHealVisual(int heal)
    {
        StartCoroutine(ReceiveHeal(heal));
    }

    public IEnumerator ReceiveDamage(int damage)
    {
        damageNumberText.text = damage.ToString();
        explosionEffect.SetActive(true);
        explosionCanvasGroup.alpha = 0.1f;
        while (explosionCanvasGroup.alpha < 1)
        {
            explosionCanvasGroup.alpha += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }

        while (explosionCanvasGroup.alpha > 0)
        {
            explosionCanvasGroup.alpha -= 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public IEnumerator ReceiveHeal(int heal)
    {
        healNumberText.text = heal.ToString();
        healEffect.SetActive(true);
        healCanvasGroup.alpha = 0.1f;
        while (healCanvasGroup.alpha < 1)
        {
            healCanvasGroup.alpha += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }

        while (healCanvasGroup.alpha > 0)
        {
            healCanvasGroup.alpha -= 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
