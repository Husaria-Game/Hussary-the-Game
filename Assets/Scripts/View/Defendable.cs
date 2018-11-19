using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Defendable : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Position ownerPosition;
    private Transform defenderCardTransform;
    private bool pointerEnter;
    private bool pointerExit;

    public void Start()
    {
        pointerEnter = false;
        pointerExit = false;
    }

    public void Update()
    {
        setCardRedGlowWhenAimed();
    }

    public void OnDrop(PointerEventData eventData)
    {
        Attackable attackerCard = eventData.pointerDrag.GetComponent<Attackable>();

        // Emulate pointer exit to disable unitPointerGlowImage in Update()
        pointerExit = true;
        
        if (this.GetComponent<HeroVisualManager>()) // behaviour for Hero
        {
            ownerPosition = this.GetComponent<HeroVisualManager>().ownerPosition;

            if (attackerCard != null && ownerPosition != attackerCard.t_Reference.GetComponent<IDAssignment>().ownerPosition && attackerCard.t_Reference.GetComponent<CardVisualState>().cardVisualStateEnum == CardVisualStateEnum.Unit)
            {
                attackerCard.attackOnHeroSuccess = true;
                attackerCard.hero = this.gameObject;
            }
        }
        else if (attackerCard.enabled) // behaviour for Unit
        {
            defenderCardTransform = this.transform;
            ownerPosition = defenderCardTransform.GetComponent<IDAssignment>().ownerPosition;
            CardVisualStateEnum cardState = attackerCard.t_Reference.GetComponent<CardVisualState>().cardVisualStateEnum;

            // allow drag if draggable object exists and card belongs to other player
            if (attackerCard != null && ownerPosition != attackerCard.t_Reference.GetComponent<IDAssignment>().ownerPosition && (cardState == CardVisualStateEnum.Unit || cardState == CardVisualStateEnum.TacticsWithAim))
            {
                attackerCard.attackOnUnitSuccess = true;
                attackerCard.defenderCard = defenderCardTransform.GetComponent<Defendable>();
                attackerCard.defenderUnit = this.transform.GetComponent<CardDisplayLoader>().Unit.transform;
            }
            // allow drag success for card with state TacticsAttackAll - behaviour as in dropzone for this card (because drop on card unit blocks detecting drop on enemy dropzone)
            else if (attackerCard != null && ownerPosition != attackerCard.t_Reference.GetComponent<IDAssignment>().ownerPosition && (cardState == CardVisualStateEnum.TacticsAttackAll))
            {
                this.GetComponent<Draggable>().dropZone.startAttackAll(attackerCard);
            }
        }
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // flag to create attack red glow on target
        pointerEnter = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // flag to disable attack red glow on target
        pointerExit = true;
    }

    public void setCardRedGlowWhenAimed()
    {
        if (transform.GetComponent<CardVisualState>() != null && GameManager.Instance.isAttackableDraggingActive && pointerEnter && transform.GetComponent<CardVisualState>().cardVisualStateEnum == CardVisualStateEnum.Unit && transform.GetComponent<IDAssignment>().ownerPosition != GameManager.Instance.currentPlayer.position)
        {
            Image unitPointerGlowImage = transform.GetComponent<CardDisplayLoader>().Unit.GetComponent<UnitVisualManager>().unitPointerGlowImage;
            unitPointerGlowImage.enabled = true;
            unitPointerGlowImage.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
            pointerEnter = false;
        }
        if (transform.GetComponent<CardVisualState>() != null && GameManager.Instance.isAttackableDraggingActive && pointerExit && transform.GetComponent<CardVisualState>().cardVisualStateEnum == CardVisualStateEnum.Unit && transform.GetComponent<IDAssignment>().ownerPosition != GameManager.Instance.currentPlayer.position)
        {
            Image unitPointerGlowImage = transform.GetComponent<CardDisplayLoader>().Unit.GetComponent<UnitVisualManager>().unitPointerGlowImage;
            unitPointerGlowImage.enabled = false;
            pointerExit = false;
        }
        if (transform.GetComponent<CardVisualState>() != null && !GameManager.Instance.isAttackableDraggingActive )
        {
            Image unitPointerGlowImage = transform.GetComponent<CardDisplayLoader>().Unit.GetComponent<UnitVisualManager>().unitPointerGlowImage;
            unitPointerGlowImage.enabled = false;
        }
    }
}
