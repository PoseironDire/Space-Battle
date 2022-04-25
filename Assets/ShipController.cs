using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : Controller
{
    public Camera cam;
    public Rigidbody rb;

    public float moveForce = 50;

    void Start()
    {

    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        rb.AddForce((moveInput.x * transform.right + moveInput.y * transform.forward) * moveForce);
    }
}
