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
        //Debug.Log("New: " + this.transform + "      Old: " + d.parentToReturnTo);
        if(d != null && this.transform != d.parentToReturnTo)
        {
            d.dragSuccess = true;
            d.parentToReturnTo = this.transform;
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
