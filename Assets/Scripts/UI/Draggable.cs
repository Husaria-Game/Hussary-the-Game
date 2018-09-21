using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentToReturnTo = null;
    public Vector3 initialPosition;
    public Vector3 mousePosition;
    private GameObject cardPreview;
    public LineRenderer lineRenderer;
    public GameObject arrow;
    public bool dragSuccess;
    public DropZone initialDropZone;
    private Vector3 pointerDisplacement = Vector3.zero;
    private Transform t_Reference;

    public void OnBeginDrag(PointerEventData eventData)
    {
        //mousePosition.Set(0, 0, 0);
        parentToReturnTo = this.transform.parent;
        initialPosition = this.transform.position;
        pointerDisplacement = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - initialPosition.x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y - initialPosition.y, 0);
        cardPreview = this.gameObject.gameObject.transform.Find("Canvas").gameObject;
        

        //Debug.Log("SORTING: " + SortingLayer.GetLayerValueFromID(cardPreview.GetComponent<Canvas>().sortingLayerID));
        //Debug.Log("on BEGIN drag, this transform: " + this.transform.position);
        dragSuccess = false;
        //Debug.Log("on BEGIN drag INITIAL position: " + initialPosition);
        //Debug.Log("on BEGIN drag MOUSE position: " + Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)));
        //Debug.Log("on BEGIN drag PINTER DISPL position: " + pointerDisplacement);
        //Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //this.transform.SetParent(this.transform.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        if(lineRenderer != null)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, initialPosition);
        }
        if (arrow != null)
        {
            arrow.SetActive(true);
            //arrow.transform = new Vector3(initialPosition);
        }
        //LineRenderer lineRenderer = GetComponent<LineRenderer>();
        //var t = Time.time;
        //for (int i = 0; i < lengthOfLineRenderer; i++)
        //{
        
        //}
    }

    public void OnDrag(PointerEventData eventData)
    {
        //this.transform.position = eventData.position;
        //Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 pz = Camera.main.ScreenToWorldPoint(eventData.position);
        pz.z = 0;
        //gameObject.transform.position = pz;


        //Debug.Log("X: " + eventData.position.x + "     Y: " + eventData.position.y);
        //Debug.Log("X: " + this);
        //Debug.Log("XXX: " + pz);// Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 0)));//.x + "     YYYY: " + Camera.main.ScreenToWorldPoint(eventData.position).y);
        //this.transform.position = new Vector2(eventData.position.x - pointerDisplacement.x, eventData.position.y - pointerDisplacement.y);
        Debug.Log("line: " + lineRenderer);
        Debug.Log("arrow: " + arrow);
        if (lineRenderer != null)
        {
            Vector3 A = initialPosition;
            //Vector3 B = new Vector3(pz.x, pz.y, 0.0f);
            //float B = (pz.y - pointerDisplacement.y);

            Vector3 P = Vector3.Lerp(A, pz, 0.90f );
            //Debug.Log("A: " + A);
            //Debug.Log("B: " + B);
            //Debug.Log("P: " + P);
            lineRenderer.SetPosition(1, new Vector2((P.x), (P.y)));
        }
        else
        {
            this.transform.position = new Vector2(pz.x - pointerDisplacement.x, pz.y - pointerDisplacement.y);
        }
        if (arrow != null)
        {
            arrow.transform.position = new Vector2(pz.x , pz.y );
            //arrow.transform.SetPositionAndRotation(initialPosition,);
            //float myAngle = Mathf.Atan2(eventData.position.y - initialPosition.y, eventData.position.x - initialPosition.x) * Mathf.Rad2Deg;
            //Debug.Log("myAngle: " + myAngle);
            //arrow.transform.eulerAngles = new Vector3(0, 0, myAngle);

            //t_Reference.eulerAngles = new Vector3(0, arrow.transform.eulerAngles.y, 0); // only Y axis, meaning right & left turns.

            //Vector3 v3_Dir = arrow.transform.position - initialPosition;
            //float f_AngleBetween = Vector3.Angle(t_Reference.forward, v3_Dir); // angle between the rotating object's forward , but only affected by Y axis, and the direction from origin to rotating object.

            //Debug.Log(f_AngleBetween);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //this.transform.SetParent(parentToReturnTo);
        Debug.Log("on END drag, parent: " + this.transform.parent);
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
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }
        if (arrow != null)
        {
            arrow.SetActive(false);
        }
    }
}
