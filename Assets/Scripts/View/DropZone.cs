using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Position dropZonePosition;
    public GameObject dropAreaImage;

    public void OnDrop(PointerEventData eventData)
    {

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        // allow drag if draggable object exists and dropzone belongs to player
        if (d != null && dropZonePosition == d.t_Reference.GetComponent<IDAssignment>().ownerPosition)// && this.transform.GetChild(0).GetChild(0) != d.parentToReturnTo)
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

    public void unlockUnitAttacks()
    {
        foreach (Transform child in dropAreaImage.transform)
        {
            // enable unit attack
            child.GetComponent<CardDisplayLoader>().cardUnitLoader.transform.GetComponent<Attackable>().enabled = true;
        }
    }

    public void blockAllUnitOperations()
    {
        foreach (Transform child in dropAreaImage.transform)
        {
            //TODO
            // block unit glow
            //child.GetComponent<CardDisplayLoader>().cardFaceGlowImage.enabled = false;

            // block unit attack
            child.GetComponent<CardDisplayLoader>().cardUnitLoader.transform.GetComponent<Attackable>().enabled = false;
        }
    }
}
