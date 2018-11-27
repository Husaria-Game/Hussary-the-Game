using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HandView : MonoBehaviour {

    private List<Draggable> CardsInHand = new List<Draggable>();
    public GameObject GameAreaImage;
    public Position handPosition;
    public bool isDrawingRunning = false;

    private GameObject newCard;
    private IDAssignment idAssignment;
    private void Update()
    {
        //Block any action on card that is rotating during draw
        if(idAssignment != null)
        {
            if (idAssignment.whereIsCard == WhereIsCard.Rotating)
            {
                newCard.GetComponent<Draggable>().enabled = false;
                newCard.GetComponent<Attackable>().enabled = false;
                if (newCard.GetComponent<Defendable>() != null)
                    newCard.GetComponent<Defendable>().enabled = false;
                newCard.GetComponent<CardDisplayLoader>().cardFaceGlowImage.enabled = false;
                newCard.GetComponent<CardOnHoverPreview>().enabled = false;
            }
            //Unblock preview of card when it is placed in hand
            else if (idAssignment.whereIsCard == WhereIsCard.Hand)
            {
                newCard.GetComponent<CardOnHoverPreview>().enabled = true;
            }
        }

    }
    // add new card GameObject to hand
    public void MoveDrawnCardFromDeckToHand(Card cardDrawn, PlayerModel player)
    {
        // ----------instantiate drawn card given as parameter and load its display in player's hand
        this.isDrawingRunning = true;
        
        if (cardDrawn.maxHealth > 0)
        {
            newCard = Instantiate(GameManager.Instance.unitCard, player.deckVisual.transform.position, Quaternion.Euler(0, 180, 0), GameManager.Instance.visuals.transform);
        }
        else
        {
            newCard = Instantiate(GameManager.Instance.tacticsCard, player.deckVisual.transform.position, Quaternion.Euler(0, 180, 0), GameManager.Instance.visuals.transform);
        }

        idAssignment = newCard.GetComponent(typeof(IDAssignment)) as IDAssignment;


        if (idAssignment != null)
        {
            idAssignment.uniqueId = cardDrawn.cardID;
            idAssignment.ownerPosition = handPosition;
            idAssignment.whereIsCard = WhereIsCard.Rotating;
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
            newCard.transform.localScale += new Vector3(0.00015f, 0.00015f, 0);
            newCard.transform.position += cardMovementVector;
            yield return new WaitForSeconds(.01f);
        }
        yield return new WaitForSeconds(1f);
        float distanceX = this.transform.position.x - newCard.transform.position.x;
        float distanceY = this.transform.position.y - newCard.transform.position.y;
        for (float f = 1; f >= 0; f -= 0.02f)
        {

            newCard.transform.localScale -= new Vector3(0.00015f, 0.00015f, 0);
            newCard.transform.SetPositionAndRotation(new Vector3(this.transform.position.x - distanceX * f, this.transform.position.y - distanceY * f, 0), Quaternion.identity);
            yield return new WaitForSeconds(.01f);
        }
        newCard.transform.SetParent(this.transform);
        newCard.GetComponentInChildren<Canvas>().sortingLayerName = "Card";
        this.isDrawingRunning = false;
        idAssignment.whereIsCard = WhereIsCard.Hand;
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.enablePlayableCardsFlag = true;
    }

    public void setPlayableCards(int currentResources)
    {
        foreach (Transform child in transform)
        {
            if (int.Parse(child.GetComponent<CardDisplayLoader>().cardMoneyText.text.ToString()) <= currentResources)
            {
                child.GetComponent<CardDisplayLoader>().cardFaceGlowImage.enabled = true;
                if((child.GetComponent<CardDisplayLoader>().armorText) != null)
                {
                    child.GetComponent<Draggable>().enabled = true;
                    child.GetComponent<Attackable>().enabled = false;
                }
                else
                {
                    child.GetComponent<Draggable>().enabled = false;
                    child.GetComponent<Attackable>().enabled = true;
                }
            }
            else
            {
                child.GetComponent<CardDisplayLoader>().cardFaceGlowImage.enabled = false;
                child.GetComponent<Draggable>().enabled = false;
                child.GetComponent<Attackable>().enabled = false;
            }
        }
    }

    public void blockAllOperations()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<Draggable>().enabled = false;
            child.GetComponent<Attackable>().enabled = false;
            if(child.GetComponent<Defendable>() != null)
                child.GetComponent<Defendable>().enabled = false;
            child.GetComponent<CardDisplayLoader>().cardFaceGlowImage.enabled = false;
        }
    }
}
