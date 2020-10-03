using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogControl : MonoBehaviour
{
    Rigidbody rb;
    public float maxVelocity;
    //public Transform root;

    void Awake()
    {
        //rb = GetComponent<Rigidbody>();
        //rb.inertiaTensorRotation = Quaternion.identity;
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
        transform.Rotate(transform.up, hAxis * anglesMultiplier* Time.fixedDeltaTime);
        transform.position += transform.forward * vAxis * maxVelocity * Time.fixedDeltaTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + velocity);
    }
}
