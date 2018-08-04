using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class CardRotation : MonoBehaviour {

    //public float moveSpeed = 2f;
    //public float turnSpeed = 100f;

    //// Use this for initialization
    //void Start()
    //{

    //}

    //void Update()
    //{
    //    if (Input.GetMouseButton(0))
    //    {
    //        //transform.Rotate(new Vector3(2, 1, 0) * turnSpeed * Time.deltaTime);
    //        //transform.Translate(new Vector3(2, 1, 0) * moveSpeed * Time.deltaTime);
    //        Vector3 b = transform.forward;
    //        Debug.Log("position " + (transform.position));
    //        Debug.Log("rotation " + (transform.rotation));
    //    }
    //    if (Input.GetMouseButton(1))
    //    {
    //        transform.Rotate(new Vector3(2, 1, 0) * -turnSpeed * Time.deltaTime);
    //        transform.Translate(new Vector3(-2, -1, 0) * moveSpeed * Time.deltaTime);
    //    }

    //}

    [SerializeField]
    private GameObject cardFace;

    [SerializeField]
    private GameObject cardBack;

    protected void Update()
    {
        //// camera position relative to object
        //Vector3 a = (Camera.main.transform.position - transform.position);
        //// moving object backwards
        //Vector3 b = -transform.forward;
        //float rotAngle = (Vector3.Dot(a, b) / (a.magnitude * b.magnitude)) * Mathf.Rad2Deg;
        bool back = Vector3.Dot(Camera.main.transform.forward, transform.forward) < 0;
        //bool back = rotAngle < 0;
        cardFace.SetActive(!back);
        cardBack.SetActive(back);
        //Debug.Log("CAMERA" + Camera.main.transform.position);
        //Debug.Log("OBJECT" + (-transform.position));
        //Debug.Log("SUM" + (Camera.main.transform.position - transform.position));
        //Debug.Log("OBJECT forward" + (transform.forward));
        //Debug.Log("A Magnitude " + a.magnitude);
        //Debug.Log("B Magnitude " + b.magnitude);
        //Debug.Log("A * B Magnitude " + (a.magnitude * b.magnitude));
        //Debug.Log("DOT " + (Vector3.Dot(a, b)));
        //Debug.Log("DOT_2 " + (Vector3.Dot(Camera.main.transform.forward, transform.forward)));
        //Debug.Log("------------------------------------------------");
    }
}
