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
        lastRotationDir = transform.forward;
    }


    Transform mainCam;
    private void Start()
    {
        mainCam = Camera.main.transform;
    }


    public Vector3 currentVelocity = Vector3.zero, lastRotationDir;

    public float anglesMultiplier;

    float hAxis, vAxis;

    void Update()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        currentVelocity = Vector3.Lerp(currentVelocity, Quaternion.LookRotation(Vector3.Cross(Vector3.up,Vector3.Cross(mainCam.forward, Vector3.up)), Vector3.up) * (new Vector3(hAxis, 0f, vAxis)) * maxVelocity, 0.3f);
        if (currentVelocity.magnitude > 0.1f)
            lastRotationDir = currentVelocity.normalized;
    }

    private void FixedUpdate()
    {
        //rb.rotation *= Quaternion.AngleAxis(hAxis * anglesMultiplier* Time.fixedDeltaTime, transform.up);
        //rb.velocity = transform.forward * vAxis * maxVelocity;
        rb.velocity = currentVelocity;
        rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.LookRotation(lastRotationDir, Vector3.up),0.4f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + currentVelocity);
    }
}
