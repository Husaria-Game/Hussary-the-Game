using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnDrop(PointerEventData eventData)
    {

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null)// && this.transform.GetChild(0).GetChild(0) != d.parentToReturnTo)
        {
            d.dragSuccess = true;
            d.parentToReturnTo = this.transform.GetChild(0).GetChild(0);
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
