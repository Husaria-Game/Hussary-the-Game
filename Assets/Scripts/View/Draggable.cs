using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentToReturnTo = null;
    public Vector3 initialPosition;
    public Vector3 mousePosition;
    private GameObject cardPreview;
    public LineRenderer lineRenderer;
    public GameObject arrow;
    public bool dragSuccess;
    public bool transformCardIntoUnit;
    public DropZone dropZone;
    private Vector3 pointerDisplacement = Vector3.zero;
    public Transform t_Reference;
    private CardVisualStateEnum cardDetailedType;
    private CardType cardType;

    public void OnBeginDrag(PointerEventData eventData)
    {
        t_Reference = this.transform;
        cardDetailedType = t_Reference.GetComponent<CardDisplayLoader>().cardDetailedType;

        parentToReturnTo = t_Reference.parent;
        initialPosition = t_Reference.position;
        pointerDisplacement = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - initialPosition.x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y - initialPosition.y, 0);
        cardPreview = t_Reference.gameObject.transform.Find("Canvas").gameObject;

        dragSuccess = false;
        t_Reference.gameObject.GetComponentInChildren<Canvas>().sortingLayerName = "ActiveCard";

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 pz = Camera.main.ScreenToWorldPoint(eventData.position);
        pz.z = 0;
        
        if (cardDetailedType == CardVisualStateEnum.UnitCard)
        {
            t_Reference.position = new Vector2(pz.x - pointerDisplacement.x, pz.y - pointerDisplacement.y);
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        if (dragSuccess)//for dragging successful
        {
            if (cardDetailedType == CardVisualStateEnum.UnitCard)
            {
                // change card position in view to Front
                t_Reference.SetParent(parentToReturnTo);

                // recalculate current resources
                updateResourcesModelAndView();

                // transformations for UnitCard only
                if (int.Parse(transform.GetComponent<CardDisplayLoader>().armorText.text.ToString()) > 0)
                {
                    t_Reference.GetComponent<IDAssignment>().whereIsCard = WhereIsCard.Front;
                    // change card position in model
                    cardDraggedToFrontCommand(transform.GetComponent<IDAssignment>().ownerPosition, transform.GetComponent<IDAssignment>().uniqueId);
                    // change card visual from UnitCard to Unit
                    t_Reference.GetComponent<CardVisualState>().changeStateToUnit();
                    this.GetComponentInChildren<Canvas>().sortingLayerName = "Card";

                    // sets dropEventOccurs to true, which disables Draggable script
                    dropZone.dropEventOccurs = true;
                    GameManager.Instance.enablePlayableCardsFlag = true;
                }
            }

        }
        else
        {
            t_Reference.DOMove(initialPosition, 1).SetEase(Ease.OutQuint, 0.5f, 0.1f);
        }
        this.GetComponentInChildren<Canvas>().sortingLayerName = "Card";
	}

	public void cardDraggedToFrontCommand(Position playerPosition, int cardId)
	{
		if(playerPosition == Position.North)
		{
			GameManager.Instance.playerNorth.armymodel.armyCardsModel.moveCardFromHandToFront(cardId);
		}
		else if (playerPosition == Position.South)
		{
			GameManager.Instance.playerSouth.armymodel.armyCardsModel.moveCardFromHandToFront(cardId);
		}
	}

    public void updateResourcesModelAndView()
    {
        int updatedCurrentResources = GameManager.Instance.currentPlayer.substractCurrentResources(int.Parse(t_Reference.GetComponent<CardDisplayLoader>().cardMoneyText.text.ToString()));
        int maxResources = GameManager.Instance.currentPlayer.resourcesMaxThisTurn;
        GameManager.Instance.currentPlayer.resourceVisual.updateResourcesView(updatedCurrentResources, maxResources);
    }
}
