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
            foreach (Transform child in dropAreaImage.transform)
            {
                {
                    child.GetComponent<Draggable>().enabled = false;
                    //child.GetComponent<Attackable>().enabled = true;
                    child.GetComponent<Attackable>().initialDropZone = this;
                    child.GetComponent<Defendable>().enabled = true;
                }
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
        CardVisualStateEnum cardState = eventData.pointerDrag.GetComponent<CardVisualState>().cardVisualStateEnum;

        // allow drag if draggable object exists and dropzone belongs to player
        if (d != null && cardState != CardVisualStateEnum.TacticsWithAim && dropZonePosition == d.t_Reference.GetComponent<IDAssignment>().ownerPosition)
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
            Card cardInModel = GameManager.Instance.currentPlayer.armymodel.armyCardsModel.findCardInFrontByID(child.GetComponent<IDAssignment>().uniqueId);
           
            if (cardInModel.currentAttacksPerTurn > 0 && cardInModel.isAbleToAttack)
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
