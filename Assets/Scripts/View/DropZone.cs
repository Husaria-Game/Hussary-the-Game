using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Position dropZonePosition;
    public void OnDrop(PointerEventData eventData)
    {

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        // allow drag if draggable object exists and dropzone belongs to player
        if (d != null && dropZonePosition == d.transform.GetComponent<IDAssignment>().ownerPosition)// && this.transform.GetChild(0).GetChild(0) != d.parentToReturnTo)
        {
            d.dragSuccess = true;
            d.parentToReturnTo = this.transform.GetChild(0).GetChild(0);
            d.transform.GetComponent<IDAssignment>().whereIsCard = WhereIsCard.Front;
            GameManager.Instance.cardDraggedToFrontCommand(dropZonePosition, d.transform.GetComponent<IDAssignment>().uniqueId);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("OnPointerEnter to " + gameObject.name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("OnPointerExit to " + gameObject.name);
    }
}
