using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Position dropZonePosition;
    public GameObject dropAreaImage;
    private GameObject sourceCardGOWhenDragging;
    public bool dropEventOccurs = false;
    public bool attackEventEnded = false;
    private bool pointerEnter;
    private bool pointerExit;

    public void Start()
    {
        pointerEnter = false;
        pointerExit = false;
        sourceCardGOWhenDragging = null;
    }

    void Update()
    {
        if (sourceCardGOWhenDragging != null && sourceCardGOWhenDragging.GetComponent<CardDisplayLoader>().cardDetailedType == CardVisualStateEnum.TacticsAttackAll)
        {
            setDropZoneRedGlowWhenAimed();
        }
        if (sourceCardGOWhenDragging != null && sourceCardGOWhenDragging.GetComponent<CardDisplayLoader>().cardDetailedType == CardVisualStateEnum.TacticsHealAll)
        {
            setDropZoneBlueGlowWhenAimed();
        }
       
        if (dropEventOccurs)
        {
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
        Attackable a = eventData.pointerDrag.GetComponent<Attackable>();
        CardVisualStateEnum cardState = eventData.pointerDrag.GetComponent<CardVisualState>().cardVisualStateEnum;

        // allow drag if draggable object exists and dropzone belongs to player
        if (d != null && cardState == CardVisualStateEnum.UnitCard && dropZonePosition == d.t_Reference.GetComponent<IDAssignment>().ownerPosition)
        {
            d.dropZone = this;
            d.dragSuccess = true;
            d.parentToReturnTo = this.transform.GetChild(0).GetChild(0);
        }

        // allow drag (attack) if cardState is TacticsAttackAll
        if (a != null && cardState == CardVisualStateEnum.TacticsAttackAll && dropZonePosition != a.t_Reference.GetComponent<IDAssignment>().ownerPosition)
        {
            startAttackAll(a);
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

    public void startAttackAll(Attackable a)
    {
        a.enemyDropZone = this;
        a.attackOnAllUnitsSuccess = true;
        a.parentToReturnTo = transform.GetChild(0).GetChild(0);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameManager.Instance.isAttackableDraggingActive)
        {
            sourceCardGOWhenDragging = eventData.pointerDrag;
            // flag to create attack red glow on target
            pointerEnter = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (GameManager.Instance.isAttackableDraggingActive)
        {
            // flag to disable attack red glow on target
            pointerExit = true;
        }
    }
    public void setDropZoneRedGlowWhenAimed()
    {
        if (GameManager.Instance.isAttackableDraggingActive && pointerEnter &&  this.dropZonePosition != GameManager.Instance.currentPlayer.position)
        {
            dropAreaImage.GetComponent<Image>().color = new Color32(255, 0, 0, 125);
            pointerEnter = false;
            pointerExit = false;
        }
        if (GameManager.Instance.isAttackableDraggingActive && pointerExit && this.dropZonePosition != GameManager.Instance.currentPlayer.position)
        {
            dropAreaImage.GetComponent<Image>().color = new Color32(47, 44, 45, 125);
            sourceCardGOWhenDragging = null;
            pointerEnter = false;
            pointerExit = false;
        }
        if (!GameManager.Instance.isAttackableDraggingActive)
        {
            sourceCardGOWhenDragging = null;
            dropAreaImage.GetComponent<Image>().color = new Color32(47, 44, 45, 125);
            pointerEnter = false;
            pointerExit = false;
        }
    }
    public void setDropZoneBlueGlowWhenAimed()
    {
        if (GameManager.Instance.isAttackableDraggingActive && pointerEnter && this.dropZonePosition == GameManager.Instance.currentPlayer.position)
        {
            dropAreaImage.GetComponent<Image>().color = new Color32(0, 157, 235, 209);
            pointerEnter = false;
            pointerExit = false;
        }
        if (GameManager.Instance.isAttackableDraggingActive && pointerExit && this.dropZonePosition == GameManager.Instance.currentPlayer.position)
        {
            dropAreaImage.GetComponent<Image>().color = new Color32(47, 44, 45, 125);
            sourceCardGOWhenDragging = null;
            pointerEnter = false;
            pointerExit = false;
        }
        if (!GameManager.Instance.isAttackableDraggingActive)
        {
            sourceCardGOWhenDragging = null;
            dropAreaImage.GetComponent<Image>().color = new Color32(47, 44, 45, 125);
            pointerEnter = false;
            pointerExit = false;
        }
    }
}
