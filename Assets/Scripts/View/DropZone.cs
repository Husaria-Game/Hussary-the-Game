using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public Position dropZonePosition;
    public GameObject dropAreaImage;
    public bool dropEventOccurs = false;


    void Update()
    {
        if (dropEventOccurs)
        {
            Debug.Log("dropEvent on " + this.dropZonePosition);
            foreach (Transform child in dropAreaImage.transform)
            {
                child.GetComponent<Draggable>().enabled = false;
                child.GetComponent<Attackable>().enabled = true;
                child.GetComponent<Defendable>().enabled = true;
            }

            dropEventOccurs = false;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        // allow drag if draggable object exists and dropzone belongs to player
        if (d != null && dropZonePosition == d.t_Reference.GetComponent<IDAssignment>().ownerPosition)// && this.transform.GetChild(0).GetChild(0) != d.parentToReturnTo)
        {
            d.dropZone = this;
            d.dragSuccess = true;
            d.parentToReturnTo = this.transform.GetChild(0).GetChild(0);
        }
    }

    public void unlockUnitAttacks()
    {
        foreach (Transform child in dropAreaImage.transform)
        {
            // enable unit attack
            //Debug.Log("UNLOCK: " + child.GetComponent<CardDisplayLoader>().cardUnitLoader.nameText.text + " " + child.GetComponent<CardDisplayLoader>().Unit.GetComponent<Attackable>().enabled);
            //child.GetComponent<CardDisplayLoader>().Unit.GetComponent<Attackable>().enabled = true;
            //Debug.Log("     then : " + child.GetComponent<CardDisplayLoader>().cardUnitLoader.nameText.text + " " + child.GetComponent<CardDisplayLoader>().Unit.GetComponent<Attackable>().enabled);
            child.GetComponent<Attackable>().enabled = true;
            //child.GetComponent<CardDisplayLoader>().Unit.GetComponent<Attackable>().transform.GetComponentInChildren<LineRenderer>().enabled = true;
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
            //Debug.Log("BLOCK:::: " + child.GetComponent<CardDisplayLoader>().cardUnitLoader.nameText.text + " " + child.GetComponent<CardDisplayLoader>().Unit.GetComponent<Attackable>().enabled);
            //child.GetComponent<CardDisplayLoader>().Unit.GetComponent<Attackable>().enabled = false;
            //child.GetComponent<CardDisplayLoader>().Unit.GetComponent<Attackable>().transform.GetComponentInChildren<LineRenderer>().enabled = false;
            //Debug.Log("       then 2:::: " + child.GetComponent<CardDisplayLoader>().cardUnitLoader.nameText.text + " " + child.GetComponent<CardDisplayLoader>().Unit.GetComponent<Attackable>().enabled);
            child.GetComponent<Attackable>().enabled = false;
        }
    }
}
