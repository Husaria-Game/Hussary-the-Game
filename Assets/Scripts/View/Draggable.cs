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
    private Transform t_Reference;

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentToReturnTo = this.transform.parent;
        initialPosition = this.transform.position;
        pointerDisplacement = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - initialPosition.x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y - initialPosition.y, 0);
        cardPreview = this.gameObject.gameObject.transform.Find("Canvas").gameObject;

        dragSuccess = false;
        Debug.Log("on BEGIN drag OBJECT " + this.gameObject);
        Debug.Log("on BEGIN drag LAYER " + this.gameObject.GetComponentInChildren<Canvas>().sortingLayerName);
        this.gameObject.GetComponentInChildren<Canvas>().sortingLayerName = "ActiveCard";

        GetComponent<CanvasGroup>().blocksRaycasts = false;
        if(lineRenderer != null)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, initialPosition);
        }
        if (arrow != null)
        {
            arrow.SetActive(true);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 pz = Camera.main.ScreenToWorldPoint(eventData.position);
        pz.z = 0;


       if (lineRenderer != null)
        {
            Vector3 A = initialPosition;

            Vector3 P = Vector3.Lerp(A, pz, 0.90f );
            lineRenderer.SetPosition(1, new Vector2((P.x), (P.y)));
        }
        else
        {
            this.transform.position = new Vector2(pz.x - pointerDisplacement.x, pz.y - pointerDisplacement.y);
        }
        if (arrow != null)
        {
            arrow.transform.position = new Vector2(pz.x , pz.y );
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("on END drag, parent: " + this.transform.parent);

        GetComponent<CanvasGroup>().blocksRaycasts = true;

        if (dragSuccess)//for dragging successful
        {
			// change card position in view to Front
            transform.SetParent(parentToReturnTo);

            // recalculate current resources
            updateResourcesModelAndView();

            // transformations for UnitCard only
            if (int.Parse(transform.GetComponent<CardDisplayLoader>().armorText.text.ToString()) > 0)
            {
				transform.GetComponent<IDAssignment>().whereIsCard = WhereIsCard.Front;
				// change card position in model
				cardDraggedToFrontCommand(transform.GetComponent<IDAssignment>().ownerPosition, transform.GetComponent<IDAssignment>().uniqueId);
				// change card visual from UnitCard to Unit
                transform.GetComponent<CardVisualState>().changeStateToUnit();
            }

        }
        else
        {
            transform.DOMove(initialPosition, 1).SetEase(Ease.OutQuint, 0.5f, 0.1f);
        }
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }
        if (arrow != null)
        {
            arrow.SetActive(false);
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
        int updatedCurrentResources = GameManager.Instance.whoseTurn.substractCurrentResources(int.Parse(transform.GetComponent<CardDisplayLoader>().cardMoneyText.text.ToString()));
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
