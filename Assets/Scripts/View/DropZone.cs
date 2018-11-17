using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public Position dropZonePosition;
    public GameObject dropAreaImage;
    public bool dropEventOccurs = false;
    public bool attackEventEnded = false;


    void Update()
    {
        if (dropEventOccurs)
        {
            Debug.Log("dropEvent on " + this.dropZonePosition);
            foreach (Transform child in dropAreaImage.transform)
            {
                child.GetComponent<Draggable>().enabled = false;
                child.GetComponent<Attackable>().enabled = true;
                child.GetComponent<Attackable>().initialDropZone = this;
                child.GetComponent<Defendable>().enabled = true;
            }

            dropEventOccurs = false;
        }

        if (attackEventEnded)
        {
            unlockUnitAttacks();
            attackEventEnded = false;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();

        // allow drag if draggable object exists and dropzone belongs to player
        if (d != null && dropZonePosition == d.t_Reference.GetComponent<IDAssignment>().ownerPosition)
        {
            d.dropZone = this;
            d.dragSuccess = true;
            d.parentToReturnTo = this.transform.GetChild(0).GetChild(0);
        }
    }

    public void unlockUnitAttacks()
    {
        foreach (Transform child in dropAreaImage.transform)
        {
            // enable only cards with available attack this turn
            //Debug.Log("left turns " + GameManager.Instance.currentPlayer.armymodel.armyCardsModel.findCardInFrontByID(child.GetComponent<IDAssignment>().uniqueId).cardName);
            //Debug.Log("left turns " + GameManager.Instance.currentPlayer.armymodel.armyCardsModel.findCardInFrontByID(child.GetComponent<IDAssignment>().uniqueId).currentAttacksPerTurn);
            Card cardInModel = GameManager.Instance.currentPlayer.armymodel.armyCardsModel.findCardInFrontByID(child.GetComponent<IDAssignment>().uniqueId);
            Debug.Log("Name  " + child.GetComponent<IDAssignment>().name + " Attacks no: " + cardInModel.currentAttacksPerTurn);
            if (cardInModel.currentAttacksPerTurn > 0)
            {
                // enable unit glow
                child.GetComponent<CardDisplayLoader>().Unit.GetComponent<UnitVisualManager>().unitGlowImage.enabled = true;

                // enable unit attack
                child.GetComponent<Attackable>().enabled = true;
            }
            else
            {
                // block unit glow
                child.GetComponent<CardDisplayLoader>().Unit.GetComponent<UnitVisualManager>().unitGlowImage.enabled = false;

                // block unit attack
                child.GetComponent<Attackable>().enabled = false;
            }
        }
    }
    
    public void blockAllUnitOperations()
    {
        foreach (Transform child in dropAreaImage.transform)
        {
            // block unit glow
            child.GetComponent<CardDisplayLoader>().Unit.GetComponent<UnitVisualManager>().unitGlowImage.enabled = false;

            // block unit attack
            child.GetComponent<Attackable>().enabled = false;
        }
    }
}
