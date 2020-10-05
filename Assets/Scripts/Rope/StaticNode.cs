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


    protected virtual void Start()
    {
        if(rope == null)
            rope = Instantiate(GameManager.instance.RopeDrawerPrefab, transform).GetComponent<CableProceduralSimple>();
        var cP = transform.Find("ConnectionPoint");
        if (cP != null)
        {
            cP.GetComponentInChildren<MeshRenderer>().enabled = true;
            connectionPosition = cP;
            rope.transform.position = connectionPosition.position;
            if (parent != null)
                (parent as StaticNode).UpdateRopeEndTransform();
        }
        UpdateRopeEndTransform();
        rope.sagAmplitude = 0f;
        rope.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        connectionPosition.GetComponentInChildren<MeshRenderer>().enabled = false;
    }

    public void UpdateRopeEndTransform()
    {
        if (child != null && rope!= null)
        {
            if (child.connectionPosition != null)
                rope.endPointTransform = child.connectionPosition;
            else
                rope.endPointTransform = child.transform;
        }
    }

    protected virtual void Awake()
    {
        deltaLengthCoefficientAngles = 2 * Mathf.PI * radius * fullDeg;
    }

    void Update()
    {
        if (rope != null && child != null)
        {
            rope.sagAmplitude = 1f - ropeLengthVector.magnitude / currentAvailableRopeLength;
        }
    }


    public void DisconnectFromRope()
    {
        parent.SetChild(child);
        child.SetParent(parent);
        Destroy(rope.gameObject);
        Destroy(this);
    }
    

    float dif,clockDirDif;
    private void FixedUpdate()
    {
        Vector3 newRopeLengthVector = child.position - transform.position;
        clockDirDif = Vector3.SignedAngle(ropeLengthVector, newRopeLengthVector, transform.up) * deltaLengthCoefficientAngles;
        dif = clockDirDif * (clockDirectionLeft ? 1f : -1f);

        if(rg != null)
        {
            rg.Twisting = dif < 0;
            //Debug.Log(clockDirDif);
        }

        currentAvailableRopeLength = currentAvailableRopeLength + dif;

        //Check Rotation direction and Disconnect from rope
        if(currentAvailableRopeLength > maxAvailableRopeLength + 0.1f)
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
                CreateNewStaticNode(t, clockDirDif < 0); 
            else if (t.GetComponent<Enemy>() != null)
                PushObject(hit, newRopeLengthVector);

        }

        //if(newRopeLengthVector.magnitude > currentAvailableRopeLength &&  child is RopeGod)
        //{
        //    (child as RopeGod).rb.position = transform.position + newRopeLengthVector.normalized * currentAvailableRopeLength;
        //}

        ropeLengthVector = newRopeLengthVector;
    }

    void PushObject(RaycastHit hit, Vector3 rayDir)
    {
        var enemy = hit.transform.GetComponent<Enemy>();
        Debug.DrawLine(hit.point, hit.point + hit.normal * 10f);
        if (enemy != null)
        {
            enemy.rb.velocity = Vector3.zero;
            Vector3 centerDir = enemy.rb.position - hit.point;
            Vector3 perp = Vector3.Cross(rayDir,Vector3.up);
            perp = Vector3.Dot(perp, centerDir) < 0 ? perp * -1f : perp;
            perp.Normalize();
            enemy.rb.velocity = Vector3.zero;
            enemy.rb.position += perp * 2f;
            enemy.rb.AddForce( perp  * 3f * RopeGod.instance.maxVelocity * enemy.rb.mass, ForceMode.Impulse);
            enemy.Hit();
        }
    }

    protected RopeGod rg;

    public override void SetChild(Node child)
    {
        base.SetChild(child);
        UpdateRopeEndTransform();
        rg = child as RopeGod;
        if (rg != null)
            enabled = true;
    }

    void CreateNewStaticNode(Transform t, bool clockDirectionLeft)
    {
        enabled = false;
        StaticNode n = t.gameObject.AddComponent<StaticNode>();
        n.clockDirectionLeft = clockDirectionLeft;
        child.SetParent(n);
        n.SetParent(this);
        n.SetChild(child);
        n.AllotRopeLength(AvailableRopeLength - (position - n.position).magnitude);
        SetChild(n);
    }
}
