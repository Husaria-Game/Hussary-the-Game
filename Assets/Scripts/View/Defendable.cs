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
        Attackable attackerCard = eventData.pointerDrag.GetComponent<Attackable>();

        
        if (this.GetComponent<HeroVisualManager>()) // behaviour for Hero
        {
            ownerPosition = this.GetComponent<HeroVisualManager>().ownerPosition;

            if (attackerCard != null && ownerPosition != attackerCard.t_Reference.GetComponent<IDAssignment>().ownerPosition && attackerCard.t_Reference.GetComponent<CardVisualState>().cardVisualStateEnum == CardVisualStateEnum.Unit)
            {
                attackerCard.attackOnHeroSuccess = true;
                attackerCard.hero = this.gameObject;
            }
        }
        else // behaviour for Unit
        {
            defenderCardTransform = this.transform;
            ownerPosition = defenderCardTransform.GetComponent<IDAssignment>().ownerPosition;
            
            // allow drag if draggable object exists and card belongs to other player
            if (attackerCard != null && ownerPosition != attackerCard.t_Reference.GetComponent<IDAssignment>().ownerPosition && attackerCard.t_Reference.GetComponent<CardVisualState>().cardVisualStateEnum == CardVisualStateEnum.Unit)
            {
                attackerCard.attackOnUnitSuccess = true;
                attackerCard.defenderCard = defenderCardTransform.GetComponent<Defendable>();
                attackerCard.defenderUnit = this.transform.GetComponent<CardDisplayLoader>().Unit.transform;
            }
        }
        
    }
}
