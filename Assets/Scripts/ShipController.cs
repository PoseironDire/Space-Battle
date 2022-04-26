using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipController : Controller
{
    public Camera cam;
    public Rigidbody rb;
    public Transform ship;
    public Transform groundCheck;
    public AudioSource engine;

    public float frontbackForce = 50;
    public float leftRightForce = 50;
    public float upDownForce = 50;
    public float sightSpeed = 1;

    public float useTime = 0.5f;

    Quaternion orientation = Quaternion.identity;
    float turnSmoothing = 0;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        lookInput.x = Mathf.Clamp(lookInput.x, -1, 1); //Clamp Pointer Input X
        lookInput.y = Mathf.Clamp(lookInput.y, -1, 1); //Clamp Pointer Input Y

        Movement();
        Sight();

        Animation();
    }

    void Movement()
    {
        rb.AddForce((moveInput.x * transform.right * leftRightForce + moveInput.y * transform.forward * frontbackForce), ForceMode.Acceleration); //Add Force X & Z Axis

        rb.AddForce((Mathf.Max(0, liftInput.y) * transform.up * upDownForce), ForceMode.Acceleration); //Add Force Y Axis                                                                     
        rb.AddForce((Mathf.Min(0, liftInput.y) * transform.up * upDownForce), ForceMode.Acceleration); //Add Force Y Axis

        float maxPitch = 3; //Engine Pitch Scaler
        engine.pitch = Mathf.Lerp(engine.pitch, Mathf.Min(Mathf.Abs(moveInput.x * maxPitch) + Mathf.Abs(moveInput.y * maxPitch), maxPitch) / 2 + 1, 0.1f);
    }

    void Sight()
    {
        transform.rotation = Quaternion.Euler(this.orientation.x, transform.eulerAngles.y, this.orientation.y); //Stabilize X & Z Rotation

        turnSmoothing = Mathf.Lerp(turnSmoothing, lookInput.x * sightSpeed, 0.1f); //Smoothen Input
        transform.rotation *= Quaternion.Euler(0, turnSmoothing, 0); //Rotate The Y Axis
    }

    void Use()
    {

    }

    void Animation()
    {
        var localVelocity = transform.InverseTransformDirection(rb.velocity); //Get Local Velocity

        //Ship Tilt Effect
        Vector3 tiltForce = new Vector3();
        tiltForce.z = (-lookInput.x * 20) + localVelocity.x * -0.1f;
        tiltForce.x = (lookInput.y * 20) + localVelocity.z * 0.1f;
        Quaternion tilt = Quaternion.Euler(tiltForce);
        ship.localRotation = Quaternion.Lerp(ship.localRotation, tilt, 0.1f);
    }
}
