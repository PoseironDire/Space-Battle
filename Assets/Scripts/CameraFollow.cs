using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform cameraPosition;
    public Transform sight;
    Vector3 velocity = new Vector3();

    void Start()
    {

    }

    void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, cameraPosition.position, ref velocity, 0.001f);
        transform.rotation = Quaternion.Lerp(transform.rotation, sight.rotation, 0.75f);
    }
}
