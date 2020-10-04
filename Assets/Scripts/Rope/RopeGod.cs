using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeGod : Node
{
    public float InputDirectionMultiplier, velocityCorrectingRandMargin,  velocityChangeLerpValue = 0.3f, rotationLerpValue = 0.3f;
    public Transform rotationSpineBone;
    public Rigidbody rb;

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
        inputDirection = Vector3.Lerp(inputDirection, Quaternion.LookRotation(Vector3.Cross(Vector3.up, Vector3.Cross(mainCam.forward, Vector3.up)), Vector3.up) * (new Vector3(hAxis, 0f, vAxis)) * InputDirectionMultiplier, velocityChangeLerpValue);
        if (inputDirection.magnitude > 0.01f)
            lastRotationDir = inputDirection.normalized;
    }

    [SerializeField]
    float distToEnd;

    private void FixedUpdate()
    {
        Vector3 ropeLengthVector = transform.position - parent.position;
        Vector3 tangentalVector = Vector3.Cross(ropeLengthVector, Vector3.up);
        if (Vector3.Dot(tangentalVector, inputDirection) < 0)
            tangentalVector *= -1f;

        Vector3 velocity = inputDirection;

        distToEnd = treeNode.AvailableRopeLength - ropeLengthVector.magnitude;
        if (distToEnd < velocityCorrectingRandMargin && !(Vector3.Dot(-ropeLengthVector,velocity) > 0))
        {
            float t = distToEnd / velocityCorrectingRandMargin;
            velocity = Vector3.Lerp(velocity, tangentalVector,1 - t).normalized * velocity.magnitude;
        }

        if (treeNode.AvailableRopeLength < 2)
        {
            Vector3 cross = Vector3.Cross(tangentalVector, ropeLengthVector);
            if (cross.y > 0f && treeNode.clockDirectionLeft || cross.y < 0f && !treeNode.clockDirectionLeft)
                velocity = Vector3.zero;
        }

        if (distToEnd < 0)
            rb.position = parent.position + ropeLengthVector.normalized * treeNode.AvailableRopeLength;
        rb.velocity = velocity;
        rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.LookRotation(lastRotationDir, Vector3.up), rotationLerpValue);
    }
}
