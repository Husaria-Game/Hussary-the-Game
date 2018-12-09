using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Vuforia;

public class CardOnHoverPreview : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
{
    public GameObject previewGameObject;
    private static CardOnHoverPreview cardInPreview = null;
    private bool _previewPositionAdjusted;
    private Vector3 _previewInitialPosition;

    private void Start()
    {
        _previewPositionAdjusted = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            _previewInitialPosition = transform.position;
            if (cardInPreview != null)
            {
                cardInPreview.previewGameObject.SetActive(false);
            }
            cardInPreview = this;
            previewGameObject.SetActive(!previewGameObject.activeSelf);
            previewGameObject.GetComponentInChildren<Canvas>().overrideSorting = true;
            previewGameObject.GetComponentInChildren<Canvas>().sortingLayerName = "ActiveCard";
            if (transform.position.y < -2f)
            {
                previewGameObject.transform.position += new Vector3(0,1f,0);
                _previewPositionAdjusted = true;
            }
            if (transform.position.y > 3f)
            {
                previewGameObject.transform.position += new Vector3(0,-1.5f,0);
                _previewPositionAdjusted = true;
            }

        }
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (cardInPreview != null)
            {
                previewGameObject.GetComponentInChildren<Canvas>().overrideSorting = false;
                cardInPreview.previewGameObject.SetActive(false);

            }
            if (_previewPositionAdjusted)
            {
                previewGameObject.transform.position = _previewInitialPosition;
                _previewPositionAdjusted = false;
            }
        }
    }
}
