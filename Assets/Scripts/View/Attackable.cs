using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Attackable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentToReturnTo = null;
    public Transform defenderUnit = null;
    public Vector3 initialPosition;
    public Vector3 mousePosition;
    private GameObject cardPreview;
    public LineRenderer lineRenderer;
    public GameObject arrow;
    public bool attackSuccess;
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

        parentToReturnTo = t_Reference.parent;
        initialPosition = t_Reference.position;
        pointerDisplacement = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - initialPosition.x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y - initialPosition.y, 0);
        cardPreview = t_Reference.gameObject.transform.Find("Canvas").gameObject;

        attackSuccess = false;

        t_Reference.gameObject.GetComponentInChildren<Canvas>().sortingLayerName = "ActiveCard";

        GetComponent<CanvasGroup>().blocksRaycasts = false;

        // enable aim icon for unit tactics with aim
        if (cardState == CardVisualStateEnum.Unit || cardState == CardVisualStateEnum.TacticsWithAim)
        {
            if (lineRenderer != null)
            {
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, initialPosition);
            }
            if (arrow != null)
            {
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

        Vector3 pz = Camera.main.ScreenToWorldPoint(eventData.position);
        pz.z = 0;

        if (attackSuccess)//for attack successful - attacker adjusts all the information about the fight, attacker and defender
        {
            if (cardState == CardVisualStateEnum.Unit)
            {
                Draggable defenderCard = defenderUnit.transform.GetComponent<Draggable>();
                // move card to defender and come back
                t_Reference.DOMove(pz, 0.5f).SetEase(Ease.InQuint, 0.5f, 0.1f).OnComplete(comeBack);

                // create explosion
                defenderUnit.GetComponent<UnitVisualManager>().createDamageVisual(7);



                // remove armor from defender


                // check if defender dead


                // remove armor from attacker
                GetComponent<UnitVisualManager>().createDamageVisual(4);

                // check if attacker dead



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

    public void comeBack()
    {
        t_Reference.DOMove(initialPosition, 1).SetEase(Ease.OutQuint, 0.5f, 0.1f);
    }
}

