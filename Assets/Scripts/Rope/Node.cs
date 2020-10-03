using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Node : MonoBehaviour
{
    CableProceduralSimple rope;
    public Node _parent, _child = null;

    public Rigidbody childRB;

    public float initMaxDist, maxDist, anglesToRopeLength;

    public bool clockDirectionLeft = true, isRoot = false;

    public virtual void Start()
    {
        rope = GetComponentInChildren<CableProceduralSimple>(true);
        if (_child != null)
        {
            _child._parent = this;
            childRB = _child.GetComponent<Rigidbody>();
            rope.gameObject.SetActive(true);
        }
    }

    public virtual void ConnectWithTarget(Node target)
    {
        if (target._parent != null)
        {
            _parent = target._parent;
            target._parent.SetChild(this);
        }
        target.SetParent(this);
        SetChild(target);
    }

    public virtual void DisconnectFromRope()
    {
        _child.SetParent(_parent);
        _parent.SetChild(_child);
        childRB = null;
        _child = null;
    }

    public virtual void SetParent(Node parent)
    {
        _parent = parent;
    }

    public virtual void SetChild(Node child)
    {
        _child = child;
        dist = _child.transform.position - transform.position;
        //maxDist = dist.magnitude;
        if (_parent != null)
        { 
        initMaxDist = _parent.initMaxDist - (transform.position - _parent.transform.position).magnitude;
        maxDist = initMaxDist;
        }
        rope.endPointTransform = child.transform;
        rope.gameObject.SetActive(true);
        childRB = child.GetComponent<Rigidbody>();
    }

    Vector3 dist;

    public bool createdNode = false;

    public virtual void FixedUpdate()
    {
        if (childRB == null || childRB.isKinematic)
            return;

        Vector3 newDist = childRB.position - transform.position;
        float dif = Vector3.SignedAngle(dist, newDist, transform.up) * anglesToRopeLength;

        RaycastHit hit;
        Ray r = new Ray(transform.position, newDist);
        if(Physics.Raycast(r,out hit, newDist.magnitude))
        {
            Node n = hit.transform.GetComponent<Node>();
            if (n != _child)
            {
                if(n._child != null)

                n.ConnectWithTarget(_child);
                n.clockDirectionLeft = dif < 0;
                return;
            }
        }

        
        dif *= clockDirectionLeft ? 1f : -1f;
        maxDist = Mathf.Max(1f, maxDist + dif);
        if (maxDist > initMaxDist)
        {
            maxDist = initMaxDist;
            if(!isRoot)
            {
                DisconnectFromRope();
                rope.gameObject.SetActive(false);
                return;
            }
            clockDirectionLeft = !clockDirectionLeft;
        }

        dist = newDist;

        if (dist.magnitude > maxDist)
        {
            childRB.position = transform.position + dist.normalized * maxDist;
            Vector3 childForward = childRB.transform.forward;
            if (Vector3.Dot(childForward, dist) > 0.1f)
                childRB.rotation *= Quaternion.AngleAxis(Vector3.SignedAngle(childForward, Vector3.Cross(dist, Vector3.Cross(childForward, dist)), transform.up), Vector3.up);
        }
    }
}
