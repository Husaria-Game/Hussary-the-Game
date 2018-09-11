using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardOnHoverPreview : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject previewGameObject;
    private static CardOnHoverPreview cardInPreview = null;

     public void OnPointerDown(PointerEventData eventData)
    {
        //disable all other previews
        if(cardInPreview != null)
        {
            cardInPreview.previewGameObject.SetActive(false);
        }
        cardInPreview = this;

        if (eventData.button == PointerEventData.InputButton.Right)
            previewGameObject.SetActive(!previewGameObject.activeSelf);
        //if (eventData.button == PointerEventData.InputButton.Left)
        //    Debug.Log("Left click");
        //else if (eventData.button == PointerEventData.InputButton.Middle)
        //    Debug.Log("Middle click");
        //else if (eventData.button == PointerEventData.InputButton.Right)
        //    previewGameObject.SetActive(true);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("OnPointerEnter to " + gameObject.name);
        //Debug.Log("OnPointerEnter cardInPreview " + this);
        //if (cardInPreview != null)
        //{
        //    cardInPreview.previewGameObject.SetActive(false);
        //}
        //cardInPreview = this;
        //previewGameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("OnPointerExit to " + gameObject.name);
        //if (cardInPreview != null)
        //{
        //    cardInPreview.previewGameObject.SetActive(false);
        //}
        //Debug.Log("OnPointerExit cardInPreview " + this);
    }
}
