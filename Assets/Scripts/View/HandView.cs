using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void AddDrawnCardFromDeckToHand(Card cardDrawn)
    {
        // ----------instantiate drawn card given as parameter and load its display in player's hand
        GameObject newCard;
        if (cardDrawn.maxHealth > 0)
        {
            newCard = Instantiate(GameManager.Instance.unitCard, new Vector3(0, 0, 0), Quaternion.identity, GameManager.Instance.handView.transform);
        }
        else
        {
            newCard = Instantiate(GameManager.Instance.tacticsCard, new Vector3(0, 0, 0), Quaternion.identity, GameManager.Instance.handView.transform);
        }
        CardDisplayLoader cardDisplayLoader = newCard.GetComponent<CardDisplayLoader>();
        cardDisplayLoader.card = cardDrawn;
        cardDisplayLoader.loadCardAsset();
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
