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
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentToReturnTo = this.transform.parent;
        initialPosition = this.transform.position;
        Debug.Log("on begin drag");
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
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        //transform.DOMove(parentToReturnTo, 1f);
        if (dragSuccess)//for dragging successful
        {
        }
        else
        {
            transform.DOMove(initialPosition, 1);
            //this.transform.SetParent(parentToReturnTo);
        }
    }
}
