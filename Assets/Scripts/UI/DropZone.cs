using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("OnDrop to " + gameObject.name);
        //Debug.Log(eventData.pointerDrag.name + "was dropped to " + gameObject.transform);

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        //DropZone dz = eventData.pointerDrag.GetComponent<DropZone>();
        Debug.Log("New: " + this.transform + "      Old: " + d.parentToReturnTo);
        if (d != null && this.transform.GetChild(0).GetChild(0) != d.parentToReturnTo)
        {
            d.dragSuccess = true;
            d.parentToReturnTo = this.transform.GetChild(0).GetChild(0);
        }
    }

    void OnMouseUp(PointerEventData eventData)
    {
        // If your mouse hovers over the GameObject with the script attached, output this message
        Debug.Log("Drag ended!");
    }

    void OnMouseDown(PointerEventData eventData)
    {
        Debug.Log("Drag start!");
        //if (da != null && da.CanDrag)
        //{
        //    dragging = true;
        //    // when we are dragging something, all previews should be off
        //    HoverPreview.PreviewsAllowed = false;
        //    _draggingThis = this;
        //    da.OnStartDrag();
        //    zDisplacement = -Camera.main.transform.position.z + transform.position.z;
        //    pointerDisplacement = -transform.position + MouseInWorldCoords();
        //}
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter to " + gameObject.name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("OnPointerExit to " + gameObject.name);
    }
}
