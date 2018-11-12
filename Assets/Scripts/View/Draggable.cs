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
    public DropZone initialDropZone;
    private Vector3 pointerDisplacement = Vector3.zero;
    public Transform t_Reference;
    private CardVisualStateEnum cardState;

    public void OnBeginDrag(PointerEventData eventData)
    {
        // get object reference transform
        if (transform.GetComponent<UnitVisualManager>() != null)
        {
            //if unit then get transform from parent
            t_Reference = transform.GetComponent<UnitVisualManager>().unitParentCard.gameObject.transform;
        }
        else
        {
            t_Reference = this.transform;
        }
        cardState = t_Reference.GetComponent<CardVisualState>().cardVisualStateEnum;
        Debug.Log("cardState: " + cardState);
        parentToReturnTo = t_Reference.parent;
        initialPosition = t_Reference.position;
        pointerDisplacement = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - initialPosition.x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y - initialPosition.y, 0);
        cardPreview = t_Reference.gameObject.transform.Find("Canvas").gameObject;

        dragSuccess = false;
        t_Reference.gameObject.GetComponentInChildren<Canvas>().sortingLayerName = "ActiveCard";

        GetComponent<CanvasGroup>().blocksRaycasts = false;

        // enable aim icon for unit tactics with aim
        if (cardState == CardVisualStateEnum.Unit || cardState == CardVisualStateEnum.TacticsWithAim)
        {
            if (lineRenderer != null)
            {
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, initialPosition);
                Debug.Log("JESTEM 2: ");
            }
            if (arrow != null)
            {
                Debug.Log("JESTEM 3: ");
                arrow.SetActive(true);
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 pz = Camera.main.ScreenToWorldPoint(eventData.position);
        pz.z = 0;

       

        // update aim icon for unit and tactics with aim
        if (cardState == CardVisualStateEnum.Unit || cardState == CardVisualStateEnum.TacticsWithAim)
        {
            if (lineRenderer != null)
        {
            Vector3 A = initialPosition;

                Vector3 P = Vector3.Lerp(A, pz, 0.90f);
                lineRenderer.SetPosition(1, new Vector2((P.x), (P.y)));
            }
        }
        else
        {
        t_Reference.position = new Vector2(pz.x - pointerDisplacement.x, pz.y - pointerDisplacement.y);
        }
        if (cardState == CardVisualStateEnum.Unit || cardState == CardVisualStateEnum.TacticsWithAim)
        {
            if (arrow != null)
            {
                arrow.transform.position = new Vector2(pz.x, pz.y);
            }
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        if (dragSuccess)//for dragging successful
        {
            if (cardState == CardVisualStateEnum.Card)
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
                }
            }

        }
        else
        {
            t_Reference.DOMove(initialPosition, 1).SetEase(Ease.OutQuint, 0.5f, 0.1f);
        }
        if (cardState == CardVisualStateEnum.Unit || cardState == CardVisualStateEnum.TacticsWithAim)
        {
            if (lineRenderer != null)
            {
                lineRenderer.enabled = false;
            }
            if (arrow != null)
            {
                arrow.SetActive(false);
            }
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
        int updatedCurrentResources = GameManager.Instance.whoseTurn.substractCurrentResources(int.Parse(t_Reference.GetComponent<CardDisplayLoader>().cardMoneyText.text.ToString()));
        int maxResources = GameManager.Instance.whoseTurn.resourcesMaxThisTurn;
        if (GameManager.Instance.whoseTurn == GameManager.Instance.playerNorth)
        {
            GameManager.Instance.resourcesNorth.transform.GetComponent<ResourcePool>().updateResourcesView(updatedCurrentResources, maxResources);
        }
        else if (GameManager.Instance.whoseTurn == GameManager.Instance.playerSouth)
        {
            GameManager.Instance.resourcesSouth.transform.GetComponent<ResourcePool>().updateResourcesView(updatedCurrentResources, maxResources);
        }
    }
}
