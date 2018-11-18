using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class CardRotation : MonoBehaviour {
    

    [SerializeField]
    private GameObject cardFace;

    [SerializeField]
    private GameObject cardBack;

    protected void Update()
    {
        bool back = Vector3.Dot(Camera.main.transform.forward, transform.forward) < 0;
        
        cardFace.SetActive(!back);
        cardBack.SetActive(back);
    }
}
