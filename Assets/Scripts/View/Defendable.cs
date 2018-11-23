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
    private GameObject sourceCardGOWhenDragging;

    public void Start()
    {
        pointerEnter = false;
        pointerExit = false;
        sourceCardGOWhenDragging = null;
    }

    public void Update()
    {
        if (sourceCardGOWhenDragging != null && (sourceCardGOWhenDragging.GetComponent<CardDisplayLoader>().cardDetailedType == CardVisualStateEnum.TacticsWithAim ||
            sourceCardGOWhenDragging.GetComponent<CardDisplayLoader>().cardDetailedType == CardVisualStateEnum.Unit))
        {
            setCardGlowWhenAimed(new Color32(255, 0, 0, 255), true);
        }
        if (sourceCardGOWhenDragging != null && sourceCardGOWhenDragging.GetComponent<CardDisplayLoader>().cardDetailedType == CardVisualStateEnum.TacticsHealOne)
        {
            setCardGlowWhenAimed(new Color32(63, 79, 246, 255), false);
        }
        if (sourceCardGOWhenDragging != null && sourceCardGOWhenDragging.GetComponent<CardDisplayLoader>().cardDetailedType == CardVisualStateEnum.TacticsStrengthOne)
        {
            setCardGlowWhenAimed(new Color32(253, 239, 6, 255), false);
        }
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
            CardVisualStateEnum attackerCardDetailedType = attackerCard.t_Reference.GetComponent<CardDisplayLoader>().cardDetailedType;

            // allow ATTACK ON ONE unit if attackable object exists and card belongs to other player
            if (attackerCard != null && ownerPosition != attackerCard.t_Reference.GetComponent<IDAssignment>().ownerPosition && (attackerCardDetailedType == CardVisualStateEnum.Unit || attackerCardDetailedType == CardVisualStateEnum.TacticsWithAim))
            {
                attackerCard.defenderCard = defenderCardTransform.GetComponent<Defendable>();
                attackerCard.defenderUnit = this.transform.GetComponent<CardDisplayLoader>().Unit.transform;
                attackerCard.attackOnUnitSuccess = true;
            }
            // allow HEAL ON ONE unit if attackable object exists and card belongs to current player
            else if (attackerCard != null && ownerPosition == attackerCard.t_Reference.GetComponent<IDAssignment>().ownerPosition && (attackerCardDetailedType == CardVisualStateEnum.TacticsHealOne))
            {
                attackerCard.defenderCard = defenderCardTransform.GetComponent<Defendable>();
                attackerCard.defenderUnit = this.transform.GetComponent<CardDisplayLoader>().Unit.transform;
                attackerCard.healOnOneUnitSuccess = true;
            }
            // allow STRENGTH ON ONE unit if attackable object exists and card belongs to current player
            else if (attackerCard != null && ownerPosition == attackerCard.t_Reference.GetComponent<IDAssignment>().ownerPosition && (attackerCardDetailedType == CardVisualStateEnum.TacticsStrengthOne))
            {
                attackerCard.defenderCard = defenderCardTransform.GetComponent<Defendable>();
                attackerCard.defenderUnit = this.transform.GetComponent<CardDisplayLoader>().Unit.transform;
                attackerCard.strenghtOnOneUnitSuccess = true;
            }
            // allow drag success for card with state TacticsAttackAll - behaviour as in dropzone for this card (because drop on card unit blocks detecting drop on enemy dropzone)
            else if (attackerCard != null && ownerPosition != attackerCard.t_Reference.GetComponent<IDAssignment>().ownerPosition && (attackerCardDetailedType == CardVisualStateEnum.TacticsAttackAll))
            {
                this.GetComponent<Draggable>().dropZone.startAttackAll(attackerCard);
            }
            // allow drag success for card with state TacticsHealAll - behaviour as in dropzone for this card (because drop on card unit blocks detecting drop on friendly dropzone)
            else if (attackerCard != null && ownerPosition == attackerCard.t_Reference.GetComponent<IDAssignment>().ownerPosition && (attackerCardDetailedType == CardVisualStateEnum.TacticsHealAll))
            {
                this.GetComponent<Draggable>().dropZone.startHealAll(attackerCard);
            }
        }
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        sourceCardGOWhenDragging = eventData.pointerDrag;
        // flag to create attack/heal glow on target unit
        pointerEnter = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // flag to disable attack/heal glow on target unit
        pointerExit = true;
    }

    public void setCardGlowWhenAimed(Color32 glowColor, bool isGlowForEnemyUnits)
    {
        if (isGlowForEnemyUnits)
        {  // Glow for enemy units
            if (transform.GetComponent<CardDisplayLoader>() != null && GameManager.Instance.isAttackableDraggingActive && pointerEnter && transform.GetComponent<CardDisplayLoader>().cardDetailedType == CardVisualStateEnum.Unit && transform.GetComponent<IDAssignment>().ownerPosition != GameManager.Instance.currentPlayer.position)
            {
                Image unitPointerGlowImage = transform.GetComponent<CardDisplayLoader>().Unit.GetComponent<UnitVisualManager>().unitPointerGlowImage;
                unitPointerGlowImage.enabled = true;
                unitPointerGlowImage.GetComponent<Image>().color = glowColor;
                pointerEnter = false;
                pointerExit = false;
            }
            if (transform.GetComponent<CardDisplayLoader>() != null && GameManager.Instance.isAttackableDraggingActive && pointerExit && transform.GetComponent<CardDisplayLoader>().cardDetailedType == CardVisualStateEnum.Unit && transform.GetComponent<IDAssignment>().ownerPosition != GameManager.Instance.currentPlayer.position)
            {
                Image unitPointerGlowImage = transform.GetComponent<CardDisplayLoader>().Unit.GetComponent<UnitVisualManager>().unitPointerGlowImage;
                sourceCardGOWhenDragging = null;
                unitPointerGlowImage.enabled = false;
                pointerEnter = false;
                pointerExit = false;
            }
        }
        else // Glow for friendly units
        {
            if (transform.GetComponent<CardDisplayLoader>() != null && GameManager.Instance.isAttackableDraggingActive && pointerEnter && transform.GetComponent<CardDisplayLoader>().cardDetailedType == CardVisualStateEnum.Unit && transform.GetComponent<IDAssignment>().ownerPosition == GameManager.Instance.currentPlayer.position)
            {
                Image unitPointerGlowImage = transform.GetComponent<CardDisplayLoader>().Unit.GetComponent<UnitVisualManager>().unitPointerGlowImage;
                unitPointerGlowImage.enabled = true;
                unitPointerGlowImage.GetComponent<Image>().color = glowColor;
                pointerEnter = false;
                pointerExit = false;
            }
            if (transform.GetComponent<CardDisplayLoader>() != null && GameManager.Instance.isAttackableDraggingActive && pointerExit && transform.GetComponent<CardDisplayLoader>().cardDetailedType == CardVisualStateEnum.Unit && transform.GetComponent<IDAssignment>().ownerPosition == GameManager.Instance.currentPlayer.position)
            {
                Image unitPointerGlowImage = transform.GetComponent<CardDisplayLoader>().Unit.GetComponent<UnitVisualManager>().unitPointerGlowImage;
                sourceCardGOWhenDragging = null;
                unitPointerGlowImage.enabled = false;
                pointerEnter = false;
                pointerExit = false;
            }
        }

        if (transform.GetComponent<CardDisplayLoader>() != null && !GameManager.Instance.isAttackableDraggingActive)
        {
            Image unitPointerGlowImage = transform.GetComponent<CardDisplayLoader>().Unit.GetComponent<UnitVisualManager>().unitPointerGlowImage;
            sourceCardGOWhenDragging = null;
            unitPointerGlowImage.enabled = false;
            pointerEnter = false;
            pointerExit = false;
        }
    }
}
