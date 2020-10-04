using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticNode : Node
{
    public CableProceduralSimple rope;

    public float radius = 0.5f;

    public bool clockDirectionLeft = true;
    float exitUpDir, deltaLengthCoefficientAngles;

    [SerializeField]
    float maxAvailableRopeLength, currentAvailableRopeLength;

    public float AvailableRopeLength
    {
        get { return currentAvailableRopeLength; }
        private set { }
    }

    [SerializeField]
    Vector3 ropeLengthVector;

    public Vector3 CurrentRopeLength
    {
        get { return ropeLengthVector; }
        private set { ropeLengthVector = value; }
    }

    public void AllotRopeLength(float length)
    {
        maxAvailableRopeLength = length;
        currentAvailableRopeLength = length;
        CurrentRopeLength = child.position - transform.position;
    }


    //360 Deg
    const float fullDeg = 0.0027777f;


    public virtual void Start()
    {
        if(rope == null)
            rope = Instantiate(GameManager.instance.RopeDrawerPrefab, transform).GetComponent<CableProceduralSimple>();
        if (child != null)
            rope.endPointTransform = child.transform;
        rope.sagAmplitude = 0f;
        rope.gameObject.SetActive(true);
    }

    public virtual void Awake()
    {
        deltaLengthCoefficientAngles = 2 * Mathf.PI * radius * fullDeg;
    }

    private void Update()
    {
        
    }


    public void DisconnectFromRope()
    {
        parent.SetChild(child);
        child.SetParent(parent);
        Destroy(rope.gameObject);
        Destroy(this);
    }
    float dif;
    private void FixedUpdate()
    {
        Vector3 newRopeLengthVector = child.position - transform.position;
        dif = Vector3.SignedAngle(ropeLengthVector, newRopeLengthVector, transform.up) * deltaLengthCoefficientAngles;
        dif *= clockDirectionLeft ? 1f : -1f;

        currentAvailableRopeLength = currentAvailableRopeLength + dif;

        //Check Rotation direction and Disconnect from rope
        if(currentAvailableRopeLength > maxAvailableRopeLength)
        {
            if (parent != null)
            {
                DisconnectFromRope();
                return;
            }
            else
                clockDirectionLeft = !clockDirectionLeft;
        }

        RaycastHit hit;
        Ray r = new Ray(transform.position, newRopeLengthVector);
        if (Physics.Raycast(r, out hit, newRopeLengthVector.magnitude))
        {
            Transform t = hit.transform;
            Node n = t.GetComponent<Node>();
            if (t.tag == "Node" && n != child && n != this)
                CreateNewStaticNode(t, dif < 0); 
            else if (t.GetComponent<RopeGod>() == null)
                PushObject(hit);

        }

        //if(newRopeLengthVector.magnitude > currentAvailableRopeLength &&  child is RopeGod)
        //{
        //    (child as RopeGod).rb.position = transform.position + newRopeLengthVector.normalized * currentAvailableRopeLength;
        //}

        ropeLengthVector = newRopeLengthVector;
    }

    void PushObject(RaycastHit hit)
    {
        var enemy = hit.transform.GetComponent<Enemy>();
        Debug.DrawLine(hit.point, hit.point + hit.normal * 10f);
        if (enemy != null)
        {
            enemy.rb.velocity = Vector3.zero;
            enemy.rb.AddForce(-hit.normal * 10f * enemy.rb.mass, ForceMode.Impulse);
        }
    }

    public override void SetChild(Node child)
    {
        base.SetChild(child);
        if(rope != null)
            rope.endPointTransform = child.transform;
    }

    void CreateNewStaticNode(Transform t, bool clockDirectionLeft)
    {
        StaticNode n = t.gameObject.AddComponent<StaticNode>();
        n.clockDirectionLeft = clockDirectionLeft;
        child.SetParent(n);
        n.SetParent(this);
        n.SetChild(child);
        n.AllotRopeLength(maxAvailableRopeLength - (position - n.position).magnitude);
        SetChild(n);
    }
}
