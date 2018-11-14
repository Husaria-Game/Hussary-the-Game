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
        Debug.Log("DEFENDABLE " + this);
        // get transform from parent
        //GameObject defenderUnit = this.GetComponent<CardDisplayLoader>().Unit;
        Attackable attackerCard = eventData.pointerDrag.GetComponent<Attackable>();

        if (this.GetComponent<HeroVisualManager>())
        {
            Debug.Log("HERO");
            ownerPosition = this.GetComponent<HeroVisualManager>().ownerPosition;

            if (attackerCard != null && ownerPosition != attackerCard.t_Reference.GetComponent<IDAssignment>().ownerPosition && attackerCard.t_Reference.GetComponent<CardVisualState>().cardVisualStateEnum == CardVisualStateEnum.Unit)
            {
                attackerCard.attackOnHeroSuccess = true;
                attackerCard.hero = this.gameObject;

                Debug.Log("ATTACK ON HERO");
            }
        }
        else
        {
            defenderCardTransform = transform.GetComponent<UnitVisualManager>().unitParentCard.transform;
            ownerPosition = defenderCardTransform.GetComponent<IDAssignment>().ownerPosition;
            //Debug.Log(" attacker card " + eventData.pointerDrag);
            //Attackable attackerUnit = eventData.pointerDrag.GetComponent<Attackable>();
            
            // allow drag if draggable object exists and card belongs to other player
            if (attackerCard != null && ownerPosition != attackerCard.t_Reference.GetComponent<IDAssignment>().ownerPosition && attackerCard.t_Reference.GetComponent<CardVisualState>().cardVisualStateEnum == CardVisualStateEnum.Unit)
            {
                attackerCard.attackOnUnitSuccess = true;
                attackerCard.defenderCard = defenderCardTransform.GetComponent<Defendable>();
                attackerCard.defenderUnit = this.transform;

                Debug.Log("ATTACK");
            }
        }
        
    }
}
