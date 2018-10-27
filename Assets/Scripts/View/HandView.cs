using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HandView : MonoBehaviour {

    private List<GameObject> CardsInHand = new List<GameObject>();
    public GameObject GameAreaImage;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // add new card GameObject to hand
    public void AddDrawnCardFromToHand(Card cardDrawn)
    {
        // ----------instantiate drawn card given as parameter and load its display in player's hand
        GameObject newCard;
        //if (cardDrawn.maxHealth > 0)
        //{
        //    newCard = Instantiate(GameManager.Instance.unitCard, new Vector3(0, 0, 0), Quaternion.identity, GameManager.Instance.handView.transform);
        //}
        //else
        //{
        //    newCard = Instantiate(GameManager.Instance.tacticsCard, new Vector3(0, 0, 0), Quaternion.identity, GameManager.Instance.handView.transform);
        //}
        if (cardDrawn.maxHealth > 0)
        {
            newCard = Instantiate(GameManager.Instance.unitCard, new Vector3(6.68f, -1.67f, 0.96f), Quaternion.Euler(0, 180, 0), GameManager.Instance.visuals.transform);
        }
        else
        {
            newCard = Instantiate(GameManager.Instance.tacticsCard, new Vector3(6.68f, -1.67f, 0.96f), Quaternion.Euler(0, 180, 0), GameManager.Instance.visuals.transform);
        }

        CardDisplayLoader cardDisplayLoader = newCard.GetComponent<CardDisplayLoader>();
        cardDisplayLoader.card = cardDrawn;
        cardDisplayLoader.loadCardAsset();
        //Debug.Log("SORTING: " + SortingLayer.GetLayerValueFromID(newCard.GetComponentInChildren<Canvas>().sortingLayerID));
        //SortingLayer.GetLayerValueFromID(newCard.GetComponentInChildren<Canvas>().sortingLayerID));
        //newCard.GetComponentInChildren<Canvas>().sortingLayerID = 3;
        //Debug.Log("SORTING: " + SortingLayer.GetLayerValueFromID(newCard.GetComponentInChildren<Canvas>().sortingLayerID));
        //Debug.Log("SORTING: " + newCard.GetComponentInChildren<Canvas>().sortingLayerName);
        //Debug.Log("SORTING: " + newCard.GetComponentInChildren<Canvas>().sortingLayerName);

        //Debug.Log("rot INIT: " + newCard.transform.rotation);
        StartCoroutine(rotateWhenDrawn(newCard));
        //StartCoroutine(moveToHandAfterRotate(newCard));

        //Debug.Log("rot END: " + newCard.transform.rotation);
        //Debug.Log("SORTING: " + newCard.GetComponentInChildren<Canvas>().sortingLayerName);
        
        //newCard.transform.DOMove(this.transform.position, 2).SetEase(Ease.OutQuint, 0.5f, 0.1f);
        //Canvas cardCanvas = newCard.GetComponentInParent<Canvas>();
        //cardCanvas.sortingOrder = 2;

    }


    IEnumerator rotateWhenDrawn(GameObject newCard)
    {
        newCard.GetComponentInChildren<Canvas>().sortingLayerName = "ActiveCard";
        for (float f = 1f; f >= 0; f -= 0.02f)
        {
            newCard.transform.rotation = Quaternion.Euler(0, 180.0f * f, 0);
            newCard.transform.localScale += new Vector3(0.0001f, 0.0001f, 0);
            newCard.transform.position += new Vector3(-0.05f, 0.05f, 0);
            //Debug.Log("rot: "+newCard.transform.rotation);
            yield return new WaitForSeconds(.01f);
        }
        yield return new WaitForSeconds(1f);
        float distanceX = this.transform.position.x - newCard.transform.position.x;
        float distanceY = this.transform.position.y - newCard.transform.position.y;

        //Debug.Log("pos this " + this.transform.position);
        //Debug.Log("pos card " + newCard.transform.position);
        //Debug.Log("distance X " + distanceX);
        //Debug.Log("distance Y " + distanceY);
        for (float f = 1; f >= 0; f -= 0.02f)
        {

            newCard.transform.localScale -= new Vector3(0.0001f, 0.0001f, 0);
            newCard.transform.SetPositionAndRotation(new Vector3(this.transform.position.x - distanceX * f, this.transform.position.y - distanceY * f, 0), Quaternion.identity);
            Debug.Log("pos displ " + this.transform.position);
            Debug.Log("pos card " + newCard.transform.position);
            yield return new WaitForSeconds(.01f);
        }
        newCard.transform.SetParent(this.transform);
        newCard.GetComponentInChildren<Canvas>().sortingLayerName = "Card";
    }

    //GameObject CreateACardAtPosition(CardAsset c, Vector3 position, Vector3 eulerAngles)
    //{
    //    // Instantiate a card depending on its type
    //    GameObject card;
    //    if (c.MaxHealth > 0)
    //    {
    //        // this card is a creature card
    //        card = GameObject.Instantiate(GlobalSettings.Instance.CreatureCardPrefab, position, Quaternion.Euler(eulerAngles)) as GameObject;
    //    }
    //    else
    //    {
    //        // this is a spell: checking for targeted or non-targeted spell
    //        if (c.Targets == TargetingOptions.NoTarget)
    //            card = GameObject.Instantiate(GlobalSettings.Instance.NoTargetSpellCardPrefab, position, Quaternion.Euler(eulerAngles)) as GameObject;
    //        else
    //        {
    //            card = GameObject.Instantiate(GlobalSettings.Instance.TargetedSpellCardPrefab, position, Quaternion.Euler(eulerAngles)) as GameObject;
    //            // pass targeting options to DraggingActions
    //            DragSpellOnTarget dragSpell = card.GetComponentInChildren<DragSpellOnTarget>();
    //            dragSpell.Targets = c.Targets;
    //        }

    //    }

    //    // apply the look of the card based on the info from CardAsset
    //    OneCardManager manager = card.GetComponent<OneCardManager>();
    //    manager.cardAsset = c;
    //    manager.ReadCardFromAsset();

    //    return card;
    //}
}
