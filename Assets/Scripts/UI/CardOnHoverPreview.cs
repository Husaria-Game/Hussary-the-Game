using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardOnHoverPreview : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject previewGameObject;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("OnPointerEnter to " + gameObject.name);
        previewGameObject.SetActive(true);

        //gameObject.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("OnPointerExit to " + gameObject.name);
        previewGameObject.SetActive(false);
        //gameObject.SetActive(true);
    }
}
