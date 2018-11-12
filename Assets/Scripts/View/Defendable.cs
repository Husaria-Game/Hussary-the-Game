using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Defendable : MonoBehaviour, IDropHandler
{
    private Position ownerPosition;
    private Transform defenderCardTransform;

    public void OnDrop(PointerEventData eventData)
    {

        //if unit then get transform from parent
        defenderCardTransform = transform.GetComponent<UnitVisualManager>().unitParentCard.gameObject.transform;
        ownerPosition = defenderCardTransform.GetComponent<IDAssignment>().ownerPosition;

        Attackable attackerUnit = eventData.pointerDrag.GetComponent<Attackable>();
        Draggable attackerCard = attackerUnit.t_Reference.GetComponent<Draggable>();
        // allow drag if draggable object exists and card belongs to other player
        if (attackerCard != null && ownerPosition != attackerCard.t_Reference.GetComponent<IDAssignment>().ownerPosition && attackerCard.t_Reference.GetComponent<CardVisualState>().cardVisualStateEnum == CardVisualStateEnum.Unit)
        {
            attackerUnit.attackSuccess = true;
            attackerUnit.defenderUnit = transform;

            Debug.Log("ATTACK");
        }
    }
}
