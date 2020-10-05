using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeGod : Node
{
    public float initMaxVelocity, maxMaxVelocity, velocityCorrectingRandMargin, velocityGainLerp,  velocityChangeLerpValue = 0.3f, rotationLerpValue = 0.3f;
    public Transform SpineTop, Neck, lookTarget;
    public bool lockOnTarget = false;
    [HideInInspector]
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
    Animator anim;
    int speedAnim = Animator.StringToHash("Speed");
    int idleTimeAnim = Animator.StringToHash("IdleTime");
    int idleAnim = Animator.StringToHash("isIdle");
    [Header("Dog animation"), SerializeField]
    float maxIdleTime = 10f;
    float curIdleTime;


    Transform speedSpinCollider;

    [Header("speedSpinCollider")]
    public float speedSpinMaxScaleDist;
    float speedSpinCurScaleDist;
    Vector3 speedSpinMinScale, speedSpinMaxScale;

    private void Awake()
    {
        instance = this;
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        SetParent(parent);
        lastRotationDir = transform.forward;
        maxVelocity = initMaxVelocity;
    }

    void Start()
    {
        mainCam = Camera.main.transform;
        rope = RopeSystem.instnace;
        //speedSpinCollider = GameObject.Find("speedSpinCollider").transform;
        //speedSpinMinScale = speedSpinCollider.localScale;
        //speedSpinMaxScale = speedSpinMinScale * speedSpinMaxScaleDist;
        StartCoroutine(WalkSound());
    }

    public override void SetParent(Node parent)
    {
        base.SetParent(parent);
        treeNode = parent as StaticNode;
    }

    Vector3 TargetDir;

    bool idle;

    bool ableToMove = true;
    void Update()
    {

        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        inputDirection = Vector3.Lerp(inputDirection, Quaternion.LookRotation(Vector3.Cross(Vector3.up, Vector3.Cross(mainCam.forward, Vector3.up)), Vector3.up) * (new Vector3(hAxis, 0f, vAxis)), velocityChangeLerpValue);
        if (inputDirection.magnitude < 0.01f)
        {
            idle = true;
            locked = false;
        }
        else
        {
            idle = false;
            curIdleTime = 0f;
        }

        if(idle)
        {
            curIdleTime += Time.deltaTime;
        }

        ableToMove = anim.GetCurrentAnimatorStateInfo(0).IsName("GettingUp");

        anim.SetBool(idleAnim, idle);
        anim.SetFloat(idleTimeAnim, curIdleTime / maxIdleTime);
        anim.SetFloat(speedAnim, inputDirection.magnitude);
    }

    private void LateUpdate()
    {

        //Neck.rotation *= Quaternion.Euler(0,0,Vector3.Angle);

        SpineTop.transform.rotation *= Quaternion.Euler(Mathf.Lerp(0f, -25f, howNearEnd * inputDirection.magnitude),
            Mathf.Clamp(Vector3.SignedAngle(inputDirection, ropeLengthVector, Vector3.up) * inputDirection.magnitude * howNearEnd , - 45f,45f), 0f);//* (Vector3.Dot(inputDirection,lastRotationDir) > 0? 1f: -1f)
                                                                                                                                                    //* Quaternion.Lerp(Quaternion.identity, Quaternion.LookRotation(ropeLengthVector,Vector3.up), howNearEnd);
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

    float howNearEnd = 0f;

    Vector3 ropeLengthVector;
    private void FixedUpdate()
    {
        ropeLengthVector = transform.position - parent.position;
        Vector3 newtangentalVector = Vector3.Cross(ropeLengthVector, Vector3.up).normalized;
        if (Vector3.Dot(newtangentalVector, locked? tangentalVector :  rb.velocity) < 0)
            newtangentalVector *= -1f;
        if (Vector3.Dot(newtangentalVector, tangentalVector) < 0f)
            maxVelocity = initMaxVelocity;
        tangentalVector = newtangentalVector;

        TargetDir = lookTarget.position - transform.position;

        Vector3 velocity = inputDirection;

        distToEnd = treeNode.AvailableRopeLength - ropeLengthVector.magnitude;
        //if (distToEnd <= 0f)
        //{
        //    locked = true;
        //    velocity = tangentalVector;
        //}
        //else 
        howNearEnd = Mathf.Max(0f, 1f - distToEnd / velocityCorrectingRandMargin);
        //if (distToEnd < velocityCorrectingRandMargin && !(Vector3.Dot(-ropeLengthVector, velocity) > 0))
        //{
        //    velocity = Vector3.Lerp(velocity * velocity.magnitude, tangentalVector, howNearEnd).normalized ;
        //    if(Twisting)
        //        maxVelocity = Mathf.MoveTowards(maxVelocity, maxMaxVelocity, Time.fixedDeltaTime * velocityGainLerp);
        //    else
        //        maxVelocity = Mathf.MoveTowards(maxVelocity, initMaxVelocity, Time.fixedDeltaTime * velocityGainLerp * 4f);
        //}
        //else
        //    maxVelocity = Mathf.MoveTowards(maxVelocity, initMaxVelocity, Time.fixedDeltaTime * velocityGainLerp * 4f);

        //speedSpinCollider.localScale = Vector3.Lerp(speedSpinMinScale, speedSpinMaxScale, Mathf.InverseLerp(initMaxVelocity, maxMaxVelocity, maxVelocity));

        if (!ableToMove || (treeNode.AvailableRopeLength < 1f && Twisting))
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

        if (lockOnTarget)
            lastRotationDir = TargetDir;
        else if (velocity.magnitude > 0.01f)
            lastRotationDir = velocity.normalized;
        rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.LookRotation(lastRotationDir, Vector3.up), rotationLerpValue);
    }
}
