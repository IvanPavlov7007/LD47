using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogControl : MonoBehaviour
{
    Rigidbody rb;
    public float maxVelocity;
    //public Transform root;


    public static DogControl instance;


    void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = Vector3.zero;
        rb.inertiaTensorRotation = Quaternion.identity;
    }



    public Vector3 velocity;

    public float anglesMultiplier;

    float hAxis, vAxis;

    void Update()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        rb.rotation *= Quaternion.AngleAxis(hAxis * anglesMultiplier* Time.fixedDeltaTime, transform.up);
        rb.velocity = transform.forward * vAxis * maxVelocity;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + velocity);
    }
}
