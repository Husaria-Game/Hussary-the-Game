using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("OnDrop to " + gameObject.name);
        Debug.Log(eventData.pointerDrag.name + "was dropped to " + gameObject.name);
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if(d != null)
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
