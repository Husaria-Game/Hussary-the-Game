using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentToReturnTo = null;
    public Vector3 initialPosition;
    public bool dragSuccess;
    public DropZone initialDropZone;
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentToReturnTo = this.transform.parent;
        initialPosition = this.transform.position;
        //Debug.Log("on BEGIN drag, parent: " + parentToReturnTo);
        dragSuccess = false;
        //this.transform.SetParent(this.transform.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //this.transform.SetParent(parentToReturnTo);
        //Debug.Log("on END drag, parent: " + this.transform.parent);

        GetComponent<CanvasGroup>().blocksRaycasts = true;
        //transform.DOMove(parentToReturnTo, 1f);
        if (dragSuccess)//for dragging successful
        {
            this.transform.SetParent(parentToReturnTo);
        }
        else
        {
            transform.DOMove(initialPosition, 1).SetEase(Ease.OutQuint, 0.5f, 0.1f);
            //this.transform.SetParent(parentToReturnTo);
        }
    }
}
