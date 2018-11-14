using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HandView : MonoBehaviour {

    private List<Draggable> CardsInHand = new List<Draggable>();
    public GameObject GameAreaImage;
    public Position handPosition;
    public bool isDrawingRunning = false;

    // add new card GameObject to hand
    public void MoveDrawnCardFromDeckToHand(Card cardDrawn, PlayerModel player, GameObject deckVisual)
    {
        // ----------instantiate drawn card given as parameter and load its display in player's hand
        this.isDrawingRunning = true;
        GameObject newCard;
        if (cardDrawn.maxHealth > 0)
        {
            newCard = Instantiate(GameManager.Instance.unitCard, deckVisual.transform.position, Quaternion.Euler(0, 180, 0), GameManager.Instance.visuals.transform);
        }
        else
        {
            newCard = Instantiate(GameManager.Instance.tacticsCard, deckVisual.transform.position, Quaternion.Euler(0, 180, 0), GameManager.Instance.visuals.transform);
        }

        IDAssignment idAssignment = newCard.GetComponent(typeof(IDAssignment)) as IDAssignment;


        if (idAssignment != null)
        {
            idAssignment.uniqueId = cardDrawn.cardID;
            idAssignment.ownerPosition = handPosition;
            idAssignment.whereIsCard = WhereIsCard.Hand;
        }

        CardDisplayLoader cardDisplayLoader = newCard.GetComponent<CardDisplayLoader>();
        cardDisplayLoader.card = cardDrawn;
        cardDisplayLoader.loadCardAsset();
        StartCoroutine(rotateWhenDrawn(newCard, player));
    }

    // Visual represantation of card drawing
    IEnumerator rotateWhenDrawn(GameObject newCard, PlayerModel player)
    {
        Vector3 cardMovementVector = new Vector3(0f, 0f, 0f);
        if (player.position == Position.South)
        {
            cardMovementVector = new Vector3(-0.05f, 0.05f, 0);
        }
        else if(player.position == Position.North)
        {
            cardMovementVector = new Vector3(-0.05f, -0.05f, 0);
        }
        newCard.GetComponentInChildren<Canvas>().sortingLayerName = "ActiveCard";
        for (float f = 1f; f >= 0; f -= 0.02f)
        {
            newCard.transform.rotation = Quaternion.Euler(0, 180.0f * f, 0);
            newCard.transform.localScale += new Vector3(0.0001f, 0.0001f, 0);
            newCard.transform.position += cardMovementVector;
            yield return new WaitForSeconds(.01f);
        }
        yield return new WaitForSeconds(1f);
        float distanceX = this.transform.position.x - newCard.transform.position.x;
        float distanceY = this.transform.position.y - newCard.transform.position.y;
        for (float f = 1; f >= 0; f -= 0.02f)
        {

            newCard.transform.localScale -= new Vector3(0.0001f, 0.0001f, 0);
            newCard.transform.SetPositionAndRotation(new Vector3(this.transform.position.x - distanceX * f, this.transform.position.y - distanceY * f, 0), Quaternion.identity);
            yield return new WaitForSeconds(.01f);
        }
        newCard.transform.SetParent(this.transform);
        newCard.GetComponentInChildren<Canvas>().sortingLayerName = "Card";
        this.isDrawingRunning = false;
    }

    public void setPlayableCards(int currentResources)
    {
        foreach (Transform child in transform)
        {
            if (int.Parse(child.GetComponent<CardDisplayLoader>().cardMoneyText.text.ToString()) <= currentResources)
            {
                child.GetComponent<CardDisplayLoader>().cardFaceGlowImage.enabled = true;
                child.GetComponent<Draggable>().enabled = true;
            }
            else
            {
                child.GetComponent<CardDisplayLoader>().cardFaceGlowImage.enabled = false;
                child.GetComponent<Draggable>().enabled = false;
            }
        }
    }

    public void blockAllOperations()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<Draggable>().enabled = false;
            child.GetComponent<Attackable>().enabled = false;
            child.GetComponent<Defendable>().enabled = false;
            child.GetComponent<CardDisplayLoader>().cardFaceGlowImage.enabled = false;

            // block unit attack
            child.GetComponent<CardDisplayLoader>().cardUnitLoader.transform.GetComponent<Attackable>().enabled = false;
        }
    }
}
