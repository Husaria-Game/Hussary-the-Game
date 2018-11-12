using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardOnHoverPreview : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
{
    public GameObject previewGameObject;
    private static CardOnHoverPreview cardInPreview = null;

     public void OnPointerDown(PointerEventData eventData)
    {
        ////disable all other previews
        //if(cardInPreview != null)
        //{
        //    cardInPreview.previewGameObject.SetActive(false);
        //}
        //cardInPreview = this;

        //if (eventData.button == PointerEventData.InputButton.Right)
        //    previewGameObject.SetActive(!previewGameObject.activeSelf);
        ////if (eventData.button == PointerEventData.InputButton.Left)
        ////    Debug.Log("Left click");
        ////else if (eventData.button == PointerEventData.InputButton.Middle)
        ////    Debug.Log("Middle click");
        ////else if (eventData.button == PointerEventData.InputButton.Right)
        ////    previewGameObject.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (cardInPreview != null)
            {
                cardInPreview.previewGameObject.SetActive(false);
            }
            cardInPreview = this;
            previewGameObject.SetActive(!previewGameObject.activeSelf);
        }
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (cardInPreview != null)
            {
                cardInPreview.previewGameObject.SetActive(false);
            }
        }
    }
}
