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

    public GameObject missilePrefab;
    public Transform leftFrontCanon;
    public Transform rightFrontCanon;

    public AudioSource engine;

    public float frontbackForce = 50;
    public float leftRightForce = 50;
    public float upDownForce = 50;
    public float sightSpeed = 1;

    public float useTime = 0.5f;
    float useTimer = 0;
    bool firstFired = false;

    bool hoverMode = false;
    bool switchedMode = false;

    public Vector3 localVelocity;
    public Quaternion orientation = Quaternion.identity;
    Vector3 turnSmoothing = Vector3.zero;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        localVelocity = transform.InverseTransformDirection(rb.velocity); //Get Local Velocity

        lookInput.x = Mathf.Clamp(lookInput.x, -1, 1); //Clamp Pointer Input X
        lookInput.y = Mathf.Clamp(lookInput.y, -1, 1); //Clamp Pointer Input Y

        useTimer += Time.deltaTime;

        ModeShift();
        Use();
    }

    void HoverMode()
    {
        //Movement
        rb.AddForce((moveInput.x * transform.right * leftRightForce + moveInput.y * transform.forward * frontbackForce), ForceMode.Acceleration); //Add Force X & Z Axis

        rb.AddForce((Mathf.Max(0, liftInput.y) * transform.up * upDownForce), ForceMode.Acceleration); //Add Force Y Axis                                                                     
        rb.AddForce((Mathf.Min(0, liftInput.y) * transform.up * upDownForce), ForceMode.Acceleration); //Add Force Y Axis

        float maxPitch = 3; //Engine Pitch Scaler
        engine.pitch = Mathf.Lerp(engine.pitch, Mathf.Min(Mathf.Abs(moveInput.x * maxPitch) + Mathf.Abs(moveInput.y * maxPitch), maxPitch) / 2 + 1, 0.1f);

        //Sight
        transform.rotation = Quaternion.Euler(this.orientation.x, transform.eulerAngles.y, this.orientation.y); //Stabilize X & Z Rotation

        turnSmoothing.x = Mathf.Lerp(turnSmoothing.x, lookInput.x * sightSpeed, 0.1f); //Smoothen Input
        transform.rotation *= Quaternion.Euler(0, turnSmoothing.x, 0); //Rotate The Y Axis

        //Ship Tilt Effect
        Vector3 tiltForce = new Vector3();
        tiltForce.z = (-lookInput.x * 20) + localVelocity.x * -0.1f;
        tiltForce.x = (-lookInput.y * 20) + localVelocity.z * 0.1f;
        Quaternion tilt = Quaternion.Euler(tiltForce);
        ship.localRotation = Quaternion.Lerp(ship.localRotation, tilt, 0.1f);
    }

    void FlyMode()
    {
        rb.AddForce(transform.forward * frontbackForce, ForceMode.Acceleration);

        turnSmoothing = new Vector3(Mathf.Lerp(turnSmoothing.x, moveInput.y * sightSpeed, 0.1f), Mathf.Lerp(turnSmoothing.y, moveInput.x * sightSpeed, 0.1f), Mathf.Lerp(turnSmoothing.z, -liftInput.y * sightSpeed, 0.1f)); //Smoothen Input
        transform.rotation *= Quaternion.Euler(turnSmoothing); //Rotate The Y Axis
    }

    void Use()
    {
        if (useInput)
        {
            if (useTimer > useTime / 2 && !firstFired)
            {
                GameObject missile = Instantiate(missilePrefab, rightFrontCanon.transform.position, rightFrontCanon.transform.rotation); /**/ missile.transform.SetParent(transform.parent); //Spawn & Make The Projectile A Child Of This Game Object 
                foreach (var collider in GetComponentsInChildren<Collider>())
                {
                    Physics.IgnoreCollision(collider, missile.GetComponentInChildren<Collider>());
                }
                firstFired = true;
            }
            if (useTimer > useTime)
            {
                GameObject missile = Instantiate(missilePrefab, leftFrontCanon.transform.position, leftFrontCanon.transform.rotation); /**/ missile.transform.SetParent(transform.parent); //Spawn & Make The Projectile A Child Of This Game Object 
                foreach (var collider in GetComponentsInChildren<Collider>())
                {
                    Physics.IgnoreCollision(collider, missile.GetComponentInChildren<Collider>());
                }
                useTimer = 0; /**/ firstFired = false;
            }
        }
    }

    void ModeShift()
    {
        if (toggleInput)
        {
            if (!switchedMode)
            {
                hoverMode = !hoverMode;
                switchedMode = true;
            }
        }
        else switchedMode = false;

        if (hoverMode) HoverMode(); else FlyMode();
    }
}
