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

    Transform speedSpinCollider;

    [Header("speedSpinCollider")]
    public float speedSpinMaxScaleDist;
    float speedSpinCurScaleDist;
    Vector3 speedSpinMinScale, speedSpinMaxScale;

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
        speedSpinCollider = GameObject.Find("speedSpinCollider").transform;
        speedSpinMinScale = speedSpinCollider.localScale;
        speedSpinMaxScale = speedSpinMinScale * speedSpinMaxScaleDist;
        StartCoroutine(WalkSound());
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
        if (inputDirection.magnitude < 0.01f)
            locked = false;
    }

    [SerializeField]
    float distToEnd;

    Vector3 tangentalVector;
    bool locked = false;

    public bool Twisting;

    public AudioClip walkS;

    IEnumerator WalkSound()
    {
        float t = 0, maxT = 0.7f;
        while(true)
        {
            if(t >= maxT)
            {
                SoundManager.instance.PlaySound(walkS);
                t = 0f;
            }
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

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
            if(Twisting)
                maxVelocity = Mathf.MoveTowards(maxVelocity, maxMaxVelocity, Time.fixedDeltaTime * velocityGainLerp);
            else
                maxVelocity = Mathf.MoveTowards(maxVelocity, initMaxVelocity, Time.fixedDeltaTime * velocityGainLerp * 4f);
        }
        else
            maxVelocity = Mathf.MoveTowards(maxVelocity, initMaxVelocity, Time.fixedDeltaTime * velocityGainLerp * 4f);

        speedSpinCollider.localScale = Vector3.Lerp(speedSpinMinScale, speedSpinMaxScale, Mathf.InverseLerp(initMaxVelocity, maxMaxVelocity, maxVelocity));

        if (treeNode.AvailableRopeLength < 1f && Twisting)
        {
                velocity = Vector3.zero;
        }

        if (distToEnd < 0)
        {
            Vector3 pos = parent.position + ropeLengthVector.normalized * treeNode.AvailableRopeLength;
            rb.position = new Vector3(pos.x, rb.position.y, pos.z);
        }
        velocity *= maxVelocity;
        rb.velocity = new Vector3(velocity.x, rb.velocity.y,velocity.z);

        if (velocity.magnitude > 0.01f)
            lastRotationDir = velocity.normalized;
        rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.LookRotation(lastRotationDir, Vector3.up), rotationLerpValue);
    }
}
