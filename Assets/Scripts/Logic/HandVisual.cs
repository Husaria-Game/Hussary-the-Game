using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandVisual : MonoBehaviour {

    private List<GameObject> CardsInHand = new List<GameObject>();
    public GameObject GameAreaImage;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // add new card GameObject to hand
    public void AddCard(GameObject card)
    {
        // we allways insert a new card as 0th element in CardsInHand List 
        CardsInHand.Insert(0, card);

        // parent this card to our Slots GameObject
        card.transform.SetParent(GameAreaImage.transform);

        // re-calculate the position of the hand
        //PlaceCardsOnNewSlots();
        //UpdatePlacementOfSlots();
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
