using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeGod : Node
{
    public float initMaxVelocity, maxMaxVelocity, velocityCorrectingRandMargin, velocityGainLerp,  velocityChangeLerpValue = 0.3f, rotationLerpValue = 0.3f;
    public Transform rotationSpineBone;
    public Rigidbody rb;

    public float maxVelocity;

    Transform mainCam;

    float hAxis, vAxis;

    RopeSystem rope;

    StaticNode treeNode;

    Vector3 inputDirection = Vector3.zero, lastRotationDir;

    //public float availableDistToEndRope
    //{
    //    get { return treeNode.AvailableRopeLength - treeNode.CurrentRopeLength.magnitude; }
    //    private set { }
    //}


    public static RopeGod instance;
    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
        SetParent(parent);
        lastRotationDir = transform.forward;
        maxVelocity = initMaxVelocity;
    }

    void Start()
    {
        mainCam = Camera.main.transform;
        rope = RopeSystem.instnace;
    }

    public override void SetParent(Node parent)
    {
        base.SetParent(parent);
        treeNode = parent as StaticNode;
    }

    void Update()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        inputDirection = Vector3.Lerp(inputDirection, Quaternion.LookRotation(Vector3.Cross(Vector3.up, Vector3.Cross(mainCam.forward, Vector3.up)), Vector3.up) * (new Vector3(hAxis, 0f, vAxis)), velocityChangeLerpValue);
        if (inputDirection.magnitude > 0.01f)
            lastRotationDir = inputDirection.normalized;
        if (inputDirection.magnitude < 0.01f)
            locked = false;
    }

    [SerializeField]
    float distToEnd;

    Vector3 tangentalVector;
    bool locked = false;

    private void FixedUpdate()
    {
        Vector3 ropeLengthVector = transform.position - parent.position;
        Vector3 newtangentalVector = Vector3.Cross(ropeLengthVector, Vector3.up).normalized;
        if (Vector3.Dot(newtangentalVector, locked? tangentalVector :  rb.velocity) < 0)
            newtangentalVector *= -1f;
        if (Vector3.Dot(newtangentalVector, tangentalVector) < 0f)
            maxVelocity = initMaxVelocity;
        tangentalVector = newtangentalVector;

        Vector3 velocity = inputDirection;

        distToEnd = treeNode.AvailableRopeLength - ropeLengthVector.magnitude;
        //if (distToEnd <= 0f)
        //{
        //    locked = true;
        //    velocity = tangentalVector;
        //}
        //else 
        if (distToEnd < velocityCorrectingRandMargin && !(Vector3.Dot(-ropeLengthVector, velocity) > 0))
        {
            float t = distToEnd / velocityCorrectingRandMargin;
            velocity = Vector3.Lerp(velocity, tangentalVector, 1 - t).normalized * velocity.magnitude;
            maxVelocity = Mathf.Lerp(maxVelocity, maxMaxVelocity, velocityGainLerp);
        }
        else
            maxVelocity = Mathf.Lerp(maxVelocity, initMaxVelocity, 0.7f);

        if (treeNode.AvailableRopeLength < 1f)
        {
            maxVelocity = initMaxVelocity;
            Vector3 cross = Vector3.Cross(tangentalVector, ropeLengthVector);
            if (cross.y > 0f && treeNode.clockDirectionLeft || cross.y < 0f && !treeNode.clockDirectionLeft)
                velocity = Vector3.zero;
        }

        if (distToEnd < 0)
            rb.position = parent.position + ropeLengthVector.normalized * treeNode.AvailableRopeLength;
        rb.velocity = velocity * maxVelocity;
        rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.LookRotation(velocity, Vector3.up), rotationLerpValue);
    }
}
