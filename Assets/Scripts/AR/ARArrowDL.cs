using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARArrow : MonoBehaviour {

    float speed = 0f;
    public Transform target;
    private Rigidbody rb;

    // Use this for initialization
    void Start() {
        speed = 0.01f;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }

    private void OnTriggerEnter(Collider other)
    {

    }

}