using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HandView : MonoBehaviour {

    private List<GameObject> CardsInHand = new List<GameObject>();
    public GameObject GameAreaImage;
    public bool isDrawingRunning = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // add new card GameObject to hand
    public void AddDrawnCardFromToHand(Card cardDrawn, PlayerModel player, GameObject deckVisual)
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

        CardDisplayLoader cardDisplayLoader = newCard.GetComponent<CardDisplayLoader>();
        cardDisplayLoader.card = cardDrawn;
        cardDisplayLoader.loadCardAsset();
        StartCoroutine(rotateWhenDrawn(newCard, player));
    }


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
}
