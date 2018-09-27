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
    public CanvasGroup canvasGroup;
    [Header("Text References")]
    public Text nameText;
    public Text healthText;
    public Text attackText;
    [Header("Image References")]
    public Image profileImage;
    public Image unitGlowImage;


    // Use this for initialization
    void Start()
    {
        if (card != null) loadUnitAsset();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
            ReceiveDamage();
    }

    // Method for loading unit parameters from coresponding card
    void loadUnitAsset()
    {
        nameText.text = card.cardName;
        profileImage.sprite = card.cardImage;
        if (card.maxHealth > 0)
        {
            healthText.text = card.maxHealth.ToString();
            attackText.text = card.attack.ToString();
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

    public IEnumerator ReceiveDamage()
    {
        Debug.Log("yoyo");
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
        Destroy(this.gameObject);
    }
}
